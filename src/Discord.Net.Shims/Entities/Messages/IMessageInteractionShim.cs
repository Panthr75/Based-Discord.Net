namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IMessageInteraction"/>
    /// </summary>
    public interface IMessageInteractionShim : IMessageInteraction
    {
        /// <inheritdoc cref="IMessageInteraction.Id"/>
        new ulong Id { get; set; }

        /// <inheritdoc cref="IMessageInteraction.Type"/>
        new InteractionType Type { get; set; }

        /// <inheritdoc cref="IMessageInteraction.Name"/>
        new string Name { get; set; }

        /// <inheritdoc cref="IMessageInteraction.User"/>
        new IUserShim User { get; set; }
    }
}
