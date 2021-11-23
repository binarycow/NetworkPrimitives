using System;

#nullable enable

namespace NetworkPrimitives.Utilities
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class Parsing
    {

        public static bool TryParseHexUshort(
            ref this SpanWrapper text, 
            ref int charsRead, 
            out ushort value
        )
        {
            var length = GetHexChars(text);
            length = Math.Min(4, length);
            value = default;
            for (var i = 0; i < length; ++i)
            {
                var hex = text[0];
                text = text[1..];
                value <<= 4;
                value |= GetValue(hex);
            }
            charsRead += length;
            return length > 0;

            static ushort GetValue(char ch) => ch switch
            {
                >= '0' and <= '9' => (ushort)(ch - '0'),
                >= 'a' and <= 'f' => (ushort)(ch - 'a' + 10),
                >= 'A' and <= 'F' => (ushort)(ch - 'A' + 10),
                _ => 0,
            };
        }
        public static bool TryParseUInt64(
            ref this SpanWrapper text, 
            ref int charsRead, 
            out ulong value
        )
        {
            value = default;
            var length = Parsing.GetDigitLength(text);
            if (length == 0) return false;
            text.SplitKeepSecond(length, out var remainder);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var success = ulong.TryParse(remainder.GetSpan(), out value);
#else
            var success = ulong.TryParse(remainder.GetString(), out value);
#endif
            if (!success) return false;
            charsRead += length;
            return true;
        }
        public static bool TryParseByte(
            ref this SpanWrapper text, 
            ref int charsRead, 
            out byte value
        )
        {
            value = default;
            var length = Parsing.GetDigitLength(text);
            if (length == 0) return false;
            text.SplitKeepSecond(length, out var remainder);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var success = byte.TryParse(remainder.GetSpan(), out value);
#else
            var success = byte.TryParse(remainder.GetString(), out value);
#endif
            if (!success) return false;
            charsRead += length;
            return true;
        }
        
        public static bool TryParseByte(
            ref this ReadOnlySpan<char> text, 
            ref int charsRead, 
            out byte value
        )
        {
            value = default;
            var length = Parsing.GetDigitLength(text);
            if (length == 0) return false;
            text.SplitKeepSecond(length, out var remainder);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var success = byte.TryParse(remainder, out value);
#else
            var success = byte.TryParse(remainder.CreateString(), out value);
#endif
            if (!success) return false;
            charsRead += length;
            return true;
        }
        
        private static int GetDigitLength(SpanWrapper text)
        {
            var length = 0;
            while (text.TrySliceFirst(out var ch) && ch is >= '0' and <= '9')
                ++length;
            return length;
        }
        private static int GetDigitLength(ReadOnlySpan<char> text)
        {
            var length = 0;
            while (text.TrySliceFirst(out var ch) && ch is >= '0' and <= '9')
                ++length;
            return length;
        }
        
        private static int GetHexChars(SpanWrapper text)
        {
            var length = 0;
            while (text.TrySliceFirst(out var ch) && ch.IsHex())
                ++length;
            return length;
        }

        public static bool TryReadCharacter(ref this SpanWrapper span, ref int charsRead, char expected)
        {
            if (!span.TrySliceFirst(out var actual) || actual != expected)
                return false;
            ++charsRead;
            return true;
        }
        public static bool TryReadCharacter(ref this ReadOnlySpan<char> span, ref int charsRead, char expected)
        {
            if (!span.TrySliceFirst(out var actual) || actual != expected)
                return false;
            ++charsRead;
            return true;
        }
    }
}