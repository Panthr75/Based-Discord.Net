using System.Diagnostics;

namespace Discord.Shims
{
    /// <summary>
    /// A shim for <see cref="MessageActivity"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class MessageActivityShim : IConvertibleShim<MessageActivity>
    {
        private MessageActivityType m_type;
        private string? m_partyId;

        public MessageActivityShim()
        { }

        public MessageActivityShim(MessageActivity messageActivity)
        {
            Preconditions.NotNull(messageActivity, nameof(messageActivity));
            this.Apply(messageActivity);
        }

        public void Apply(MessageActivity value)
        {
            if (value is null)
            {
                return;
            }

            this.m_type = value.Type;
            this.m_partyId = value.PartyId;
        }

        public MessageActivity UnShim()
        {
            return new MessageActivity()
            {
                Type = this.Type,
                PartyId = this.PartyId,
            };
        }

        /// <inheritdoc cref="MessageActivity.Type"/>
        public virtual MessageActivityType Type
        {
            get => this.m_type;
            set => this.m_type = value;
        }
        /// <inheritdoc cref="MessageActivity.PartyId"/>
        public virtual string? PartyId
        {
            get => this.m_partyId;
            set => this.m_partyId = value;
        }

        private string DebuggerDisplay
            => $"{Type}{(string.IsNullOrWhiteSpace(PartyId) ? "" : $" {PartyId}")}";

        /// <inheritdoc cref="MessageActivity.ToString"/>
        public override string ToString() => DebuggerDisplay;

        public static implicit operator MessageActivity(MessageActivityShim v)
        {
            return v.UnShim();
        }
    }

}
