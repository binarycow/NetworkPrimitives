#nullable enable

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