#nullable enable

using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace NetworkPrimitives
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class SpanExtensions
    {

        public static bool EqualToArray<T>(this Span<T> span, T[] array, IEqualityComparer<T>? comparer = null)
        {
            comparer ??= EqualityComparer<T>.Default;
            if (span.Length != array.Length) return false;
            for (var i = 0; i < span.Length; ++i)
            {
                if (comparer.Equals(span[i], array[i]) is false) 
                    return false;
            }
            return true;
        }
        
        public static string CreateString(this ReadOnlySpan<char> span)
        {
#if NETSTANDARD2_1_OR_GREATER
            return new (span);
#else
            return new(GetArray(span.ToArray()));


            static char[] GetArray(char[]? arr) => arr ?? Array.Empty<char>();
#endif
        }
        public static string CreateString(this Span<char> span)
        {
#if NETSTANDARD2_1_OR_GREATER
            return new (span);
#else
            return new(GetArray(span.ToArray()));


            static char[] GetArray(char[]? arr) => arr ?? Array.Empty<char>();
#endif
        }
        
        public static bool TryReadUInt32BigEndian(ref this ReadOnlySpan<byte> span, out uint result)
        {
            var bytesRead = 0;
            return span.TryReadUInt32BigEndian(ref bytesRead, out result);
        }
        
        public static bool TryReadUInt32BigEndian(ref this ReadOnlySpan<byte> span, ref int bytesRead, out uint result)
        {
            if (!BinaryPrimitives.TryReadUInt32BigEndian(span, out result)) return false;
            span = span[4..];
            bytesRead += 4;
            return true;
        }
        
        public static bool TryWrite<T>(ref this Span<T> span, T item, ref int charsWritten)
        {
            if (span.Length == 0)
                return false;
            span[0] = item;
            span = span[1..];
            ++charsWritten;
            return true;
        }
        
        public static bool TryWrite<T>(ref this Span<T> span, T item)
        {
            var written = 0;
            return span.TryWrite(item, ref written);
        }
        
    }
}