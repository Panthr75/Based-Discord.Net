using Discord.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedImage"/>
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedImageShim : IConvertibleShim<EmbedImage>, IEquatable<EmbedImage>, IEquatable<EmbedImageShim>
    {
        private string m_url;
        private string? m_proxyUrl;
        private int? m_height;
        private int? m_width;

        /// <inheritdoc cref="EmbedImage.Url"/>
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
        /// <inheritdoc cref="EmbedImage.ProxyUrl"/>
        public virtual string? ProxyUrl
        {
            get => this.m_proxyUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_proxyUrl = value;
            }
        }
        /// <inheritdoc cref="EmbedImage.Height"/>
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
        /// <inheritdoc cref="EmbedImage.Width"/>
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

        public EmbedImageShim(string url)
        {
            if (!UrlValidation.Validate(url, true))
            {
                throw new ArgumentNullException(nameof(url));
            }
            this.m_url = url;
        }

        public EmbedImageShim(EmbedImage embedImage)
        {
            this.m_url = string.Empty;
            this.Apply(embedImage);
        }

        public void Apply(EmbedImage value)
        {
            this.m_url = value.Url;
            this.m_height = value.Height;
            this.m_width = value.Width;
            this.m_proxyUrl = value.ProxyUrl;
        }

        public EmbedImage UnShim()
        {
            return new EmbedImage(this.Url, this.ProxyUrl, this.Height, this.Width);
        }

        private string DebuggerDisplay => $"{Url} ({(Width != null && Height != null ? $"{Width}x{Height}" : "0x0")})";
        /// <inheritdoc cref="EmbedImage.ToString()"/>
        public override string ToString() => Url;

        public static bool operator ==(EmbedImageShim? left, EmbedImageShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedImageShim? left, EmbedImageShim? right)
            => !(left == right);

        /// <inheritdoc cref="EmbedImage.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedImageShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedImage other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedImage.Equals(EmbedImage)"/>
        public bool Equals(EmbedImage embedImage)
        {
            return this.Url == embedImage.Url &&
                this.ProxyUrl == embedImage.ProxyUrl &&
                this.Width == embedImage.Width &&
                this.Height == embedImage.Height;
        }

        /// <inheritdoc cref="EmbedImage.Equals(EmbedImage?)"/>
        public bool Equals([NotNullWhen(true)] EmbedImage? embedImage)
        {
            if (!embedImage.HasValue)
            {
                return false;
            }
            return this.Equals(embedImage.Value);
        }

        /// <inheritdoc cref="EmbedImage.Equals(EmbedImage)"/>
        public bool Equals([NotNullWhen(true)] EmbedImageShim? embedImage)
        {
            return embedImage is not null &&
                this.Url == embedImage.Url &&
                this.ProxyUrl == embedImage.ProxyUrl &&
                this.Width == embedImage.Width &&
                this.Height == embedImage.Height;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Height, this.Width, this.Url, this.ProxyUrl);

        public static implicit operator EmbedImage(EmbedImageShim v)
        {
            return v.UnShim();
        }
    }
}
