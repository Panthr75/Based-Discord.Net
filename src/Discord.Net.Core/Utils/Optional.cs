using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization.Metadata;
using System.Collections;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Discord;

internal interface IOptional
{
    bool IsSpecified { get; }
    object? GetValueOrDefault();
}

//Based on https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Nullable.cs
[DebuggerDisplay(@"{DebuggerDisplay,nq}")]
[JsonConverter(typeof(Optional.ConverterFactory))]
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

    object? IOptional.GetValueOrDefault() => GetValueOrDefault();

    public Optional<U> Map<U>(Func<T, Optional<U>> mapper)
    {
        if (!IsSpecified)
        {
            return Optional<U>.Unspecified;
        }

        return mapper(Value);
    }

    public Optional<U> Map<U>(Func<T, U> mapper)
    {
        if (!IsSpecified)
        {
            return Optional<U>.Unspecified;
        }

        return new Optional<U>(mapper(Value));
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

    public static Optional<TCollection> NotEmpty<TCollection>(this Optional<TCollection> val)
        where TCollection : IEnumerable
    {
        var value = val.GetValueOrDefault();
        if (value is not null)
        {
            // adopted from Linq Enumerable<T>.Any,
            // but for non-generic enumerable
            var enumerator = value.GetEnumerator();
            var hasValue = enumerator.MoveNext();
            if (enumerator is IDisposable disposable)
            {
                disposable.Dispose();
            }

            if (hasValue)
            {
                return val;
            }
        }
        return Optional<TCollection>.Unspecified;
    }

    public static Optional<TCollection> NotEmpty<TCollection, T>(this Optional<TCollection> val)
        where TCollection : IEnumerable<T>
    {
        return (val.GetValueOrDefault()?.Any() ?? false) ? val : Optional<TCollection>.Unspecified;
    }

    public static T? ToNullable<T>(this Optional<T> val)
        where T : struct
        => val.IsSpecified ? val.Value : null;

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
    /// <example>
    /// <code>
    /// class Foo
    /// {
    ///     [JsonPropertyName("baz")]
    ///     public string? Baz { get; set; }
    ///     [JsonPropertyName("bar")]
    ///     public Optional&lt;int&gt; Bar { get; set; }
    /// }
    /// 
    /// var serializerOptions = new JsonSerializerOptions()
    /// {
    ///     TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    ///     {
    ///         Modifiers = { Optional.JsonModifier }
    ///     }
    /// }
    ///
    /// string specifiedJson = @"{
    ///     ""baz"": ""lorem ipsum"",
    ///     ""bar"": 420
    /// }";
    ///
    /// Foo specifiedFoo = JsonSerializer.Deserialize&lt;Foo&gt;(specifiedJson, serializerOptions)!;
    /// // - Baz: "lorem ipsum"
    /// // - Bar: 420
    ///
    /// specifiedJson = JsonSerializer.Serialize(specifiedFoo, serializerOptions);
    /// // {"baz":"lorem ipsum","bar":420}
    /// 
    /// string unspecifiedJson = @"{
    ///     ""baz"": ""lorem ipsum""
    /// }";
    /// 
    /// Foo unspecifiedFoo = JsonSerializer.Deserialize&lt;Foo&gt;(specifiedJson, serializerOptions)!;
    /// // - Baz: "lorem ipsum"
    /// // - Bar: Optional&lt;int&gt;.Unspecified
    ///
    /// unspecifiedJson = JsonSerializer.Serialize(unspecifiedFoo, serializerOptions);
    /// // {"baz":"lorem ipsum"}
    /// </code>
    /// </example>
    public static void JsonModifier(JsonTypeInfo jsonTypeInfo)
    {
        foreach (var property in jsonTypeInfo.Properties)
        {
            // check if the property's underlying type is an optional type.
            if (!typeof(IOptional).IsAssignableFrom(property.PropertyType))
            {
                continue;
            }

            // override ShouldSerialize property
            // todo: check if ShouldSerialize is already defined, and if so, combine with existing.
            property.ShouldSerialize = ShouldSerializeProperty;
        }
    }

    internal sealed class ConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var type = typeof(Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()[0]);

            return (JsonConverter?)Activator.CreateInstance(type);
        }

        private sealed class Converter<T> : JsonConverter<Optional<T>>
        {
            public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return JsonSerializer.Deserialize<T>(ref reader, options)!;
            }

            public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            {
                if (value.IsSpecified)
                {
                    JsonSerializer.Serialize(writer, value.Value, options);
                }
                else
                {
                    // this case should never be hit...
                    // unless not using the JsonModifier
                    writer.WriteNullValue();
                }
            }
        }
    }
}
