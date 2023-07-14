using System.Collections.Generic;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="GuildFeatures"/>
    /// </summary>
    public class GuildFeaturesShim : IConvertibleShim<GuildFeatures>
    {
        private readonly List<string> m_experimental;
        private GuildFeature m_value;

        public GuildFeaturesShim()
        {
            this.m_experimental = new();
        }
        public GuildFeaturesShim(GuildFeatures features) : this()
        {
            this.Apply(features);
        }

        public void Apply(GuildFeatures value)
        {
            if (value is null)
            {
                return;
            }

            this.m_value = value.Value;
            this.m_experimental.Clear();
            this.m_experimental.AddRange(value.Experimental);
        }

        public GuildFeatures UnShim()
        {
            return new GuildFeatures(this.m_value, this.m_experimental.ToArray());
        }

        /// <inheritdoc cref="GuildFeatures.Value"/>
        public virtual GuildFeature Value
        {
            get => this.m_value;
            set => this.m_value = value;
        }

        /// <inheritdoc cref="GuildFeatures.Experimental"/>
        public virtual ICollection<string> Experimental => this.m_experimental;

        /// <inheritdoc cref="GuildFeatures.HasThreads"/>
        public virtual bool HasThreads
        {
            get => this.HasFeatureImpl(GuildFeature.ThreadsEnabled | GuildFeature.ThreadsEnabledTesting);
            set => this.SetFeatureImpl(GuildFeature.ThreadsEnabled | GuildFeature.ThreadsEnabledTesting, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasTextInVoice"/>
        public virtual bool HasTextInVoice
        {
            get => this.HasFeatureImpl(GuildFeature.TextInVoiceEnabled);
            set => this.SetFeatureImpl(GuildFeature.TextInVoiceEnabled, value);
        }

        /// <inheritdoc cref="GuildFeatures.IsStaffServer"/>
        public virtual bool IsStaffServer
        {
            get => this.HasFeatureImpl(GuildFeature.InternalEmployeeOnly);
            set => this.SetFeatureImpl(GuildFeature.InternalEmployeeOnly, value);
        }

        /// <inheritdoc cref="GuildFeatures.IsHub"/>
        public virtual bool IsHub
        {
            get => this.HasFeatureImpl(GuildFeature.Hub);
            set => this.SetFeatureImpl(GuildFeature.Hub, value);
        }

        /// <inheritdoc cref="GuildFeatures.IsLinkedToHub"/>
        public virtual bool IsLinkedToHub
        {
            get => this.HasFeatureImpl(GuildFeature.LinkedToHub);
            set => this.SetFeatureImpl(GuildFeature.LinkedToHub, value);
        }

        /// <inheritdoc cref="GuildFeatures.IsPartnered"/>
        public virtual bool IsPartnered
        {
            get => this.HasFeatureImpl(GuildFeature.Partnered);
            set => this.SetFeatureImpl(GuildFeature.Partnered, value);
        }

        /// <inheritdoc cref="GuildFeatures.IsVerified"/>
        public virtual bool IsVerified
        {
            get => this.HasFeatureImpl(GuildFeature.Verified);
            set => this.SetFeatureImpl(GuildFeature.Verified, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasVanityUrl"/>
        public virtual bool HasVanityUrl
        {
            get => this.HasFeatureImpl(GuildFeature.VanityUrl);
            set => this.SetFeatureImpl(GuildFeature.VanityUrl, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasRoleSubscriptions"/>
        public virtual bool HasRoleSubscriptions
        {
            get => this.HasFeatureImpl(GuildFeature.RoleSubscriptionsEnabled | GuildFeature.RoleSubscriptionsAvailableForPurchase);
            set => this.SetFeatureImpl(GuildFeature.RoleSubscriptionsEnabled | GuildFeature.RoleSubscriptionsAvailableForPurchase, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasRoleIcons"/>
        public virtual bool HasRoleIcons
        {
            get => this.HasFeatureImpl(GuildFeature.RoleIcons);
            set => this.SetFeatureImpl(GuildFeature.RoleIcons, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasPrivateThreads"/>
        public virtual bool HasPrivateThreads
        {
            get => this.HasFeatureImpl(GuildFeature.PrivateThreads);
            set => this.SetFeatureImpl(GuildFeature.PrivateThreads, value);
        }

        /// <inheritdoc cref="GuildFeatures.HasFeature(GuildFeature)"/>
        public virtual bool HasFeature(GuildFeature feature)
            => this.HasFeatureImpl(feature);

        private bool HasFeatureImpl(GuildFeature feature)
            => this.Value.HasFlag(feature);

        /// <inheritdoc cref="GuildFeatures.HasFeature(string)"/>
        public virtual bool HasFeature(string feature)
            => this.HasFeatureImpl(feature);
        private bool HasFeatureImpl(string feature)
            => this.Experimental.Contains(feature);

        /// <summary>
        /// Sets whether the guild has the specified feature(s).
        /// </summary>
        /// <param name="feature">The guild feature(s).</param>
        /// <param name="value">
        /// Whether or not they should be added, or removed.
        /// </param>
        public virtual void SetFeature(GuildFeature feature, bool value)
            => this.SetFeatureImpl(feature, value);
        private void SetFeatureImpl(GuildFeature feature, bool value)
        {
            if (value)
            {
                this.IncludeFeature(feature);
            }
            else
            {
                this.RemoveFeature(feature);
            }
        }

        /// <summary>
        /// Removes the specified feature(s) from the guild.
        /// </summary>
        /// <param name="feature">The guild feature(s) to remove.</param>
        public void RemoveFeature(GuildFeature feature)
            => this.RemoveFeatureImpl(feature);
        private void RemoveFeatureImpl(GuildFeature feature)
        {
            this.m_value &= ~feature;
        }

        /// <summary>
        /// Adds the specified feature(s) to the guild.
        /// </summary>
        /// <param name="feature">The guild feature(s) to add.</param>
        public virtual void IncludeFeature(GuildFeature feature)
            => this.IncludeFeatureImpl(feature);
        private void IncludeFeatureImpl(GuildFeature feature)
        {
            this.m_value = (this.m_value & ~feature) | feature;
        }

        /// <summary>
        /// Sets whether the guild has the specified feature.
        /// </summary>
        /// <param name="feature">The guild feature.</param>
        /// <param name="value">
        /// Whether or not they should be added, or removed.
        /// </param>
        public virtual void SetFeature(string feature, bool value)
            => this.SetFeatureImpl(feature, value);
        private void SetFeatureImpl(string feature, bool value)
        {
            if (value)
            {
                this.IncludeFeature(feature);
            }
            else
            {
                this.RemoveFeature(feature);
            }
        }

        /// <summary>
        /// Removes the specified feature from the guild.
        /// </summary>
        /// <param name="feature">The guild feature to remove.</param>
        public virtual void RemoveFeature(string feature)
            => this.RemoveFeatureImpl(feature);
        private void RemoveFeatureImpl(string feature)
        {
            if (feature == null)
            {
                return;
            }

            this.m_experimental.Remove(feature);
        }

        /// <summary>
        /// Adds the specified feature to the guild.
        /// </summary>
        /// <param name="feature">The guild feature to add.</param>
        public virtual void IncludeFeature(string feature)
            => this.IncludeFeatureImpl(feature);

        private void IncludeFeatureImpl(string feature)
        {
            if (feature == null)
            {
                return;
            }

            if (!this.m_experimental.Contains(feature))
            {
                this.m_experimental.Add(feature);
            }
        }

        public static implicit operator GuildFeatures(GuildFeaturesShim v)
        {
            return v.UnShim();
        }
    }
}
