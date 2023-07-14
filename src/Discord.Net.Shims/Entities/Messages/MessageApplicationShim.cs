using System.Diagnostics;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="MessageApplication"/>
    /// </summary>
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public class MessageApplicationShim : IConvertibleShim<MessageApplication>
    {
        private ulong m_id;
        private string? m_coverImage;
        private string m_description;
        private string? m_icon;
        private string m_name;

        public MessageApplicationShim(string name, string description)
        {
            Preconditions.NotNullOrWhitespace(name, nameof(name));
            Preconditions.NotNullOrWhitespace(description, nameof(description));

            this.m_name = name.Trim();
            this.m_description = description.Trim();
        }

        public MessageApplicationShim(MessageApplication messageApplication)
        {
            Preconditions.NotNull(messageApplication, nameof(messageApplication));

            this.m_name = string.Empty;
            this.m_description = string.Empty;

            this.Apply(messageApplication);
        }

        public void Apply(MessageApplication value)
        {
            if (value is null)
            {
                return;
            }

            this.m_name = value.Name;
            this.m_description = value.Description;
            this.m_icon = value.Icon;
            this.m_coverImage = value.CoverImage;
            this.m_id = value.Id;
        }

        public MessageApplication UnShim()
        {
            return new MessageApplication()
            {
                CoverImage = this.CoverImage,
                Description = this.Description,
                Icon = this.Icon,
                Id = this.Id,
                Name = this.Name,
            };
        }

        public virtual ulong Id
        {
            get => this.m_id;
            set => this.m_id = value;
        }
        public virtual string? CoverImage
        {
            get => this.m_coverImage;
            set => this.m_coverImage = value;
        }
        public virtual string Description
        {
            get => this.m_description;
            set
            {
                value = value?.Trim()!;
                Preconditions.NotNullOrEmpty(value, nameof(value));
                this.m_description = value;
            }
        }
        public virtual string? Icon
        {
            get => this.m_icon;
            set => this.m_icon = value;
        }
        public string? IconUrl
            => string.IsNullOrEmpty(Icon) ? null : $"https://cdn.discordapp.com/app-icons/{Id}/{Icon}";
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                value = value?.Trim()!;
                Preconditions.NotNullOrEmpty(value, nameof(value));
                this.m_name = value;
            }
        }
        private string DebuggerDisplay
            => $"{Name} ({Id}): {Description}";
        /// <inheritdoc cref="MessageApplication.ToString()"/>
        public override string ToString()
            => DebuggerDisplay;

        public static implicit operator MessageApplication(MessageApplicationShim v)
        {
            return v.UnShim();
        }
    }
}
