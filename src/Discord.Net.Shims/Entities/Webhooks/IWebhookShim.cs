namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IWebhook"/>
    /// </summary>
    public interface IWebhookShim : IWebhook, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IWebhook.Token"/>
        new string Token { get; set; }

        /// <inheritdoc cref="IWebhook.Name"/>
        new string? Name { get; set; }

        /// <inheritdoc cref="IWebhook.AvatarId"/>
        new string? AvatarId { get; set; }

        /// <inheritdoc cref="IWebhook.Channel"/>
        new IIntegrationChannelShim Channel { get; }

        /// <inheritdoc cref="IWebhook.Guild"/>
        new IGuildShim Guild { get; }

        /// <inheritdoc cref="IWebhook.Creator"/>
        new IUserShim? Creator { get; set; }
    }
}
