#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetworkPrimitives
{
    internal static class SliceExtensions
    {
        [return: NotNullIfNotNull("defaultValue")]
        public static TItem? SliceOrDefault<TDerived, TItem>(
            ref this TDerived span, 
            TItem? defaultValue = default
        ) where TDerived : struct, ISlice<TDerived, TItem>
        {
            if (span.Length == 0)
                return defaultValue;
            var value = span[0];
            span = span[1..];
            return value;
        }

        public static bool TrySliceFirst<TDerived, TItem>(ref this TDerived span, ref int charsRead, TItem expected)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
            => span.TrySliceFirst(ref charsRead, expected, out _);
        
        public static bool TrySliceFirst<TDerived, TItem>(ref this TDerived span, ref int charsRead, TItem expected, out TItem value)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
        {
            if (!span.TrySliceFirst(expected, out value))
                return false;
            ++charsRead;
            return true;
        }
        
        public static bool TrySliceFirst<TDerived, TItem>(ref this TDerived span, ref int charsRead, out TItem value)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
        {
            if (!span.TrySliceFirst(out value))
                return false;
            ++charsRead;
            return true;
        }
        
        public static bool TrySliceFirst<TDerived, TItem>(ref this TDerived span, TItem expected, out TItem value)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
        {
            value = default;
            if (span.Length == 0)
                return false;
            if (!EqualityComparer<TItem>.Default.Equals(span[0], expected))
                return false;
            value = span[0];
            span = span[1..];
            return true;
        }
        
        public static bool TrySliceFirst<TDerived, TItem>(ref this TDerived span, out TItem value)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
        {
            value = default;
            if (span.Length == 0)
                return false;
            value = span[0];
            span = span[1..];
            return true;
        }

        public static bool TrySliceLast<TDerived, TItem>(ref this TDerived span, out TItem value)
            where TDerived : struct, ISlice<TDerived, TItem>
            where TItem : struct
        {
            value = default;
            if (span.Length == 0)
                return false;
            value = span[^1];
            span = span[..^1];
            return true;
        }
    }
}