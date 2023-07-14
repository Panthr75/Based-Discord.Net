using System;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="Emoji"/>
    /// </summary>
    public class EmojiShim : IEmoteShim, IEquatable<Emoji>, IEquatable<EmojiShim>, IComparable<Emoji>, IComparable<EmojiShim>, IConvertibleShim<Emoji>
#if NET7_0_OR_GREATER
        , System.Numerics.IEqualityOperators<EmojiShim, EmojiShim, bool>
#endif
    {
        private string m_name;

        /// <inheritdoc cref="Emoji.Name"/>
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                if (value is null)
                {
                    this.m_name = string.Empty;
                    return;
                }

                this.m_name = value;
            }
        }

        public EmojiShim()
        {
            this.m_name = string.Empty;
        }

        public EmojiShim(Emoji emoji) : this()
        {
            this.Apply(emoji);
        }

        void IConvertibleShim<IEmote>.Apply(IEmote value)
        {
            if (value is not Emoji emoji)
            {
                return;
            }
            this.Apply(emoji);
        }

        /// <inheritdoc/>
        public void Apply(Emoji value)
        {
            if (value is null)
            {
                return;
            }

            this.Name = value.Name;
        }

        /// <inheritdoc/>
        public Emoji UnShim()
        {
            return new Emoji(this.Name);
        }

        IEmote IConvertibleShim<IEmote>.UnShim() => this.UnShim();


        /// <inheritdoc cref="Emoji.ToString"/>
        public override string ToString() => this.Name;

        /// <inheritdoc cref="Emoji.CompareTo(Emoji)"/>
        public int CompareTo(EmojiShim? other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.Name.CompareTo(other.Name);
        }

        /// <inheritdoc cref="Emoji.CompareTo(Emoji)"/>
        public int CompareTo(Emoji? other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.Name.CompareTo(other.Name);
        }

        bool IEquatable<IEmote>.Equals([NotNullWhen(true)] IEmote? other)
        {
            return other is Emoji emoji && this.Equals(emoji);
        }

        bool IEquatable<IEmoteShim>.Equals([NotNullWhen(true)] IEmoteShim? other)
        {
            return other is EmojiShim emoji && this.Equals(emoji);
        }

        /// <inheritdoc cref="Emoji.Equals(object)"/>
        public override bool Equals([NotNullWhen(true)] object? other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(other, this))
                return true;

            if (other is Emoji emojiShim)
            {
                return this.Equals(emojiShim);
            }
            if (other is Emoji emoji)
            {
                return this.Equals(emoji);
            }
            return false;
        }

        /// <inheritdoc cref="Emoji.Equals(Emoji)"/>
        public bool Equals([NotNullWhen(true)] Emoji? other)
        {
            return other != null && other.Name == this.Name;
        }

        /// <inheritdoc cref="Emoji.Equals(Emoji)"/>
        public bool Equals([NotNullWhen(true)] EmojiShim? other)
        {
            return other != null && other.Name == this.Name;
        }

        /// <inheritdoc cref="Emoji.GetHashCode"/>
        public override int GetHashCode() => this.Name.GetHashCode();

        public static implicit operator Emoji(EmojiShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmojiShim? a, EmojiShim? b)
        {
            return a is null ?
                b is null :
                a.Equals(b);
        }
        public static bool operator !=(EmojiShim? a, EmojiShim? b)
        {
            return !(a == b);
        }
    }
}
