using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Discord.WebSocket
{
    internal static class SocketChannelHelper
    {
        public static IAsyncEnumerable<IReadOnlyCollection<IMessage>> GetMessagesAsync(ISocketMessageChannel channel, DiscordSocketClient discord, MessageCache? messages,
            ulong? fromMessageId, Direction dir, int limit, CacheMode mode, RequestOptions? options)
        {
            if (dir == Direction.After && fromMessageId == null)
                return AsyncEnumerable.Empty<IReadOnlyCollection<IMessage>>();

            var cachedMessages = GetCachedMessages(channel, discord, messages, fromMessageId, dir, limit);
            var result = ImmutableArray.Create(cachedMessages).ToAsyncEnumerable<IReadOnlyCollection<IMessage>>();

            if (dir == Direction.Before)
            {
                limit -= cachedMessages.Count;
                if (mode == CacheMode.CacheOnly || limit <= 0)
                    return result;

                //Download remaining messages
                ulong? minId = cachedMessages.Count > 0 ? cachedMessages.Min(x => x.Id) : fromMessageId;
                var downloadedMessages = ChannelHelper.GetMessagesAsync(channel, discord, minId, dir, limit, options);
                if (cachedMessages.Count != 0)
                    return result.Concat(downloadedMessages);
                else
                    return downloadedMessages;
            }
            else if (dir == Direction.After)
            {
                if (mode == CacheMode.CacheOnly)
                    return result;

                bool ignoreCache = false;

                // We can find two cases:
                // 1. fromMessageId is not null and corresponds to a message that is not in the cache,
                // so we have to make a request and ignore the cache
                if (fromMessageId.HasValue && messages?.Get(fromMessageId.Value) == null)
                {
                    ignoreCache = true;
                }
                // 2. fromMessageId is null or already in the cache, so we start from the cache
                else if (cachedMessages.Count > 0)
                {
                    fromMessageId = cachedMessages.Max(x => x.Id);
                    limit -= cachedMessages.Count;

                    if (limit <= 0)
                        return result;
                }

                //Download remaining messages
                var downloadedMessages = ChannelHelper.GetMessagesAsync(channel, discord, fromMessageId, dir, limit, options);
                if (!ignoreCache && cachedMessages.Count != 0)
                    return result.Concat(downloadedMessages);
                else
                    return downloadedMessages;
            }
            else //Direction.Around
            {
                if (mode == CacheMode.CacheOnly || limit <= cachedMessages.Count)
                    return result;

                //Cache isn't useful here since Discord will send them anyways
                return ChannelHelper.GetMessagesAsync(channel, discord, fromMessageId, dir, limit, options);
            }
        }
        public static IReadOnlyCollection<SocketMessage> GetCachedMessages(ISocketMessageChannel channel, DiscordSocketClient discord, MessageCache? messages,
            ulong? fromMessageId, Direction dir, int limit)
        {
            if (messages != null) //Cache enabled
                return messages.GetMany(fromMessageId, dir, limit);
            else
                return ImmutableArray.Create<SocketMessage>();
        }
        /// <exception cref="NotSupportedException">Unexpected <see cref="ISocketMessageChannel"/> type.</exception>
        public static void AddMessage(ISocketMessageChannel channel, DiscordSocketClient discord,
            SocketMessage msg)
        {
            switch (channel)
            {
                case SocketDMChannel dmChannel:
                    dmChannel.AddMessage(msg);
                    break;
                case SocketGroupChannel groupChannel:
                    groupChannel.AddMessage(msg);
                    break;
                case SocketThreadChannel threadChannel:
                    threadChannel.AddMessage(msg);
                    break;
                case SocketTextChannel textChannel:
                    textChannel.AddMessage(msg);
                    break;
                default:
                    throw new NotSupportedException($"Unexpected {nameof(ISocketMessageChannel)} type.");
            }
        }
        /// <exception cref="NotSupportedException">Unexpected <see cref="ISocketMessageChannel"/> type.</exception>
        public static SocketMessage? RemoveMessage(ISocketMessageChannel channel, DiscordSocketClient discord,
            ulong id)
        {
            return channel switch
            {
                SocketDMChannel dmChannel => dmChannel.RemoveMessage(id),
                SocketGroupChannel groupChannel => groupChannel.RemoveMessage(id),
                SocketTextChannel textChannel => textChannel.RemoveMessage(id),
                _ => throw new NotSupportedException($"Unexpected {nameof(ISocketMessageChannel)} type."),
            };
        }
    }
}
