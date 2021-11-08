#nullable enable

using System;
using System.Buffers.Binary;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    internal static class Ipv4Formatting
    {
        public static bool TryFormatDottedDecimal(uint value, Span<char> destination, out int charsWritten)
        {
            Span<byte> octets = stackalloc byte[4];
            BinaryPrimitives.TryWriteUInt32BigEndian(octets, value);
            charsWritten = 0;
            for (var i = 0; i < 4; ++i)
            {
                if (i != 0)
                {
                    if (!destination.TryWrite('.', ref charsWritten))
                        return false;
                }
                if (!octets[i].TryFormatTo(ref destination, ref charsWritten))
                    return false;
            }
            return true;
        }


        public static bool TryFormat(Ipv4Address value, Span<char> destination, out int charsWritten)
            => TryFormatDottedDecimal(value.Value, destination, out charsWritten);
        public static bool TryFormat(Ipv4SubnetMask value, Span<char> destination, out int charsWritten)
            => TryFormatDottedDecimal(value.Value, destination, out charsWritten);
        public static bool TryFormat(Ipv4WildcardMask value, Span<char> destination, out int charsWritten)
            => TryFormatDottedDecimal(value.Value, destination, out charsWritten);

        public static string ToString(Ipv4Subnet value, string? format, IFormatProvider? formatProvider) => format switch
        {
            null => $"{value.NetworkAddress.ToString()}/{value.Mask.ToCidr().ToString()}",
            "M" => $"{value.NetworkAddress.ToString()} {value.Mask.ToString()}",
            "C" => $"{value.NetworkAddress.ToString()}/{value.Mask.ToCidr().ToString()}",
            "W" => $"{value.NetworkAddress.ToString()} {value.Mask.ToWildcardMask().ToString()}",
            _ => throw new FormatException("Input string was not in a correct format."),
        };

        public static bool TryFormat(Ipv4Subnet value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? formatProvider)
        {
            charsWritten = 0;
            var success = format.Length switch
            {
                0 => TryFormatCidr(value, destination, ref charsWritten),
                1 when format[0] == 'M' => TryFormatMask(value, destination, ref charsWritten),
                1 when format[0] == 'C' => TryFormatCidr(value, destination, ref charsWritten),
                1 when format[0] == 'W' => TryFormatWildcard(value, destination, ref charsWritten),
                _ => throw new FormatException("Input string was not in a correct format."),
            };
            if (!success) charsWritten = default;
            return success;
            
            static bool TryFormatMask(Ipv4Subnet value, Span<char> destination, ref int charsWritten) 
                => value.NetworkAddress.TryWriteTo(ref destination, ref charsWritten)
                    && ' '.TryWriteTo(ref destination, ref charsWritten)
                    && value.Mask.TryWriteTo(ref destination, ref charsWritten);

            static bool TryFormatCidr(Ipv4Subnet value, Span<char> destination, ref int charsWritten)
                => value.NetworkAddress.TryWriteTo(ref destination, ref charsWritten)
                    && ' '.TryWriteTo(ref destination, ref charsWritten)
                    && value.Mask.ToCidr().TryWriteTo(ref destination, ref charsWritten);
            
            static bool TryFormatWildcard(Ipv4Subnet value, Span<char> destination, ref int charsWritten)
                => value.NetworkAddress.TryWriteTo(ref destination, ref charsWritten)
                    && ' '.TryWriteTo(ref destination, ref charsWritten)
                    && value.Mask.ToWildcardMask().TryWriteTo(ref destination, ref charsWritten);
        }

        public static bool TryWriteTo(this Ipv4Address value, ref Span<char> destination, ref int charsWritten)
        {
            if (!TryFormat(value, destination, out var len)) return false;
            destination = destination[len..];
            charsWritten += len;
            return true;
        }
        public static bool TryWriteTo(this char value, ref Span<char> destination, ref int charsWritten)
        {
            if (destination.Length == 0) return false;
            destination[0] = value;
            ++charsWritten;
            destination = destination[1..];
            return true;
        }
        public static bool TryWriteTo(this Ipv4SubnetMask value, ref Span<char> destination, ref int charsWritten)
        {
            if (!TryFormat(value, destination, out var len)) return false;
            destination = destination[len..];
            charsWritten += len;
            return true;
        }
        public static bool TryWriteTo(this Ipv4Cidr value, ref Span<char> destination, ref int charsWritten) 
            => value.Value.TryFormatTo(ref destination, ref charsWritten);

        public static bool TryWriteTo(this Ipv4WildcardMask value, ref Span<char> destination, ref int charsWritten)
        {
            if (!TryFormat(value, destination, out var len)) return false;
            destination = destination[len..];
            charsWritten += len;
            return true;
        }

        public static bool TryFormat(
            Ipv4AddressRange value,
            Span<char> destination, 
            out int charsWritten, 
            ReadOnlySpan<char> format,
            IFormatProvider? formatProvider
        )
        {
            throw new NotImplementedException();
        }
    }
}