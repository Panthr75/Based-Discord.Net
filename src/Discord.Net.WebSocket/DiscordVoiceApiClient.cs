using Discord.API;
using Discord.API.Voice;
using Discord.Net.Converters;
using Discord.Net.Udp;
using Discord.Net.WebSockets;
using System.Text.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord.Rest;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;

namespace Discord.Audio
{
    internal class DiscordVoiceAPIClient : IDisposable
    {
        #region DiscordVoiceAPIClient
        public const int MaxBitrate = 128 * 1024;
        public const string Mode = "xsalsa20_poly1305";

        public event Func<string, string, double, Task> SentRequest { add { _sentRequestEvent.Add(value); } remove { _sentRequestEvent.Remove(value); } }
        private readonly AsyncEvent<Func<string, string, double, Task>> _sentRequestEvent = new();
        public event Func<VoiceOpCode, Task> SentGatewayMessage { add { _sentGatewayMessageEvent.Add(value); } remove { _sentGatewayMessageEvent.Remove(value); } }
        private readonly AsyncEvent<Func<VoiceOpCode, Task>> _sentGatewayMessageEvent = new();
        public event Func<Task> SentDiscovery { add { _sentDiscoveryEvent.Add(value); } remove { _sentDiscoveryEvent.Remove(value); } }
        private readonly AsyncEvent<Func<Task>> _sentDiscoveryEvent = new();
        public event Func<int, Task> SentData { add { _sentDataEvent.Add(value); } remove { _sentDataEvent.Remove(value); } }
        private readonly AsyncEvent<Func<int, Task>> _sentDataEvent = new();

        public event Func<VoiceOpCode, object?, Task> ReceivedEvent { add { _receivedEvent.Add(value); } remove { _receivedEvent.Remove(value); } }
        private readonly AsyncEvent<Func<VoiceOpCode, object?, Task>> _receivedEvent = new();
        public event Func<byte[], Task> ReceivedPacket { add { _receivedPacketEvent.Add(value); } remove { _receivedPacketEvent.Remove(value); } }
        private readonly AsyncEvent<Func<byte[], Task>> _receivedPacketEvent = new();
        public event Func<Exception, Task> Disconnected { add { _disconnectedEvent.Add(value); } remove { _disconnectedEvent.Remove(value); } }
        private readonly AsyncEvent<Func<Exception, Task>> _disconnectedEvent = new();

        private readonly JsonSerializerOptions _serializerOptions;
        private readonly SemaphoreSlim _connectionLock;
        private readonly IUdpSocket _udp;
        private CancellationTokenSource? _connectCancelToken;
        private bool _isDisposed;
        private ulong _nextKeepalive;

        public ulong GuildId { get; }
        internal IWebSocketClient WebSocketClient { get; }
        public ConnectionState ConnectionState { get; private set; }

        public ushort UdpPort => _udp.Port;

