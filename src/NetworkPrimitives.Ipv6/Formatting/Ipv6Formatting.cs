#nullable enable

using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;

namespace NetworkPrimitives.Ipv6
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class Ipv6Formatting
    {
        public static string FormatIpv6Address(
            ulong low,
            ulong high,
            string? format = null,
            IFormatProvider? formatProvider = null
        )
        {
            Span<ushort> bitGroups = stackalloc ushort[8];
            PopulateBitGroups(low, high, bitGroups);
            var formatValues = format is null
                ? new Ipv6FormatInfo(true, true, true, false)
                : GetFormatValues(format);
            
            return format switch
            {
                null => FormatIpv6Address(bitGroups, formatValues),
                "x" => FormatIpv6Address(bitGroups, formatValues),
                "X" => FormatIpv6Address(bitGroups, formatValues),
                _ => throw new FormatException(),
            };
        }

        private static Ipv6FormatInfo GetFormatValues(string format)
        {
            bool? compress = null;
            bool? compressGroups = null;
            bool? hex = null;
            bool? upper = null;
            foreach (var ch in format)
            {
                switch (ch)
                {
                    case 'D':
                    case 'd':
                        hex = false;
                        upper = false;
                        break;
                    case 'x':
                        hex = true;
                        upper = false;
                        break;
                    case 'X':
                        hex = true;
                        upper = true;
                        break;
                    case 'g':
                        compressGroups = false;
                        break;
                    case 'G':
                        compressGroups = true;
                        break;
                    case '<':
                        compress = true;
                        break;
                    case '>':
                        compress = false;
                        break;
                    default:
                        throw new FormatException();
                }
            }

            if (compress is null || compressGroups is null || hex is null || upper is null)
                throw new FormatException();
            return new Ipv6FormatInfo(compress.Value, compressGroups.Value, hex.Value, upper.Value);
        }

        private static string FormatIpv6Address(
            ReadOnlySpan<ushort> bitGroups,
            Ipv6FormatInfo format
        )
        {
            var sb = new StringBuilder();
            if (format.Compress == false || !TryFindCompression(bitGroups, out var start, out var length))
            {
                Ipv6Formatting.FormatBitGroups(sb, bitGroups, format);
                return sb.ToString();
            }
            GetCompressed(bitGroups, (start, length),  out var high, out var low);
            Ipv6Formatting.FormatBitGroups(sb, high, format);
            sb.Append("::");
            Ipv6Formatting.FormatBitGroups(sb, low, format);
            return sb.ToString();
        }

        private static void GetCompressed(
            ReadOnlySpan<ushort> bitGroups,
            (int Start, int Length) splitPoint,
            out ReadOnlySpan<ushort> high,
            out ReadOnlySpan<ushort> low
        )
        {
            high = bitGroups[..splitPoint.Start];
            low = bitGroups[(splitPoint.Start + splitPoint.Length)..];
        }

        private static bool TryFindCompression(ReadOnlySpan<ushort> bitGroups, out int start, out int length)
        {
            start = 0;
            length = 0;
            for (var i = 0; i < bitGroups.Length; ++i)
            {
                var partLength = GetZeroLength(bitGroups[i..]);
                if (partLength <= length) continue;
                start = i;
                length = partLength;
            }
            return length > 0;

            static int GetZeroLength(ReadOnlySpan<ushort> bitGroups)
            {
                for (var i = 0; i < bitGroups.Length; ++i)
                {
                    if (bitGroups[i] != 0)
                        return i;
                }
                return bitGroups.Length;
            }
        }

        private static void FormatBitGroups(
            StringBuilder sb,
            ReadOnlySpan<ushort> bitGroups,
            Ipv6FormatInfo format
        )
        {
            if (bitGroups.Length == 0) return;
            FormatBitGroup(sb, bitGroups[0], format);
            bitGroups = bitGroups[1..];
            while (bitGroups.Length > 0)
            {
                sb.Append(':');
                FormatBitGroup(sb, bitGroups[0], format);
                bitGroups = bitGroups[1..];
            }
        }

        private static void FormatBitGroup(
            StringBuilder sb,
            ushort bitGroup,
            Ipv6FormatInfo format
        )
        {
            var (_, compressGroup, hex, upper) = format;
            sb.Append((hex, upper, compressGroup) switch
            {
                (hex: true, upper: true, compressGroup: true) => bitGroup.ToString("X"),
                (hex: true, upper: true, compressGroup: false) => bitGroup.ToString("X4"),
                (hex: true, upper: false, compressGroup: true) => bitGroup.ToString("x"),
                (hex: true, upper: false, compressGroup: false) => bitGroup.ToString("x4"),
                _ => bitGroup.ToString() ?? "0",
            });
        }

        private static void PopulateBitGroups(ulong low, ulong high, Span<ushort> bitGroups)
        {
            PopulateBitGroup(high, bitGroups[..4]);
            PopulateBitGroup(low, bitGroups[4..]);
        }

        private static void PopulateBitGroup(ulong value, Span<ushort> bitGroups)
        {
            Span<byte> bytes = stackalloc byte[8];
            BinaryPrimitives.WriteUInt64BigEndian(bytes, value);
            MemoryMarshal.Cast<byte, ushort>(bytes).CopyTo(bitGroups);
        }
    }
}