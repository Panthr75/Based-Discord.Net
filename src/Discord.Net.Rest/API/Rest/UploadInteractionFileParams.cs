using Discord.Net.Converters;
using Discord.Net.Rest;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.API.Rest
{
    internal class UploadInteractionFileParams
    {
        public FileAttachment[] Files { get; }

        public InteractionResponseType Type { get; set; }
        public Optional<string> Content { get; set; }
        public Optional<bool> IsTTS { get; set; }
        public Optional<Embed[]> Embeds { get; set; }
        public Optional<AllowedMentions> AllowedMentions { get; set; }
        public Optional<ActionRowComponent[]> MessageComponents { get; set; }
        public Optional<MessageFlags> Flags { get; set; }

        public bool HasData
            => Content.IsSpecified ||
               IsTTS.IsSpecified ||
               Embeds.IsSpecified ||
               AllowedMentions.IsSpecified ||
               MessageComponents.IsSpecified ||
               Flags.IsSpecified ||
               Files.Any();

        public UploadInteractionFileParams(params FileAttachment[] files)
        {
            Files = files;
        }

        public IReadOnlyDictionary<string, object> ToDictionary(JsonSerializerOptions? options)
        {
            var d = new Dictionary<string, object>();


            var payload = new Dictionary<string, object>();
            payload["type"] = Type;

            var data = new Dictionary<string, object>();
            if (Content.IsSpecified)
                data["content"] = Content.Value;
            if (IsTTS.IsSpecified)
                data["tts"] = IsTTS.Value;
            if (MessageComponents.IsSpecified)
                data["components"] = MessageComponents.Value;
            if (Embeds.IsSpecified)
                data["embeds"] = Embeds.Value;
            if (AllowedMentions.IsSpecified)
                data["allowed_mentions"] = AllowedMentions.Value;
            if (Flags.IsSpecified)
                data["flags"] = Flags.Value;

            List<object> attachments = new();

            for (int n = 0; n != Files.Length; n++)
            {
                var attachment = Files[n];

                var filename = attachment.FileName ?? "unknown.dat";
                if (attachment.IsSpoiler && !filename.StartsWith(AttachmentExtensions.SpoilerPrefix))
                    filename = filename.Insert(0, AttachmentExtensions.SpoilerPrefix);
                d[$"files[{n}]"] = new MultipartFile(attachment.Stream, filename);

                attachments.Add(new
                {
                    id = (ulong)n,
                    filename = filename,
                    description = attachment.Description ?? Optional<string>.Unspecified
                });
            }

            data["attachments"] = attachments;

            payload["data"] = data;


            if (data.Any())
            {
                d["payload_json"] = JsonSerializer.Serialize(payload, options);
            }

            return d;
        }
    }
}
