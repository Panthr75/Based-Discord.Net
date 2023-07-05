using System;
using System.Collections.Generic;
using System.Linq;

namespace Discord.Rest.Utils
{
    internal static class EnumConversionUtils
    {
#if NETSTANDARD2_0 || NETSTANDARD2_1
        private static readonly IEnumerable<AllowedMentionTypes> AllAllowedMentionTypes = Enum.GetValues(typeof(AllowedMentionTypes)).Cast<AllowedMentionTypes>().Where(v => v != AllowedMentionTypes.None);

        public static AllowedMentionTypes ToAllowedMentionTypes(string[] values)
        {
            return AllAllowedMentionTypes.Where(v => values.Any(i => Enum.GetName(typeof(AllowedMentionTypes), v)?.Equals(i, StringComparison.OrdinalIgnoreCase) ?? false))
                .Aggregate(AllowedMentionTypes.None, (a, b) => a | b);
        }
#else
        private static readonly IEnumerable<AllowedMentionTypes> AllAllowedMentionTypes = Enum.GetValues<AllowedMentionTypes>().Where(v => v != AllowedMentionTypes.None);

        public static AllowedMentionTypes ToAllowedMentionTypes(string[] values)
        {
            return AllAllowedMentionTypes.Where(v => values.Any(i => Enum.GetName(v)?.Equals(i, StringComparison.OrdinalIgnoreCase) ?? false))
                .Aggregate(AllowedMentionTypes.None, (a, b) => a | b);
        }
#endif

        public static string[] Convert(AllowedMentionTypes value)
        {
            return AllAllowedMentionTypes
                .Where(v => (value & v) == v)
                .Select(v => v.ToString().ToLower())
                .ToArray();
        }
    }
}
