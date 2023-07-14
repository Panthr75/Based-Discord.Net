using System;
using System.Threading.Tasks;

namespace Discord.Shims
{
    public class RoleShim : SnowflakeShimEntity, IRoleShim
    {
        private readonly GuildShim m_guild;
        private string m_name;
        private Color m_color;
        private bool m_isHoisted;
        private bool m_isManaged;
        private bool m_isMentionable;
        private string? m_icon;
        private EmojiShim? m_emoji;
        private GuildPermissions m_permissions;
        private int m_position;
        private RoleTagsShim? m_tags;

        public RoleShim(GuildShim guild) : base(guild.Client)
        {
            this.m_guild = guild;
            this.m_name = "new role";
        }

        /// <inheritdoc cref="IRole.Guild"/>
        public virtual GuildShim Guild => this.m_guild;

        /// <inheritdoc cref="IRole.Color"/>
        public virtual Color Color
        {
            get => this.m_color;
            set => this.m_color = value;
        }

        /// <inheritdoc cref="IRole.IsHoisted"/>
        public virtual bool IsHoisted
        {
            get => this.m_isHoisted;
            set => this.m_isHoisted = value;
        }

        /// <inheritdoc cref="IRole.IsManaged"/>
        public virtual bool IsManaged
        {
            get => this.m_isManaged;
            set => this.m_isManaged = value;
        }

        /// <inheritdoc cref="IRole.IsMentionable"/>
        public virtual bool IsMentionable
        {
            get => this.m_isMentionable;
            set => this.m_isMentionable = value;
        }

        /// <inheritdoc cref="IRole.Name"/>
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                value = value?.Trim()!;
                if (string.IsNullOrEmpty(value))
                {
                    this.m_name = "new role";
                    return;
                }
                this.m_name = value.Substring(0, Math.Min(value.Length, 100));
            }
        }

        /// <inheritdoc cref="IRole.Icon"/>
        public virtual string? Icon
        {
            get => this.m_icon;
            set => this.m_icon = value;
        }

        /// <inheritdoc cref="IRole.Emoji"/>
        public virtual EmojiShim? Emoji
        {
            get => this.m_emoji;
            set => this.m_emoji = value;
        }

        /// <inheritdoc cref="IRole.Permissions"/>
        public virtual GuildPermissions Permissions
        {
            get => this.m_permissions;
            set => this.m_permissions = value;
        }

        /// <inheritdoc cref="IRole.Position"/>
        public virtual int Position
        {
            get => this.m_position;
            set => this.m_position = value;
        }

        /// <inheritdoc cref="IRole.Tags"/>
        public virtual RoleTagsShim? Tags
        {
            get => this.m_tags;
            set => this.m_tags = value;
        }

        public bool IsEveryone => this.Id == this.Guild.Id;

        public string Mention => this.IsEveryone ? "@everyone" : MentionUtils.MentionRole(this.Id);

        IGuildShim IRoleShim.Guild => this.m_guild;

        IGuild IRole.Guild => this.Guild;

        Emoji? IRole.Emoji => this.Emoji?.UnShim();

        RoleTags? IRole.Tags => this.Tags?.UnShim();

        public int CompareTo(IRole? other) => RoleUtils.Compare(this, other);

        public virtual Task DeleteAsync(RequestOptions? options = null)
        {
            this.Guild.DeleteRole(this.Id);
            return Task.CompletedTask;
        }

        public string? GetIconUrl()
        {
            return CDN.GetGuildRoleIconUrl(this.Id, this.Icon);
        }

        public virtual Task ModifyAsync(Action<RoleProperties> func, RequestOptions? options = null)
        {
            RoleProperties properties = new();
            func(properties);

            if (properties.Name.IsSpecified)
            {
                this.Name = properties.Name.Value;
            }
            if (properties.Color.IsSpecified)
            {
                this.m_color = properties.Color.Value;
            }
            if (properties.Emoji.IsSpecified)
            {
                this.m_emoji = new EmojiShim(properties.Emoji.Value);
            }
            if (properties.Hoist.IsSpecified)
            {
                this.m_isHoisted = properties.Hoist.Value;
            }
            if (properties.Mentionable.IsSpecified)
            {
                this.m_isMentionable = properties.Mentionable.Value;
            }
            if (properties.Permissions.IsSpecified)
            {
                this.m_permissions = properties.Permissions.Value;
            }
            if (properties.Position.IsSpecified)
            {
                this.m_position = properties.Position.Value;
            }

            return Task.CompletedTask;
        }
    }
}
