using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IMessage"/>
    /// </summary>
    public interface IMessageShim : IMessage, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IMessage.IsTTS"/>
        new bool IsTTS { get; set; }
        /// <inheritdoc cref="IMessage.IsSuppressed"/>
        new bool IsSuppressed { get; set; }
        /// <inheritdoc cref="IMessage.MentionedEveryone"/>
        new bool MentionedEveryone { get; set; }
        /// <inheritdoc cref="IMessage.Content"/>
        new string Content { get; set; }
        /// <inheritdoc cref="IMessage.EditedTimestamp"/>
        new DateTimeOffset? EditedTimestamp { get; set; }
        /// <inheritdoc cref="IMessage.Channel"/>
        new IMessageChannelShim Channel { get; }
        /// <inheritdoc cref="IMessage.Author"/>
        new IUserShim Author { get; }

        /// <inheritdoc cref="IMessage.Thread"/>
        new IThreadChannelShim? Thread { get; set; }

        /// <inheritdoc cref="IMessage.Attachments"/>
        new ICollection<IAttachmentShim> Attachments { get; }
        /// <inheritdoc cref="IMessage.Embeds"/>
        new ICollection<IEmbedShim> Embeds { get; }
        /// <inheritdoc cref="IMessage.Activity"/>
        new MessageActivityShim? Activity { get; set; }
        /// <inheritdoc cref="IMessage.Application"/>
        new MessageApplicationShim? Application { get; set; }

        /// <inheritdoc cref="IMessage.Reference"/>
        new MessageReferenceShim? Reference { get; set; }

        /// <inheritdoc cref="IMessage.Reactions"/>
        new IDictionary<IEmoteShim, ReactionMetadataShim> Reactions { get; }

        /// <inheritdoc cref="IMessage.Components"/>
        new ICollection<IMessageComponentShim> Components { get; }

        /// <inheritdoc cref="IMessage.Stickers"/>
        new ICollection<IStickerItemShim> Stickers { get; }

        /// <inheritdoc cref="IMessage.Flags"/>
        new MessageFlags? Flags { get; set; }

        /// <inheritdoc cref="IMessage.Interaction"/>
        new IMessageInteractionShim? Interaction { get; set; }

        /// <inheritdoc cref="IMessage.RoleSubscriptionData"/>
        new MessageRoleSubscriptionDataShim? RoleSubscriptionData { get; set; }

        /// <inheritdoc cref="IMessage.AddReactionAsync(IEmote, RequestOptions?)"/>
        Task AddReactionAsync(IEmoteShim emote, RequestOptions? options = null);

        /// <inheritdoc cref="IMessage.RemoveReactionAsync(IEmote, IUser, RequestOptions?)"/>
        Task RemoveReactionAsync(IEmoteShim emote, IUserShim user, RequestOptions? options = null);
        /// <inheritdoc cref="IMessage.RemoveReactionAsync(IEmote, ulong, RequestOptions?)"/>
        Task RemoveReactionAsync(IEmoteShim emote, ulong userId, RequestOptions? options = null);
        /// <inheritdoc cref="IMessage.RemoveAllReactionsForEmoteAsync(IEmote, RequestOptions?)"/>
        Task RemoveAllReactionsForEmoteAsync(IEmoteShim emote, RequestOptions? options = null);


        /// <inheritdoc cref="IMessage.GetReactionUsersAsync(IEmote, int, RequestOptions?)"/>
        IAsyncEnumerable<IReadOnlyCollection<IUserShim>> GetReactionUsersAsync(IEmoteShim emoji, int limit, RequestOptions? options = null);
    }
}
