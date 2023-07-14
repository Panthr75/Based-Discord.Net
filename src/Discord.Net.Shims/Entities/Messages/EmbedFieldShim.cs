using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Discord.Shims
{
    /// <summary>
    /// A shim of <see cref="EmbedField"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmbedFieldShim : IConvertibleShim<EmbedField>, IEquatable<EmbedField>, IEquatable<EmbedFieldShim>
    {
        private string m_name;
        private string m_value;
        private bool m_inline;

        public EmbedFieldShim(string name, string value, bool inline = false)
        {
            Preconditions.NotNullOrWhitespace(name, nameof(name));
            Preconditions.NotNullOrWhitespace(value, nameof(value));

            this.m_name = string.Empty;
            this.m_value = string.Empty;

            this.Name = name;
            this.Value = value;
            this.Inline = inline;
        }

        public EmbedFieldShim(EmbedField embedField)
        {
            this.m_name = string.Empty;
            this.m_value = string.Empty;

            this.Apply(embedField);
        }

        public void Apply(EmbedField value)
        {
            this.m_name = value.Name;
            this.m_value = value.Value;
            this.m_inline = value.Inline;
        }

        public EmbedField UnShim()
        {
            return new EmbedField(this.m_name, this.m_value, this.m_inline);
        }

        /// <inheritdoc cref="EmbedField.Name"/>
        public virtual string Name
        {
            get => this.m_name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                value = value.Trim();
                this.m_name = value.Substring(0, Math.Min(value.Length, EmbedFieldBuilder.MaxFieldNameLength));
            }
        }
        /// <inheritdoc cref="EmbedField.Value"/>
        public virtual string Value
        {
            get => this.m_value;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }

                value = value.Trim();
                this.m_value = value.Substring(0, Math.Min(value.Length, EmbedFieldBuilder.MaxFieldValueLength));
            }
        }
        /// <inheritdoc cref="EmbedField.Inline"/>
        public virtual bool Inline
        {
            get => this.m_inline;
            set => this.m_inline = value;
        }

        private string DebuggerDisplay => $"{Name} ({Value}";
        /// <inheritdoc cref="EmbedField.ToString"/>
        public override string ToString() => Name;

        public static implicit operator EmbedField(EmbedFieldShim v)
        {
            return v.UnShim();
        }

        public static bool operator ==(EmbedFieldShim? left, EmbedFieldShim? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedFieldShim? left, EmbedFieldShim? right)
            => !(left == right);

        /// <inheritdoc cref="EmbedField.Equals(object?)"/>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EmbedFieldShim shim)
            {
                return this.Equals(shim);
            }
            else if (obj is EmbedField other)
            {
                return this.Equals(other);
            }
            return false;
        }

        /// <inheritdoc cref="EmbedField.Equals(EmbedField?)"/>
        public bool Equals([NotNullWhen(true)] EmbedField? embedField)
        {
            if (!embedField.HasValue)
            {
                return false;
            }
            return this.Equals(embedField.Value);
        }

        /// <inheritdoc cref="EmbedField.Equals(EmbedField)"/>
        public bool Equals(EmbedField embedField)
        {
            return this.Name == embedField.Name &&
                this.Value == embedField.Value &&
                this.Inline == embedField.Inline;
        }

        /// <inheritdoc cref="EmbedField.Equals(EmbedField)"/>
        public bool Equals([NotNullWhen(true)] EmbedFieldShim? embedField)
        {
            return embedField != null &&
                this.Name == embedField.Name &&
                this.Value == embedField.Value &&
                this.Inline == embedField.Inline;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.Value, this.Inline);
    }
}
