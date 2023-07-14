namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IDMChannel"/>
    /// </summary>
    public interface IDMChannelShim : IDMChannel, IMessageChannelShim, IPrivateChannelShim
    {
        /// <inheritdoc cref="IDMChannel.Recipient"/>
        new IUserShim Recipient { get; set; }
    }
}
