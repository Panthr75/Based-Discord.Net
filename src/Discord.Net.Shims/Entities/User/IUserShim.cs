namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IUser"/>
    /// </summary>
    public interface IUserShim : IUser, IPresenceShim, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IUser.AvatarId"/>
        new string? AvatarId { get; set; }

        /// <inheritdoc cref="IUser.Discriminator"/>
        new string Discriminator { get; set; }

        /// <inheritdoc cref="IUser.DiscriminatorValue"/>
        new ushort DiscriminatorValue { get; set; }

        /// <inheritdoc cref="IUser.IsBot"/>
        new bool IsBot { get; set; }

        /// <inheritdoc cref="IUser.Username"/>
        new string Username { get; set; }

        /// <inheritdoc cref="IUser.PublicFlags"/>
        new UserProperties? PublicFlags { get; set; }

        /// <inheritdoc cref="IUser.GlobalName"/>
        new string? GlobalName { get; set; }

        /// <inheritdoc cref="IUser.Pronouns"/>
        new string? Pronouns { get; set; }
    }
}
