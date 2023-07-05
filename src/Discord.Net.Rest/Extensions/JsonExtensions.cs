using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.Rest
{
    internal static class JsonExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddConverter<T>(this JsonSerializerOptions options)
            where T : JsonConverter, new()
        {
            options.Converters.Add(new T());
        }

        public static JsonSerializerOptions CopyWithConverter(this JsonSerializerOptions options, JsonConverter converter)
        {
            JsonSerializerOptions result = options.DeepCopy();
            result.Converters.Add(converter);
            return result;
        }

        public static JsonSerializerOptions DeepCopy(this JsonSerializerOptions options)
        {
            JsonSerializerOptions result = new()
            {
                AllowTrailingCommas = options.AllowTrailingCommas,
                DefaultBufferSize = options.DefaultBufferSize,
                DefaultIgnoreCondition = options.DefaultIgnoreCondition,
                DictionaryKeyPolicy = options.DictionaryKeyPolicy,
                Encoder = options.Encoder,
                IgnoreReadOnlyFields = options.IgnoreReadOnlyFields,
                IgnoreReadOnlyProperties = options.IgnoreReadOnlyProperties,
                IncludeFields = options.IncludeFields,
                MaxDepth = options.MaxDepth,
                NumberHandling = options.NumberHandling,
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
                PropertyNamingPolicy = options.PropertyNamingPolicy,
                ReadCommentHandling = options.ReadCommentHandling,
                ReferenceHandler = options.ReferenceHandler,
                TypeInfoResolver = options.TypeInfoResolver,
                UnknownTypeHandling = options.UnknownTypeHandling,
                WriteIndented = options.WriteIndented,
            };
            foreach (JsonConverter converter in options.Converters)
            {
                result.Converters.Add(converter);
            }
            return result;
        }
    }
}
