using Discord.Utils;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedFooter"/>
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedFooterShim : IConvertibleShim<EmbedFooter>, IEquatable<EmbedFooter>, IEquatable<EmbedFooterShim>
    {
        private string m_text;
        private string? m_iconUrl;
        private string? m_proxyUrl;

        public EmbedFooterShim(string text)
        {
            Preconditions.NotNull(text, nameof(text));

            this.m_text = string.Empty;
            this.Text = text;
        }

        public EmbedFooterShim(EmbedFooter embedFooter)
        {
            this.m_text = string.Empty;
            this.Apply(embedFooter);
        }

        public void Apply(EmbedFooter value)
        {
            this.m_text = value.Text;
            this.m_iconUrl = value.IconUrl;
            this.m_proxyUrl = value.ProxyUrl;
        }

        public EmbedFooter UnShim()
        {
            return new EmbedFooter(this.Text, this.IconUrl, this.ProxyUrl);
        }

        /// <inheritdoc cref="EmbedFooter.Text"/>
        public virtual string Text
        {
            get => this.m_text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                value = value.Trim();
                this.m_text = value.Substring(0, Math.Min(0, EmbedFooterBuilder.MaxFooterTextLength));
            }
        }

        /// <inheritdoc cref="EmbedFooter.IconUrl"/>
        public virtual string? IconUrl
        {
            get => this.m_iconUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_iconUrl = value;
            }
        }

        /// <inheritdoc cref="EmbedFooter.ProxyUrl"/>
        public virtual string? ProxyUrl
        {
            get => this.m_proxyUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_proxyUrl = value;
            }
        }

        private string DebuggerDisplay => $"{Text} ({IconUrl})";
        /// <inheritdoc cref="EmbedFooter.ToString"/>
        public override string ToString() => Text;

        public static implicit operator EmbedFooter(EmbedFooterShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmbedFooterShim? left, EmbedFooterShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedFooterShim? left, EmbedFooterShim? right)
            => !(left == right);

        /// <inheritdoc cref="EmbedFooter.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedFooterShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedFooter other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedFooter.Equals(EmbedFooter)"/>
        public bool Equals(EmbedFooter embedFooter)
        {
            return this.Text == embedFooter.Text &&
                this.IconUrl == embedFooter.IconUrl &&
                this.ProxyUrl == embedFooter.ProxyUrl;
        }

        /// <inheritdoc cref="EmbedFooter.Equals(EmbedFooter)"/>
        public bool Equals([NotNullWhen(true)] EmbedFooter? embedFooter)
        {
            if (!embedFooter.HasValue)
            {
                return false;
            }

            return this.Equals(embedFooter.Value);
        }

        /// <inheritdoc cref="EmbedFooter.Equals(EmbedFooter)"/>
        public bool Equals([NotNullWhen(true)] EmbedFooterShim? embedFooter)
        {
            return embedFooter != null &&
                this.Text == embedFooter.Text &&
                this.IconUrl == embedFooter.IconUrl &&
                this.ProxyUrl == embedFooter.ProxyUrl;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(Text, IconUrl, ProxyUrl);
    }
}
