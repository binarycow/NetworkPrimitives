#nullable enable

using System;
using System.Buffers.Binary;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class Ipv4Parsing
    {
        public static bool TryParseDottedDecimalUInt32(string? text, out uint result)
        {
            result = default;
            return text is not null && Ipv4Parsing.TryParseDottedDecimalUInt32(text, out var charsRead, out result) &&
                charsRead == text.Length;
        }

        public static bool TryParseDottedDecimalUInt32(string? text, out int charsRead, out uint result)
        {
            charsRead = default;
            var span = new SpanWrapper(text);
            return TryParseDottedDecimalUInt32(ref span, ref charsRead, out result);
        }
        
        internal static bool TryParseDottedDecimalUInt32(ref SpanWrapper text, ref int charsRead, out uint result)
        {
            result = default;
            if (text.Length < Ipv4Address.MINIMUM_LENGTH)
                return false;
            Span<byte> octets = stackalloc byte[4];
            var textCopy = text;
            var charsReadCopy = charsRead;
            for (var i = 0; i < 4; ++i)
            {
                if (i != 0 && !textCopy.TryReadCharacter(ref charsReadCopy, '.'))
                    return false;
                if (!textCopy.TryParseByte(ref charsReadCopy, out var octet))
                    return false;
                octets[i] = octet;
            }
            charsRead = charsReadCopy;
            text = textCopy;
            result = BinaryPrimitives.ReadUInt32BigEndian(octets);
            return true;
        }
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public static bool TryParseDottedDecimalUInt32(ReadOnlySpan<char> text, out uint result)
            => TryParseDottedDecimalUInt32(text, out var charsRead, out result) && charsRead == text.Length;
        public static bool TryParseDottedDecimalUInt32(ReadOnlySpan<char> text, out int charsRead, out uint result)
        {
            charsRead = default;
            var span = new SpanWrapper(text);
            return TryParseDottedDecimalUInt32(ref span, ref charsRead, out result);
        }
#endif
    }
}