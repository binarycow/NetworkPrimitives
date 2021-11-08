#nullable enable

using System;

namespace NetworkPrimitives.Utilities
{
    internal static class Formatting
    {
        private const int MAX_BYTE_LENGTH = 3;

        public static bool TryFormatTo(
            this byte value,
            ref Span<char> destination,
            ref int charsWritten
        )
        {
            Span<char> source = stackalloc char[Formatting.MAX_BYTE_LENGTH];
            if (!value.TryFormat(source, out var length))
                return false;
            source = source[..length];
            source.CopyTo(destination);
            destination = destination[length..];
            charsWritten += length;
            return true;
        }
        
#if !NETSTANDARD2_1_OR_GREATER
        public static bool TryFormat(
            this byte value,
            Span<char> destination,
            out int charsWritten
        )
        {
            charsWritten = default;
            var length = value switch
            {
                >= 100 => 3,
                >= 10 => 2,
                _ => 1,
            };
            if (destination.Length < length)
                return false;
            Span<char> chars = stackalloc char[length];
            for (var i = 0; i < chars.Length; ++i)
            {
                chars[i] = (char)(value % 10 + '0');
                value /= 10;
            }
            chars.Reverse();
            if (!chars.TryCopyTo(destination))
                return false;
            charsWritten = length;
            return true;
        }
#endif
    }
}