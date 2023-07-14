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
    /// A shim of <see cref="EmbedAuthor"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedAuthorShim : IConvertibleShim<EmbedAuthor>, IEquatable<EmbedAuthorShim>, IEquatable<EmbedAuthor>
    {
        private string m_name;
        private string? m_url;
        private string? m_iconUrl;
        private string? m_proxyIconUrl;

        public EmbedAuthorShim(string name)
        {
            Preconditions.NotNull(name, nameof(name));

            this.m_name = string.Empty;
            this.Name = name;
        }

        public EmbedAuthorShim(EmbedAuthor embedAuthor)
        {
            this.m_name = string.Empty;
            this.Apply(embedAuthor);
        }

        public void Apply(EmbedAuthor value)
        {
            this.m_iconUrl = value.IconUrl;
            this.m_name = value.Name;
            this.m_proxyIconUrl = value.ProxyIconUrl;
            this.m_url = value.Url;
        }

        public EmbedAuthor UnShim()
        {
            return new EmbedAuthor(this.Name, this.Url, this.IconUrl, this.ProxyIconUrl);
        }

        /// <inheritdoc cref="EmbedAuthor.Name"/>
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                value = value.Trim();
                this.m_name = value.Substring(0, Math.Min(value.Length, EmbedAuthorBuilder.MaxAuthorNameLength));
            }
        }
        /// <inheritdoc cref="EmbedAuthor.Url"/>
        public virtual string? Url
        {
            get => this.m_url;
            set
            {
                UrlValidation.Validate(value);
                this.m_url = value;
            }
        }
        /// <inheritdoc cref="EmbedAuthor.IconUrl"/>
        public virtual string? IconUrl
        {
            get => this.m_iconUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_iconUrl = value;
            }
        }
        /// <inheritdoc cref="EmbedAuthor.ProxyIconUrl"/>
        public virtual string? ProxyIconUrl
        {
            get => this.m_proxyIconUrl;
            set
            {
                UrlValidation.Validate(value);
                this.m_proxyIconUrl = value;
            }
        }


        private string DebuggerDisplay => $"{Name} ({Url})";
        /// <inheritdoc cref="EmbedAuthor.ToString"/>
        public override string ToString() => Name;

        public static implicit operator EmbedAuthor(EmbedAuthorShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmbedAuthorShim? left, EmbedAuthorShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedAuthorShim? left, EmbedAuthorShim? right)
            => !(left == right);

        /// <inheritdoc cref="EmbedAuthor.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedAuthorShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedAuthor other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedAuthor.Equals(EmbedAuthor)"/>
        public bool Equals(EmbedAuthor embedAuthor)
        {
            return this.Name == embedAuthor.Name &&
                this.Url == embedAuthor.Url &&
                this.IconUrl == embedAuthor.IconUrl &&
                this.ProxyIconUrl == embedAuthor.ProxyIconUrl;
        }

        /// <inheritdoc cref="EmbedAuthor.Equals(EmbedAuthor?)"/>
        public bool Equals([NotNullWhen(true)] EmbedAuthor? embedAuthor)
        {
            if (!embedAuthor.HasValue)
            {
                return false;
            }
            return this.Equals(embedAuthor.Value);
        }
        /// <inheritdoc cref="EmbedAuthor.Equals(EmbedAuthor)"/>
        public bool Equals([NotNullWhen(true)] EmbedAuthorShim? embedAuthor)
        {
            return embedAuthor != null &&
                this.Name == embedAuthor.Name &&
                this.Url == embedAuthor.Url &&
                this.IconUrl == embedAuthor.IconUrl &&
                this.ProxyIconUrl == embedAuthor.ProxyIconUrl;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.Url ?? string.Empty, this.IconUrl ?? string.Empty, this.ProxyIconUrl ?? string.Empty);
    }
}
