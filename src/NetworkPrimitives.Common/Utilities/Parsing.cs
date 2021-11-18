using System;

#nullable enable

namespace NetworkPrimitives.Utilities
{
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
        
        public static bool TryParseByte(
            ref this SpanWrapper text, 
            ref int charsRead, 
            out byte value
        )
        {
            value = default;
            var length = Parsing.GetByteChars(text);
            if (length == 0) return false;
            text.SplitKeepSecond(length, out var remainder);
            var intValue = 0;
            while (remainder.TrySliceFirst(out var ch))
            {
                intValue *= 10;
                intValue += ch - '0';
            }
            charsRead += length;
            value = (byte)intValue;
            return true;
        }
        
        private static int GetByteChars(SpanWrapper text)
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
    }
}