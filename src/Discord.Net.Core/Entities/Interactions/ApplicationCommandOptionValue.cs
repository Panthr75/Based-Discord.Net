using System;

namespace Discord
{
    /// <summary>
    /// The value for an application command option.
    /// </summary>
    public readonly struct ApplicationCommandOptionValue : IConvertible
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

        public string ToString(IFormatProvider? provider)
        {
            return this.ToString();
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

        bool IConvertible.ToBoolean(IFormatProvider? provider)
        {
            return this.ToBool();
        }

        byte IConvertible.ToByte(IFormatProvider? provider)
        {
            return System.Convert.ToByte(this.ToNumber(), provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider)
        {
            return System.Convert.ToSByte(this.ToNumber(), provider);
        }

        short IConvertible.ToInt16(IFormatProvider? provider)
        {
            return System.Convert.ToInt16(this.ToNumber(), provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider)
        {
            return System.Convert.ToUInt16(this.ToNumber(), provider);
        }

        int IConvertible.ToInt32(IFormatProvider? provider)
        {
            return System.Convert.ToInt32(this.ToNumber(), provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider)
        {
            return System.Convert.ToUInt32(this.ToNumber(), provider);
        }

        long IConvertible.ToInt64(IFormatProvider? provider)
        {
            return System.Convert.ToInt64(this.ToNumber(), provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider)
        {
            return System.Convert.ToUInt64(this.ToNumber(), provider);
        }

        double IConvertible.ToDouble(IFormatProvider? provider)
        {
            return System.Convert.ToDouble(this.ToNumber(), provider);
        }

        float IConvertible.ToSingle(IFormatProvider? provider)
        {
            return System.Convert.ToSingle(this.ToNumber(), provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider)
        {
            return System.Convert.ToDecimal(this.ToNumber(), provider);
        }

        char IConvertible.ToChar(IFormatProvider? provider)
        {
            return this.m_type switch
            {
                Type.None => throw new InvalidCastException("Cannot convert None value to char"),
                Type.Boolean => System.Convert.ToChar(this.m_booleanValue),
                Type.String => System.Convert.ToChar(this.m_stringValue),
                Type.Number => System.Convert.ToChar(this.m_numericValue),
                Type.User => System.Convert.ToChar(this.m_userValue),
                Type.Channel => System.Convert.ToChar(this.m_channelValue),
                Type.Role => System.Convert.ToChar(this.m_roleValue),
                Type.Mentionable => System.Convert.ToChar(this.m_mentionableValue),
                Type.Attachment => System.Convert.ToChar(this.m_attachmentValue),
                _ => throw new InvalidCastException()
            };
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider)
        {
            return this.m_type switch
            {
                Type.None => throw new InvalidCastException("Cannot convert None value to DateTime"),
                Type.Boolean => System.Convert.ToDateTime(this.m_booleanValue),
                Type.String => System.Convert.ToDateTime(this.m_stringValue),
                Type.Number => System.Convert.ToDateTime(this.m_numericValue),
                Type.User => System.Convert.ToDateTime(this.m_userValue),
                Type.Channel => System.Convert.ToDateTime(this.m_channelValue),
                Type.Role => System.Convert.ToDateTime(this.m_roleValue),
                Type.Mentionable => System.Convert.ToDateTime(this.m_mentionableValue),
                Type.Attachment => System.Convert.ToDateTime(this.m_attachmentValue),
                _ => throw new InvalidCastException()
            };
        }

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        object IConvertible.ToType(System.Type conversionType, IFormatProvider? provider)
        {
            IConvertible convertible = this;

            if (conversionType == typeof(string))
            {
                return convertible.ToString(provider); 
            }
            else if (conversionType == typeof(DateTime))
            {
                return convertible.ToDateTime(provider);
            }
            else if (conversionType == typeof(bool))
            {
                return convertible.ToBoolean(provider);
            }
            else if (conversionType == typeof(char))
            {
                return convertible.ToChar(provider);
            }
            else if (conversionType == typeof(decimal))
            {
                return convertible.ToDecimal(provider);
            }
            else if (conversionType == typeof(double))
            {
                return convertible.ToDouble(provider);
            }
            else if (conversionType == typeof(ushort))
            {
                return convertible.ToUInt16(provider);
            }
            else if (conversionType == typeof(uint))
            {
                return convertible.ToUInt32(provider);
            }
            else if (conversionType == typeof(ulong))
            {
                return convertible.ToUInt64(provider);
            }
            else if (conversionType == typeof(short))
            {
                return convertible.ToInt16(provider);
            }
            else if (conversionType == typeof(int))
            {
                return convertible.ToInt32(provider);
            }
            else if (conversionType == typeof(long))
            {
                return convertible.ToInt64(provider);
            }
            else if (conversionType == typeof(sbyte))
            {
                return convertible.ToSByte(provider);
            }
            else if (conversionType == typeof(float))
            {
                return convertible.ToSingle(provider);
            }
            else if (typeof(IChannel).IsAssignableFrom(conversionType))
            {
                return System.Convert.ChangeType(this.m_channelValue!, conversionType);
            }
            else if (typeof(IRole).IsAssignableFrom(conversionType))
            {
                return System.Convert.ChangeType(this.m_roleValue!, conversionType);
            }
            else if (typeof(IAttachment).IsAssignableFrom(conversionType))
            {
                return System.Convert.ChangeType(this.m_attachmentValue!, conversionType);
            }
            else if (typeof(IUser).IsAssignableFrom(conversionType))
            {
                return System.Convert.ChangeType(this.m_userValue!, conversionType);
            }
            else if (typeof(IMentionable).IsAssignableFrom(conversionType))
            {
                return System.Convert.ChangeType(this.m_mentionableValue!, conversionType);
            }
            else
            {
                throw new InvalidCastException("Cannot convert ApplicationCommandOptionValue to " + conversionType.FullName);
            }
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
