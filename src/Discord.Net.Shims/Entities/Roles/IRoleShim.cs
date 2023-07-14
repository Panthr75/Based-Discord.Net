namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IRole"/>
    /// </summary>
    public interface IRoleShim : IRole, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IRole.Guild"/>
        new IGuildShim Guild { get; }

        /// <inheritdoc cref="IRole.Color"/>
        new Color Color { get; set; }

        /// <inheritdoc cref="IRole.IsHoisted"/>
        new bool IsHoisted { get; set; }

        /// <inheritdoc cref="IRole.IsManaged"/>
        new bool IsManaged { get; set; }

        /// <inheritdoc cref="IRole.IsMentionable"/>
        new bool IsMentionable { get; set; }

        /// <inheritdoc cref="IRole.Name"/>
        new string Name { get; set; }

        /// <inheritdoc cref="IRole.Icon"/>
        new string? Icon { get; set; }

        /// <inheritdoc cref="IRole.Emoji"/>
        new EmojiShim? Emoji { get; set; }

        /// <inheritdoc cref="IRole.Permissions"/>
        new GuildPermissions Permissions { get; set; }

        /// <inheritdoc cref="IRole.Position"/>
        new int Position { get; set; }

        /// <inheritdoc cref="IRole.Tags"/>
        new RoleTagsShim? Tags { get; set; }

        /// <inheritdoc cref="IEntity{TId}.Id"/>
        new ulong Id { get; set; }
    }
}
