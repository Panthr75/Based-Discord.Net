using System;

namespace Discord.Shims
{
    /// <summary>
    /// Represents an shimmable entity, which will always
    /// have a Client property.
    /// </summary>
    /// <typeparam name="TId">The id property</typeparam>
    public interface IShimEntity<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// The client of this entity.
        /// </summary>
        IDiscordClientShim Client { get; }

        /// <inheritdoc cref="IEntity{TId}.Id"/>
        new TId Id { get; set; }
    }
}
