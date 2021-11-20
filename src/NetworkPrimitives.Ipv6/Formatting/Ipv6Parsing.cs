#nullable enable

using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv6
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class Ipv6Parsing
    {
        internal static bool TryParseIpv6Address(SpanWrapper text, out int charsRead, out ulong high, out ulong low)
        {
            high = default;
            low = default;
            charsRead = default;
            var doubleColonPos = text.IndexOf("::");
            if (doubleColonPos == -1)
                return TryParseUnabbreviated(text, out charsRead, out high, out low);
            var highSpan = text[..doubleColonPos];
            var lowSpan = text[(doubleColonPos + 2)..];
            var highGroupCount = GetColonCount(highSpan) + 1;
            var lowGroupCount = GetColonCount(lowSpan) + 1;
            Span<ushort> bitGroups = stackalloc ushort[8];
            if (highGroupCount + lowGroupCount > 8)
            {
                return false;
            }
            var highGroups = bitGroups[..highGroupCount];
            var lowGroups = bitGroups[^lowGroupCount..];
            if (!TryFillBitGroups(highSpan, ref charsRead, highGroups))
            {
                return false;
            }
            if (!TryFillBitGroups(lowSpan, ref charsRead, lowGroups))
            {
                return false;
            }
            return Create(bitGroups, out high, out low);

            static bool TryFillBitGroups(SpanWrapper text, ref int charsRead, Span<ushort> bitGroups)
            {
                while (bitGroups.Length > 0 && text.TryParseHexUshort(ref charsRead, out var bitGroup))
                {
                    bitGroups[0] = bitGroup;
                    bitGroups = bitGroups[1..];
                    if (text.Length == 0 || text[0] != ':')
                        break;
                    text = text[1..];
                    ++charsRead;
                }
                return bitGroups.Length == 0;
            }
            static int GetColonCount(SpanWrapper text)
            {
                var ct = 0;
                for (var i = 0; i < text.Length; ++i)
                {
                    if (text[i] == ':')
                        ++ct;
                }
                return ct;
            }
        }
        
        
        internal static bool TryParseUnabbreviated(SpanWrapper text, out int charsRead, out ulong high, out ulong low)
        {
            charsRead = default;
            Span<ushort> bitGroups = stackalloc ushort[8];
            // Read the first bit groups
            var startLength = 0;
            while (text.TryParseHexUshort(ref charsRead, out var bitGroup))
            {
                bitGroups[startLength++] = bitGroup;
                if (text.Length == 0 || text[0] != ':')
                    break;
                text = text[1..];
                ++charsRead;
            }
            if (startLength == 8) 
                return Create(bitGroups, out high, out low);
            high = default;
            low = default;
            return false;
        }
        
        

        private static bool Create(Span<ushort> bitGroups, out ulong high, out ulong low)
        {
            high = Convert(bitGroups[..4]);
            low = Convert(bitGroups[4..]);
            return true;
            static ulong Convert(Span<ushort> ushorts) 
                => BinaryPrimitives.ReadUInt64BigEndian(
                    MemoryMarshal.Cast<ushort, Byte>(ushorts)
                );
        }
    }
}