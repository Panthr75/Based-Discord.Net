#if !NETSTANDARD2_1_OR_GREATER
namespace System;

internal static class ReadOnlySpanCharExtensions
{
    public static bool Equals(this ReadOnlySpan<char> span, string other, StringComparison comparison)
    {
        return span.Equals(other.AsSpan(), comparison);
    }
}
#endif
