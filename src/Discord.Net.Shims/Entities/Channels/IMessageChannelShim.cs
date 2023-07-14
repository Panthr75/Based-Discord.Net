using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IMessageChannel"/>
    /// </summary>
    public interface IMessageChannelShim : IMessageChannel, IChannelShim
    {
        /// <inheritdoc cref="IMessageChannel.SendMessageAsync(string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        new Task<IUserMessageShim> SendMessageAsync(string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFileAsync(string, string?, bool, Embed?, RequestOptions?, bool, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        new Task<IUserMessageShim> SendFileAsync(string filePath, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, bool isSpoiler = false, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFileAsync(Stream, string, string?, bool, Embed?, RequestOptions?, bool, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        new Task<IUserMessageShim> SendFileAsync(Stream stream, string filename, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, bool isSpoiler = false, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None);
        /// <inheritdoc cref="IMessageChannel.SendFileAsync(FileAttachment, string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        new Task<IUserMessageShim> SendFileAsync(FileAttachment attachment, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFilesAsync(IEnumerable{FileAttachment}, string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        new Task<IUserMessageShim> SendFilesAsync(IEnumerable<FileAttachment> attachments, string? text = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReference? messageReference = null, MessageComponent? components = null, ISticker[]? stickers = null, Embed[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendMessageAsync(string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        Task<IUserMessageShim> SendMessageAsync(string? text = null, bool isTTS = false, EmbedShim? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReferenceShim? messageReference = null, MessageComponentShim? components = null, IStickerShim[]? stickers = null, EmbedShim[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFileAsync(string, string?, bool, Embed?, RequestOptions?, bool, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        Task<IUserMessageShim> SendFileAsync(string filePath, string? text = null, bool isTTS = false, EmbedShim? embed = null, RequestOptions? options = null, bool isSpoiler = false, AllowedMentions? allowedMentions = null, MessageReferenceShim? messageReference = null, MessageComponentShim? components = null, IStickerShim[]? stickers = null, EmbedShim[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFileAsync(Stream, string, string?, bool, Embed?, RequestOptions?, bool, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        Task<IUserMessageShim> SendFileAsync(Stream stream, string filename, string? text = null, bool isTTS = false, EmbedShim? embed = null, RequestOptions? options = null, bool isSpoiler = false, AllowedMentions? allowedMentions = null, MessageReferenceShim? messageReference = null, MessageComponentShim? components = null, IStickerShim[]? stickers = null, EmbedShim[]? embeds = null, MessageFlags flags = MessageFlags.None);
        /// <inheritdoc cref="IMessageChannel.SendFileAsync(FileAttachment, string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        Task<IUserMessageShim> SendFileAsync(FileAttachment attachment, string? text = null, bool isTTS = false, EmbedShim? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReferenceShim? messageReference = null, MessageComponentShim? components = null, IStickerShim[]? stickers = null, EmbedShim[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.SendFilesAsync(IEnumerable{FileAttachment}, string?, bool, Embed?, RequestOptions?, AllowedMentions?, MessageReference?, MessageComponent?, ISticker[], Embed[], MessageFlags)"/>
        Task<IUserMessageShim> SendFilesAsync(IEnumerable<FileAttachment> attachments, string? text = null, bool isTTS = false, EmbedShim? embed = null, RequestOptions? options = null, AllowedMentions? allowedMentions = null, MessageReferenceShim? messageReference = null, MessageComponentShim? components = null, IStickerShim[]? stickers = null, EmbedShim[]? embeds = null, MessageFlags flags = MessageFlags.None);

        /// <inheritdoc cref="IMessageChannel.GetMessageAsync(ulong, CacheMode, RequestOptions?)"/>
        new Task<IMessageShim?> GetMessageAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

        /// <inheritdoc cref="IMessageChannel.GetMessagesAsync(int, CacheMode, RequestOptions?)"/>
        new IAsyncEnumerable<IReadOnlyCollection<IMessageShim>> GetMessagesAsync(int limit = DiscordConfig.MaxMessagesPerBatch,
            CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
        /// <inheritdoc cref="IMessageChannel.GetMessagesAsync(ulong, Direction, int, CacheMode, RequestOptions?)"/>
        new IAsyncEnumerable<IReadOnlyCollection<IMessageShim>> GetMessagesAsync(ulong fromMessageId, Direction dir, int limit = DiscordConfig.MaxMessagesPerBatch,
            CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

        /// <inheritdoc cref="IMessageChannel.GetMessagesAsync(IMessage, Direction, int, CacheMode, RequestOptions?)"/>
        IAsyncEnumerable<IReadOnlyCollection<IMessageShim>> GetMessagesAsync(IMessageShim fromMessage, Direction dir, int limit = DiscordConfig.MaxMessagesPerBatch,
            CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);

        /// <inheritdoc cref="IMessageChannel.GetPinnedMessagesAsync(RequestOptions?)"/>
        new Task<IReadOnlyCollection<IMessageShim>> GetPinnedMessagesAsync(RequestOptions? options = null);

        /// <inheritdoc cref="IMessageChannel.DeleteMessageAsync(IMessage, RequestOptions?)"/>
        Task DeleteMessageAsync(IMessageShim message, RequestOptions? options = null);
    }
}
