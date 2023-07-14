using System;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="WelcomeScreenChannel"/>
    /// </summary>
    public class WelcomeScreenChannelShim : IConvertibleShim<WelcomeScreenChannel>, ISnowflakeEntity
    {
        private string m_description;
        private IEmoteShim? m_emoji;
        private ulong m_id;

        public WelcomeScreenChannelShim()
        {
            this.m_description = string.Empty;
            this.m_id = ShimUtils.GenerateSnowflake();
        }

        public WelcomeScreenChannelShim(WelcomeScreenChannel welcomeScreenChannel)
        {
            Preconditions.NotNull(welcomeScreenChannel, nameof(welcomeScreenChannel));
            this.m_description = string.Empty;
            this.Apply(welcomeScreenChannel);
        }

        public WelcomeScreenChannel UnShim()
        {
            return new WelcomeScreenChannel(
                id: this.m_id,
                description: this.m_description,
                emojiName: (this.m_emoji as EmojiShim)?.Name,
                emoteId: (this.m_emoji as EmoteShim)?.Id);
        }

        public void Apply(WelcomeScreenChannel value)
        {
            if (value is null)
            {
                return;
            }

            this.m_id = value.Id;
            this.m_description = value.Description;
            this.m_emoji = this.ShimEmote(value.Emoji);
        }

        [return: NotNullIfNotNull(nameof(emote))]
        protected virtual IEmoteShim? ShimEmote(IEmote? emote)
        {
            if (emote is null)
            {
                return null;
            }

            if (emote is IEmoteShim shimmedEmote)
            {
                return shimmedEmote;
            }
            else if (emote is GuildEmote guildEmote)
            {
                return new GuildEmoteShim(guildEmote);
            }
            else if (emote is Emote normalEmote)
            {
                return new EmoteShim(normalEmote);
            }
            else if (emote is Emoji emoji)
            {
                return new EmojiShim(emoji);
            }
            else
            {
                return new EmojiShim(new Emoji(emote.Name!));
            }
        }

        /// <inheritdoc/>
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);

        /// <inheritdoc cref="WelcomeScreenChannel.Id"/>
        public virtual ulong Id
        {
            get => this.m_id;
            set => this.m_id = value;
        }

        /// <inheritdoc cref="WelcomeScreenChannel.Description"/>
        public virtual string Description
        {
            get => this.m_description;
            set
            {
                Preconditions.NotNull(value, nameof(value));
                this.m_description = value;
            }
        }

        /// <inheritdoc cref="WelcomeScreenChannel.Emoji"/>
        public virtual IEmoteShim? Emoji
        {
            get => this.m_emoji;
            set => this.m_emoji = value;
        }

        public static implicit operator WelcomeScreenChannel(WelcomeScreenChannelShim v)
        {
            return v.UnShim();
        }

    }
}
