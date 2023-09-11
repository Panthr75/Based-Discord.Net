using System;
using System.Diagnostics;
using System.Linq;
using Model = Discord.API.User;

namespace Discord.WebSocket
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    internal class SocketGlobalUser : SocketUser
    {
        /// <inheritdoc/>
        public override bool IsBot { get; internal set; }
        /// <inheritdoc/>
        public override string Username { get; internal set; }
        /// <inheritdoc/>
        public override ushort DiscriminatorValue { get; internal set; }
        /// <inheritdoc/>
        public override string? AvatarId { get; internal set; }
        /// <inheritdoc/>
        public override string? GlobalName { get { return GlobalUser.GlobalName; } internal set { GlobalUser.GlobalName = value; } }
        internal override SocketPresence? Presence { get; set; }

        /// <inheritdoc/>
        public override bool IsWebhook => false;
        internal override SocketGlobalUser GlobalUser { get => this; set => throw new NotImplementedException(); }

        private readonly object _lockObj = new object();
        private ushort _references;

        private SocketGlobalUser(DiscordSocketClient discord, ulong id)
            : base(discord, id)
        {
            this.Username = string.Empty;
        }
        internal static SocketGlobalUser Create(DiscordSocketClient discord, ClientState state, Model model)
        {
            var entity = new SocketGlobalUser(discord, model.Id);
            entity.Update(state, model);
            return entity;
        }

        internal void AddRef()
        {
            checked
            {
                lock (_lockObj)
                    _references++;
            }
        }
        internal void RemoveRef(DiscordSocketClient discord)
        {
            lock (_lockObj)
            {
                if (--_references <= 0)
                    discord.RemoveUser(Id);
            }
        }

        private string DebuggerDisplay => DiscriminatorValue != 0
            ? $"{Username}#{Discriminator} ({Id}{(IsBot ? ", Bot" : "")}, Guild)"
            : $"{Username} ({Id}{(IsBot ? ", Bot" : "")}, Guild)";

        internal new SocketGlobalUser Clone() => (SocketGlobalUser)MemberwiseClone();
    }
}
