#nullable enable

using System;
using System.Buffers.Binary;
using System.Net;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    [ExcludeFromCodeCoverage("Internal")]
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
        
        public static IPAddress ToIpAddress(this uint value)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            Span<byte> bytes = stackalloc byte[4];
#else
            var bytes = new byte[4];
#endif
            value.TryWriteBigEndian(bytes, out _);
            return new (bytes);
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
        
        

        public static bool TryFormat(Ipv4NetworkMatch value, Span<char> destination, out int charsWritten)
        {
            charsWritten = default;
            return value.Address.TryWriteTo(ref destination, ref charsWritten)
                && ' '.TryWriteTo(ref destination, ref charsWritten)
                && value.Mask.TryWriteTo(ref destination, ref charsWritten);
        }

        public static bool TryFormat(Ipv4Subnet value, Span<char> destination, out int charsWritten, string? format, IFormatProvider? formatProvider)
        {
            charsWritten = 0;
            var success = format switch
            {
                null => TryFormatCidr(value, destination, ref charsWritten),
                "" => TryFormatCidr(value, destination, ref charsWritten),
                "M" => TryFormatMask(value, destination, ref charsWritten),
                "C" => TryFormatCidr(value, destination, ref charsWritten),
                "W" => TryFormatWildcard(value, destination, ref charsWritten),
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
                    && '/'.TryWriteTo(ref destination, ref charsWritten)
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
        
        internal static bool TryWriteTo(this char value, ref Span<char> destination, ref int charsWritten)
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
            string? format,
            IFormatProvider? formatProvider
        )
        {
            charsWritten = default;
            var success = format switch
            {
                _ when value.IsSubnet(out var subnet) => TryFormat(subnet, destination, out charsWritten, format, formatProvider),
                "C" or "M" or "W" => throw new FormatException($"Cannot use '{format}' format string unless range is a valid {nameof(Ipv4Subnet)}"),
                null when value.Length <= 1 => TryFormat(value.StartAddress, destination, out charsWritten),
                null when value.AllIpsSameFirstThreeOctets => TryFormatRange(value, destination, out charsWritten),
                null => false,
                _ => throw new FormatException("Input string was not in a correct format."),
            };
            if (!success) charsWritten = default;
            return success;

            static bool TryFormatRange(
                Ipv4AddressRange value,
                Span<char> destination, 
                out int charsWritten
            )
            {
                charsWritten = default;
                Span<char> chars = stackalloc char[Ipv4Address.MAXIMUM_LENGTH + 4];
                var lastOctet = value.LastAddressInclusive.GetOctet(3);
                return value.StartAddress.TryWriteTo(ref chars, ref charsWritten)
                    && '-'.TryWriteTo(ref chars, ref charsWritten)
                    && lastOctet.TryFormatTo(ref chars, ref charsWritten)
                    && chars[..charsWritten].TryCopyTo(destination);
            }
        }
    }
}