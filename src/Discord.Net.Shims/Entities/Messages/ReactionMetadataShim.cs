using System.Diagnostics;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="ReactionMetadata"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class ReactionMetadataShim : IConvertibleShim<ReactionMetadata>
    {
        private bool m_isMe;
        private int m_reactionCount;

        private string DebuggerDisplay => $"{(this.m_isMe ? "Me " : string.Empty)}({this.m_reactionCount})";

        public ReactionMetadataShim()
        { }

        public ReactionMetadataShim(ReactionMetadata reactionMetadata)
        {
            this.Apply(reactionMetadata);
        }

        public ReactionMetadata UnShim()
        {
            return new ReactionMetadata()
            {
                IsMe = this.IsMe,
                ReactionCount = this.ReactionCount
            };
        }

        public void Apply(ReactionMetadata value)
        {
            this.m_isMe = value.IsMe;
            this.m_reactionCount = value.ReactionCount;
        }

        /// <summary>
        ///     Gets the number of reactions.
        /// </summary>
        /// <returns>
        ///     An <see cref="int"/> representing the number of this reactions that has been added to this message.
        /// </returns>
        public virtual int ReactionCount
        {
            get => this.m_reactionCount;
            set => this.m_reactionCount = value;
        }

        /// <summary>
        ///     Gets a value that indicates whether the current user has reacted to this.
        /// </summary>
        /// <returns>
        ///     <see langword="true" /> if the user has reacted to the message; otherwise <see langword="false" />.
        /// </returns>
        public virtual bool IsMe
        {
            get => this.m_isMe;
            set => this.m_isMe = value;
        }

        public static implicit operator ReactionMetadata(ReactionMetadataShim v)
        {
            return v.UnShim();
        }
    }
}
