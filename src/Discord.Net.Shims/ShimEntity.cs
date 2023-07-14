using System;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    public abstract class ShimEntity<T> : IShimEntity<T>, IEquatable<ShimEntity<T>>
        where T : IEquatable<T>
    {
        private T m_id;
        private IDiscordClientShim m_client;

        /// <inheritdoc/>
        public IDiscordClientShim Client => this.m_client;
        /// <inheritdoc />
        public virtual T Id
        {
            get => this.m_id;
            set => this.m_id = value;
        }

        protected ShimEntity(IDiscordClientShim client, T id)
        {
            this.m_client = client;
            this.m_id = id;
        }

        protected void ApplyFrom(IEntity<T> entity)
        {
            this.m_id = entity.Id;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is ShimEntity<T> otherEntity)
            {
                return otherEntity.Equals(this);
            }

            return false;
        }

        public virtual bool Equals([NotNullWhen(true)] ShimEntity<T>? other)
        {
            return other is not null &&
                other.Id.Equals(this.Id);
        }

        public override int GetHashCode() => this.Id.GetHashCode();
    }
}
