using System;

namespace Discord
{
    /// <summary>
    /// The value for an application command option.
    /// </summary>
    public readonly struct ApplicationCommandOptionValue
    {
        private readonly bool m_booleanValue;
        private readonly string m_stringValue;
        private readonly NumericValue m_numericValue;
        private readonly IUser? m_userValue;
        private readonly IChannel? m_channelValue;
        private readonly IRole? m_roleValue;
        private readonly IMentionable? m_mentionableValue;
        private readonly IAttachment? m_attachmentValue;
        private readonly Type m_type;

        private ApplicationCommandOptionValue(IUser user)
        {
            this.m_type = Type.User;
            this.m_booleanValue = false;
            this.m_stringValue = MentionUtils.MentionUser(user.Id);
            this.m_numericValue = user.Id;
            this.m_userValue = user;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = user;
            this.m_attachmentValue = null;
        }
        private ApplicationCommandOptionValue(IChannel channel)
        {
            this.m_type = Type.Channel;
            this.m_booleanValue = false;
            this.m_stringValue = MentionUtils.MentionChannel(channel.Id);
            this.m_numericValue = channel.Id;
            this.m_userValue = null;
            this.m_channelValue = channel;
            this.m_roleValue = null;
            this.m_mentionableValue = channel as IMentionable;
            this.m_attachmentValue = null;
        }
        private ApplicationCommandOptionValue(IRole role)
        {
            this.m_type = Type.Role;
            this.m_booleanValue = false;
            this.m_stringValue = MentionUtils.MentionRole(role.Id);
            this.m_numericValue = role.Id;
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = role;
            this.m_mentionableValue = role;
            this.m_attachmentValue = null;
        }
        private ApplicationCommandOptionValue(IMentionable mentionable)
        {
            this.m_type = Type.Mentionable;
            this.m_booleanValue = false;
            this.m_stringValue = mentionable.Mention;
            this.m_numericValue = 0;
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = mentionable;
            this.m_attachmentValue = null;
        }

        private ApplicationCommandOptionValue(bool value)
        {
            this.m_type = Type.Boolean;
            this.m_booleanValue = value;
            this.m_stringValue = value.ToString();
            this.m_numericValue = value ? 1 : 0;
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = null;
            this.m_attachmentValue = null;
        }

        private ApplicationCommandOptionValue(string value)
        {

            this.m_type = Type.String;
            bool.TryParse(value, out this.m_booleanValue);
            NumericValue.TryParse(value, out this.m_numericValue);
            this.m_stringValue = value;
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = null;
            this.m_attachmentValue = null;
        }

        private ApplicationCommandOptionValue(NumericValue value)
        {
            this.m_type = Type.Number;
            this.m_booleanValue = value != 0;
            this.m_numericValue = value;
            this.m_stringValue = value.ToString();
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = null;
            this.m_attachmentValue = null;
        }

        private ApplicationCommandOptionValue(IAttachment value)
        {
            this.m_type = Type.Attachment;
            this.m_booleanValue = false;
            this.m_numericValue = value.Id;
            this.m_stringValue = value.Url;
            this.m_userValue = null;
            this.m_channelValue = null;
            this.m_roleValue = null;
            this.m_mentionableValue = null;
            this.m_attachmentValue = value;
        }

        public bool IsNone => this.m_type == Type.None;
        public bool IsString => this.m_type == Type.String || this.m_type == Type.User || this.m_type == Type.Channel || this.m_type == Type.Role || this.m_type == Type.Mentionable || this.m_type == Type.Attachment;
        public bool IsNumber => this.m_type == Type.Number;
        public bool IsBool => this.m_type == Type.Boolean;
        public bool IsUser => this.m_type == Type.User || (this.IsMentionable && this.m_mentionableValue is IUser);
        public bool IsChannel => this.m_type == Type.Channel || (this.IsMentionable && this.m_mentionableValue is IChannel);
        public bool IsRole => this.m_type == Type.Role || (this.IsMentionable && this.m_mentionableValue is IRole);
        public bool IsMentionable => this.m_type == Type.Mentionable;
        public bool IsAttachment => this.m_type == Type.Attachment;

        public static readonly ApplicationCommandOptionValue None = new();

        public override string ToString()
        {
            return this.m_stringValue ?? string.Empty;
        }

        public NumericValue ToNumber()
        {
            return this.m_numericValue;
        }

        public bool ToBool()
        {
            return this.m_booleanValue;
        }
        /// <exception cref="InvalidOperationException"></exception>
        public IUser GetUser()
        {
            if (this.m_type == Type.User && this.m_userValue is not null)
            {
                return this.m_userValue;
            }
            else if (this.IsMentionable && this.m_mentionableValue is IUser user)
            {
                return user;
            }
            throw new InvalidOperationException();
        }
        /// <exception cref="InvalidOperationException"></exception>
        public IChannel GetChannel()
        {
            if (this.m_type == Type.Channel && this.m_channelValue is not null)
            {
                return this.m_channelValue;
            }
            else if (this.IsMentionable && this.m_mentionableValue is IChannel channel)
            {
                return channel;
            }
            throw new InvalidOperationException();
        }
        /// <exception cref="InvalidOperationException"></exception>
        public IRole GetRole()
        {
            if (this.m_type == Type.Role && this.m_roleValue is not null)
            {
                return this.m_roleValue;
            }
            else if (this.IsMentionable && this.m_mentionableValue is IRole role)
            {
                return role;
            }
            throw new InvalidOperationException();
        }
        /// <exception cref="InvalidOperationException"></exception>
        public IMentionable GetMentionable()
        {
            if (this.IsMentionable && this.m_mentionableValue is not null)
            {
                return this.m_mentionableValue;
            }
            throw new InvalidOperationException();
        }
        /// <exception cref="InvalidOperationException"></exception>
        public IAttachment GetAttachment()
        {
            if (this.IsAttachment && this.m_attachmentValue is not null)
            {
                return this.m_attachmentValue;
            }
            throw new InvalidOperationException();
        }

        public static implicit operator ApplicationCommandOptionValue(string value)
        {
            if (value is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(value);
        }

        public static implicit operator ApplicationCommandOptionValue(NumericValue value)
        {
            return new ApplicationCommandOptionValue(value);
        }

        public static implicit operator ApplicationCommandOptionValue(bool value)
        {
            return new ApplicationCommandOptionValue(value);
        }

        public static ApplicationCommandOptionValue Convert(IUser user)
        {
            if (user is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(user);
        }

        public static ApplicationCommandOptionValue Convert(IRole role)
        {
            if (role is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(role);
        }

        public static ApplicationCommandOptionValue Convert(IChannel channel)
        {
            if (channel is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(channel);
        }

        public static ApplicationCommandOptionValue Convert(IMentionable mentionable)
        {
            if (mentionable is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(mentionable);
        }

        public static ApplicationCommandOptionValue Convert(IAttachment attachment)
        {
            if (attachment is null)
            {
                return ApplicationCommandOptionValue.None;
            }

            return new ApplicationCommandOptionValue(attachment);
        }

        /// <summary>
        /// Converts the given value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="InvalidCastException"><paramref name="value"/> cannot be converted.</exception>
        public static ApplicationCommandOptionValue Convert(object? value)
        {
            if (value is null)
            {
                return ApplicationCommandOptionValue.None;
            }
            else if (value is string str)
            {
                return str;
            }
            else if (value is NumericValue numericValue)
            {
                return numericValue;
            }
            else if (value is bool boolValue)
            {
                return boolValue;
            }
            else if (value is IUser user)
            {
                return new ApplicationCommandOptionValue(user);
            }
            else if (value is IChannel channel)
            {
                return new ApplicationCommandOptionValue(channel);
            }
            else if (value is IRole role)
            {
                return new ApplicationCommandOptionValue(role);
            }
            else if (value is IMentionable mentionable)
            {
                return new ApplicationCommandOptionValue(mentionable);
            }
            else if (value is IAttachment attachment)
            {
                return new ApplicationCommandOptionValue(attachment);
            }
            else
            {
                return NumericValue.Convert(value);
            }

        }

        public static explicit operator string(ApplicationCommandOptionValue value)
        {
            return value.ToString();
        }

        public static explicit operator NumericValue(ApplicationCommandOptionValue value)
        {
            return value.ToNumber();
        }

        public static explicit operator bool(ApplicationCommandOptionValue value)
        {
            return value.ToBool();
        }

        private enum Type : byte
        {
            None,
            Boolean,
            String,
            Number,
            User,
            Channel,
            Role,
            Mentionable,
            Attachment,
        }
    }
}
