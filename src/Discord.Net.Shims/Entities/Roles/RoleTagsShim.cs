namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="RoleTags"/>.
    /// </summary>
    public class RoleTagsShim : IConvertibleShim<RoleTags>
    {
        private ulong? m_botId;
        private ulong? m_integrationId;
        private bool m_isPremiumSubscriberRole;

        public RoleTagsShim()
        { }

        public RoleTagsShim(RoleTags roleTags)
        {
            this.Apply(roleTags);
        }

        /// <inheritdoc cref="RoleTags.BotId"/>
        public virtual ulong? BotId
        {
            get => this.m_botId;
            set => this.m_botId = value;
        }
        /// <inheritdoc cref="RoleTags.IntegrationId"/>
        public virtual ulong? IntegrationId
        {
            get => this.m_integrationId;
            set => this.m_integrationId = value;
        }
        /// <inheritdoc cref="RoleTags.IsPremiumSubscriberRole"/>
        public virtual bool IsPremiumSubscriberRole
        {
            get => this.m_isPremiumSubscriberRole;
            set => this.m_isPremiumSubscriberRole = value;
        }

        /// <inheritdoc/>
        public void Apply(RoleTags value)
        {
            if (value is null)
            {
                return;
            }

            this.m_botId = value.BotId;
            this.m_integrationId = value.IntegrationId;
            this.m_isPremiumSubscriberRole = value.IsPremiumSubscriberRole;
        }

        /// <inheritdoc/>
        public RoleTags UnShim()
        {
            return new RoleTags(this.BotId, this.IntegrationId, this.IsPremiumSubscriberRole);
        }

        public static implicit operator RoleTags(RoleTagsShim v)
        {
            return v.UnShim();
        }
    }
}
