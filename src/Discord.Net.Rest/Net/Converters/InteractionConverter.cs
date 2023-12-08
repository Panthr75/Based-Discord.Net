using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using Discord.Rest;

namespace Discord.Net.Converters;

internal sealed class InteractionConverter : JsonConverter<API.Interaction>
{
    /// <summary>
    /// A dummy converter that's only job is to write null to the Data
    /// property of an <see cref="API.Interaction"/>.
    /// </summary>
    private sealed class InteractionDataNullConverter : JsonConverter<IDiscordInteractionData>
    {
        public override IDiscordInteractionData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // parse value to properly advance the reader
            JsonElement.ParseValue(ref reader);
            return null;
        }

        public override void Write(Utf8JsonWriter writer, IDiscordInteractionData value, JsonSerializerOptions options)
        {
            writer.WriteNullValue();
        }
    }

    // a wrapper class used for deserialization to only
    // deserialize the data property.
    private sealed class InteractionDataWrapper<TData>
        where TData : IDiscordInteractionData
    {
        [JsonPropertyName("data")]
        public TData? Data { get; set; }


        /// <summary>
        /// A utility method that deserializes the passed reader as
        /// an <see cref="InteractionDataWrapper{TData}"/>, then returns
        /// the <see cref="Data"/> property.
        /// </summary>
        /// <param name="reader">The Json Reader</param>
        /// <param name="options">The serializer options</param>
        /// <returns>
        /// <see langword="null"/> if deserializing to an
        /// <see cref="InteractionDataWrapper{TData}"/> returned
        /// <see langword="null"/>, or if <see cref="Data"/> was
        /// <see langword="null"/>
        /// </returns>
        public static TData? Deserialize(ref Utf8JsonReader reader, JsonSerializerOptions? options = null)
        {
            var wrapper = JsonSerializer.Deserialize<InteractionDataWrapper<TData>>(ref reader, options);

            if (wrapper is null)
            {
                return default;
            }

            return wrapper.Data;
        }

        public static void DeserializeAndApply(API.Interaction apiInteraction, ref Utf8JsonReader reader, JsonSerializerOptions? options = null)
        {
            var data = Deserialize(ref reader, options);
            if (data is null)
            {
                apiInteraction.Data = Optional<IDiscordInteractionData>.Unspecified;
                return;
            }

            apiInteraction.Data = data;
        }
    }

    public override API.Interaction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        // make a copy of the reader for serializing the base interaction.
        var interactionReader = reader;

        var optionsThatSkipData = options.DeepCopy();
        optionsThatSkipData.Converters.Add(new InteractionDataNullConverter());
        // use .ToArray() so that we make a copy of the list and dont run into
        // concurrent modification exceptions.
        foreach (var thisConverter in optionsThatSkipData.Converters.OfType<InteractionConverter>().ToArray())
        {
            optionsThatSkipData.Converters.Remove(thisConverter);
        }

        var apiInteraction = JsonSerializer.Deserialize<API.Interaction>(ref interactionReader, optionsThatSkipData);

        if (apiInteraction == null)
        {
            return null;
        }

        // now use the base reader and options for this
        switch (apiInteraction.Type)
        {
            case InteractionType.ApplicationCommand:
                InteractionDataWrapper<API.ApplicationCommandInteractionData>.DeserializeAndApply(apiInteraction, ref reader, options);
                break;
            case InteractionType.MessageComponent:
                InteractionDataWrapper<API.MessageComponentInteractionData>.DeserializeAndApply(apiInteraction, ref reader, options);
                break;
            case InteractionType.ApplicationCommandAutocomplete:
                InteractionDataWrapper<API.AutocompleteInteractionData>.DeserializeAndApply(apiInteraction, ref reader, options);
                break;
            case InteractionType.ModalSubmit:
                InteractionDataWrapper<API.ModalInteractionData>.DeserializeAndApply(apiInteraction, ref reader, options);
                break;
            default:
                apiInteraction.Data = Optional<IDiscordInteractionData>.Unspecified;
                break;
        }

        return apiInteraction;
    }

    public override void Write(Utf8JsonWriter writer, API.Interaction value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
