using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of User
    /// </summary>
    public class UserShim : ShimEntity<ulong>, IUserShim
    {
        private ushort m_discriminator;
        private string m_username;
        private string? m_globalName;
        private string? m_pronouns;
        private readonly List<ClientType> m_activeClients;
        private readonly List<IActivityShim> m_activities;

        public UserShim(IDiscordClientShim client) : base(client, ShimUtils.GenerateSnowflake())
        {
            this.m_username = string.Empty;
            this.m_activeClients = new();
            this.m_activities = new();
        }

        public UserShim(IDiscordClientShim client) : base(client, ShimUtils.GenerateSnowflake())
        {
            this.m_username = string.Empty;
            this.m_activeClients = new();
            this.m_activities = new();
        }

        /// <inheritdoc cref="IUser.AvatarId"/>
        public string? AvatarId { get; set; }

        /// <inheritdoc cref="IUser.Discriminator"/>
        public string Discriminator
        {
            get => this.DiscriminatorValue.ToString("D4");
            set
            {
                if (!ushort.TryParse(value, out ushort v))
                {
                    return;
                }

                this.DiscriminatorValue = v;
            }
        }

        /// <inheritdoc cref="IUser.DiscriminatorValue"/>
        public ushort DiscriminatorValue
        {
            get => this.m_discriminator;
            set
            {
                value = (ushort)Math.Min((int)value, 9999);
                this.m_discriminator = value;
            }
        }

        /// <inheritdoc cref="IUser.AvatarId"/>
        public bool IsBot { get; set; }

        /// <inheritdoc cref="IUser.IsWebhook"/>
        public bool IsWebhook => false;

        /// <inheritdoc cref="IUser.Username"/>
        public string Username
        {
            get => this.m_username;
            set
            {
                value = value?.Trim()!;
                if (string.IsNullOrEmpty(value) || value.Length < 2)
                {
                    this.m_username = string.Empty;
                    return;
                }

                if (!this.IsBot)
                {
                    value = value.ToLower();
                    StringBuilder newValueBuilder = new();
                    bool prevIsPeriod = false;
                    foreach (char c in value)
                    {
                        if (!char.IsDigit(c) && !char.IsLetter(c) && c != '_')
                        {
                            if (c == ' ')
                            {
                                prevIsPeriod = false;
                                newValueBuilder.Append('_');
                            }
                            else if (c == '.')
                            {
                                if (prevIsPeriod)
                                {
                                    prevIsPeriod = false;
                                    continue;
                                }
                                prevIsPeriod = true;
                            }
                            else
                            {
                                prevIsPeriod = false;
                                continue;
                            }
                        }

                        newValueBuilder.Append(c);
                    }
                    value = newValueBuilder.ToString();
                }

                value = value.Substring(0, Math.Min(value.Length, 32));

                this.m_username = value;
            }
        }

        /// <inheritdoc cref="IUser.PublicFlags"/>
        public UserProperties? PublicFlags { get; set; }

        /// <inheritdoc cref="IUser.GlobalName"/>
        public string? GlobalName
        {
            get => this.m_globalName;
            set
            {
                value = value?.Trim()!;
                if (string.IsNullOrEmpty(value))
                {
                    this.m_globalName = null;
                    return;
                }

                value = value.Substring(0, Math.Min(value.Length, 32));
                this.m_globalName = value;
            }
        }

        /// <inheritdoc cref="IUser.Pronouns"/>
        public string? Pronouns
        {
            get => this.m_pronouns;
            set
            {
                if (value == null)
                {
                    this.m_pronouns = null;
                    return;
                }

                value = value.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    this.m_pronouns = string.Empty;
                }

                this.m_pronouns = value.Substring(0, Math.Min(value.Length, 40));
            }
        }

        /// <inheritdoc cref="ISnowflakeEntity.CreatedAt"/>
        public DateTimeOffset CreatedAt => SnowflakeUtils.FromSnowflake(this.Id);

        /// <inheritdoc cref="IMentionable.Mention"/>
        public string Mention => MentionUtils.MentionUser(this.Id);

        /// <inheritdoc cref="IPresence.Status"/>
        public UserStatus Status { get; set; }

        /// <inheritdoc cref="IPresence.ActiveClients"/>
        public List<ClientType> ActiveClients => this.m_activeClients;

        /// <inheritdoc cref="IPresence.Activities"/>
        public List<IActivityShim> Activities => this.m_activities;

        /// <inheritdoc/>
        public virtual Task<IDMChannel> CreateDMChannelAsync(RequestOptions? options = null)
        {
            return Task.FromException<IDMChannel>(new NotImplementedException());
        }

        /// <inheritdoc/>
        public string? GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            return CDN.GetUserAvatarUrl(this.Id, this.AvatarId, size, format);
        }
        /// <inheritdoc/>
        public string GetDefaultAvatarUrl()
        {
            return (this.DiscriminatorValue != 0) ? CDN.GetDefaultUserAvatarUrl(this.DiscriminatorValue) : CDN.GetDefaultUserAvatarUrl(this.Id);
        }

        ICollection<IActivityShim> IPresenceShim.Activities => this.Activities;
        ICollection<ClientType> IPresenceShim.ActiveClients => this.ActiveClients;

        IReadOnlyCollection<ClientType> IPresence.ActiveClients => this.ActiveClients;
        IReadOnlyCollection<IActivity> IPresence.Activities => this.Activities;
    }
}
