#nullable enable

using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Yang
{
    internal static class Parsing
    {
        public static bool TryReadQuotedText(
            ref SpanWrapper text,
            ref int charsRead,
            out string result
        ) => TryReadQuotedText(ref text, ref charsRead, out result, out _);
        
        public static bool TryReadQuotedText(
            ref SpanWrapper text,
            ref int charsRead,
            out string result,
            out char quoteType
        )
        {
            if (!TryReadQuotedText(text, out var length, out result, out quoteType))
                return false;
            charsRead += length;
            text = text[length..];
            return true;
        }
        
        public static bool TryReadQuotedText(
            SpanWrapper text,
            out int charsRead,
            out string result
        ) => TryReadQuotedText(text, out charsRead, out result, out _);
        
        public static bool TryReadQuotedText(
            SpanWrapper text,
            out int charsRead,
            out string result,
            out char quoteType
        )
        {
            charsRead = default;
            result = string.Empty;
            quoteType = default;
            if (text.Length == 0 || text[0] is not ('\'' or '"'))
                return false;
            quoteType = text[0];
            text = text[1..];
            
            var substring = text;
            var length = 0;
            while (text.IsEmpty == false && text[0] != quoteType)
            {
                ++length;
                text = text[1..];
            }
            if (text.IsEmpty || text[0] != quoteType)
                return false;

            substring = substring[..length];
            charsRead = length + 2;
            result = substring.GetString();
            return true;
        }
    }
}