using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="GuildEmote"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class GuildEmoteShim : EmoteShim, IConvertibleShim<GuildEmote>
    {
        private readonly List<ulong> m_roleIds;
        private bool m_isManaged;
        private bool m_requireColons;
        private ulong? m_creatorId;

        /// <inheritdoc cref="GuildEmote.IsManaged"/>
        public virtual bool IsManaged
        {
            get => this.m_isManaged;
            set => this.m_isManaged = value;
        }
        /// <inheritdoc cref="GuildEmote.RequireColons"/>
        public virtual bool RequireColons
        {
            get => this.m_requireColons;
            set => this.m_requireColons = value;
        }
        /// <inheritdoc cref="GuildEmote.RoleIds"/>
        public virtual ICollection<ulong> RoleIds => this.m_roleIds;
        /// <inheritdoc cref="GuildEmote.CreatorId"/>
        public virtual ulong? CreatorId
        {
            get => this.m_creatorId;
            set => this.m_creatorId = value;
        }

        public GuildEmoteShim() : base()
        {
            this.m_roleIds = new();
        }

        public GuildEmoteShim(GuildEmote emote) : base(emote)
        {
            this.m_roleIds = new();
            this.Apply(emote);
        }

        internal override void ApplyImpl(Emote value)
        {
            if (value is not GuildEmote emote)
            {
                return;
            }

            this.Apply(emote);
        }

        /// <inheritdoc/>
        public void Apply(GuildEmote value)
        {
            if (value is null)
            {
                return;
            }

            base.ApplyImpl(value);
            this.m_isManaged = value.IsManaged;
            this.m_requireColons = value.RequireColons;
            this.m_roleIds.Clear();
            this.m_roleIds.AddRange(value.RoleIds);

        }

        internal override Emote UnShimImpl()
        {
            return this.UnShim();
        }

        /// <inheritdoc/>
        new public GuildEmote UnShim()
        {
            HashSet<ulong> roleIds = new HashSet<ulong>(this.RoleIds);

            return new GuildEmote(
                id: this.Id,
                name: this.Name,
                animated: this.Animated,
                isManaged: this.IsManaged,
                requireColons: this.RequireColons,
                roleIds: roleIds.ToImmutableArray(),
                userId: this.CreatorId);
        }

        private string DebuggerDisplay => $"{this.Name} ({this.Id})";
        /// <inheritdoc cref="GuildEmote.ToString"/>
        public override string ToString() => $"<{(this.Animated ? "a" : "")}:{this.Name}:{this.Id}>";

        public static implicit operator GuildEmote(GuildEmoteShim v)
        {
            return v.UnShim();
        }
    }
}
