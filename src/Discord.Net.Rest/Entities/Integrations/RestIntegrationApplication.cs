using Model = Discord.API.IntegrationApplication;

namespace Discord.Rest
{
    /// <summary>
    ///     Represents a Rest-based implementation of <see cref="IIntegrationApplication"/>.
    /// </summary>
    public class RestIntegrationApplication : RestEntity<ulong>, IIntegrationApplication
    {
        public string Name { get; private set; }

        public string? Icon { get; private set; }

        public string Description { get; private set; }

        public IUser Bot { get; private set; }

        internal RestIntegrationApplication(BaseDiscordClient discord, ulong id)
            : base(discord, id)
        {
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Bot = null!;
        }

        internal static RestIntegrationApplication Create(BaseDiscordClient discord, Model model)
        {
            var entity = new RestIntegrationApplication(discord, model.Id);
            entity.Update(model);
            return entity;
        }

        internal void Update(Model model)
        {
            Name = model.Name;
            Icon = model.Icon.IsSpecified ? model.Icon.Value : null;
            Description = model.Description;
            Bot = RestUser.Create(Discord, model.Bot.Value);
        }
    }
}
