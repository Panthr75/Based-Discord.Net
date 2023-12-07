using Discord.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataModel = Discord.API.AutocompleteInteractionData;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a <see cref="InteractionType.ApplicationCommandAutocomplete"/> received over the gateway.
    /// </summary>
    public class SocketAutocompleteInteraction : SocketInteraction, IAutocompleteInteraction, IDiscordInteraction
    {
        /// <summary>
        ///     Gets the autocomplete data of this interaction.
        /// </summary>
        public new SocketAutocompleteInteractionData Data { get; }

        /// <inheritdoc/>
        public override bool HasResponded { get; internal set; }

        /// <inheritdoc cref="SocketInteraction.UserLocale"/>
        new public string UserLocale => base.UserLocale!;

        private readonly object _lock = new object();

        internal SocketAutocompleteInteraction(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
            : base(client, model.Id, channel, user)
        {
            if (!model.Data.IsSpecified)
            {
                throw new InvalidOperationException("Cannot create socket autocomplete interaction without any data.");
            }
            var dataModel = (DataModel)model.Data.Value;

            Data = new SocketAutocompleteInteractionData(dataModel);
        }

        internal new static SocketAutocompleteInteraction Create(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
        {
            var entity = new SocketAutocompleteInteraction(client, model, channel, user);
            entity.Update(model);
            return entity;
        }

        /// <summary>
        ///     Responds to this interaction with a set of choices.
        /// </summary>
        /// <param name="result">
        ///     The set of choices for the user to pick from.
        ///     <remarks>
        ///         A max of 25 choices are allowed. Passing <see langword="null"/> for this argument will show the executing user that
        ///         there is no choices for their autocompleted input.
        ///     </remarks>
        /// </param>
        /// <param name="options">The request options for this response.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation of responding to this interaction.
        /// </returns>
        public async Task RespondAsync(IEnumerable<AutocompleteResult> result, RequestOptions? options = null)
        {
            if (!InteractionHelper.CanSendResponse(this))
                throw new TimeoutException($"Cannot respond to an interaction after {InteractionHelper.ResponseTimeLimit} seconds!");

            lock (_lock)
            {
                if (HasResponded)
                {
                    throw new InvalidOperationException("Cannot respond twice to the same interaction");
                }
            }

            await InteractionHelper.SendAutocompleteResultAsync(Discord, result, Id, Token, options).ConfigureAwait(false);
            lock (_lock)
            {
                HasResponded = true;
            }
        }

        /// <summary>
        ///     Responds to this interaction with a set of choices.
        /// </summary>
        /// <param name="options">The request options for this response.</param>
        /// <param name="result">
        ///  The set of choices for the user to pick from.
        ///     <remarks>
        ///         A max of 25 choices are allowed. Passing <see langword="null"/> for this argument will show the executing user that
        ///         there is no choices for their autocompleted input.
        ///     </remarks>
        /// </param>
        /// <returns>
        ///     A task that represents the asynchronous operation of responding to this interaction.
        /// </returns>
        public Task RespondAsync(RequestOptions? options = null, params AutocompleteResult[] result)
            => RespondAsync(result, options);

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete("This operation is not supported on Autocomplete Interactions. Instead, use SocketAutocompleteInteraction.RespondAsync(RequestOptions?, AutocompleteResult), or SocketAutocompleteInteraction.RespondAsync(IEnumerable<AutocompleteResult>, RequestOptions?)", error: true)]
        public override Task RespondAsync(string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
            => throw new NotSupportedException("Autocomplete interactions don't support this method!");

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete("This operation is not supported on Autocomplete Interactions. Instead, use SocketAutocompleteInteraction.RespondAsync(RequestOptions?, AutocompleteResult), or SocketAutocompleteInteraction.RespondAsync(IEnumerable<AutocompleteResult>, RequestOptions?)", error: true)]
        public override Task<RestFollowupMessage> FollowupAsync(string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
            => throw new NotSupportedException("Autocomplete interactions don't support this method!");

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete("This operation is not supported on Autocomplete Interactions. Instead, use SocketAutocompleteInteraction.RespondAsync(RequestOptions?, AutocompleteResult), or SocketAutocompleteInteraction.RespondAsync(IEnumerable<AutocompleteResult>, RequestOptions?)", error: true)]
        public override Task<RestFollowupMessage> FollowupWithFilesAsync(IEnumerable<FileAttachment> attachments,
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
            => throw new NotSupportedException("Autocomplete interactions don't support this method!");

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        public override Task DeferAsync(bool ephemeral = false, RequestOptions? options = null)
            => throw new NotSupportedException("Autocomplete interactions don't support this method!");

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete("This operation is not supported on Autocomplete Interactions. Instead, use SocketAutocompleteInteraction.RespondAsync(RequestOptions?, AutocompleteResult), or SocketAutocompleteInteraction.RespondAsync(IEnumerable<AutocompleteResult>, RequestOptions?)", error: true)]
        public override Task RespondWithFilesAsync(IEnumerable<FileAttachment> attachments,
            string? text = null,
            Embed[]? embeds = null,
            bool isTTS = false,
            bool ephemeral = false,
            AllowedMentions? allowedMentions = null,
            MessageComponent? components = null,
            Embed? embed = null,
            RequestOptions? options = null)
            => throw new NotSupportedException("Autocomplete interactions don't support this method!");

        /// <inheritdoc/>
        /// <remarks>
        /// <b>Not supported!</b>
        /// </remarks>
        /// <exception cref="NotSupportedException"></exception>
        [Obsolete("This operation is not supported on Autocomplete Interactions. Instead, use SocketAutocompleteInteraction.RespondAsync(RequestOptions?, AutocompleteResult), or SocketAutocompleteInteraction.RespondAsync(IEnumerable<AutocompleteResult>, RequestOptions?)", error: true)]
        public override Task RespondWithModalAsync(Modal modal, RequestOptions? requestOptions = null)
            => throw new NotSupportedException("Autocomplete interactions cannot have normal responses!");
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

        //IAutocompleteInteraction
        /// <inheritdoc/>
        IAutocompleteInteractionData IAutocompleteInteraction.Data => Data;

        //IDiscordInteraction
        /// <inheritdoc/>
        IDiscordInteractionData IDiscordInteraction.Data => Data;
    }
}
