using Discord.Utils;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ButtonComponent"/>
    /// </summary>
    public class ButtonComponentShim : IConvertibleShim<ButtonComponent>, IMessageComponentShim, IEquatable<ButtonComponent>, IEquatable<ButtonComponentShim>
    {
        private ButtonStyle m_style;
        private string? m_label;
        private IEmoteShim? m_emote;
        private string? m_customId;
        private string? m_url;
        private bool m_isDisabled;

        public ButtonComponentShim()
        { }

        public ButtonComponentShim(ButtonComponent component)
        {
            Preconditions.NotNull(component, nameof(component));
            this.Apply(component);
        }

        IMessageComponent IConvertibleShim<IMessageComponent>.UnShim()
        {
            return this.UnShim();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Throws <see cref="InvalidOperationException"/> if
        /// this component is in an invalid state, or if the url
        /// is invalid.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// This component is in an invalid state.
        /// </exception>
        public ButtonComponent UnShim()
        {
            ButtonStyle style = this.Style;
            string? url = this.Url;
            string? id = this.CustomId;
            if (style == ButtonStyle.Link)
            {
                if (!UrlValidation.Validate(url))
                {
                    throw new InvalidOperationException("Url is required for link buttons");
                }
                id = null;
            }
            else
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new InvalidOperationException("Id is required for non-link buttons");
                }
                url = null;
            }

            return new ButtonComponent(style: style,
                label: this.Label,
                emote: this.Emote?.UnShim(),
                customId: id,
                url: url,
                isDisabled: this.IsDisabled);
        }

        void IConvertibleShim<IMessageComponent>.Apply(IMessageComponent value)
        {
            if (value is not ButtonComponent button)
            {
                return;
            }

            this.Apply(button);
        }

        public void Apply(ButtonComponent value)
        {
            if (value is null)
            {
                return;
            }

            this.m_customId = value.CustomId;
            this.m_emote = this.ShimEmote(value.Emote);
            this.m_isDisabled = value.IsDisabled;
            this.m_label = value.Label;
            this.m_style = value.Style;
            this.m_url = value.Url;
        }

        bool IEquatable<IMessageComponent>.Equals([NotNullWhen(true)] IMessageComponent? other)
        {
            return other is ButtonComponent component && this.Equals(component);
        }

        bool IEquatable<IMessageComponentShim>.Equals([NotNullWhen(true)] IMessageComponentShim? other)
        {
            return other is ButtonComponentShim component && this.Equals(component);
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is ButtonComponentShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is ButtonComponent other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals([NotNullWhen(true)] ButtonComponent? other)
        {
            if (other is null ||
                this.Style != other.Style ||
                this.Label != other.Label ||
                this.CustomId != other.CustomId ||
                this.Url != other.Url ||
                this.IsDisabled != other.IsDisabled)
            {
                return false;
            }

            IEmoteShim? emote = this.Emote;
            if (emote is not null)
            {
                return emote.Equals(other.Emote);
            }
            else
            {
                return other.Emote is null;
            }
        }

        public bool Equals([NotNullWhen(true)] ButtonComponentShim? other)
        {
            if (other is null ||
                this.Style != other.Style ||
                this.Label != other.Label ||
                this.CustomId != other.CustomId ||
                this.Url != other.Url ||
                this.IsDisabled != other.IsDisabled)
            {
                return false;
            }

            IEmoteShim? emote = this.Emote;
            if (emote is not null)
            {
                return emote.Equals(other.Emote);
            }
            else
            {
                return other.Emote is null;
            }
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

        /// <inheritdoc cref="ButtonComponent.Style"/>
        public virtual ButtonStyle Style
        {
            get => this.m_style;
            set => this.m_style = value;
        }

        /// <inheritdoc cref="ButtonComponent.Label"/>
        public virtual string? Label
        {
            get => this.m_label;
            set
            {
                if (value is null)
                {
                    this.m_label = null;
                    return;
                }

                value = value.Trim();
                this.m_label = value.Substring(0, Math.Min(value.Length, ButtonBuilder.MaxButtonLabelLength));
            }
        }

        /// <inheritdoc cref="ButtonComponent.Emote"/>
        public virtual IEmoteShim? Emote
        {
            get => this.m_emote;
            set => this.m_emote = value;
        }

        /// <inheritdoc cref="ButtonComponent.CustomId"/>
        public virtual string? CustomId
        {
            get => this.m_customId;
            set
            {
                if (value is null)
                {
                    this.m_customId = null;
                    return;
                }

                value = value.Trim();
                this.m_customId = value.Substring(0, Math.Min(value.Length, ComponentBuilder.MaxCustomIdLength));
            }
        }

        /// <inheritdoc cref="ButtonComponent.Url"/>
        public virtual string? Url
        {
            get => this.m_url;
            set => this.m_url = value;
        }

        /// <inheritdoc cref="ButtonComponent.IsDisabled"/>
        public virtual bool IsDisabled
        {
            get => this.m_isDisabled;
            set => this.m_isDisabled = value;
        }

        ComponentType IMessageComponent.Type => ComponentType.Button;

        // is explicit because of InvalidOperationException on UnShim.
        public static explicit operator ButtonComponent(ButtonComponentShim v)
        {
            return v.UnShim();
        }
    }
}
