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
        
        public static bool TryConsumeWhiteSpace(ref this ReadOnlySpan<char> span, ref int charsRead)
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

        public static string GetString(this ReadOnlySpan<char> span)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            return new string(span);
#else
            return new string(span.ToArray() ?? Array.Empty<char>()); // TODO: Is this the right way to do it?
#endif
        }
        
        public static void SplitKeepSecond(ref this ReadOnlySpan<char> first, int length, out ReadOnlySpan<char> second)
        {
            second = first[..length];
            first = first[length..];
        }

        public static bool TrySliceFirst(ref this ReadOnlySpan<char> span, out char value)
        {
            value = default;
            if (span.IsEmpty)
                return false;
            value = span[0];
            if (span.Length == 1)
            {
                // Workaround because for some reason on .NET Framework, it turns 1..
                // into Slice(1, -1) when resulting length will be zero?
                span = string.Empty.AsSpan();
                return true;
            }
            span = span[1..];
            return true;
        }
    }
}