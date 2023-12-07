using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using AuditLogChange = Discord.API.AuditLogChange;
using EntryModel = Discord.API.AuditLogEntry;
using Model = Discord.API.AuditLog;

namespace Discord.Rest;

internal static class AuditLogHelper
{
    private static readonly Dictionary<ActionType, Func<BaseDiscordClient, EntryModel, Model?, IAuditLogData>> CreateMapping
        = new();

    private static void AddAction(ActionType type, Func<BaseDiscordClient, EntryModel, Model?, IAuditLogData> func)
    {
        CreateMapping[type] = func;
    }
#if NET7_0_OR_GREATER
    private static void AddAction<T>(ActionType type)
        where T : ICreatableAuditLogData<T>, IAuditLogData
    {
        AuditLogHelper.AddAction(type, (BaseDiscordClient discord, EntryModel entry, Model? log) => T.Create(discord, entry, log));
    }
#else

#endif

    static AuditLogHelper()
    {
#if NET7_0_OR_GREATER
        AddAction<GuildUpdateAuditLogData>(ActionType.GuildUpdated); // log
        AddAction<ChannelCreateAuditLogData>(ActionType.ChannelCreated);
        AddAction<ChannelUpdateAuditLogData>(ActionType.ChannelUpdated);
        AddAction<ChannelDeleteAuditLogData>(ActionType.ChannelDeleted);

        AddAction<OverwriteCreateAuditLogData>(ActionType.OverwriteCreated);
        AddAction<OverwriteUpdateAuditLogData>(ActionType.OverwriteUpdated);
        AddAction<OverwriteDeleteAuditLogData>(ActionType.OverwriteDeleted);

        AddAction<KickAuditLogData>(ActionType.Kick);
        AddAction<PruneAuditLogData>(ActionType.Prune);
        AddAction<BanAuditLogData>(ActionType.Ban);
        AddAction<UnbanAuditLogData>(ActionType.Unban);
        AddAction<MemberUpdateAuditLogData>(ActionType.MemberUpdated);
        AddAction<MemberRoleAuditLogData>(ActionType.MemberRoleUpdated);
        AddAction<MemberMoveAuditLogData>(ActionType.MemberMoved);
        AddAction<MemberDisconnectAuditLogData>(ActionType.MemberDisconnected);
        AddAction<BotAddAuditLogData>(ActionType.BotAdded);

        AddAction<RoleCreateAuditLogData>(ActionType.RoleCreated);
        AddAction<RoleUpdateAuditLogData>(ActionType.RoleUpdated);
        AddAction<RoleDeleteAuditLogData>(ActionType.RoleDeleted);

        AddAction<InviteCreateAuditLogData>(ActionType.InviteCreated);
        AddAction<InviteUpdateAuditLogData>(ActionType.InviteUpdated);
        AddAction<InviteDeleteAuditLogData>(ActionType.InviteDeleted);

        AddAction<WebhookCreateAuditLogData>(ActionType.WebhookCreated);
        AddAction<WebhookUpdateAuditLogData>(ActionType.WebhookUpdated);
        AddAction<WebhookDeleteAuditLogData>(ActionType.WebhookDeleted);

        AddAction<EmoteCreateAuditLogData>(ActionType.EmojiCreated);
        AddAction<EmoteUpdateAuditLogData>(ActionType.EmojiUpdated);
        AddAction<EmoteDeleteAuditLogData>(ActionType.EmojiDeleted);

        AddAction<MessageDeleteAuditLogData>(ActionType.MessageDeleted);
        AddAction<MessageBulkDeleteAuditLogData>(ActionType.MessageBulkDeleted);
        AddAction<MessagePinAuditLogData>(ActionType.MessagePinned);
        AddAction<MessageUnpinAuditLogData>(ActionType.MessageUnpinned);

        AddAction<ScheduledEventCreateAuditLogData>(ActionType.EventCreate);
        AddAction<ScheduledEventUpdateAuditLogData>(ActionType.EventUpdate);
        AddAction<ScheduledEventDeleteAuditLogData>(ActionType.EventDelete);

        AddAction<ThreadCreateAuditLogData>(ActionType.ThreadCreate);
        AddAction<ThreadUpdateAuditLogData>(ActionType.ThreadUpdate);
        AddAction<ThreadDeleteAuditLogData>(ActionType.ThreadDelete);

        AddAction<CommandPermissionUpdateAuditLogData>(ActionType.ApplicationCommandPermissionUpdate);

        AddAction<IntegrationCreatedAuditLogData>(ActionType.IntegrationCreated);
        AddAction<IntegrationUpdatedAuditLogData>(ActionType.IntegrationUpdated);
        AddAction<IntegrationDeletedAuditLogData>(ActionType.IntegrationDeleted);

        AddAction<StageInstanceCreateAuditLogData>(ActionType.StageInstanceCreated);
        AddAction<StageInstanceUpdatedAuditLogData>(ActionType.StageInstanceUpdated);
        AddAction<StageInstanceDeleteAuditLogData>(ActionType.StageInstanceDeleted);

        AddAction<StickerCreatedAuditLogData>(ActionType.StickerCreated);
        AddAction<StickerUpdatedAuditLogData>(ActionType.StickerUpdated);
        AddAction<StickerDeletedAuditLogData>(ActionType.StickerDeleted);

        AddAction<AutoModRuleCreatedAuditLogData>(ActionType.AutoModerationRuleCreate);
        AddAction<AutoModRuleUpdatedAuditLogData>(ActionType.AutoModerationRuleUpdate);
        AddAction<AutoModRuleDeletedAuditLogData>(ActionType.AutoModerationRuleDelete);

        AddAction<AutoModBlockedMessageAuditLogData>(ActionType.AutoModerationBlockMessage);
        AddAction<AutoModFlaggedMessageAuditLogData>(ActionType.AutoModerationFlagToChannel);
        AddAction<AutoModTimeoutUserAuditLogData>(ActionType.AutoModerationUserCommunicationDisabled);

        AddAction<OnboardingPromptCreatedAuditLogData>(ActionType.OnboardingQuestionCreated);
        AddAction<OnboardingPromptUpdatedAuditLogData>(ActionType.OnboardingQuestionUpdated);
        AddAction<OnboardingUpdatedAuditLogData>(ActionType.OnboardingUpdated);

        AddAction<VoiceChannelStatusUpdateAuditLogData>(ActionType.VoiceChannelStatusUpdated);
        AddAction<VoiceChannelStatusDeletedAuditLogData>(ActionType.VoiceChannelStatusDeleted);
#else
        AddAction(ActionType.GuildUpdated, GuildUpdateAuditLogData.Create); // log
        AddAction(ActionType.ChannelCreated, ChannelCreateAuditLogData.Create);
        AddAction(ActionType.ChannelUpdated, ChannelUpdateAuditLogData.Create);
        AddAction(ActionType.ChannelDeleted, ChannelDeleteAuditLogData.Create);

        AddAction(ActionType.OverwriteCreated, OverwriteCreateAuditLogData.Create);
        AddAction(ActionType.OverwriteUpdated, OverwriteUpdateAuditLogData.Create);
        AddAction(ActionType.OverwriteDeleted, OverwriteDeleteAuditLogData.Create);

        AddAction(ActionType.Kick, KickAuditLogData.Create);
        AddAction(ActionType.Prune, PruneAuditLogData.Create);
        AddAction(ActionType.Ban, BanAuditLogData.Create);
        AddAction(ActionType.Unban, UnbanAuditLogData.Create);
        AddAction(ActionType.MemberUpdated, MemberUpdateAuditLogData.Create);
        AddAction(ActionType.MemberRoleUpdated, MemberRoleAuditLogData.Create);
        AddAction(ActionType.MemberMoved, MemberMoveAuditLogData.Create);
        AddAction(ActionType.MemberDisconnected, MemberDisconnectAuditLogData.Create);
        AddAction(ActionType.BotAdded, BotAddAuditLogData.Create);

        AddAction(ActionType.RoleCreated, RoleCreateAuditLogData.Create);
        AddAction(ActionType.RoleUpdated, RoleUpdateAuditLogData.Create);
        AddAction(ActionType.RoleDeleted, RoleDeleteAuditLogData.Create);

        AddAction(ActionType.InviteCreated, InviteCreateAuditLogData.Create);
        AddAction(ActionType.InviteUpdated, InviteUpdateAuditLogData.Create);
        AddAction(ActionType.InviteDeleted, InviteDeleteAuditLogData.Create);

        AddAction(ActionType.WebhookCreated, WebhookCreateAuditLogData.Create);
        AddAction(ActionType.WebhookUpdated, WebhookUpdateAuditLogData.Create);
        AddAction(ActionType.WebhookDeleted, WebhookDeleteAuditLogData.Create);

        AddAction(ActionType.EmojiCreated, EmoteCreateAuditLogData.Create);
        AddAction(ActionType.EmojiUpdated, EmoteUpdateAuditLogData.Create);
        AddAction(ActionType.EmojiDeleted, EmoteDeleteAuditLogData.Create);

        AddAction(ActionType.MessageDeleted, MessageDeleteAuditLogData.Create);
        AddAction(ActionType.MessageBulkDeleted, MessageBulkDeleteAuditLogData.Create);
        AddAction(ActionType.MessagePinned, MessagePinAuditLogData.Create);
        AddAction(ActionType.MessageUnpinned, MessageUnpinAuditLogData.Create);

        AddAction(ActionType.EventCreate, ScheduledEventCreateAuditLogData.Create);
        AddAction(ActionType.EventUpdate, ScheduledEventUpdateAuditLogData.Create);
        AddAction(ActionType.EventDelete, ScheduledEventDeleteAuditLogData.Create);

        AddAction(ActionType.ThreadCreate, ThreadCreateAuditLogData.Create);
        AddAction(ActionType.ThreadUpdate, ThreadUpdateAuditLogData.Create);
        AddAction(ActionType.ThreadDelete, ThreadDeleteAuditLogData.Create);

        AddAction(ActionType.ApplicationCommandPermissionUpdate, CommandPermissionUpdateAuditLogData.Create);

        AddAction(ActionType.IntegrationCreated, IntegrationCreatedAuditLogData.Create);
        AddAction(ActionType.IntegrationUpdated, IntegrationUpdatedAuditLogData.Create);
        AddAction(ActionType.IntegrationDeleted, IntegrationDeletedAuditLogData.Create);

        AddAction(ActionType.StageInstanceCreated, StageInstanceCreateAuditLogData.Create);
        AddAction(ActionType.StageInstanceUpdated, StageInstanceUpdatedAuditLogData.Create);
        AddAction(ActionType.StageInstanceDeleted, StageInstanceDeleteAuditLogData.Create);

        AddAction(ActionType.StickerCreated, StickerCreatedAuditLogData.Create);
        AddAction(ActionType.StickerUpdated, StickerUpdatedAuditLogData.Create);
        AddAction(ActionType.StickerDeleted, StickerDeletedAuditLogData.Create);

        AddAction(ActionType.AutoModerationRuleCreate, AutoModRuleCreatedAuditLogData.Create);
        AddAction(ActionType.AutoModerationRuleUpdate, AutoModRuleUpdatedAuditLogData.Create);
        AddAction(ActionType.AutoModerationRuleDelete, AutoModRuleDeletedAuditLogData.Create);

        AddAction(ActionType.AutoModerationBlockMessage, AutoModBlockedMessageAuditLogData.Create);
        AddAction(ActionType.AutoModerationFlagToChannel, AutoModFlaggedMessageAuditLogData.Create);
        AddAction(ActionType.AutoModerationUserCommunicationDisabled, AutoModTimeoutUserAuditLogData.Create);

        AddAction(ActionType.OnboardingQuestionCreated, OnboardingPromptCreatedAuditLogData.Create);
        AddAction(ActionType.OnboardingQuestionUpdated, OnboardingPromptUpdatedAuditLogData.Create);
        AddAction(ActionType.OnboardingUpdated, OnboardingUpdatedAuditLogData.Create);


        AddAction(ActionType.VoiceChannelStatusUpdated, VoiceChannelStatusUpdateAuditLogData.Create);
        AddAction(ActionType.VoiceChannelStatusDeleted, VoiceChannelStatusDeletedAuditLogData.Create);
#endif
    }

    public static IAuditLogData? CreateData(BaseDiscordClient discord, EntryModel entry, Model? log = null)
    {
        if (CreateMapping.TryGetValue(entry.Action, out var func))
            return func(discord, entry, log);

        return null;
    }

    internal static (T, T) CreateAuditLogEntityInfo<T>(AuditLogChange[] changes, BaseDiscordClient discord) where T : IAuditLogInfoModel
    {
        var oldModel = (T)Activator.CreateInstance(typeof(T))!;
        var newModel = (T)Activator.CreateInstance(typeof(T))!;

        var props = typeof(T).GetProperties();

        foreach (var property in props)
        {
            if (property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true).FirstOrDefault() is not JsonPropertyNameAttribute jsonAttr)
                continue;
            
            var change = changes.FirstOrDefault(x => x.ChangedProperty == jsonAttr.Name);

            if (change is null)
                continue;
            
            property.SetValue(oldModel, change.OldValue?.Deserialize(property.PropertyType, discord.ApiClient.SerializerOptions));
            property.SetValue(newModel, change.NewValue?.Deserialize(property.PropertyType, discord.ApiClient.SerializerOptions));
        }

        return (oldModel, newModel);
    }
}
