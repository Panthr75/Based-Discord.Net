using System;

namespace Discord
{
    public interface IEntity<TId>
        where TId : IEquatable<TId>
    {

        /// <summary>
        ///     Gets the unique identifier for this object.
        /// </summary>
        TId Id { get; }

    }
}
