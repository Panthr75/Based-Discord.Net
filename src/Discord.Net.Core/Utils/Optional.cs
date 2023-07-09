using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;

namespace Discord
{
    interface IOptional
    {
        bool IsSpecified { get; }
        object? GetValueOrDefault();
    }

    //Based on https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Nullable.cs
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public readonly struct Optional<T> : IOptional
    {
        public static Optional<T> Unspecified => default;
        private readonly T? _value;

        /// <summary> Gets the value for this parameter. </summary>
        /// <exception cref="InvalidOperationException" accessor="get">This property has no value set.</exception>
        public T Value
        {
            get
            {
                if (!IsSpecified)
                    throw new InvalidOperationException("This property has no value set.");
                return _value!;
            }
        }
        /// <summary> Returns true if this value has been specified. </summary>
        public bool IsSpecified { get; }

        /// <summary> Creates a new Parameter with the provided value. </summary>
        public Optional(T value)
        {
            _value = value;
            IsSpecified = true;
        }

        public T? GetValueOrDefault() => _value;
        [return: NotNullIfNotNull(nameof(defaultValue))]
        public T? GetValueOrDefault(T? defaultValue) => (IsSpecified ? _value : defaultValue)!;

        object? IOptional.GetValueOrDefault() => this.GetValueOrDefault();

        public Optional<U> Map<U>(Func<T, U> mapper)
        {
            if (this.IsSpecified)
            {
                return Optional<U>.Unspecified;
            }

            return new Optional<U>(mapper(this.Value));
        }

        public override bool Equals([NotNullWhen(true)] object? other)
        {
            if (!IsSpecified)
                return other == null;
            if (other == null)
                return false;
            return _value?.Equals(other) ?? false;
        }
        public override int GetHashCode() => IsSpecified ? (_value?.GetHashCode() ?? 0) : 0;

        public override string? ToString() => IsSpecified ? _value?.ToString() : null;
        private string DebuggerDisplay => IsSpecified ? _value?.ToString() ?? "<null>" : "<unspecified>";

        public static implicit operator Optional<T>(T value) => new(value);
        public static explicit operator T(Optional<T> value) => value.Value;
    }
    public static class Optional
    {
        public static Optional<T> Create<T>()
            => Optional<T>.Unspecified;
        public static Optional<T> Create<T>(T value)
            => new(value);
        public static Optional<T> CreateFromNullable<T>(T? value)
            where T : struct
        {
            if (value.HasValue)
            {
                return new Optional<T>(value.Value);
            }
            else
            {
                return Optional<T>.Unspecified;
            }
        }
        public static Optional<T> CreateFromNullable<T>(T? value)
            where T : notnull
        {
            if (value is null)
            {
                return Optional<T>.Unspecified;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns whether or not a property should be serialized.
        /// </summary>
        /// <remarks>
        /// This function is passed into <see cref="JsonPropertyInfo.ShouldSerialize"/>.
        /// </remarks>
        /// <param name="parent">The parent object.</param>
        /// <param name="propertyValue">The <see cref="IOptional"/> property value</param>
        /// <returns></returns>
        private static bool ShouldSerializeProperty(object parent, object? propertyValue)
        {
            if (propertyValue is not IOptional optional)
            {
                return true;
            }

            return optional.IsSpecified;
        }

        /// <summary>
        /// A modifier for JSON so that optional properties are skipped.
        /// <para>
        /// Adapted from the official <see href="https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts#example-ignore-properties-with-a-specific-type">documentation</see>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This modifies JSON types, so that their optional properties will
        /// only be serialized if they have a specified value.
        /// </remarks>
        /// <param name="jsonTypeInfo">The JSON Type</param>
        internal static void OptionalModifier(JsonTypeInfo jsonTypeInfo)
        {
            foreach (JsonPropertyInfo property in jsonTypeInfo.Properties)
            {
                // check if the property's underlying type is an optional type.
                if (!typeof(IOptional).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                // override ShouldSerialize property
                property.ShouldSerialize = ShouldSerializeProperty;
            }
        }

        public static T? ToNullable<T>(this Optional<T> val)
            where T : struct
            => val.IsSpecified ? val.Value : null;
    }
}
