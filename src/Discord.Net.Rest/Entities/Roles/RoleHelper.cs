using System;
using System.Threading.Tasks;
using BulkParams = Discord.API.Rest.ModifyGuildRolesParams;
using Model = Discord.API.Role;

namespace Discord.Rest
{
    internal static class RoleHelper
    {
        #region General
        public static Task DeleteAsync(IGuild guild, ulong roleId, BaseDiscordClient client,
            RequestOptions? options)
            => client.ApiClient.DeleteGuildRoleAsync(guild.Id, roleId, options);

        public static async Task<Model> ModifyAsync(IGuild guild, ulong roleId, BaseDiscordClient client,
            Action<RoleProperties> func, RequestOptions? options = null)
        {
            Preconditions.NotNull(func, nameof(func));

            var args = new RoleProperties();
            func(args);

            if (args.Icon.IsSpecified || args.Emoji.IsSpecified)
            {
               guild.Features.EnsureFeature(GuildFeature.RoleIcons);

                if (args.Icon.IsSpecified && args.Icon.Value != null && args.Emoji.IsSpecified && args.Emoji.Value != null)
                {
                    throw new ArgumentException("Emoji and Icon properties cannot be present on a role at the same time.");
                }
            }

            var apiArgs = new API.Rest.ModifyGuildRoleParams
            {
                Color = args.Color.IsSpecified ? args.Color.Value.RawValue : Optional.Create<uint>(),
                Hoist = args.Hoist,
                Mentionable = args.Mentionable,
                Name = args.Name,
                Permissions = args.Permissions.IsSpecified ? args.Permissions.Value.RawValue.ToString() : Optional.Create<string>(),
                Icon = args.Icon.IsSpecified ? args.Icon.Value?.ToModel() ?? null : Optional<API.Image?>.Unspecified,
                Emoji = args.Emoji.IsSpecified ? args.Emoji.Value?.Name ?? "" : Optional.Create<string>(),
            };

            if (args.Icon.IsSpecified && args.Icon.Value != null)
            {
                apiArgs.Emoji = "";
            }
            else if (args.Emoji.IsSpecified && args.Emoji.Value != null)
            {
                apiArgs.Icon = Optional<API.Image?>.Unspecified;
            }

            var model = await client.ApiClient.ModifyGuildRoleAsync(guild.Id, roleId, apiArgs, options).ConfigureAwait(false);

            if (args.Position.IsSpecified)
            {
                var bulkArgs = new[] { new BulkParams(roleId, args.Position.Value) };
                await client.ApiClient.ModifyGuildRolesAsync(guild.Id, bulkArgs, options).ConfigureAwait(false);
                model.Position = args.Position.Value;
            }
            return model;
        }
        #endregion
    }
}
