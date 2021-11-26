#nullable enable

using System;

namespace NetworkPrimitives.Switching
{
    internal static class MacFormatting
    {
        public static bool TryFormat(
            MacAddress macAddress, 
            Span<char> destination, 
            out int charsWritten, 
            string? format, 
            MacAddressFormat formatProvider
        )
        {
            charsWritten = default;
            if (destination.Length == 0)
                return false;
            return format switch
            {
                null or "" or "G" or "g" => TryFormat(macAddress, destination, out charsWritten, formatProvider),
                _ => TryFormat(macAddress, destination, out charsWritten, DeriveNewFormat(formatProvider, format.GetSpan())),
            };
        }

        public static bool TryFormatCisco(
            MacAddress macAddress,
            Span<char> destination,
            out int charsWritten
        ) => TryFormat(macAddress, destination, out charsWritten, MacAddressFormats.Cisco);
        
        
        private static MacAddressFormat DeriveNewFormat(MacAddressFormat macFormat, ReadOnlySpan<char> formatString)
        {
            var (nibblesPerGroup, groupSeparator, casing) = macFormat;
            // 6:X
            if (formatString.Length == 0) return Validate(nibblesPerGroup, groupSeparator, casing);
            if (TryReadNibblesPerGroup(formatString, out var length, out var newNibblesPerGroup))
            {
                nibblesPerGroup = newNibblesPerGroup;
                formatString = formatString[length..];
            }
            if (formatString.Length == 0) return Validate(nibblesPerGroup, groupSeparator, casing);
            if (formatString[0] is not ('X' or 'x'))
            {
                groupSeparator = formatString[0];
                formatString = formatString[1..];
            }
            if (formatString.Length == 0) return Validate(nibblesPerGroup, groupSeparator, casing);
            if (formatString[0] is not ('X' or 'x'))
                throw new FormatException();
            casing = formatString[0] == 'X' ? Casing.Uppercase : Casing.Lowercase;
            formatString = formatString[1..];
            if (formatString.Length != 0) throw new FormatException();
            return Validate(nibblesPerGroup, groupSeparator, casing);

            static bool TryReadNibblesPerGroup(ReadOnlySpan<char> formatString, out int length, out int nibblesPerGroup)
            {
                switch (formatString.Length)
                {
                    case >= 2 when formatString[0] is '1' && formatString[1] is >= '0' and <= '2':
                        nibblesPerGroup = 10 + (formatString[1] - '0');
                        length = 2;
                        return true;
                    case >= 1 when formatString[0] is >= '0' and <= '9':
                        nibblesPerGroup = formatString[0] - '0';
                        length = 1;
                        return true;
                    default:
                        length = 0;
                        nibblesPerGroup = 0;
                        return false;
                }
            }
            
            static MacAddressFormat Validate(int nibblesPerGroup, char groupSeparator, Casing casing)
            {
                var validNibbles = nibblesPerGroup is 1 or 2 or 3 or 4 or 6 or 12;
                var validSep = nibblesPerGroup is 12 || groupSeparator is not '\0';
                if (validNibbles && validSep)
                    return new (nibblesPerGroup, groupSeparator, casing);
                throw new FormatException();
            }
        }


        public static bool TryFormat(
            MacAddress macAddress, 
            Span<char> destination, 
            out int charsWritten, 
            MacAddressFormat format
        )
        {
            charsWritten = default;
            if (destination.Length < format.CharactersRequired)
            {
                return false;
            }
            
            Span<byte> bytes = stackalloc byte[6];
            macAddress.TryWriteBytes(bytes, out _);
            
            Span<byte> nibbles = stackalloc byte[12];
            MacFormatting.FillNibbles(bytes, nibbles);

            var nibblesWritten = 0;
            for (var i = 0; i < nibbles.Length; ++i)
            {
                var nibble = nibbles[i];
                var nibbleChar = MacFormatting.GetNibbleChar(nibble, format.Casing);
                MacFormatting.WriteChar(nibbleChar, ref destination, ref charsWritten);
                ++nibblesWritten;
                if(nibblesWritten < format.NibblesPerGroup || i == nibbles.Length - 1)
                    continue;
                MacFormatting.WriteChar(format.GroupSeparator, ref destination, ref charsWritten);
                nibblesWritten = 0;
            }
            return true;
        }

        
        private static void WriteChar(
            char charToWrite, 
            ref Span<char> destination, 
            ref int charsWritten
        )
        {
            destination[0] = charToWrite;
            destination = destination[1..];
            ++charsWritten;
        }

        private static char GetNibbleChar(byte nibble, Casing casing) => (nibble, casing) switch
        {
            (>= 0 and <= 9, _) => (char)('0' + nibble),
            (> 9, Casing.Uppercase) => (char)('A' + nibble - 10),
            (> 9, Casing.Lowercase) => (char)('a' + nibble - 10),
            _ => (char)('a' + nibble - 10),
        };

        private static void FillNibbles(Span<byte> bytes, Span<byte> nibbles)
        {
            if (nibbles.Length != bytes.Length * 2)
                throw new ArgumentException($"Length of {nameof(nibbles)} must be twice the length of {nameof(bytes)}");
            for (var byteIndex = 0; byteIndex < bytes.Length; ++byteIndex)
            {
                nibbles[byteIndex * 2] = (byte)(bytes[byteIndex] >> 4);
                nibbles[byteIndex * 2 + 1] = (byte)(bytes[byteIndex] & 0xF);
            }
        }
    }
}