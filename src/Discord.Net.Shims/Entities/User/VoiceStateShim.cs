using System;

namespace Discord.Shims
{
    public class VoiceStateShim : IVoiceStateShim
    {
        private bool m_isDeafened;
        private bool m_isMuted;
        private bool m_isSelfDeafened;
        private bool m_isSelfMuted;
        private bool m_isSuppressed;
        private IVoiceChannelShim? m_voiceChannel;
        private string? m_voiceSessionId;
        private bool m_isStreaming;
        private bool m_isVideoing;
        private DateTimeOffset? m_requestToSpeakTimestamp;

        /// <inheritdoc cref="IVoiceState.IsDeafened"/>
        public virtual bool IsDeafened
        {
            get => this.m_isDeafened;
            set => this.m_isDeafened = value;
        }

        /// <inheritdoc cref="IVoiceState.IsMuted"/>
        public virtual bool IsMuted
        {
            get => this.m_isMuted;
            set => this.m_isMuted = value;
        }

        /// <inheritdoc cref="IVoiceState.IsSelfDeafened"/>
        public virtual bool IsSelfDeafened
        {
            get => this.m_isSelfDeafened;
            set => this.m_isSelfDeafened = value;
        }

        /// <inheritdoc cref="IVoiceState.IsSelfMuted"/>
        public virtual bool IsSelfMuted
        {
            get => this.m_isSelfMuted;
            set => this.m_isSelfMuted = value;
        }

        /// <inheritdoc cref="IVoiceState.IsSuppressed"/>
        public virtual bool IsSuppressed
        {
            get => this.m_isSuppressed;
            set => this.m_isSuppressed = value;
        }

        /// <inheritdoc cref="IVoiceState.VoiceChannel"/>
        public virtual IVoiceChannelShim? VoiceChannel
        {
            get => this.m_voiceChannel;
            set => this.m_voiceChannel = value;
        }

        IVoiceChannel IVoiceState.VoiceChannel => this.VoiceChannel;

        /// <inheritdoc cref="IVoiceState.VoiceSessionId"/>
        public virtual string? VoiceSessionId
        {
            get => this.m_voiceSessionId;
            set => this.m_voiceSessionId = value;
        }

        /// <inheritdoc cref="IVoiceState.IsStreaming"/>
        public virtual bool IsStreaming
        {
            get => this.m_isStreaming;
            set => this.m_isStreaming = value;
        }

        /// <inheritdoc cref="IVoiceState.IsVideoing"/>
        public virtual bool IsVideoing
        {
            get => this.m_isVideoing;
            set => this.m_isVideoing = value;
        }

        /// <inheritdoc cref="IVoiceState.RequestToSpeakTimestamp"/>
        public virtual DateTimeOffset? RequestToSpeakTimestamp
        {
            get => this.m_requestToSpeakTimestamp;
            set => this.m_requestToSpeakTimestamp = value;
        }
    }
}
