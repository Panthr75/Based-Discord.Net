namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IChannel"/>
    /// </summary>
    public interface IChannelShim : IChannel, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IChannel.Name"/>
        new string Name { get; set; }
    }
}
