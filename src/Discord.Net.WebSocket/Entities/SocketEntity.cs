using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Discord.WebSocket
{
    public abstract class SocketEntity<T, TSelf> : SocketEntity<T>, IEquatable<TSelf>
#if NET7_0_OR_GREATER
        , System.Numerics.IEqualityOperators<TSelf, TSelf, bool>
#endif
        where T : IEquatable<T>
        where TSelf : SocketEntity<T, TSelf>
    {
        internal SocketEntity(DiscordSocketClient discord, T id) : base(discord, id)
        { }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is TSelf self)
            {
                return this.Equals(self);
            }
            return base.Equals(obj);
        }

        public sealed override bool Equals([NotNullWhen(true)] SocketEntity<T>? other)
        {
            if (other is TSelf self)
            {
                return this.Equals(self);
            }
            return false;
        }

        public virtual bool Equals([NotNullWhen(true)] TSelf? other)
        {
            return other is not null &&
                other.Id.Equals(this.Id);
        }

        public static bool operator ==(SocketEntity<T, TSelf>? a, SocketEntity<T, TSelf>? b)
        {
            return a is null ? b is null : a.Equals(b);
        }

        public static bool operator !=(SocketEntity<T, TSelf>? a, SocketEntity<T, TSelf>? b)
        {
            return !(a == b);
        }

#if NET7_0_OR_GREATER
        static bool IEqualityOperators<TSelf, TSelf, bool>.operator ==(TSelf? left, TSelf? right) => left == right;

        static bool IEqualityOperators<TSelf, TSelf, bool>.operator !=(TSelf? left, TSelf? right) => left != right;
#endif
    }

    public abstract class SocketEntity<T> : IEntity<T>, IEquatable<SocketEntity<T>>
        where T : IEquatable<T>
    {
        internal DiscordSocketClient Discord { get; }
        /// <inheritdoc />
        public T Id { get; }

        internal SocketEntity(DiscordSocketClient discord, T id)
        {
            Discord = discord;
            Id = id;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is SocketEntity<T> otherEntity)
            {
                return otherEntity.Equals(this);
            }

            return false;
        }

        public virtual bool Equals([NotNullWhen(true)] SocketEntity<T>? other)
        {
            return other is not null &&
                other.Id.Equals(this.Id);
        }

        public override int GetHashCode() => this.Id.GetHashCode();
    }
}
