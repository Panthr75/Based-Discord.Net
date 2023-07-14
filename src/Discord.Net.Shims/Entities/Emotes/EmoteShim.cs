using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="Emote"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class EmoteShim : IEmoteShim, ISnowflakeEntity, IEquatable<Emote>, IEquatable<EmoteShim>, IComparable<Emote>, IComparable<EmoteShim>, IConvertibleShim<Emote>
#if NET7_0_OR_GREATER
        , System.Numerics.IEqualityOperators<EmoteShim, EmoteShim, bool>
#endif
    {
        private string m_name;

        /// <inheritdoc cref="Emote.Name"/>
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

                value = value.Trim().Replace(" ", "_");
                this.m_name = value.Substring(0, Math.Min(value.Length, 32));
            }
        }
        /// <inheritdoc cref="Emote.Id"/>
        public virtual ulong Id { get; set; }
        /// <inheritdoc cref="Emote.Animated"/>
        public virtual bool Animated { get; set; }
        /// <inheritdoc cref="Emote.CreatedAt"/>
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);
        /// <inheritdoc cref="Emote.Url"/>
        public string Url => CDN.GetEmojiUrl(this.Id, this.Animated);

        public EmoteShim()
        {
            this.m_name = string.Empty;
            this.Id = ShimUtils.GenerateSnowflake();
        }

        public EmoteShim(Emote emote)
        {
            Preconditions.NotNull(emote, nameof(emote));
            this.m_name = string.Empty;
            this.Apply(emote);
        }

        internal virtual void ApplyImpl(Emote value)
        {
            if (value is null)
            {
                return;
            }

            this.Id = value.Id;
            this.Name = value.Name;
            this.Animated = value.Animated;
        }

        void IConvertibleShim<IEmote>.Apply(IEmote value)
        {
            if (value is not Emote emote)
            {
                return;
            }
            this.Apply(emote);
        }

        /// <inheritdoc/>
        public void Apply(Emote value)
        {
            this.ApplyImpl(value);
        }

        internal virtual Emote UnShimImpl()
        {
            return new Emote(this.Id, this.Name, this.Animated);
        }

        /// <inheritdoc/>
        public Emote UnShim()
        {
            return this.UnShimImpl();
        }


        IEmote IConvertibleShim<IEmote>.UnShim() => this.UnShimImpl();
        Emote IConvertibleShim<Emote>.UnShim() => this.UnShimImpl();

        bool IEquatable<IEmoteShim>.Equals([NotNullWhen(true)] IEmoteShim? other)
        {
            return other is EmoteShim emote && this.Equals(emote);
        }
        bool IEquatable<IEmote>.Equals([NotNullWhen(true)] IEmote? other)
        {
            return other is Emote emote && this.Equals(emote);
        }

        /// <inheritdoc cref="Emote.Equals(object)"/>
        public override bool Equals([NotNullWhen(true)] object? other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(other, this))
                return true;

            if (other is EmoteShim otherShim)
            {
                return this.Equals(otherShim);
            }
            if (other is Emote otherEmote)
            {
                return this.Equals(otherEmote);
            }
            return false;
        }

        /// <inheritdoc cref="Emote.CompareTo(Emote)"/>
        public int CompareTo(EmoteShim? other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.Id.CompareTo(other.Id);
        }

        /// <inheritdoc cref="Emote.CompareTo(Emote)"/>
        public int CompareTo(Emote? other)
        {
            if (other is null)
            {
                return 1;
            }

            return this.Id.CompareTo(other.Id);
        }

        /// <inheritdoc cref="Emote.GetHashCode"/>
        public override int GetHashCode()
            => this.Id.GetHashCode();

        private string DebuggerDisplay => $"{this.Name} ({this.Id})";

        /// <inheritdoc cref="Emote.ToString"/>
        public override string ToString() => $"<{(this.Animated ? "a" : "")}:{this.Name}:{this.Id}>";

        /// <inheritdoc cref="Emote.Equals(Emote)"/>
        public bool Equals([NotNullWhen(true)] EmoteShim? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        /// <inheritdoc cref="Emote.Equals(Emote)"/>
        public bool Equals([NotNullWhen(true)] Emote? other)
        {
            if (other is null)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public static implicit operator Emote(EmoteShim v)
        {
            return v.UnShim();
        }


        public static bool operator ==(EmoteShim? a, EmoteShim? b)
        {
            return a is null ?
                b is null :
                a.Equals(b);
        }
        public static bool operator !=(EmoteShim? a, EmoteShim? b)
        {
            return !(a == b);
        }
    }
}
