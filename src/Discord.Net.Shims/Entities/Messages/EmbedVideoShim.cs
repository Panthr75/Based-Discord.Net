using Discord.Utils;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedVideo"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedVideoShim : IConvertibleShim<EmbedVideo>, IEquatable<EmbedVideo>, IEquatable<EmbedVideoShim>
    {
        private string? m_url;
        private int? m_width;
        private int? m_height;

        /// <inheritdoc cref="EmbedVideo.Url"/>
        public virtual string? Url
        {
            get => this.m_url;
            set
            {
                UrlValidation.Validate(value, true);
                this.m_url = value;
            }
        }
        /// <inheritdoc cref="EmbedVideo.Height"/>
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
        /// <inheritdoc cref="EmbedVideo.Width"/>
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

        public EmbedVideoShim()
        { }

        public EmbedVideoShim(EmbedVideo embedVideo)
        {
            this.Apply(embedVideo);
        }

        public void Apply(EmbedVideo value)
        {
            this.m_url = value.Url;
            this.m_height = value.Height;
            this.m_width = value.Width;
        }

        public EmbedVideo UnShim()
        {
            return new EmbedVideo(this.Url, this.Height, this.Width);
        }

        private string DebuggerDisplay => $"{Url} ({(Width != null && Height != null ? $"{Width}x{Height}" : "0x0")})";
        /// <inheritdoc cref="EmbedVideo.ToString()"/>
        public override string? ToString() => Url;

        public static implicit operator EmbedVideo(EmbedVideoShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmbedVideoShim? left, EmbedVideoShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedVideoShim? left, EmbedVideoShim? right)
            => !(left == right);

        /// <inheritdoc cref="EmbedVideo.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedVideoShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedVideo other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedVideo.Equals(EmbedVideo)"/>
        public bool Equals(EmbedVideo embedVideo)
        {
            return this.Url == embedVideo.Url &&
                this.Width == embedVideo.Width &&
                this.Height == embedVideo.Height;
        }

        /// <inheritdoc cref="EmbedVideo.Equals(EmbedVideo?)"/>
        public bool Equals([NotNullWhen(true)] EmbedVideo? embedVideo)
        {
            if (!embedVideo.HasValue)
            {
                return false;
            }
            return this.Equals(embedVideo.Value);
        }

        /// <inheritdoc cref="EmbedVideo.Equals(EmbedVideo)"/>
        public bool Equals([NotNullWhen(true)] EmbedVideoShim? embedVideo)
        {
            return embedVideo != null &&
                this.Url == embedVideo.Url &&
                this.Width == embedVideo.Width &&
                this.Height == embedVideo.Height;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Width, this.Height, this.Url);
    }
}
