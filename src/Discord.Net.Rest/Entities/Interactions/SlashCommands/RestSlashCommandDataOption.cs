using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = Discord.API.ApplicationCommandInteractionDataOption;


namespace Discord.Rest
{
    /// <summary>
    ///     Represents a REST-based option for a slash command.
    /// </summary>
    public class RestSlashCommandDataOption : IApplicationCommandInteractionDataOption
    {
        #region RestSlashCommandDataOption
        /// <inheritdoc/>
        public string Name { get; private set; }

        /// <inheritdoc/>
        public ApplicationCommandOptionValue Value { get; private set; }

        /// <inheritdoc/>
        public ApplicationCommandOptionType Type { get; private set; }

        /// <summary>
        ///      Gets a collection of sub command options received for this sub command group.
        /// </summary>
        public IReadOnlyCollection<RestSlashCommandDataOption> Options { get; private set; }

        internal RestSlashCommandDataOption()
        {
            this.Name = string.Empty;
            this.Options = ImmutableArray<RestSlashCommandDataOption>.Empty;
        }
        internal RestSlashCommandDataOption(RestSlashCommandData data, Model model)
        {
            Name = model.Name;
            Type = model.Type;

            if (model.Value.IsSpecified)
            {
                switch (Type)
                {
                    case ApplicationCommandOptionType.User:
                    case ApplicationCommandOptionType.Role:
                    case ApplicationCommandOptionType.Channel:
                    case ApplicationCommandOptionType.Mentionable:
                    case ApplicationCommandOptionType.Attachment:
                        if (ulong.TryParse($"{model.Value.Value}", out var valueId))
                        {
                            switch (Type)
                            {
                                case ApplicationCommandOptionType.User:
                                    {
                                        var guildUser = data.ResolvableData!.GuildMembers.FirstOrDefault(x => x.Key == valueId).Value;

                                        if (guildUser != null)
                                            Value = ApplicationCommandOptionValue.Convert(guildUser);
                                        else
                                            Value = ApplicationCommandOptionValue.Convert(data.ResolvableData!.Users.FirstOrDefault(x => x.Key == valueId).Value);
                                    }
                                    break;
                                case ApplicationCommandOptionType.Channel:
                                    Value = ApplicationCommandOptionValue.Convert(data.ResolvableData!.Channels.FirstOrDefault(x => x.Key == valueId).Value);
                                    break;
                                case ApplicationCommandOptionType.Role:
                                    Value = ApplicationCommandOptionValue.Convert(data.ResolvableData!.Roles.FirstOrDefault(x => x.Key == valueId).Value);
                                    break;
                                case ApplicationCommandOptionType.Mentionable:
                                    {
                                        if (data.ResolvableData!.GuildMembers.Any(x => x.Key == valueId) || data.ResolvableData!.Users.Any(x => x.Key == valueId))
                                        {
                                            var guildUser = data.ResolvableData.GuildMembers.FirstOrDefault(x => x.Key == valueId).Value;

                                            if (guildUser != null)
                                                Value = ApplicationCommandOptionValue.Convert(guildUser);
                                            else
                                                Value = ApplicationCommandOptionValue.Convert(data.ResolvableData!.Users.FirstOrDefault(x => x.Key == valueId).Value);
                                        }
                                        else if (data.ResolvableData!.Roles.Any(x => x.Key == valueId))
                                        {
                                            Value = ApplicationCommandOptionValue.Convert(data.ResolvableData.Roles.FirstOrDefault(x => x.Key == valueId).Value);
                                        }
                                    }
                                    break;
                                case ApplicationCommandOptionType.Attachment:
                                    Value = ApplicationCommandOptionValue.Convert(data.ResolvableData!.Attachments.FirstOrDefault(x => x.Key == valueId).Value);
                                    break;
                                default:
                                    try
                                    {
                                        Value = ApplicationCommandOptionValue.Convert(model.Value.Value);
                                    }
                                    catch (InvalidCastException)
                                    {
                                        Value = ApplicationCommandOptionValue.None;
                                    }
                                    break;
                            }
                        }
                        break;
                    case ApplicationCommandOptionType.String:
                        Value = model.Value.GetValueOrDefault().ToString();
                        break;
                    case ApplicationCommandOptionType.Integer:
                    case ApplicationCommandOptionType.Number:
                        Value = model.Value.GetValueOrDefault().ToNumber();
                        break;
                    case ApplicationCommandOptionType.Boolean:
                        Value = model.Value.GetValueOrDefault().ToBool();
                        break;
                }
            }

            Options = model.Options.IsSpecified
                ? model.Options.Value.Select(x => new RestSlashCommandDataOption(data, x)).ToImmutableArray()
                : ImmutableArray.Create<RestSlashCommandDataOption>();
        }
        #endregion

        #region Converters
        public static explicit operator bool(RestSlashCommandDataOption option)
            => (bool)option.Value;
        public static explicit operator int(RestSlashCommandDataOption option)
            => (int)option.Value.ToNumber();
        public static explicit operator string(RestSlashCommandDataOption option)
            => (string)option.Value;
        #endregion

        #region IApplicationCommandInteractionDataOption
        IReadOnlyCollection<IApplicationCommandInteractionDataOption> IApplicationCommandInteractionDataOption.Options
            => Options;
        #endregion
    }
}
