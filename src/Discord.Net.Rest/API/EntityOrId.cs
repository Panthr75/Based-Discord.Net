using Discord.Net.Converters;
using System.Text.Json.Serialization;

namespace Discord.API
{
    [JsonConverter(typeof(UInt64EntityOrIdConverter))]
    internal struct EntityOrId<T>
    {
        public ulong Id { get; }
        public bool IsObject { get; }
        public T? Object { get; }

        public EntityOrId(ulong id)
        {
            Id = id;
            Object = default(T);
            IsObject = false;
        }
        public EntityOrId(T obj)
        {
            Id = 0;
            Object = obj;
            IsObject = true;
        }
    }
}
