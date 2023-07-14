using Discord.Utils;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedThumbnail"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedThumbnailShim : IConvertibleShim<EmbedThumbnail>, IEquatable<EmbedThumbnail>, IEquatable<EmbedThumbnailShim>
    {
        private string m_url;
        private string? m_proxyUrl;
        private int? m_height;
        private int? m_width;


        /// <inheritdoc cref="EmbedThumbnail.Url"/>
        public virtual string Url
        {
            get => this.m_url;
            set
            {
                if (value is null)
                {
                    return;
                }

                if (!UrlValidation.Validate(value, true))
                {
                    return;
                }

                this.m_url = value;
            }
        }
        /// <inheritdoc cref="EmbedThumbnail.ProxyUrl"/>
        public virtual string? ProxyUrl
        {
            get => this.m_proxyUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_proxyUrl = value;
            }
        }
        /// <inheritdoc cref="EmbedThumbnail.Height"/>
        public virtual int? Height
        {
            get => this.m_height;
            set
            {
                if (value.HasValue && value.Value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Height must be greater than 0.");
                }
                this.m_height = value;
            }
        }
        /// <inheritdoc cref="EmbedThumbnail.Width"/>
        public virtual int? Width
        {
            get => this.m_width;
            set
            {
                if (value.HasValue && value.Value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater than 0.");
                }
                this.m_width = value;
            }
        }

        public EmbedThumbnailShim(string url)
        {
            UrlValidation.Validate(url, true);
            this.m_url = url;
        }

        public EmbedThumbnailShim(EmbedThumbnail embedThumbnail)
        {
            this.m_url = string.Empty;
            this.Apply(embedThumbnail);
        }

        public void Apply(EmbedThumbnail value)
        {
            this.m_url = value.Url;
            this.m_proxyUrl = value.ProxyUrl;
            this.m_height = value.Height;
            this.m_width = value.Width;
        }

        public EmbedThumbnail UnShim()
        {
            return new EmbedThumbnail(this.Url, this.ProxyUrl, this.Height, this.Width);
        }

        private string DebuggerDisplay => $"{Url} ({(Width != null && Height != null ? $"{Width}x{Height}" : "0x0")})";

        /// <inheritdoc cref="EmbedThumbnail.ToString()"/>
        public override string ToString() => Url;

        public static bool operator ==(EmbedThumbnailShim? left, EmbedThumbnailShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedThumbnailShim? left, EmbedThumbnailShim? right)
            => !(left == right);

        public static implicit operator EmbedThumbnail(EmbedThumbnailShim v)
        {
            return v.UnShim();
        }

        /// <inheritdoc cref="EmbedThumbnail.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedThumbnailShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedThumbnail other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedThumbnail.Equals(EmbedThumbnail)"/>
        public bool Equals(EmbedThumbnail embedThumbnail)
        {
            return this.Url == embedThumbnail.Url &&
                this.ProxyUrl == embedThumbnail.ProxyUrl &&
                this.Width == embedThumbnail.Width &&
                this.Height == embedThumbnail.Height;
        }

        /// <inheritdoc cref="EmbedThumbnail.Equals(EmbedThumbnail?)"/>
        public bool Equals([NotNullWhen(true)] EmbedThumbnail? embedThumbnail)
        {
            if (!embedThumbnail.HasValue)
            {
                return false;
            }
            return this.Equals(embedThumbnail.Value);
        }

        /// <inheritdoc cref="EmbedThumbnail.Equals(EmbedThumbnail)"/>
        public bool Equals([NotNullWhen(true)] EmbedThumbnailShim? embedThumbnail)
        {
            return embedThumbnail != null &&
                this.Url == embedThumbnail.Url &&
                this.ProxyUrl == embedThumbnail.ProxyUrl &&
                this.Width == embedThumbnail.Width &&
                this.Height == embedThumbnail.Height;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Width, this.Height, this.Url, this.ProxyUrl);
    }
}
