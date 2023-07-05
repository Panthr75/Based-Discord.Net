using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    ///     A author field of an <see cref="Embed"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct EmbedAuthor : IEquatable<EmbedAuthor>
    {
        /// <summary>
        ///     Gets the name of the author field.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        ///     Gets the URL of the author field.
        /// </summary>
        public string? Url { get; internal set; }
        /// <summary>
        ///     Gets the icon URL of the author field.
        /// </summary>
        public string? IconUrl { get; internal set; }
        /// <summary>
        ///     Gets the proxified icon URL of the author field.
        /// </summary>
        public string? ProxyIconUrl { get; internal set; }

        internal EmbedAuthor(string name, string? url, string? iconUrl, string? proxyIconUrl)
        {
            Name = name;
            Url = url;
            IconUrl = iconUrl;
            ProxyIconUrl = proxyIconUrl;
        }

        private string DebuggerDisplay => $"{Name} ({Url})";
        /// <summary>
        ///     Gets the name of the author field.
        /// </summary>
        /// <returns>
        ///     
        /// </returns>
        public override string ToString() => Name;

        public static bool operator ==(EmbedAuthor? left, EmbedAuthor? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedAuthor? left, EmbedAuthor? right)
            => !(left == right);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="EmbedAuthor"/>.
        /// </summary>
        /// <remarks>
        /// If the object passes is an <see cref="EmbedAuthor"/>, <see cref="Equals(EmbedAuthor?)"/> will be called to compare the 2 instances
        /// </remarks>
        /// <param name="obj">The object to compare with the current <see cref="EmbedAuthor"/></param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is EmbedAuthor embedAuthor && Equals(embedAuthor);

        /// <summary>
        /// Determines whether the specified <see cref="EmbedAuthor"/> is equal to the current <see cref="EmbedAuthor"/>
        /// </summary>
        /// <param name="embedAuthor">The <see cref="EmbedAuthor"/> to compare with the current <see cref="EmbedAuthor"/></param>
        /// <returns></returns>
        public bool Equals(EmbedAuthor embedAuthor)
        {
            return this.Name == embedAuthor.Name &&
                this.Url == embedAuthor.Url &&
                this.IconUrl == embedAuthor.IconUrl &&
                this.ProxyIconUrl == embedAuthor.ProxyIconUrl;
        }

        /// <inheritdoc cref="EmbedAuthor.Equals(EmbedAuthor)"/>
        public bool Equals([NotNullWhen(true)] EmbedAuthor? embedAuthor)
        {
            if (!embedAuthor.HasValue)
            {
                return false;
            }
            return this.Equals(embedAuthor.Value);
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.Url ?? string.Empty, this.IconUrl ?? string.Empty, this.ProxyIconUrl ?? string.Empty);
    }
}
