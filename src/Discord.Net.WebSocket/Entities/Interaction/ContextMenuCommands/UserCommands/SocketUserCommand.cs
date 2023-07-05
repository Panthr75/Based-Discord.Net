using System;
using DataModel = Discord.API.ApplicationCommandInteractionData;
using Model = Discord.API.Interaction;

namespace Discord.WebSocket
{
    /// <summary>
    ///     Represents a Websocket-based slash command received over the gateway.
    /// </summary>
    public class SocketUserCommand : SocketCommandBase, IUserCommandInteraction, IDiscordInteraction
    {
        /// <summary>
        ///     The data associated with this interaction.
        /// </summary>
        public new SocketUserCommandData Data { get; }

        internal SocketUserCommand(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
            : base(client, model, channel, user)
        {
            if (!model.Data.IsSpecified)
            {
                throw new InvalidOperationException("Cannot create socket user command without any data.");
            }
            var dataModel = (DataModel)model.Data.Value;

            ulong? guildId = model.GuildId.ToNullable();

            Data = SocketUserCommandData.Create(client, dataModel, model.Id, guildId);
        }

        internal new static SocketInteraction Create(DiscordSocketClient client, Model model, ISocketMessageChannel? channel, SocketUser user)
        {
            var entity = new SocketUserCommand(client, model, channel, user);
            entity.Update(model);
            return entity;
        }

        //IUserCommandInteraction
        /// <inheritdoc/>
        IUserCommandInteractionData IUserCommandInteraction.Data => Data;

        //IDiscordInteraction
        /// <inheritdoc/>
        IDiscordInteractionData IDiscordInteraction.Data => Data;

        //IApplicationCommandInteraction
        /// <inheritdoc/>
        IApplicationCommandInteractionData IApplicationCommandInteraction.Data => Data;
    }
}
