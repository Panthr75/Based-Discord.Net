namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IAudioChannel"/>
    /// </summary>
    public interface IAudioChannelShim : IAudioChannel, IChannelShim
    {
        /// <inheritdoc cref="IAudioChannel.RTCRegion"/>
        new string? RTCRegion { get; set; }
    }
}
