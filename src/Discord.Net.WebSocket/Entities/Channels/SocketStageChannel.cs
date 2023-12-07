using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Model = Discord.API.Channel;
using StageInstance = Discord.API.StageInstance;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a stage channel received over the gateway.
    /// </summary>
    public class SocketStageChannel : SocketVoiceChannel, IStageChannel
    {
        /// <inheritdoc/>
        /// <remarks>
        ///     This field is always true for stage channels.
        /// </remarks>
        [Obsolete("This property is no longer used because Discord enabled text-in-stage for all channels.")]
        public override bool IsTextInVoice
            => true;

        /// <inheritdoc/>
        public StagePrivacyLevel? PrivacyLevel { get; private set; }

        /// <inheritdoc/>
        public bool? IsDiscoverableDisabled { get; private set; }

        /// <inheritdoc/>
        public bool IsLive { get; private set; }

        /// <summary>
        ///     Returns <see langword="true"/> if the current user is a speaker within the stage, otherwise <see langword="false"/>.
        /// </summary>
        public bool IsSpeaker
            => !Guild.CurrentUser.IsSuppressed;

        /// <summary>
        ///     Gets a collection of users who are speakers within the stage.
        /// </summary>
        public IReadOnlyCollection<SocketGuildUser> Speakers
            => Users.Where(x => !x.IsSuppressed).ToImmutableArray();

        /// <inheritdoc/>
        /// <remarks>
        ///     This property is not supported in stage channels and will always return <see langword="null"/>
        /// </remarks>
        public override string? Status => null;

        internal new SocketStageChannel Clone() => (SocketStageChannel)MemberwiseClone();

        internal SocketStageChannel(DiscordSocketClient discord, ulong id, SocketGuild guild)
            : base(discord, id, guild) { }

        internal new static SocketStageChannel Create(SocketGuild? guild, ClientState state, Model model)
        {
            var entity = new SocketStageChannel(guild?.Discord!, model.Id, guild!);
            entity.Update(state, model);
            return entity;
        }
        internal void Update(StageInstance? model, bool isLive = false)
        {
            IsLive = isLive;
            if (isLive && model != null)
            {
                PrivacyLevel = model.PrivacyLevel;
                IsDiscoverableDisabled = model.DiscoverableDisabled;
            }
            else
            {
                PrivacyLevel = null;
                IsDiscoverableDisabled = null;
            }
        }

        /// <inheritdoc/>
        public async Task StartStageAsync(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly, RequestOptions? options = null)
        {
            Preconditions.NotNull(topic, nameof(topic));
            topic = topic.Trim();
            Preconditions.AtLeast(topic.Length, 1, nameof(topic), "A stage topic cannot be empty.");
            Preconditions.AtMost(topic.Length, 120, nameof(topic), "A stage topic cannot exceed 120 characters");

            var args = new API.Rest.CreateStageInstanceParams
            {
                ChannelId = Id,
                Topic = topic,
                PrivacyLevel = privacyLevel
            };

            var model = await Discord.ApiClient.CreateStageInstanceAsync(args, options).ConfigureAwait(false);

            Update(model, true);
        }

        /// <inheritdoc/>
        public async Task ModifyInstanceAsync(Action<StageInstanceProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyAsync(this, Discord, func, options);

            Update(model, true);
        }

        /// <inheritdoc/>
        public async Task StopStageAsync(RequestOptions? options = null)
        {
            await Discord.ApiClient.DeleteStageInstanceAsync(Id, options);

            Update(null);
        }

        /// <inheritdoc/>
        public Task RequestToSpeakAsync(RequestOptions? options = null)
        {
            var args = new API.Rest.ModifyVoiceStateParams
            {
                ChannelId = Id,
                RequestToSpeakTimestamp = DateTimeOffset.UtcNow
            };
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task BecomeSpeakerAsync(RequestOptions? options = null)
        {
            var args = new API.Rest.ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = false
            };
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task StopSpeakingAsync(RequestOptions? options = null)
        {
            var args = new API.Rest.ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = true
            };
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task MoveToSpeakerAsync(IGuildUser user, RequestOptions? options = null)
        {
            var args = new API.Rest.ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = false
            };

            return Discord.ApiClient.ModifyUserVoiceState(Guild.Id, user.Id, args);
        }

        /// <inheritdoc/>
        public Task RemoveFromSpeakerAsync(IGuildUser user, RequestOptions? options = null)
        {
            var args = new API.Rest.ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = true
            };

            return Discord.ApiClient.ModifyUserVoiceState(Guild.Id, user.Id, args);
        }

        /// <inheritdoc />
        /// <remarks>
        ///     Setting voice channel status is not supported in stage channels.
        /// </remarks>
        /// <exception cref="NotSupportedException">Setting voice channel status is not supported in stage channels.</exception>
        public override Task SetStatusAsync(string? status, RequestOptions? options = null)
            => throw new NotSupportedException("Setting voice channel status is not supported in stage channels.");
    }
}
