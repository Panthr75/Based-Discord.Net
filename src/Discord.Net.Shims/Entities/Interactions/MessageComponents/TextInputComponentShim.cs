using System;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="TextInputComponent"/>
    /// </summary>
    public class TextInputComponentShim : IMessageComponentShim, IConvertibleShim<TextInputComponent>, IEquatable<TextInputComponent>, IEquatable<TextInputComponentShim>
    {
        private string m_customId;
        private string m_label;
        private string? m_placeholder;
        private int? m_minLength;
        private int? m_maxLength;
        private TextInputStyle m_style;
        private bool? m_required;
        private string? m_value;

        public TextInputComponentShim(string label, string customId)
        {
            Preconditions.NotNull(label, nameof(label));
            Preconditions.NotNull(customId, nameof(customId));

            label = label.Trim();

            this.m_label = label.Substring(0, Math.Min(label.Length, TextInputBuilder.MaxLabelLength));
            this.m_customId = customId.Substring(0, Math.Min(customId.Length, ComponentBuilder.MaxCustomIdLength));
        }

        public TextInputComponentShim(TextInputComponent component)
        {
            Preconditions.NotNull(component, nameof(component));
            this.m_label = string.Empty;
            this.m_customId = string.Empty;

            this.Apply(component);
        }

        void IConvertibleShim<IMessageComponent>.Apply(IMessageComponent value)
        {
            if (value is not TextInputComponent component)
            {
                return;
            }
            this.Apply(component);
        }

        public void Apply(TextInputComponent component)
        {
            if (component is null)
            {
                return;
            }

            this.m_customId = component.CustomId;
            this.m_label = component.Label;
            this.m_minLength = component.MinLength;
            this.m_maxLength = component.MaxLength;
            this.m_placeholder = component.Placeholder;
            this.m_required = component.Required;
            this.m_style = component.Style;
            this.m_value = component.Value;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is TextInputComponentShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is TextInputComponent other)
            {
                return this.Equals(other);
            }
            return false;
        }

        bool IEquatable<IMessageComponent>.Equals([NotNullWhen(true)] IMessageComponent? other)
        {
            return other is TextInputComponent component && this.Equals(component);
        }

        bool IEquatable<IMessageComponentShim>.Equals([NotNullWhen(true)] IMessageComponentShim? other)
        {
            return other is TextInputComponentShim component && this.Equals(component);
        }

        public override int GetHashCode() => base.GetHashCode();

        public TextInputComponent UnShim()
        {
            return new TextInputComponent(
                customId: this.CustomId,
                label: this.Label,
                placeholder: this.Placeholder,
                minLength: this.MinLength,
                maxLength: this.MaxLength,
                style: this.Style,
                required: this.Required,
                value: this.Value);
        }
        
        IMessageComponent IConvertibleShim<IMessageComponent>.UnShim()
        {
            return this.UnShim();
        }

        public bool Equals([NotNullWhen(true)] TextInputComponentShim? other)
        {
            return other != null &&
                this.CustomId == other.CustomId &&
                this.Label == other.Label &&
                this.MaxLength == other.MaxLength &&
                this.MinLength == other.MinLength &&
                this.Placeholder == other.Placeholder &&
                this.Required == other.Required &&
                this.Style == other.Style &&
                this.Value == other.Value;
        }

        public bool Equals([NotNullWhen(true)] TextInputComponent? other)
        {
            return other != null &&
                this.CustomId == other.CustomId &&
                this.Label == other.Label &&
                this.MaxLength == other.MaxLength &&
                this.MinLength == other.MinLength &&
                this.Placeholder == other.Placeholder &&
                this.Required == other.Required &&
                this.Style == other.Style &&
                this.Value == other.Value;
        }

        ComponentType IMessageComponent.Type => ComponentType.TextInput;

        /// <inheritdoc cref="TextInputComponent.CustomId"/>
        public virtual string CustomId
        {
            get => this.m_customId;
            set
            {
                Preconditions.NotNull(value, nameof(value));
                value = value.Trim();
                this.m_customId = value.Substring(0, Math.Min(value.Length, ComponentBuilder.MaxCustomIdLength));
            }
        }

        /// <inheritdoc cref="TextInputComponent.Label"/>
        public virtual string Label
        {
            get => this.m_label;
            set
            {
                Preconditions.NotNull(value, nameof(value));
                value = value.Trim();
                this.m_label = value.Substring(0, Math.Min(value.Length, TextInputBuilder.MaxLabelLength));
            }
        }

        /// <inheritdoc cref="TextInputComponent.Placeholder"/>
        public virtual string? Placeholder
        {
            get => this.m_placeholder;
            set
            {
                if (value is null)
                {
                    this.m_placeholder = null;
                    return;
                }

                value = value.Trim();
                this.m_placeholder = value.Substring(0, Math.Min(value.Length, TextInputBuilder.MaxPlaceholderLength));
            }
        }

        /// <inheritdoc cref="TextInputComponent.MinLength"/>
        public virtual int? MinLength
        {
            get => this.m_minLength;
            set
            {
                if (!value.HasValue)
                {
                    this.m_minLength = null;
                    return;
                }

                int v = value.Value;
                int? maxLength = this.m_maxLength;
                int upperBound = maxLength.HasValue ? maxLength.Value : TextInputBuilder.LargestMaxLength;

                this.m_minLength = Math.Clamp(v, 0, upperBound - 1);
            }
        }

        /// <inheritdoc cref="TextInputComponent.MaxLength"/>
        public virtual int? MaxLength
        {
            get => this.m_maxLength;
            set
            {
                if (!value.HasValue)
                {
                    this.m_maxLength = null;
                    return;
                }

                int v = value.Value;
                int? minLength = this.m_minLength;
                int lowerBound = minLength.HasValue ? minLength.Value : 0;

                this.m_maxLength = Math.Clamp(v, lowerBound + 1, TextInputBuilder.LargestMaxLength);
            }
        }

        /// <inheritdoc cref="TextInputComponent.Style"/>
        public virtual TextInputStyle Style
        {
            get => this.m_style;
            set => this.m_style = value;
        }

        /// <inheritdoc cref="TextInputComponent.Required"/>
        public virtual bool? Required
        {
            get => this.m_required;
            set => this.m_required = value;
        }

        /// <inheritdoc cref="TextInputComponent.Value"/>
        public virtual string? Value
        {
            get => this.m_value;
            set
            {
                if (value is null)
                {
                    this.m_value = null;
                    return;
                }

                this.m_value = value.Substring(0, this.m_maxLength ?? 0);
            }
        }

        public static explicit operator TextInputComponent(TextInputComponentShim v)
        {
            return v.UnShim();
        }
    }
}