        internal DiscordVoiceAPIClient(ulong guildId, WebSocketProvider webSocketProvider, UdpSocketProvider udpSocketProvider, JsonSerializerOptions? serializer = null)
        {
            GuildId = guildId;
            _connectionLock = new SemaphoreSlim(1, 1);
            _udp = udpSocketProvider();
            _udp.ReceivedDatagram += (data, index, count) =>
            {
                if (index != 0 || count != data.Length)
                {
                    var newData = new byte[count];
                    Buffer.BlockCopy(data, index, newData, 0, count);
                    data = newData;
                }
                return _receivedPacketEvent.InvokeAsync(data);
            };

            WebSocketClient = webSocketProvider();
            //_gatewayClient.SetHeader("user-agent", DiscordConfig.UserAgent); //(Causes issues in .Net 4.6+)
            WebSocketClient.BinaryMessage += async (data, index, count) =>
            {
                using (var compressed = new MemoryStream(data, index + 2, count - 2))
                using (var decompressed = new MemoryStream())
                {
                    using (var zlib = new DeflateStream(compressed, CompressionMode.Decompress))
                        zlib.CopyTo(decompressed);
                    decompressed.Position = 0;
                    using (var reader = new StreamReader(decompressed))
                    {
                        var msg = JsonSerializer.Deserialize<SocketFrame>(reader.ReadToEnd(), this._serializerOptions)!;
                        await _receivedEvent.InvokeAsync((VoiceOpCode)msg.Operation, msg.Payload).ConfigureAwait(false);
                    }
                }
            };
            WebSocketClient.TextMessage += text =>
            {
                var msg = JsonSerializer.Deserialize<SocketFrame>(text, this._serializerOptions)!;
                return _receivedEvent.InvokeAsync((VoiceOpCode)msg.Operation, msg.Payload);
            };
            WebSocketClient.Closed += async ex =>
            {
                await DisconnectAsync().ConfigureAwait(false);
                await _disconnectedEvent.InvokeAsync(ex).ConfigureAwait(false);
            };

            if (serializer == null)
            {

                _serializerOptions = new JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    IncludeFields = true,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                    TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                    {
                        Modifiers = { Optional.OptionalModifier }
                    }
                };
                _serializerOptions.AddConverter<EmbedTypeConverter>();
                _serializerOptions.AddConverter<UInt64Converter>();
                _serializerOptions.AddConverter<EnumStringValueConverter<GuildPermission>>();
                _serializerOptions.AddConverter<GuildFeaturesConverter>();
                _serializerOptions.AddConverter<ApplicationCommandOptionValueConverter>();
                _serializerOptions.AddConverter<OptionalConverterFactory>();
                _serializerOptions.AddConverter<ImageConverter>();
                _serializerOptions.AddConverter<InteractionConverter>();
                _serializerOptions.AddConverter<NumericValueConverter>();
                _serializerOptions.AddConverter<JsonNodeConverter>();
                _serializerOptions.AddConverter<MessageComponentConverter>();
                _serializerOptions.AddConverter<StringEntityConverter>();
                _serializerOptions.AddConverter<UInt64EntityConverter>();
                _serializerOptions.AddConverter<UInt64EntityOrIdConverter>();
                _serializerOptions.AddConverter<UserStatusConverter>();
                _serializerOptions.AddConverter<SelectMenuDefaultValueTypeConverter>();
            }
            else
                _serializerOptions = serializer;
        }
        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _connectCancelToken?.Dispose();
                    _udp?.Dispose();
                    WebSocketClient?.Dispose();
                    _connectionLock?.Dispose();
                }
                _isDisposed = true;
            }
        }
        public void Dispose() => Dispose(true);

        public async Task SendAsync(VoiceOpCode opCode, object? payload, RequestOptions? options = null)
        {
            byte[] bytes = Array.Empty<byte>();
            payload = new SocketFrame { Operation = (int)opCode, Payload = payload };
            if (payload != null)
                bytes = Encoding.UTF8.GetBytes(SerializeJson(payload));
            await WebSocketClient.SendAsync(bytes, 0, bytes.Length, true).ConfigureAwait(false);
            await _sentGatewayMessageEvent.InvokeAsync(opCode).ConfigureAwait(false);
        }
        public async Task SendAsync(byte[] data, int offset, int bytes)
        {
            await _udp.SendAsync(data, offset, bytes).ConfigureAwait(false);
            await _sentDataEvent.InvokeAsync(bytes).ConfigureAwait(false);
        }
        #endregion

        #region WebSocket
        public Task SendHeartbeatAsync(RequestOptions? options = null)
            => SendAsync(VoiceOpCode.Heartbeat, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), options: options);

        public Task SendIdentityAsync(ulong userId, string sessionId, string token)
        {
            return SendAsync(VoiceOpCode.Identify, new IdentifyParams
            {
                GuildId = GuildId,
                UserId = userId,
                SessionId = sessionId,
                Token = token
            });
        }

        public Task SendSelectProtocol(string externalIp, int externalPort)
        {
            return SendAsync(VoiceOpCode.SelectProtocol, new SelectProtocolParams
            {
                Protocol = "udp",
                Data = new UdpProtocolInfo
                {
                    Address = externalIp,
                    Port = externalPort,
                    Mode = Mode
                }
            });
        }

        public Task SendSetSpeaking(bool value)
        {
            return SendAsync(VoiceOpCode.Speaking, new SpeakingParams
            {
                IsSpeaking = value,
                Delay = 0
            });
        }

        public async Task ConnectAsync(string url)
        {
            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await ConnectInternalAsync(url).ConfigureAwait(false);
            }
            finally { _connectionLock.Release(); }
        }

        private async Task ConnectInternalAsync(string url)
        {
            ConnectionState = ConnectionState.Connecting;
            try
            {
                _connectCancelToken?.Dispose();
                _connectCancelToken = new CancellationTokenSource();
                var cancelToken = _connectCancelToken.Token;

                WebSocketClient.SetCancelToken(cancelToken);
                await WebSocketClient.ConnectAsync(url).ConfigureAwait(false);

                _udp.SetCancelToken(cancelToken);
                await _udp.StartAsync().ConfigureAwait(false);

                ConnectionState = ConnectionState.Connected;
            }
            catch
            {
                await DisconnectInternalAsync().ConfigureAwait(false);
                throw;
            }
        }

        public async Task DisconnectAsync()
        {
            await _connectionLock.WaitAsync().ConfigureAwait(false);
            try
            {
                await DisconnectInternalAsync().ConfigureAwait(false);
            }
            finally { _connectionLock.Release(); }
        }
        private async Task DisconnectInternalAsync()
        {
            if (ConnectionState == ConnectionState.Disconnected)
                return;
            ConnectionState = ConnectionState.Disconnecting;

            try
            { _connectCancelToken?.Cancel(false); }
            catch { }

            //Wait for tasks to complete
            await _udp.StopAsync().ConfigureAwait(false);
            await WebSocketClient.DisconnectAsync().ConfigureAwait(false);

            ConnectionState = ConnectionState.Disconnected;
        }
        #endregion

        #region Udp
        public async Task SendDiscoveryAsync(uint ssrc)
        {
            var packet = new byte[74];
            packet[1] = 1;
            packet[3] = 70;
            packet[4] = (byte)(ssrc >> 24);
            packet[5] = (byte)(ssrc >> 16);
            packet[6] = (byte)(ssrc >> 8);
            packet[7] = (byte)(ssrc >> 0);
            await SendAsync(packet, 0, 74).ConfigureAwait(false);
            await _sentDiscoveryEvent.InvokeAsync().ConfigureAwait(false);
        }
        public async Task<ulong> SendKeepaliveAsync()
        {
            var value = _nextKeepalive++;
            var packet = new byte[8];
            packet[0] = (byte)(value >> 0);
            packet[1] = (byte)(value >> 8);
            packet[2] = (byte)(value >> 16);
            packet[3] = (byte)(value >> 24);
            packet[4] = (byte)(value >> 32);
            packet[5] = (byte)(value >> 40);
            packet[6] = (byte)(value >> 48);
            packet[7] = (byte)(value >> 56);
            await SendAsync(packet, 0, 8).ConfigureAwait(false);
            return value;
        }

        public void SetUdpEndpoint(string ip, int port)
        {
            _udp.SetDestination(ip, port);
        }
        #endregion

        #region Helpers
        private static double ToMilliseconds(Stopwatch stopwatch) => Math.Round((double)stopwatch.ElapsedTicks / (double)Stopwatch.Frequency * 1000.0, 2);
        private string SerializeJson(object value)
        {
            return JsonSerializer.Serialize(value, this._serializerOptions);
        }
        private T DeserializeJson<T>(Stream jsonStream)
        {
            return JsonSerializer.Deserialize<T>(jsonStream, this._serializerOptions)!;
        }
        #endregion
    }
}
