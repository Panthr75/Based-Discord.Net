using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

#if NET7_0_OR_GREATER
internal interface ICreatableAuditLogData<TSelf>
    where TSelf : ICreatableAuditLogData<TSelf>, IAuditLogData
{
    static abstract TSelf Create(BaseDiscordClient discord, EntryModel entry, Model? log);
}
#endif

// creatable datas
#if NET7_0_OR_GREATER
partial class AutoModBlockedMessageAuditLogData : ICreatableAuditLogData<AutoModBlockedMessageAuditLogData>
{
    static AutoModBlockedMessageAuditLogData ICreatableAuditLogData<AutoModBlockedMessageAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class AutoModFlaggedMessageAuditLogData : ICreatableAuditLogData<AutoModFlaggedMessageAuditLogData>
{
    static AutoModFlaggedMessageAuditLogData ICreatableAuditLogData<AutoModFlaggedMessageAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class AutoModRuleCreatedAuditLogData : ICreatableAuditLogData<AutoModRuleCreatedAuditLogData>
{
    static AutoModRuleCreatedAuditLogData ICreatableAuditLogData<AutoModRuleCreatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class AutoModRuleDeletedAuditLogData : ICreatableAuditLogData<AutoModRuleDeletedAuditLogData>
{
    static AutoModRuleDeletedAuditLogData ICreatableAuditLogData<AutoModRuleDeletedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class AutoModRuleUpdatedAuditLogData : ICreatableAuditLogData<AutoModRuleUpdatedAuditLogData>
{
    static AutoModRuleUpdatedAuditLogData ICreatableAuditLogData<AutoModRuleUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class AutoModTimeoutUserAuditLogData : ICreatableAuditLogData<AutoModTimeoutUserAuditLogData>
{
    static AutoModTimeoutUserAuditLogData ICreatableAuditLogData<AutoModTimeoutUserAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class BanAuditLogData : ICreatableAuditLogData<BanAuditLogData>
{
    static BanAuditLogData ICreatableAuditLogData<BanAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class BotAddAuditLogData : ICreatableAuditLogData<BotAddAuditLogData>
{
    static BotAddAuditLogData ICreatableAuditLogData<BotAddAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class ChannelCreateAuditLogData : ICreatableAuditLogData<ChannelCreateAuditLogData>
{
    static ChannelCreateAuditLogData ICreatableAuditLogData<ChannelCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ChannelDeleteAuditLogData : ICreatableAuditLogData<ChannelDeleteAuditLogData>
{
    static ChannelDeleteAuditLogData ICreatableAuditLogData<ChannelDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ChannelUpdateAuditLogData : ICreatableAuditLogData<ChannelUpdateAuditLogData>
{
    static ChannelUpdateAuditLogData ICreatableAuditLogData<ChannelUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class CommandPermissionUpdateAuditLogData : ICreatableAuditLogData<CommandPermissionUpdateAuditLogData>
{
    static CommandPermissionUpdateAuditLogData ICreatableAuditLogData<CommandPermissionUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class EmoteCreateAuditLogData : ICreatableAuditLogData<EmoteCreateAuditLogData>
{
    static EmoteCreateAuditLogData ICreatableAuditLogData<EmoteCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class EmoteDeleteAuditLogData : ICreatableAuditLogData<EmoteDeleteAuditLogData>
{
    static EmoteDeleteAuditLogData ICreatableAuditLogData<EmoteDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class EmoteUpdateAuditLogData : ICreatableAuditLogData<EmoteUpdateAuditLogData>
{
    static EmoteUpdateAuditLogData ICreatableAuditLogData<EmoteUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class GuildUpdateAuditLogData : ICreatableAuditLogData<GuildUpdateAuditLogData>
{
    static GuildUpdateAuditLogData ICreatableAuditLogData<GuildUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class IntegrationCreatedAuditLogData : ICreatableAuditLogData<IntegrationCreatedAuditLogData>
{
    static IntegrationCreatedAuditLogData ICreatableAuditLogData<IntegrationCreatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class IntegrationDeletedAuditLogData : ICreatableAuditLogData<IntegrationDeletedAuditLogData>
{
    static IntegrationDeletedAuditLogData ICreatableAuditLogData<IntegrationDeletedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class IntegrationUpdatedAuditLogData : ICreatableAuditLogData<IntegrationUpdatedAuditLogData>
{
    static IntegrationUpdatedAuditLogData ICreatableAuditLogData<IntegrationUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class InviteCreateAuditLogData : ICreatableAuditLogData<InviteCreateAuditLogData>
{
    static InviteCreateAuditLogData ICreatableAuditLogData<InviteCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class InviteDeleteAuditLogData : ICreatableAuditLogData<InviteDeleteAuditLogData>
{
    static InviteDeleteAuditLogData ICreatableAuditLogData<InviteDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class InviteUpdateAuditLogData : ICreatableAuditLogData<InviteUpdateAuditLogData>
{
    static InviteUpdateAuditLogData ICreatableAuditLogData<InviteUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class KickAuditLogData : ICreatableAuditLogData<KickAuditLogData>
{
    static KickAuditLogData ICreatableAuditLogData<KickAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class MemberDisconnectAuditLogData : ICreatableAuditLogData<MemberDisconnectAuditLogData>
{
    static MemberDisconnectAuditLogData ICreatableAuditLogData<MemberDisconnectAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class MemberMoveAuditLogData : ICreatableAuditLogData<MemberMoveAuditLogData>
{
    static MemberMoveAuditLogData ICreatableAuditLogData<MemberMoveAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class MemberRoleAuditLogData : ICreatableAuditLogData<MemberRoleAuditLogData>
{
    static MemberRoleAuditLogData ICreatableAuditLogData<MemberRoleAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class MemberUpdateAuditLogData : ICreatableAuditLogData<MemberUpdateAuditLogData>
{
    static MemberUpdateAuditLogData ICreatableAuditLogData<MemberUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class MessageBulkDeleteAuditLogData : ICreatableAuditLogData<MessageBulkDeleteAuditLogData>
{
    static MessageBulkDeleteAuditLogData ICreatableAuditLogData<MessageBulkDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class MessageDeleteAuditLogData : ICreatableAuditLogData<MessageDeleteAuditLogData>
{
    static MessageDeleteAuditLogData ICreatableAuditLogData<MessageDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class MessagePinAuditLogData : ICreatableAuditLogData<MessagePinAuditLogData>
{
    static MessagePinAuditLogData ICreatableAuditLogData<MessagePinAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class MessageUnpinAuditLogData : ICreatableAuditLogData<MessageUnpinAuditLogData>
{
    static MessageUnpinAuditLogData ICreatableAuditLogData<MessageUnpinAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class OnboardingPromptCreatedAuditLogData : ICreatableAuditLogData<OnboardingPromptCreatedAuditLogData>
{
    static OnboardingPromptCreatedAuditLogData ICreatableAuditLogData<OnboardingPromptCreatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OnboardingPromptUpdatedAuditLogData : ICreatableAuditLogData<OnboardingPromptUpdatedAuditLogData>
{
    static OnboardingPromptUpdatedAuditLogData ICreatableAuditLogData<OnboardingPromptUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OnboardingUpdatedAuditLogData : ICreatableAuditLogData<OnboardingUpdatedAuditLogData>
{
    static OnboardingUpdatedAuditLogData ICreatableAuditLogData<OnboardingUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteCreateAuditLogData : ICreatableAuditLogData<OverwriteCreateAuditLogData>
{
    static OverwriteCreateAuditLogData ICreatableAuditLogData<OverwriteCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteDeleteAuditLogData : ICreatableAuditLogData<OverwriteDeleteAuditLogData>
{
    static OverwriteDeleteAuditLogData ICreatableAuditLogData<OverwriteDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteUpdateAuditLogData : ICreatableAuditLogData<OverwriteUpdateAuditLogData>
{
    static OverwriteUpdateAuditLogData ICreatableAuditLogData<OverwriteUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class PruneAuditLogData : ICreatableAuditLogData<PruneAuditLogData>
{
    static PruneAuditLogData ICreatableAuditLogData<PruneAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class RoleCreateAuditLogData : ICreatableAuditLogData<RoleCreateAuditLogData>
{
    static RoleCreateAuditLogData ICreatableAuditLogData<RoleCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class RoleDeleteAuditLogData : ICreatableAuditLogData<RoleDeleteAuditLogData>
{
    static RoleDeleteAuditLogData ICreatableAuditLogData<RoleDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class RoleUpdateAuditLogData : ICreatableAuditLogData<RoleUpdateAuditLogData>
{
    static RoleUpdateAuditLogData ICreatableAuditLogData<RoleUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ScheduledEventCreateAuditLogData : ICreatableAuditLogData<ScheduledEventCreateAuditLogData>
{
    static ScheduledEventCreateAuditLogData ICreatableAuditLogData<ScheduledEventCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class ScheduledEventDeleteAuditLogData : ICreatableAuditLogData<ScheduledEventDeleteAuditLogData>
{
    static ScheduledEventDeleteAuditLogData ICreatableAuditLogData<ScheduledEventDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ScheduledEventUpdateAuditLogData : ICreatableAuditLogData<ScheduledEventUpdateAuditLogData>
{
    static ScheduledEventUpdateAuditLogData ICreatableAuditLogData<ScheduledEventUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class StageInstanceCreateAuditLogData : ICreatableAuditLogData<StageInstanceCreateAuditLogData>
{
    static StageInstanceCreateAuditLogData ICreatableAuditLogData<StageInstanceCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class StageInstanceDeleteAuditLogData : ICreatableAuditLogData<StageInstanceDeleteAuditLogData>
{
    static StageInstanceDeleteAuditLogData ICreatableAuditLogData<StageInstanceDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class StageInstanceUpdatedAuditLogData : ICreatableAuditLogData<StageInstanceUpdatedAuditLogData>
{
    static StageInstanceUpdatedAuditLogData ICreatableAuditLogData<StageInstanceUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class StickerCreatedAuditLogData : ICreatableAuditLogData<StickerCreatedAuditLogData>
{
    static StickerCreatedAuditLogData ICreatableAuditLogData<StickerCreatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class StickerDeletedAuditLogData : ICreatableAuditLogData<StickerDeletedAuditLogData>
{
    static StickerDeletedAuditLogData ICreatableAuditLogData<StickerDeletedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class StickerUpdatedAuditLogData : ICreatableAuditLogData<StickerUpdatedAuditLogData>
{
    static StickerUpdatedAuditLogData ICreatableAuditLogData<StickerUpdatedAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ThreadCreateAuditLogData : ICreatableAuditLogData<ThreadCreateAuditLogData>
{
    static ThreadCreateAuditLogData ICreatableAuditLogData<ThreadCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class ThreadDeleteAuditLogData : ICreatableAuditLogData<ThreadDeleteAuditLogData>
{
    static ThreadDeleteAuditLogData ICreatableAuditLogData<ThreadDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ThreadUpdateAuditLogData : ICreatableAuditLogData<ThreadUpdateAuditLogData>
{
    static ThreadUpdateAuditLogData ICreatableAuditLogData<ThreadUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class UnbanAuditLogData : ICreatableAuditLogData<UnbanAuditLogData>
{
    static UnbanAuditLogData ICreatableAuditLogData<UnbanAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class WebhookCreateAuditLogData : ICreatableAuditLogData<WebhookCreateAuditLogData>
{
    static WebhookCreateAuditLogData ICreatableAuditLogData<WebhookCreateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
partial class WebhookDeleteAuditLogData : ICreatableAuditLogData<WebhookDeleteAuditLogData>
{
    static WebhookDeleteAuditLogData ICreatableAuditLogData<WebhookDeleteAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class WebhookUpdateAuditLogData : ICreatableAuditLogData<WebhookUpdateAuditLogData>
{
    static WebhookUpdateAuditLogData ICreatableAuditLogData<WebhookUpdateAuditLogData>.Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry, log);
}
#else
partial class AutoModBlockedMessageAuditLogData
{
    internal static AutoModBlockedMessageAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class AutoModFlaggedMessageAuditLogData
{
    internal static AutoModFlaggedMessageAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class AutoModRuleCreatedAuditLogData
{
    internal static AutoModRuleCreatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class AutoModRuleDeletedAuditLogData
{
    internal static AutoModRuleDeletedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class AutoModTimeoutUserAuditLogData
{
    internal static AutoModTimeoutUserAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class ChannelCreateAuditLogData
{
    internal static ChannelCreateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ChannelDeleteAuditLogData
{
    internal static ChannelDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ChannelUpdateAuditLogData
{
    internal static ChannelUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class EmoteCreateAuditLogData 
{
    internal static EmoteCreateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class EmoteDeleteAuditLogData
{
    internal static EmoteDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class EmoteUpdateAuditLogData
{
    internal static EmoteUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class IntegrationDeletedAuditLogData
{
    internal static IntegrationDeletedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class InviteUpdateAuditLogData
{
    internal static InviteUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class MemberDisconnectAuditLogData
{
    internal static MemberDisconnectAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class MemberMoveAuditLogData
{
    internal static MemberMoveAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class MessageBulkDeleteAuditLogData
{
    internal static MessageBulkDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class OnboardingPromptCreatedAuditLogData
{
    internal static OnboardingPromptCreatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OnboardingPromptUpdatedAuditLogData
{
    internal static OnboardingPromptUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OnboardingUpdatedAuditLogData
{
    internal static OnboardingUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteCreateAuditLogData
{
    internal static OverwriteCreateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteDeleteAuditLogData
{
    internal static OverwriteDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class OverwriteUpdateAuditLogData
{
    internal static OverwriteUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class PruneAuditLogData
{
    internal static PruneAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(entry);
}
partial class RoleCreateAuditLogData
{
    internal static RoleCreateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class RoleDeleteAuditLogData
{
    internal static RoleDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class RoleUpdateAuditLogData
{
    internal static RoleUpdateAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ScheduledEventDeleteAuditLogData
{
    internal static ScheduledEventDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class StickerCreatedAuditLogData
{
    internal static StickerCreatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class StickerDeletedAuditLogData
{
    internal static StickerDeletedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class StickerUpdatedAuditLogData
{
    internal static StickerUpdatedAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class ThreadDeleteAuditLogData
{
    internal static ThreadDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
partial class WebhookDeleteAuditLogData
{
    internal static WebhookDeleteAuditLogData Create(BaseDiscordClient discord, EntryModel entry, Model? log) => Create(discord, entry);
}
#endif
