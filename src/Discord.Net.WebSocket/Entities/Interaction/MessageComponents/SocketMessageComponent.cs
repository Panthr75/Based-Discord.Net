using Discord.Net.Rest;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataModel = Discord.API.MessageComponentInteractionData;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a Websocket-based interaction type for Message Components.
    /// </summary>
    public class SocketMessageComponent : SocketInteraction, IComponentInteraction, IDiscordInteraction
    {
        /// <summary>
        ///     Gets the data received with this interaction, contains the button that was clicked.
        /// </summary>
        public new SocketMessageComponentData Data { get; }
        
        /// <inheritdoc cref="IComponentInteraction.Message"/>
        public SocketUserMessage Message { get; private set; }

        private object _lock = new object();
        public override bool HasResponded { get; internal set; } = false;

        /// <inheritdoc cref="SocketInteraction.UserLocale"/>
        new public string UserLocale => base.UserLocale!;

        internal SocketMessageComponent(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
            : base(client, model.Id, channel, user)
        {
            if (!model.Data.IsSpecified)
            {
                throw new InvalidOperationException("Cannot create socket message component interaction without any data.");
            }
            var dataModel = (DataModel)model.Data.Value;

            this.Message = null!;

            Data = new SocketMessageComponentData(dataModel, client, client.State, client.Guilds.FirstOrDefault(x => x.Id == model.GuildId.GetValueOrDefault()), model.User.GetValueOrDefault());
        }

        internal new static SocketMessageComponent Create(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
        {
            var entity = new SocketMessageComponent(client, model, channel, user);
            entity.Update(model);
            return entity;
        }
        internal override void Update(Model model)
        {
            base.Update(model);

            if (!model.Message.IsSpecified)
            {
                return;
            }

            if (Message == null)
            {
                SocketUser? author = null;
                if (Channel is SocketGuildChannel channel)
                {
                    if (model.Message.Value.WebhookId.IsSpecified)
                        author = SocketWebhookUser.Create(channel.Guild, Discord.State, model.Message.Value.Author.Value, model.Message.Value.WebhookId.Value);
                    else if (model.Message.Value.Author.IsSpecified)
                        author = channel.Guild.GetUser(model.Message.Value.Author.Value.Id);
                }
                else if (model.Message.Value.Author.IsSpecified)
                    author = (Channel as SocketChannel)?.GetUser(model.Message.Value.Author.Value.Id);

                author ??= Discord.State.GetOrAddUser(model.Message.Value.Author.Value.Id, _ => SocketGlobalUser.Create(Discord, Discord.State, model.Message.Value.Author.Value));

                Message = SocketUserMessage.Create(Discord, Discord.State, author, Channel!, model.Message.Value);
            }
            else
            {
                Message.Update(Discord.State, model.Message.Value);
            }
        }
        /// <inheritdoc/>
        public override async Task RespondWithFilesAsync(
            IEnumerable<FileAttachment> attachments,
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
        {
            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot respond to an interaction after {InteractionHelper.ResponseTimeLimit} seconds!");

            embeds ??= Array.Empty<Embed>();
            if (embed != null)
                embeds = new[] { embed }.Concat(embeds).ToArray();

            Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions.RoleIds), "A max of 100 role Ids are allowed.");
            Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions.UserIds), "A max of 100 user Ids are allowed.");
            Preconditions.AtMost(embeds.Length, 10, nameof(embeds), "A max of 10 embeds are allowed.");

            // check that user flag and user Id list are exclusive, same with role flag and role Id list
            if (allowedMentions != null && allowedMentions.AllowedTypes.HasValue)
            {
                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Users) &&
                    allowedMentions.UserIds != null && allowedMentions.UserIds.Count > 0)
                {
                    throw new ArgumentException("The Users flag is mutually exclusive with the list of User Ids.", nameof(allowedMentions));
                }

                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Roles) &&
                    allowedMentions.RoleIds != null && allowedMentions.RoleIds.Count > 0)
                {
                    throw new ArgumentException("The Roles flag is mutually exclusive with the list of Role Ids.", nameof(allowedMentions));
                }
            }

            var response = new API.Rest.UploadInteractionFileParams(attachments?.ToArray() ?? Array.Empty<FileAttachment>())
            {
                Type = InteractionResponseType.ChannelMessageWithSource,
                Content = Optional.CreateFromNullable(text),
                AllowedMentions = Optional.CreateFromNullable(allowedMentions).Map(m => m.ToModel()),
                Embeds = embeds.Select(x => x.ToModel()).ToArray(),
                IsTTS = isTTS,
                MessageComponents = Optional.CreateFromNullable(components).Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
                Flags = ephemeral ? MessageFlags.Ephemeral : Optional<MessageFlags>.Unspecified
            };

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond, update, or defer the same interaction twice");
                }
            }

            await InteractionHelper.SendInteractionResponseAsync(Discord, response, this, Channel, options).ConfigureAwait(false);
            HasResponded = true;
        }

        /// <inheritdoc/>
        public override async Task RespondAsync(
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
        {
            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot respond to an interaction after {InteractionHelper.ResponseTimeLimit} seconds!");

            embeds ??= Array.Empty<Embed>();
            if (embed != null)
                embeds = new[] { embed }.Concat(embeds).ToArray();

            Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions.RoleIds), "A max of 100 role Ids are allowed.");
            Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions.UserIds), "A max of 100 user Ids are allowed.");
            Preconditions.AtMost(embeds.Length, 10, nameof(embeds), "A max of 10 embeds are allowed.");

            // check that user flag and user Id list are exclusive, same with role flag and role Id list
            if (allowedMentions != null && allowedMentions.AllowedTypes.HasValue)
            {
                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Users) &&
                    allowedMentions.UserIds != null && allowedMentions.UserIds.Count > 0)
                {
                    throw new ArgumentException("The Users flag is mutually exclusive with the list of User Ids.", nameof(allowedMentions));
                }

                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Roles) &&
                    allowedMentions.RoleIds != null && allowedMentions.RoleIds.Count > 0)
                {
                    throw new ArgumentException("The Roles flag is mutually exclusive with the list of Role Ids.", nameof(allowedMentions));
                }
            }

            var response = new API.InteractionResponse
            {
                Type = InteractionResponseType.ChannelMessageWithSource,
                Data = new API.InteractionCallbackData
                {
                    Content = Optional.CreateFromNullable(text),
                    AllowedMentions = Optional.CreateFromNullable(allowedMentions?.ToModel()),
                    Embeds = embeds.Select(x => x.ToModel()).ToArray(),
                    TTS = isTTS,
                    Flags = ephemeral ? MessageFlags.Ephemeral : Optional<MessageFlags>.Unspecified,
                    Components = Optional.CreateFromNullable(components).Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
                }
            };

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond, update, or defer twice to the same interaction");
                }
            }

            await InteractionHelper.SendInteractionResponseAsync(Discord, response, this, Channel, options).ConfigureAwait(false);
            HasResponded = true;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Action<MessageProperties> func, RequestOptions? options = null)
        {
            var args = new MessageProperties();
            func(args);

            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot respond to an interaction after {InteractionHelper.ResponseTimeLimit} seconds!");

            if (args.AllowedMentions.IsSpecified)
            {
                var allowedMentions = args.AllowedMentions.Value;
                Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions), "A max of 100 role Ids are allowed.");
                Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions), "A max of 100 user Ids are allowed.");
            }

            var embed = args.Embed;
            var embeds = args.Embeds;

            var apiEmbeds = embed.IsSpecified || embeds.IsSpecified ? new List<API.Embed>() : null;

            if (embed.IsSpecified && embed.Value != null)
            {
                apiEmbeds!.Add(embed.Value.ToModel());
            }

            if (embeds.IsSpecified && embeds.Value != null)
            {
                apiEmbeds!.AddRange(embeds.Value.Select(x => x.ToModel()));
            }

            Preconditions.AtMost(apiEmbeds?.Count ?? 0, 10, nameof(args.Embeds), "A max of 10 embeds are allowed.");

            // check that user flag and user Id list are exclusive, same with role flag and role Id list
            if (args.AllowedMentions.IsSpecified && args.AllowedMentions.Value != null && args.AllowedMentions.Value.AllowedTypes.HasValue)
            {
                var allowedMentions = args.AllowedMentions.Value;
                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Users)
                && allowedMentions.UserIds != null && allowedMentions.UserIds.Count > 0)
                {
                    throw new ArgumentException("The Users flag is mutually exclusive with the list of User Ids.", nameof(args.AllowedMentions));
                }

                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Roles)
                && allowedMentions.RoleIds != null && allowedMentions.RoleIds.Count > 0)
                {
                    throw new ArgumentException("The Roles flag is mutually exclusive with the list of Role Ids.", nameof(args.AllowedMentions));
                }
            }

            if (!args.Attachments.IsSpecified)
            {
                var response = new API.InteractionResponse
                {
                    Type = InteractionResponseType.UpdateMessage,
                    Data = new API.InteractionCallbackData
                    {
                        Content = args.Content,
                        AllowedMentions = args.AllowedMentions.Map(o => o.ToModel()),
                        Embeds = Optional.CreateFromNullable(apiEmbeds?.ToArray()),
                        Components = args.Components.Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
                        Flags = Optional.CreateFromNullable(args.Flags.GetValueOrDefault())
                    }
                };

                await InteractionHelper.SendInteractionResponseAsync(Discord, response, this, Channel, options).ConfigureAwait(false);
            }
            else
            {
                var attachments = args.Attachments.Map(x => x.ToArray()).GetValueOrDefault(Array.Empty<FileAttachment>());

                var response = new API.Rest.UploadInteractionFileParams(attachments)
                {
                    Type = InteractionResponseType.UpdateMessage,
                    Content = args.Content,
                    AllowedMentions = args.AllowedMentions.Map(m => m.ToModel()),
                    Embeds = Optional.CreateFromNullable(apiEmbeds?.ToArray()),
                    MessageComponents = args.Components.Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
                    Flags = Optional.CreateFromNullable(args.Flags.GetValueOrDefault())
                };

                await InteractionHelper.SendInteractionResponseAsync(Discord, response, this, Channel, options).ConfigureAwait(false);
            }

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond, update, or defer twice to the same interaction");
                }
            }

            HasResponded = true;
        }

        /// <inheritdoc/>
        public override Task<RestFollowupMessage> FollowupAsync(
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
        {
            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            embeds ??= Array.Empty<Embed>();
            if (embed != null)
                embeds = new[] { embed }.Concat(embeds).ToArray();

            Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions.RoleIds), "A max of 100 role Ids are allowed.");
            Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions.UserIds), "A max of 100 user Ids are allowed.");
            Preconditions.AtMost(embeds.Length, 10, nameof(embeds), "A max of 10 embeds are allowed.");

            var args = new API.Rest.CreateWebhookMessageParams
            {
                Content = Optional.CreateFromNullable(text),
                AllowedMentions = Optional.CreateFromNullable(allowedMentions).Map(m => m.ToModel()),
                IsTTS = isTTS,
                Embeds = embeds.Select(x => x.ToModel()).ToArray(),
                Components = Optional.CreateFromNullable(components).Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
            };

            if (ephemeral)
                args.Flags = MessageFlags.Ephemeral;

            return InteractionHelper.SendFollowupAsync(Discord.Rest, args, Token, Channel!, options);
        }

        /// <inheritdoc/>
        public override Task<RestFollowupMessage> FollowupWithFilesAsync(
            IEnumerable<FileAttachment> attachments,
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
        {
            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            embeds ??= Array.Empty<Embed>();
            if (embed != null)
                embeds = new[] { embed }.Concat(embeds).ToArray();

            Preconditions.AtMost(allowedMentions?.RoleIds?.Count ?? 0, 100, nameof(allowedMentions.RoleIds), "A max of 100 role Ids are allowed.");
            Preconditions.AtMost(allowedMentions?.UserIds?.Count ?? 0, 100, nameof(allowedMentions.UserIds), "A max of 100 user Ids are allowed.");
            Preconditions.AtMost(embeds.Length, 10, nameof(embeds), "A max of 10 embeds are allowed.");

            foreach (var attachment in attachments)
            {
                Preconditions.NotNullOrEmpty(attachment.FileName, nameof(attachment.FileName), "File Name must not be empty or null");
            }

            // check that user flag and user Id list are exclusive, same with role flag and role Id list
            if (allowedMentions != null && allowedMentions.AllowedTypes.HasValue)
            {
                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Users) &&
                    allowedMentions.UserIds != null && allowedMentions.UserIds.Count > 0)
                {
                    throw new ArgumentException("The Users flag is mutually exclusive with the list of User Ids.", nameof(allowedMentions));
                }

                if (allowedMentions.AllowedTypes.Value.HasFlag(AllowedMentionTypes.Roles) &&
                    allowedMentions.RoleIds != null && allowedMentions.RoleIds.Count > 0)
                {
                    throw new ArgumentException("The Roles flag is mutually exclusive with the list of Role Ids.", nameof(allowedMentions));
                }
            }

            var flags = MessageFlags.None;

            if (ephemeral)
                flags |= MessageFlags.Ephemeral;

            var args = new API.Rest.UploadWebhookFileParams(attachments.ToArray()) {
                Flags = flags,
                Content = Optional.CreateFromNullable(text),
                IsTTS = isTTS,
                Embeds = embeds.Select(x => x.ToModel()).ToArray(),
                AllowedMentions = Optional.CreateFromNullable(allowedMentions).Map(m => m.ToModel()),
                MessageComponents = Optional.CreateFromNullable(components).Map(c => c.Components.Select(x => new API.ActionRowComponent(x)).ToArray()),
            };
            return InteractionHelper.SendFollowupAsync(Discord, args, Token, Channel!, options);
        }

        /// <inheritdoc/>
        public async Task DeferLoadingAsync(bool ephemeral = false, RequestOptions? options = null)
        {
            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot defer an interaction after {InteractionHelper.ResponseTimeLimit} seconds of no response/acknowledgement");

            var response = new API.InteractionResponse
            {
                Type = InteractionResponseType.DeferredChannelMessageWithSource,
                Data = ephemeral ? new API.InteractionCallbackData { Flags = MessageFlags.Ephemeral } : Optional<API.InteractionCallbackData>.Unspecified
            };

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond or defer twice to the same interaction");
                }
            }

            await Discord.Rest.ApiClient.CreateInteractionResponseAsync(response, Id, Token, options).ConfigureAwait(false);
            HasResponded = true;
        }

        /// <inheritdoc/>
        public override async Task DeferAsync(bool ephemeral = false, RequestOptions? options = null)
        {
            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot defer an interaction after {InteractionHelper.ResponseTimeLimit} seconds of no response/acknowledgement");

            var response = new API.InteractionResponse
            {
                Type = InteractionResponseType.DeferredUpdateMessage,
                Data = ephemeral ? new API.InteractionCallbackData { Flags = MessageFlags.Ephemeral } : Optional<API.InteractionCallbackData>.Unspecified
            };

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond or defer twice to the same interaction");
                }
            }

            await Discord.Rest.ApiClient.CreateInteractionResponseAsync(response, Id, Token, options).ConfigureAwait(false);
            HasResponded = true;
        }

        /// <inheritdoc/>
        public override async Task RespondWithModalAsync(Modal modal, RequestOptions? options = null)
        {
            if (!IsValidToken)
                throw new InvalidOperationException("Interaction token is no longer valid");

            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot respond to an interaction after {InteractionHelper.ResponseTimeLimit} seconds!");

            var response = new API.InteractionResponse
            {
                Type = InteractionResponseType.Modal,
                Data = new API.InteractionCallbackData
                {
                    CustomId = modal.CustomId,
                    Title = modal.Title,
                    Components = modal.Component.Components.Select(x => new Discord.API.ActionRowComponent(x)).ToArray()
                }
            };

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond twice to the same interaction");
                }
            }

            await InteractionHelper.SendInteractionResponseAsync(Discord, response, this, Channel, options).ConfigureAwait(false);

            lock (_lock)
            {
                HasResponded = true;
            }
        }
        //IComponentInteraction
        /// <inheritdoc/>
        IComponentInteractionData IComponentInteraction.Data => Data;

        /// <inheritdoc/>
        IUserMessage IComponentInteraction.Message => Message;

        //IDiscordInteraction
        /// <inheritdoc/>
        IDiscordInteractionData IDiscordInteraction.Data => Data;
    }
}
