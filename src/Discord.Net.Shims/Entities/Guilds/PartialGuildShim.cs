using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim for <see cref="PartialGuild"/>
    /// </summary>
    public class PartialGuildShim : SnowflakeShimEntity, IConvertibleShim<PartialGuild>
    {
        private string m_name;
        private string? m_description;
        private string? m_splashId;
        private string? m_bannerId;
        private GuildFeaturesShim? m_features;
        private string? m_iconId;
        private VerificationLevel? m_verificationLevel;
        private string? m_vanityUrlCode;
        private int? m_premiumSubscriptionCount;
        private NsfwLevel? m_nsfwLevel;
        private WelcomeScreenShim? m_welcomeScreen;
        private int? m_approximateMemberCount;
        private int? m_approximatePresenceCount;


        public PartialGuildShim(IDiscordClientShim client) : base(client)
        {
            this.m_name = string.Empty;
        }

        public PartialGuildShim(IDiscordClientShim client, PartialGuild partialGuild) : base(client, 0)
        {
            this.m_name = string.Empty;
            Preconditions.NotNull(partialGuild, nameof(partialGuild));

            this.Apply(partialGuild);
        }

        public void Apply(PartialGuild value)
        {
            if (value is null)
            {
                return;
            }

            this.ApplyFrom(value);

            this.m_approximateMemberCount = value.ApproximateMemberCount;
            this.m_approximatePresenceCount = value.ApproximatePresenceCount;
            this.m_bannerId = value.BannerId;
            this.m_description = value.Description;
            this.m_features = this.ShimFeatures(value.Features);
            this.m_iconId = value.IconId;
            this.m_name = value.Name;
            this.m_nsfwLevel = value.NsfwLevel;
            this.m_splashId = value.SplashId;
            this.m_vanityUrlCode = value.VanityURLCode;
            this.m_verificationLevel = value.VerificationLevel;
            this.m_welcomeScreen = this.ShimWelcomeScreen(value.WelcomeScreen);
        }

        public PartialGuild UnShim()
        {
            return new PartialGuild()
            {
                ApproximateMemberCount = this.m_approximateMemberCount,
                ApproximatePresenceCount = this.m_approximatePresenceCount,
                BannerId = this.m_bannerId,
                Description = this.m_description,
                Features = this.m_features?.UnShim(),
                IconId = this.m_iconId,
                Id = this.Id,
                Name = this.m_name,
                NsfwLevel = this.m_nsfwLevel,
                SplashId = this.m_splashId,
                VanityURLCode = this.m_vanityUrlCode,
                VerificationLevel = this.m_verificationLevel,
                WelcomeScreen = this.m_welcomeScreen?.UnShim()
            };
        }

        [return: NotNullIfNotNull(nameof(features))]
        protected virtual GuildFeaturesShim? ShimFeatures(GuildFeatures? features)
        {
            if (features == null)
            {
                return null;
            }
            return new GuildFeaturesShim(features);
        }

        [return: NotNullIfNotNull(nameof(welcomeScreen))]
        protected virtual WelcomeScreenShim? ShimWelcomeScreen(WelcomeScreen? welcomeScreen)
        {
            if (welcomeScreen == null)
            {
                return null;
            }
            return new WelcomeScreenShim(this.Client, welcomeScreen);
        }

        /// <inheritdoc cref="PartialGuild.Name"/>
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                Preconditions.NotNull(value, nameof(value));
                value = value.Trim();
                this.m_name = value.Substring(0, Math.Min(value.Length, 100));
            }
        }

        /// <inheritdoc cref="PartialGuild.Description"/>
        public virtual string? Description
        {
            get => this.m_description;
            set => this.m_description = value;
        }

        /// <inheritdoc cref="PartialGuild.SplashId"/>
        public virtual string? SplashId
        {
            get => this.m_splashId;
            set => this.m_splashId = value;
        }

        /// <inheritdoc cref="PartialGuild.SplashUrl"/>
        public string? SplashUrl => CDN.GetGuildSplashUrl(Id, SplashId);

        /// <inheritdoc cref="PartialGuild.BannerId"/>
        public virtual string? BannerId
        {
            get => this.m_bannerId;
            set => this.m_bannerId = value;
        }

        /// <inheritdoc cref="PartialGuild.BannerUrl"/>
        public string? BannerUrl => CDN.GetGuildBannerUrl(Id, BannerId, ImageFormat.Auto);

        /// <inheritdoc cref="PartialGuild.Features"/>
        public virtual GuildFeaturesShim? Features
        {
            get => this.m_features;
            set => this.m_features = value;
        }

        /// <inheritdoc cref="PartialGuild.IconId"/>
        public virtual string? IconId
        {
            get => this.m_iconId;
            set => this.m_iconId = value;
        }

        /// <inheritdoc cref="PartialGuild.IconUrl"/>
        public string? IconUrl => CDN.GetGuildIconUrl(Id, IconId);

        /// <inheritdoc cref="PartialGuild.VerificationLevel"/>
        public virtual VerificationLevel? VerificationLevel
        {
            get => this.m_verificationLevel;
            set => this.m_verificationLevel = value;
        }

        /// <inheritdoc cref="PartialGuild.VanityURLCode"/>
        public string? VanityURLCode
        {
            get => this.m_vanityUrlCode;
            set => this.m_vanityUrlCode = value;
        }

        /// <inheritdoc cref="PartialGuild.PremiumSubscriptionCount"/>
        public virtual int? PremiumSubscriptionCount
        {
            get => this.m_premiumSubscriptionCount;
            set => this.m_premiumSubscriptionCount = value;
        }

        /// <inheritdoc cref="PartialGuild.NsfwLevel"/>
        public virtual NsfwLevel? NsfwLevel
        {
            get => this.m_nsfwLevel;
            set => this.m_nsfwLevel = value;
        }

        /// <inheritdoc cref="PartialGuild.WelcomeScreen"/>
        public virtual WelcomeScreenShim? WelcomeScreen
        {
            get => this.m_welcomeScreen;
            set => this.m_welcomeScreen = value;
        }

        /// <inheritdoc cref="PartialGuild.ApproximateMemberCount"/>
        public virtual int? ApproximateMemberCount
        {
            get => this.m_approximateMemberCount;
            set => this.m_approximateMemberCount = value;
        }

        /// <inheritdoc cref="PartialGuild.ApproximatePresenceCount"/>
        public virtual int? ApproximatePresenceCount
        {
            get => this.m_approximatePresenceCount;
            set => this.m_approximatePresenceCount = value;
        }

        public static implicit operator PartialGuild(PartialGuildShim v)
        {
            return v.UnShim();
        }
    }
}
