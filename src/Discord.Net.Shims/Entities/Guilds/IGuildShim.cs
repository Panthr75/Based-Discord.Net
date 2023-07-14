using Discord.Audio;
using System;
using System.Globalization;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="IGuild"/>
    /// </summary>
    public interface IGuildShim : IGuild, ISnowflakeShimEntity
    {
        /// <inheritdoc cref="IGuild.Name"/>
        new string Name { get; set; }

        /// <inheritdoc cref="IGuild.AFKTimeout"/>
        new int AFKTimeout { get; set; }

        /// <inheritdoc cref="IGuild.IsWidgetEnabled"/>
        new bool IsWidgetEnabled { get; set; }

        /// <inheritdoc cref="IGuild.DefaultMessageNotifications"/>
        new DefaultMessageNotifications DefaultMessageNotifications { get; set; }

        /// <inheritdoc cref="IGuild.MfaLevel"/>
        new MfaLevel MfaLevel { get; set; }

        /// <inheritdoc cref="IGuild.VerificationLevel"/>
        new VerificationLevel VerificationLevel { get; set; }

        /// <inheritdoc cref="IGuild.ExplicitContentFilter"/>
        new ExplicitContentFilterLevel ExplicitContentFilter { get; set; }

        /// <inheritdoc cref="IGuild.IconId"/>
        new string? IconId { get; set; }

        /// <inheritdoc cref="IGuild.SplashId"/>
        new string? SplashId { get; set; }

        /// <inheritdoc cref="IGuild.DiscoverySplashId"/>
        new string? DiscoverySplashId { get; }

        /// <inheritdoc cref="IGuild.Available"/>
        new bool Available { get; set; }

        /// <inheritdoc cref="IGuild.AFKChannelId"/>
        new ulong? AFKChannelId { get; set; }
        /// <inheritdoc cref="IGuild.WidgetChannelId"/>
        new ulong? WidgetChannelId { get; set; }
        /// <inheritdoc cref="IGuild.SafetyAlertsChannelId"/>
        new ulong? SafetyAlertsChannelId { get; set; }
        /// <inheritdoc cref="IGuild.SystemChannelId"/>
        new ulong? SystemChannelId { get; set; }
        /// <inheritdoc cref="IGuild.RulesChannelId"/>
        new ulong? RulesChannelId { get; set; }
        /// <inheritdoc cref="IGuild.PublicUpdatesChannelId"/>
        new ulong? PublicUpdatesChannelId { get; set; }
        /// <inheritdoc cref="IGuild.OwnerId"/>
        new ulong OwnerId { get; set; }
        /// <inheritdoc cref="IGuild.ApplicationId"/>
        new ulong? ApplicationId { get; set; }
        /// <inheritdoc cref="IGuild.VoiceRegionId"/>
        [Obsolete("Use IAudioChannel.RTCRegion instead")]
        new string? VoiceRegionId { get; set; }
        /// <inheritdoc cref="IGuild.AudioClient"/>
        new IAudioClient? AudioClient { get; set; }
        /// <inheritdoc cref="IGuild.Features"/>
        new GuildFeaturesShim Features { get; set; }
        /// <inheritdoc cref="IGuild.PremiumTier"/>
        new PremiumTier PremiumTier { get; set; }
        /// <inheritdoc cref="IGuild.BannerId"/>
        new string? BannerId { get; set; }
        /// <inheritdoc cref="IGuild.VanityURLCode"/>
        new string? VanityURLCode { get; set; }
        /// <inheritdoc cref="IGuild.SystemChannelFlags"/>
        new SystemChannelMessageDeny SystemChannelFlags { get; set; }
        /// <inheritdoc cref="IGuild.Description"/>
        new string? Description { get; set; }
        /// <inheritdoc cref="IGuild.PremiumSubscriptionCount"/>
        new int PremiumSubscriptionCount { get; set; }
        /// <inheritdoc cref="IGuild.MaxPresences"/>
        new int? MaxPresences { get; set; }
        /// <inheritdoc cref="IGuild.MaxMembers"/>
        new int? MaxMembers { get; set; }
        /// <inheritdoc cref="IGuild.MaxVideoChannelUsers"/>
        new int? MaxVideoChannelUsers { get; set; }
        /// <inheritdoc cref="IGuild.MaxStageVideoChannelUsers"/>
        new int? MaxStageVideoChannelUsers { get; set; }
        /// <inheritdoc cref="IGuild.PreferredLocale"/>
        new string PreferredLocale { get; set; }

        /// <inheritdoc cref="IGuild.NsfwLevel"/>
        new NsfwLevel NsfwLevel { get; set; }

        /// <inheritdoc cref="IGuild.PreferredCulture"/>
        new CultureInfo PreferredCulture { get; set; }
        /// <inheritdoc cref="IGuild.IsBoostProgressBarEnabled"/>
        new bool IsBoostProgressBarEnabled { get; set; }
    }
}
