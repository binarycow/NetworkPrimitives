#nullable enable

using System;

namespace NetworkPrimitives.Switching
{
    internal static class Parsing
    {
        public static bool TryParseVlanNumber(
            ref this ReadOnlySpan<char> text, 
            ref int charsRead, 
            out ushort value
        )
        {
            value = default;
            var digitLength = text.Length switch
            {
                0 => default,
                1 => GetByteDigitLength(text[0], default, default, default),
                2 => GetByteDigitLength(text[0], text[1], default, default),
                3 => GetByteDigitLength(text[0], text[1], text[2], default),
                _ => GetByteDigitLength(text[0], text[1], text[2], text[3]),
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

            static int GetByteDigitLength(char a, char b, char c, char d) => (a, b, c, d) switch
            {
                (a: '4'              , b: '0',               c: '9',               d: >= '0' and <= '5') => 4,
                (a: '4'              , b: '0',               c: >= '0' and <= '8', d: >= '0' and <= '9') => 4,
                (a: >= '1' and <= '3', b: >= '0' and <= '9', c: >= '0' and <= '9', d: >= '0' and <= '9') => 4,
                (a: >= '1' and <= '9', b: >= '0' and <= '9', c: >= '0' and <= '9', d: _                ) => 3,
                (a: >= '1' and <= '9', b: >= '0' and <= '9', c: _                , d: _                ) => 2,
                (a: >= '0' and <= '9', b: _                , c: _                , d: _                ) => 1,
                _                                                                                        => 0,
            };
            
        }
    }
}