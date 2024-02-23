using Discord.API;
using Discord.API.Rest;
using System;
using System.Threading.Tasks;
using Model = Discord.API.Channel;

namespace Discord.Rest
{
    /// <summary>
    ///     Represents a REST-based stage channel in a guild.
    /// </summary>
    public class RestStageChannel : RestVoiceChannel, IStageChannel
    {
        /// <inheritdoc/>
        /// <remarks>
        ///     This field is always true for stage channels.
        /// </remarks>
        /// 
        [Obsolete("This property is no longer used because Discord enabled text-in-voice and text-in-stage for all channels.")]
        public override bool IsTextInVoice
            => true;

        /// <inheritdoc/>
        public StagePrivacyLevel? PrivacyLevel { get; private set; }

        /// <inheritdoc/>
        public bool? IsDiscoverableDisabled { get; private set; }

        /// <inheritdoc/>
        public bool IsLive { get; private set; }
        internal RestStageChannel(BaseDiscordClient discord, IGuild? guild, ulong id)
            : base(discord, guild, id) { }

        internal new static RestStageChannel Create(BaseDiscordClient discord, IGuild? guild, Model model)
        {
            var entity = new RestStageChannel(discord, guild, model.Id);
            entity.Update(model);
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
        public async Task ModifyInstanceAsync(Action<StageInstanceProperties> func, RequestOptions? options = null)
        {
            var model = await ChannelHelper.ModifyStageInstanceAsync(Id, Discord, func, options).ConfigureAwait(false);

            Update(model, true);
        }

        /// <inheritdoc/>
        public async Task StartStageAsync(string topic, StagePrivacyLevel privacyLevel = StagePrivacyLevel.GuildOnly, RequestOptions? options = null)
        {
            Preconditions.NotNull(topic, nameof(topic));
            topic = topic.Trim();
            Preconditions.AtLeast(topic.Length, 1, nameof(topic), "A stage topic cannot be empty.");
            Preconditions.AtMost(topic.Length, 120, nameof(topic), "A stage topic cannot exceed 120 characters");

            var args = new CreateStageInstanceParams
            {
                ChannelId = Id,
                PrivacyLevel = privacyLevel,
                Topic = topic
            };

            var model = await Discord.ApiClient.CreateStageInstanceAsync(args, options);

            Update(model, true);
        }

        /// <inheritdoc/>
        public async Task StopStageAsync(RequestOptions? options = null)
        {
            await Discord.ApiClient.DeleteStageInstanceAsync(Id, options);

            Update(null);
        }

        /// <inheritdoc/>
        public override async Task UpdateAsync(RequestOptions? options = null)
        {
            await base.UpdateAsync(options);

            var model = await Discord.ApiClient.GetStageInstanceAsync(Id, options);

            Update(model, model != null);
        }

        /// <inheritdoc/>
        public Task RequestToSpeakAsync(RequestOptions? options = null)
        {
            var args = new ModifyVoiceStateParams
            {
                ChannelId = Id,
                RequestToSpeakTimestamp = DateTimeOffset.UtcNow
            };
            ValidateGuildExists();
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task BecomeSpeakerAsync(RequestOptions? options = null)
        {
            var args = new ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = false
            };
            ValidateGuildExists();
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task StopSpeakingAsync(RequestOptions? options = null)
        {
            var args = new ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = true
            };
            ValidateGuildExists();
            return Discord.ApiClient.ModifyMyVoiceState(Guild.Id, args, options);
        }

        /// <inheritdoc/>
        public Task MoveToSpeakerAsync(IGuildUser user, RequestOptions? options = null)
        {
            var args = new ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = false
            };

            ValidateGuildExists();
            return Discord.ApiClient.ModifyUserVoiceState(Guild.Id, user.Id, args);
        }

        /// <inheritdoc/>
        public Task RemoveFromSpeakerAsync(IGuildUser user, RequestOptions? options = null)
        {
            var args = new ModifyVoiceStateParams
            {
                ChannelId = Id,
                Suppressed = true
            };

            ValidateGuildExists();
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
