#nullable enable

using System;

namespace NetworkPrimitives
{
    internal static class StringExtensions
    {
        public static ReadOnlySpan<char> GetSpan(this string? text) => (text ?? string.Empty).AsSpan();
    }
}