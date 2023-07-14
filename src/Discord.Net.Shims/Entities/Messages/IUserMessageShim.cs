namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IUserMessage"/>
    /// </summary>
    public interface IUserMessageShim : IMessageShim, IUserMessage
    {
        /// <inheritdoc cref="IUserMessage.ReferencedMessage"/>
        new IUserMessageShim? ReferencedMessage { get; set; }
    }
}
