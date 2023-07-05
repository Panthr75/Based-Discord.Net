using System;
using DataModel = Discord.API.ApplicationCommandInteractionData;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a Websocket-based slash command received over the gateway.
    /// </summary>
    public class SocketMessageCommand : SocketCommandBase, IMessageCommandInteraction, IDiscordInteraction
    {
        /// <summary>
        ///     Gets the data associated with this interaction.
        /// </summary>
        public new SocketMessageCommandData Data { get; }

        internal SocketMessageCommand(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
            : base(client, model, channel, user)
        {
            if (!model.Data.IsSpecified)
            {
                throw new InvalidOperationException("Cannot create socket message command without any data.");
            }
            var dataModel = (DataModel)model.Data.Value;

            ulong? guildId = model.GuildId.ToNullable();

            Data = SocketMessageCommandData.Create(client, dataModel, model.Id, guildId);
        }

        internal new static SocketInteraction Create(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
        {
            var entity = new SocketMessageCommand(client, model, channel, user);
            entity.Update(model);
            return entity;
        }

        //IMessageCommandInteraction
        /// <inheritdoc/>
        IMessageCommandInteractionData IMessageCommandInteraction.Data => Data;

        //IDiscordInteraction
        /// <inheritdoc/>
        IDiscordInteractionData IDiscordInteraction.Data => Data;

        //IApplicationCommandInteraction
        /// <inheritdoc/>
        IApplicationCommandInteractionData IApplicationCommandInteraction.Data => Data;
    }
}
