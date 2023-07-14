using System;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IVoiceState"/>
    /// </summary>
    public interface IVoiceStateShim : IVoiceState
    {
        /// <inheritdoc cref="IVoiceState.IsDeafened"/>
        new bool IsDeafened { get; set; }

        /// <inheritdoc cref="IVoiceState.IsMuted"/>
        new bool IsMuted { get; set; }

        /// <inheritdoc cref="IVoiceState.IsSelfDeafened"/>
        new bool IsSelfDeafened { get; set; }

        /// <inheritdoc cref="IVoiceState.IsSelfMuted"/>
        new bool IsSelfMuted { get; set; }

        /// <inheritdoc cref="IVoiceState.IsSuppressed"/>
        new bool IsSuppressed { get; set; }

        /// <inheritdoc cref="IVoiceState.VoiceChannel"/>
        new IVoiceChannelShim? VoiceChannel { get; set; }

        /// <inheritdoc cref="IVoiceState.VoiceSessionId"/>
        new string? VoiceSessionId { get; set; }

        /// <inheritdoc cref="IVoiceState.IsStreaming"/>
        new bool IsStreaming { get; set; }

        /// <inheritdoc cref="IVoiceState.IsVideoing"/>
        new bool IsVideoing { get; set; }

        /// <inheritdoc cref="IVoiceState.RequestToSpeakTimestamp"/>
        new DateTimeOffset? RequestToSpeakTimestamp { get; set; }
    }
}
