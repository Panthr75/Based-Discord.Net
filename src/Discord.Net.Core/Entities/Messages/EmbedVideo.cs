using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    ///     A video featured in an <see cref="Embed"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct EmbedVideo : IEquatable<EmbedVideo>
    {
        /// <summary>
        ///     Gets the URL of the video.
        /// </summary>
        /// <returns>
        ///     A string containing the URL of the image.
        /// </returns>
        public string? Url { get; }
        /// <summary>
        ///     Gets the height of the video.
        /// </summary>
        /// <returns>
        ///     A <see cref="int"/> representing the height of this video if it can be retrieved; otherwise 
        ///     <see langword="null" />.
        /// </returns>
        public int? Height { get; }
        /// <summary>
        ///     Gets the weight of the video.
        /// </summary>
        /// <returns>
        ///     A <see cref="int"/> representing the width of this video if it can be retrieved; otherwise 
        ///     <see langword="null" />.
        /// </returns>
        public int? Width { get; }

        internal EmbedVideo(string? url, int? height, int? width)
        {
            Url = url;
            Height = height;
            Width = width;
        }

        private string DebuggerDisplay => $"{Url} ({(Width != null && Height != null ? $"{Width}x{Height}" : "0x0")})";
        /// <summary>
        ///     Gets the URL of the video.
        /// </summary>
        /// <returns>
        ///     A string that resolves to <see cref="Url"/>.
        /// </returns>
        public override string? ToString() => Url;

        public static bool operator ==(EmbedVideo? left, EmbedVideo? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedVideo? left, EmbedVideo? right)
            => !(left == right);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="EmbedVideo"/>.
        /// </summary>
        /// <remarks>
        /// If the object passes is an <see cref="EmbedVideo"/>, <see cref="Equals(EmbedVideo?)"/> will be called to compare the 2 instances
        /// </remarks>
        /// <param name="obj">The object to compare with the current <see cref="EmbedVideo"/></param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is EmbedVideo embedVideo && Equals(embedVideo);

        /// <summary>
        /// Determines whether the specified <see cref="EmbedVideo"/> is equal to the current <see cref="EmbedVideo"/>
        /// </summary>
        /// <param name="embedVideo">The <see cref="EmbedVideo"/> to compare with the current <see cref="EmbedVideo"/></param>
        /// <returns></returns>
        public bool Equals(EmbedVideo embedVideo)
        {
            return this.Url == embedVideo.Url &&
                this.Width == embedVideo.Width &&
                this.Height == embedVideo.Height;
        }

        /// <inheritdoc cref="EmbedVideo.Equals(EmbedVideo)"/>
        public bool Equals([NotNullWhen(true)] EmbedVideo? embedVideo)
        {
            if (!embedVideo.HasValue)
            {
                return false;
            }
            return this.Equals(embedVideo.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(Width, Height, Url);
    }
}
