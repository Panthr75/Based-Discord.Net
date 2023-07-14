using Discord.Utils;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedProvider"/>
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedProviderShim : IConvertibleShim<EmbedProvider>, IEquatable<EmbedProviderShim>, IEquatable<EmbedProvider>
    {
        private string? m_name;
        private string? m_url;

        public EmbedProviderShim()
        { }

        public EmbedProviderShim(EmbedProvider embedProvider)
        {
            this.Apply(embedProvider);
        }

        public void Apply(EmbedProvider value)
        {
            this.m_name = value.Name;
            this.m_url = value.Url;
        }

        public EmbedProvider UnShim()
        {
            return new EmbedProvider(this.Name, this.Url);
        }

        /// <inheritdoc cref="EmbedProvider.Name"/>
        public virtual string? Name
        {
            get => this.m_name;
            set => this.m_name = value;
        }
        /// <inheritdoc cref="EmbedProvider.Url"/>
        public virtual string? Url
        {
            get => this.m_url;
            set
            {
                UrlValidation.Validate(value);
                this.m_url = value;
            }
        }

        private string DebuggerDisplay => $"{Name} ({Url})";
        /// <inheritdoc cref="EmbedProvider.ToString()"/>
        public override string? ToString() => Name;

        public static bool operator ==(EmbedProviderShim? left, EmbedProviderShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedProviderShim? left, EmbedProviderShim? right)
            => !(left == right);

        public static implicit operator EmbedProvider(EmbedProviderShim v)
        {
            return v.UnShim();
        }

        /// <inheritdoc cref="EmbedProvider.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedProviderShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedProvider other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedProvider.Equals(EmbedProvider)"/>
        public bool Equals(EmbedProvider embedProvider)
        {
            return this.Name == embedProvider.Name &&
                this.Url == embedProvider.Url;
        }

        /// <inheritdoc cref="EmbedProvider.Equals(EmbedProvider?)"/>
        public bool Equals([NotNullWhen(true)] EmbedProvider? embedProvider)
        {
            if (!embedProvider.HasValue)
            {
                return false;
            }
            return this.Equals(embedProvider.Value);
        }

        /// <inheritdoc cref="EmbedProvider.Equals(EmbedProvider)"/>
        public bool Equals([NotNullWhen(true)] EmbedProviderShim? embedProvider)
        {
            return embedProvider != null &&
                this.Name == embedProvider.Name &&
                this.Url == embedProvider.Url;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => (this.Name, this.Url).GetHashCode();
    }
}
