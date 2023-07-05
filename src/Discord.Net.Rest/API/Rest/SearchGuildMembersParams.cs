namespace Discord.API.Rest
{
    internal class SearchGuildMembersParams
    {
        public string Query { get; set; } = string.Empty;
        public Optional<int> Limit { get; set; }
    }
}
