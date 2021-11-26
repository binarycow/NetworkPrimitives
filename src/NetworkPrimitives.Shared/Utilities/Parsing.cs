using System;

#nullable enable

namespace NetworkPrimitives.Utilities
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class Parsing
    {

        public static bool TryParseHexUshort(
            ref this ReadOnlySpan<char> text, 
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
            ref this ReadOnlySpan<char> text, 
            ref int charsRead, 
            out ulong value
        )
        {
            value = default;
            var length = Parsing.GetDigitLength(text);
            if (length == 0) return false;
            text.SplitKeepSecond(length, out var remainder);
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var success = ulong.TryParse(remainder, out value);
#else
            var success = ulong.TryParse(remainder.GetString(), out value);
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
            /*
25[0-5]
2[0-4][0-9]
1[0-9][0-9]

[1-9][0-9]

[0-9]
 */
            value = default;
            var digitLength = text.Length switch
            {
                0 => default,
                1 => GetByteDigitLength(text[0], default, default),
                2 => GetByteDigitLength(text[0], text[1], default),
                _ => GetByteDigitLength(text[0], text[1], text[2]),
            };
            value = 0;
            for (var i = 0; i < digitLength; ++i)
            {
                value *= 10;
                value += (byte)(text[0] - '0');
                text = text[1..];
                ++charsRead;
            }
            return digitLength > 0;

            static int GetByteDigitLength(char a, char b, char c) => (a, b, c) switch
            {
                (a: '2'              , b: >= '0' and <= '4', c: >= '0' and <= '9') => 3,
                (a: '2'              , b: '5'              , c: >= '0' and <= '5') => 3,
                
                (a: >= '2'           , b: >= '0' and <= '9', c: >= '0' and <= '9') => 0,
                
                (a: '1'              , b: >= '0' and <= '9', c: >= '0' and <= '9') => 3,
                (a: >= '1' and <= '9', b: >= '0' and <= '9', c: _                ) => 2,
                (a: >= '0' and <= '9', b: _                , c: _                ) => 1,
                _                                                                  => 0,
            };
            
        }
        
        

        private static int GetDigitLength(ReadOnlySpan<char> text)
        {
            var length = 0;
            while (text.TrySliceFirst(out var ch) && ch is >= '0' and <= '9')
                ++length;
            return length;
        }
        
        private static int GetHexChars(ReadOnlySpan<char> text)
        {
            var length = 0;
            while (text.TrySliceFirst(out var ch) && ch.IsHex())
                ++length;
            return length;
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