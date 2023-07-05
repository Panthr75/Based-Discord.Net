using System.Threading.Tasks;
using DataModel = Discord.API.ApplicationCommandInteractionData;
using Model = Discord.API.Interaction;

namespace Discord.Rest
{
    /// <summary>
    ///     Represents a REST-based message command interaction.
    /// </summary>
    public class RestMessageCommand : RestCommandBase, IMessageCommandInteraction, IDiscordInteraction
    {
        /// <summary>
        ///     Gets the data associated with this interaction.
        /// </summary>
        public new RestMessageCommandData Data
        {
            get => (RestMessageCommandData)base.Data;
            private set => base.Data = value;
        }

        internal RestMessageCommand(DiscordRestClient client, Model model)
            : base(client, model)
        {
            this.Data = null!;
        }

        internal new static async Task<RestMessageCommand> CreateAsync(DiscordRestClient client, Model model, bool doApiCall)
        {
            var entity = new RestMessageCommand(client, model);
            await entity.UpdateAsync(client, model, doApiCall).ConfigureAwait(false);
            return entity;
        }

        internal override async Task UpdateAsync(DiscordRestClient client, Model model, bool doApiCall)
        {
            await base.UpdateAsync(client, model, doApiCall).ConfigureAwait(false);

            if (!model.Data.IsSpecified)
            {
                return;
            }

            var dataModel = (DataModel)model.Data.Value;

            Data = await RestMessageCommandData.CreateAsync(client, dataModel, Guild, Channel, doApiCall).ConfigureAwait(false);
        }

        //IMessageCommandInteraction
        /// <inheritdoc/>
        IMessageCommandInteractionData IMessageCommandInteraction.Data => Data;
        //IApplicationCommandInteraction
        /// <inheritdoc/>
        IApplicationCommandInteractionData IApplicationCommandInteraction.Data => Data;
    }
}
