using System.Text.Json;
using System.Text.Json.Serialization;

namespace Discord.API
{
    internal class Application
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("rpc_origins")]
        public Optional<string[]> RPCOrigins { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("id")]
        public ulong Id { get; set; }
        [JsonPropertyName("icon")]
        public string? Icon { get; set; }

        [JsonPropertyName("bot_public")]
        public Optional<bool> IsBotPublic { get; set; }
        [JsonPropertyName("bot_require_code_grant")]
        public Optional<bool> BotRequiresCodeGrant { get; set; }

        [JsonPropertyName("install_params")]
        public Optional<InstallParams> InstallParams { get; set; }
        [JsonPropertyName("team")]
        public Team? Team { get; set; }
        [JsonPropertyName("flags")]
        public Optional<ApplicationFlags> Flags { get; set; }
        [JsonPropertyName("owner")]
        public Optional<User> Owner { get; set; }
        [JsonPropertyName("tags")]
        public Optional<string[]> Tags { get; set; }

        [JsonPropertyName("verify_key")]
        public string VerifyKey { get; set; } = string.Empty;

        [JsonPropertyName("approximate_guild_count")]
        public Optional<int> ApproximateGuildCount { get; set; }

        [JsonPropertyName("guild")]
        public Optional<PartialGuild> PartialGuild { get; set; }

        /// Urls
        [JsonPropertyName("terms_of_service_url")]
        public string? TermsOfService { get; set; }

        [JsonPropertyName("privacy_policy_url")]
        public string? PrivacyPolicy { get; set; }

        [JsonPropertyName("custom_install_url")]
        public Optional<string> CustomInstallUrl { get; set; }

        [JsonPropertyName("role_connections_verification_url")]
        public Optional<string> RoleConnectionsUrl { get; set; }

        [JsonPropertyName("interactions_endpoint_url")]
        public Optional<string> InteractionsEndpointUrl { get; set; }

        [JsonPropertyName("redirect_uris")]
        public Optional<string[]> RedirectUris { get; set; }

    }
}
