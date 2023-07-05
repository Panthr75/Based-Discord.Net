using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord
{
    /// <summary>
    ///     A field for an <see cref="Embed"/>.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct EmbedField : IEquatable<EmbedField>
    {
        /// <summary>
        ///     Gets the name of the field.
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        ///     Gets the value of the field.
        /// </summary>
        public string Value { get; internal set; }
        /// <summary>
        ///     Gets a value that indicates whether the field should be in-line with each other.
        /// </summary>
        public bool Inline { get; internal set; }

        internal EmbedField(string name, string value, bool inline)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Inline = inline;
        }

        private string DebuggerDisplay => $"{Name} ({Value}";
        /// <summary>
        ///     Gets the name of the field.
        /// </summary>
        /// <returns>
        ///     A string that resolves to <see cref="EmbedField.Name"/>.
        /// </returns>
        public override string ToString() => Name;

        public static bool operator ==(EmbedField? left, EmbedField? right)
            => left is null ? right is null
                : left.Equals(right);

        public static bool operator !=(EmbedField? left, EmbedField? right)
            => !(left == right);

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="EmbedField"/>.
        /// </summary>
        /// <remarks>
        /// If the object passes is an <see cref="EmbedField"/>, <see cref="Equals(EmbedField?)"/> will be called to compare the 2 instances
        /// </remarks>
        /// <param name="obj">The object to compare with the current object</param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
            => obj is EmbedField embedField && Equals(embedField);

        /// <inheritdoc cref="EmbedField.Equals(EmbedField)"/>
        public bool Equals([NotNullWhen(true)] EmbedField? embedField)
        {
            if (!embedField.HasValue)
            {
                return false;
            }
            return this.Equals(embedField.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="EmbedField"/> is equal to the current <see cref="EmbedField"/>
        /// </summary>
        /// <param name="embedField"></param>
        /// <returns></returns>
        public bool Equals(EmbedField embedField)
        {
            return this.Name == embedField.Name &&
                this.Value == embedField.Value &&
                this.Inline == embedField.Inline;
        }

        /// <inheritdoc />
        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.Value, this.Inline);
    }
}
