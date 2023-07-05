using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class StickerPack
    {
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("stickers")]
        public Sticker[] Stickers { get; set; } = Array.Empty<Sticker>();
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("sku_id")]
        public ulong SkuId { get; set; }
        [JsonPropertyName("cover_sticker_id")]
        public Optional<ulong> CoverStickerId { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("banner_asset_id")]
        public ulong BannerAssetId { get; set; }
    }
}
