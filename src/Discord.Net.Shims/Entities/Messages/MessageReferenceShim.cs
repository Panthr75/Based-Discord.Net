using System.Diagnostics;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="MessageReference"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class MessageReferenceShim : IConvertibleShim<MessageReference>
    {
        private Optional<ulong> m_messageId;
        private Optional<ulong> m_channelId;
        private Optional<ulong> m_guildId;
        private Optional<bool> m_failIfNotExists;

        /// <inheritdoc cref="MessageReference.MessageId"/>
        public virtual Optional<ulong> MessageId
        {
            get => this.m_messageId;
            set => this.m_messageId = value;
        }


        /// <inheritdoc cref="MessageReference.ChannelId"/>
        public virtual Optional<ulong> ChannelId
        {
            get => this.m_channelId;
            set => this.m_channelId = value;
        }

        /// <inheritdoc cref="MessageReference.GuildId"/>
        public virtual Optional<ulong> GuildId
        {
            get => this.m_guildId;
            set => this.m_guildId = value;
        }

        /// <inheritdoc cref="MessageReference.FailIfNotExists"/>
        public virtual Optional<bool> FailIfNotExists
        {
            get => this.m_failIfNotExists;
            set => this.m_failIfNotExists = value;
        }

        public MessageReferenceShim()
        {
        }

        public MessageReferenceShim(MessageReference messageReference)
        {
            this.Apply(messageReference);
        }

        public MessageReference UnShim()
        {
            return new MessageReference(
                messageId: this.MessageId.ToNullable(),
                channelId: this.ChannelId.ToNullable(),
                guildId: this.GuildId.ToNullable(),
                failIfNotExists: this.FailIfNotExists.ToNullable());
        }

        public void Apply(MessageReference value)
        {
            if (value is null)
            {
                return;
            }

            this.m_failIfNotExists = value.FailIfNotExists;
            this.m_channelId = value.InternalChannelId;
            this.m_guildId = value.GuildId;
            this.m_messageId = value.MessageId;
        }

        private string DebuggerDisplay
            => $"Channel ID: ({ChannelId}){(GuildId.IsSpecified ? $", Guild ID: ({GuildId.Value})" : "")}" +
            $"{(MessageId.IsSpecified ? $", Message ID: ({MessageId.Value})" : "")}" +
            $"{(FailIfNotExists.IsSpecified ? $", FailIfNotExists: ({FailIfNotExists.Value})" : "")}";

        public override string ToString()
            => DebuggerDisplay;

        public static implicit operator MessageReference(MessageReferenceShim v)
        {
            return v.UnShim();
        }
    }
}
