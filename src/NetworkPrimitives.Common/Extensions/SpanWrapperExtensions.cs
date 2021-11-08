#nullable enable

using System;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives
{
    internal static class SpanWrapperExtensions
    {
        public static void SplitKeepFirst(ref this SpanWrapper first, int length, out SpanWrapper second)
        {
            second = first[length..];
            first = first[..length];
        }
        public static void SplitKeepSecond(ref this SpanWrapper first, int length, out SpanWrapper second)
        {
            second = first[..length];
            first = first[length..];
        }

        public static bool TrySliceFirst(ref this SpanWrapper span, out char value)
        {
            value = default;
            if (span.IsEmpty)
                return false;
            value = span[0];
            span = span[1..];
            return true;
        }

        public static bool TrySliceLast(ref this SpanWrapper span, out char value)
        {
            value = default;
            if (span.IsEmpty)
                return false;
            value = span[^1];
            span = span[..^1];
            return true;
        }
        
        
        public static bool TryConsumeWhiteSpace(ref this SpanWrapper span, ref int charsRead)
        {
            var atLeastOne = false;
            while (!span.IsEmpty && char.IsWhiteSpace(span[0]))
            {
                span = span[1..];
                ++charsRead;
                atLeastOne = true;
            }
            return atLeastOne;
        }
    }
}