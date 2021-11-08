#nullable enable

namespace NetworkPrimitives.Utilities
{
    internal static class Parsing
    {
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

        public static bool TryReadCharacter(ref this SpanWrapper span, ref int charsRead, char expected)
        {
            if (!span.TrySliceFirst(out var actual) || actual != expected)
                return false;
            ++charsRead;
            return true;
        }
    }
}