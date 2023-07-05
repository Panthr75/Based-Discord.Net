using Discord.Logging;
using Discord.Net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Discord
{
    internal class ConnectionManager : IDisposable
    {
        public event Func<Task> Connected
        {
            add
            {
                this._connectedEvent.Add(value);
            } 
            remove
            {
                this._connectedEvent.Remove(value);
            } 
        }
        private readonly AsyncEvent<Func<Task>> _connectedEvent = new();
        public event Func<Exception, bool, Task> Disconnected
        {
            add 
            {
                this._disconnectedEvent.Add(value); 
            }
            remove
            {
                this._disconnectedEvent.Remove(value);
            }
        }
        private readonly AsyncEvent<Func<Exception, bool, Task>> _disconnectedEvent = new();

        private readonly SemaphoreSlim _stateLock;
        private readonly Logger _logger;
        private readonly int _connectionTimeout;
        private readonly Func<Task> _onConnecting;
        private readonly Func<Exception, Task> _onDisconnecting;

        private TaskCompletionSource<bool>? _connectionPromise, _readyPromise;
        private CancellationTokenSource? _combinedCancelToken, _reconnectCancelToken, _connectionCancelToken;
        private Task? _task;

        private bool _isDisposed;

        public ConnectionState State { get; private set; }
        public CancellationToken CancelToken { get; private set; }

        internal ConnectionManager(SemaphoreSlim stateLock, Logger logger, int connectionTimeout,
            Func<Task> onConnecting, Func<Exception, Task> onDisconnecting, Action<Func<Exception?, Task>> clientDisconnectHandler)
        {
            this._stateLock = stateLock;
            this._logger = logger;
            this._connectionTimeout = connectionTimeout;
            this._onConnecting = onConnecting;
            this._onDisconnecting = onDisconnecting;

            clientDisconnectHandler(ex =>
            {
                if (ex != null)
                {
                    var ex2 = ex as WebSocketClosedException;
                    if (ex2?.CloseCode == 4006)
                        this.CriticalError(new Exception("WebSocket session expired", ex));
                    else if (ex2?.CloseCode == 4014)
                        this.CriticalError(new Exception("WebSocket connection was closed", ex));
                    else
                        this.Error(new Exception("WebSocket connection was closed", ex));
                }
                else
                    this.Error(new Exception("WebSocket connection was closed"));
                return Task.Delay(0);
            });
        }

        public virtual async Task StartAsync()
        {
            if (State != ConnectionState.Disconnected)
                throw new InvalidOperationException("Cannot start an already running client.");

            await this.AcquireConnectionLock().ConfigureAwait(false);
            var reconnectCancelToken = new CancellationTokenSource();
            this._reconnectCancelToken?.Dispose();
            this._reconnectCancelToken = reconnectCancelToken;
            this._task = Task.Run(async () =>
            {
                try
                {
                    Random jitter = new Random();
                    int nextReconnectDelay = 1000;
                    while (!reconnectCancelToken.IsCancellationRequested)
                    {
                        try
                        {
                            await this.ConnectAsync(reconnectCancelToken).ConfigureAwait(false);
                            nextReconnectDelay = 1000; //Reset delay
                            await this._connectionPromise!.Task.ConfigureAwait(false);
                        }
                        // remove for testing.
                        //catch (OperationCanceledException ex)
                        //{
                        //    // Added back for log out / stop to client. The connection promise would cancel and it would be logged as an error, shouldn't be the case.
                        //    // ref #2026

                        //    Cancel(); //In case this exception didn't come from another Error call
                        //    await DisconnectAsync(ex, !reconnectCancelToken.IsCancellationRequested).ConfigureAwait(false);
                        //}
                        catch (Exception ex)
                        {
                            this.Error(ex); //In case this exception didn't come from another Error call
                            if (!reconnectCancelToken.IsCancellationRequested)
                            {
                                await this._logger.WarningAsync(ex).ConfigureAwait(false);
                                await this.DisconnectAsync(ex, true).ConfigureAwait(false);
                            }
                            else
                            {
                                await this._logger.ErrorAsync(ex).ConfigureAwait(false);
                                await this.DisconnectAsync(ex, false).ConfigureAwait(false);
                            }
                        }

                        if (!reconnectCancelToken.IsCancellationRequested)
                        {
                            //Wait before reconnecting
                            await Task.Delay(nextReconnectDelay, reconnectCancelToken.Token).ConfigureAwait(false);
                            nextReconnectDelay = (nextReconnectDelay * 2) + jitter.Next(-250, 250);
                            if (nextReconnectDelay > 60000)
                                nextReconnectDelay = 60000;
                        }
                    }
                }
                finally
                {
                    this._stateLock.Release();
                }
            });
        }
        public virtual Task StopAsync()
        {
            this.Cancel();
            return Task.CompletedTask;
        }

        private async Task ConnectAsync(CancellationTokenSource reconnectCancelToken)
        {
            this._connectionCancelToken?.Dispose();
            this._combinedCancelToken?.Dispose();
            this._connectionCancelToken = new CancellationTokenSource();
            this._combinedCancelToken = CancellationTokenSource.CreateLinkedTokenSource(_connectionCancelToken.Token, reconnectCancelToken.Token);
            this.CancelToken = _combinedCancelToken.Token;

            this._connectionPromise = new TaskCompletionSource<bool>();
            this.State = ConnectionState.Connecting;
            await this._logger.InfoAsync("Connecting").ConfigureAwait(false);

            try
            {
                var readyPromise = new TaskCompletionSource<bool>();
                this._readyPromise = readyPromise;

                //Abort connection on timeout
                var cancelToken = this.CancelToken;
                var _ = Task.Run(async () =>
                {
                    try
                    {
                        await Task.Delay(this._connectionTimeout, cancelToken).ConfigureAwait(false);
                        readyPromise.TrySetException(new TimeoutException());
                    }
                    catch (OperationCanceledException) { }
                });

                await this._onConnecting().ConfigureAwait(false);

                await this._logger.InfoAsync("Connected").ConfigureAwait(false);
                this.State = ConnectionState.Connected;
                await this._logger.DebugAsync("Raising Event").ConfigureAwait(false);
                await this._connectedEvent.InvokeAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.Error(ex);
                throw;
            }
        }
        private async Task DisconnectAsync(Exception ex, bool isReconnecting)
        {
            if (this.State == ConnectionState.Disconnected)
                return;
            this.State = ConnectionState.Disconnecting;
            await this._logger.InfoAsync("Disconnecting").ConfigureAwait(false);

            await this._onDisconnecting(ex).ConfigureAwait(false);

            await this._disconnectedEvent.InvokeAsync(ex, isReconnecting).ConfigureAwait(false);
            this.State = ConnectionState.Disconnected;
            await this._logger.InfoAsync("Disconnected").ConfigureAwait(false);
        }

        public async Task CompleteAsync()
        {
            if (this._readyPromise is null)
            {
                return;
            }

            await this._readyPromise.TrySetResultAsync(true).ConfigureAwait(false);
        }
        public async Task WaitAsync()
        {
            if (this._readyPromise is null)
            {
                return;
            }

            await this._readyPromise.Task.ConfigureAwait(false);
        }

        public void Cancel()
        {
            this._readyPromise?.TrySetCanceled();
            this._connectionPromise?.TrySetCanceled();
            this._reconnectCancelToken?.Cancel();
            this._connectionCancelToken?.Cancel();
        }
        public void Error(Exception ex)
        {
            this._readyPromise?.TrySetException(ex);
            this._connectionPromise?.TrySetException(ex);
            this._connectionCancelToken?.Cancel();
        }
        public void CriticalError(Exception ex)
        {
            this._reconnectCancelToken?.Cancel();
            this.Error(ex);
        }
        public void Reconnect()
        {
            this._readyPromise?.TrySetCanceled();
            this._connectionPromise?.TrySetCanceled();
            this._connectionCancelToken?.Cancel();
        }
        private async Task AcquireConnectionLock()
        {
            while (true)
            {
                await this.StopAsync().ConfigureAwait(false);
                if (await this._stateLock.WaitAsync(0).ConfigureAwait(false))
                    break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this._combinedCancelToken?.Dispose();
                    this._reconnectCancelToken?.Dispose();
                    this._connectionCancelToken?.Dispose();
                }

                this._isDisposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
