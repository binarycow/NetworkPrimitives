using System;
using System.Buffers.Binary;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Switching
{
    /// <summary>
    /// Represents a vlan number
    /// </summary>
    public readonly struct VlanNumber 
        : IBinaryNetworkPrimitive<VlanNumber>,
            IEquatable<VlanNumber>,
            IEquatable<ushort>
    {
        internal ushort Value { get; }
        
        private VlanNumber(ushort value) => this.Value = value;

        /// <summary>
        /// Converts an <see cref="VlanNumber"/> to its <see cref="ushort"/> representation
        /// </summary>
        /// <param name="cidr">An instance of <see cref="VlanNumber"/></param>
        /// <returns>The <see cref="ushort"/> representation of the <see cref="VlanNumber"/></returns>
        public static explicit operator ushort(VlanNumber cidr) => cidr.Value;
        
        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="VlanNumber"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(VlanNumber other) => this.Value == other.Value;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="ushort"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(ushort other) => this.Value.Equals(other);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="VlanNumber"/> or
        /// <see cref="ushort"/> and equals the value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj switch
        {
            VlanNumber other => Equals(other),
            ushort other => Equals(other),
            _ => false,
        };
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => this.Value.GetHashCode();
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="VlanNumber"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(VlanNumber left, VlanNumber right) => left.Equals(right);

        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="VlanNumber"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(VlanNumber left, VlanNumber right) => !left.Equals(right);


        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="VlanNumber"/> is equal to an instance of <see cref="ushort"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ushort"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(VlanNumber left, ushort right) => left.Value == right;

        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="VlanNumber"/> is not equal to an instance of <see cref="ushort"/>
        /// </summary>
        /// <param name="left">
        /// An <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <param name="right">
        /// A <see cref="ushort"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(VlanNumber left, ushort right) => left.Value != right;

        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="ushort"/> is equal to an instance of <see cref="VlanNumber"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="ushort"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(ushort left, VlanNumber right) => left == right.Value;
        
        /// <summary>
        /// Returns a value indicating whether an instance of <see cref="ushort"/> is not equal to an instance of <see cref="VlanNumber"/>
        /// </summary>
        /// <param name="left">
        /// A <see cref="ushort"/> to compare.
        /// </param>
        /// <param name="right">
        /// An <see cref="VlanNumber"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(ushort left, VlanNumber right) => left != right.Value;
        
        #endregion Equality
        
        #region Formatting
        
        int ITryFormat.MaximumLengthRequired => 4;
        
        /// <summary>
        /// Tries to format the current vlan number into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the vlan number, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten)
        {
            charsWritten = default;
            if (destination.Length == 0)
                return false;
            if (Value == 0)
            {
                destination[0] = '0';
                charsWritten = 1;
                return true;
            }
            Span<char> chars = stackalloc char[4];
            var copy = chars;
            var value = Value;
            while (value > 0)
            {
                var digit = value % 10;
                value /= 10;
                copy[^1] = (char)('0' + digit);
                copy = copy[..^1];
                ++charsWritten;
            }
            copy = chars[^charsWritten..];
            if (copy.TryCopyTo(destination))
                return true;
            charsWritten = default;
            return false;
        }

        /// <summary>
        /// Converts an <see cref="VlanNumber"/> to its <see cref="string"/> representation
        /// </summary>
        /// <returns>
        /// A string that contains the vlan number in its <see cref="string"/> representation
        /// </returns>
        public override string ToString() => this.GetString();
        

        /// <summary>
        /// Write this vlan number's binary representation to the byte span.
        /// </summary>
        /// <param name="destination">
        /// If successful, this vlan number's binary representation, in big-endian form.
        /// </param>
        /// <param name="bytesWritten">
        /// The number of bytes written to <paramref name="destination"/>
        /// </param>
        /// <returns>
        /// <see langword="true"/> if successful; otherwise, <see langword="false"/>
        /// </returns>
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten)
        {
            bytesWritten = 0;
            return Value.TryWriteBigEndian(ref destination, ref bytesWritten);
        }

        /// <summary>
        /// Create a byte array that represents this vlan number.
        /// </summary>
        /// <returns>
        /// This vlan number as a byte array, in big-endian form.
        /// </returns>
        public byte[] GetBytes()
        {
            var bytes = new byte[2];
            TryWriteBytes(bytes, out _);
            return bytes;
        }

        #endregion Formatting
        
        #region Parsing

        /// <summary>
        /// Converts a vlan number represented as a character span to an <see cref="VlanNumber"/> instance.
        /// </summary>
        /// <param name="text">
        /// A character span that contains a vlan number
        /// </param>
        /// <returns>
        /// The converted vlan number
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid vlan number
        /// </exception>
        public static VlanNumber Parse(ReadOnlySpan<char> text)
            => TryParse(text, out var result) ? result : throw new FormatException();

        /// <summary>
        /// Converts a vlan number string to an <see cref="VlanNumber"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains the vlan number in text form.
        /// </param>
        /// <returns>
        /// An <see cref="VlanNumber"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid vlan number
        /// </exception>
        public static VlanNumber Parse(string? text)
            => TryParse(text, out var result) ? result : throw new FormatException();

        /// <summary>
        /// Converts an integer that represents a vlan number to an <see cref="VlanNumber"/> instance.
        /// </summary>
        /// <param name="value">
        /// An integer that represents a vlan number.
        /// </param>
        /// <returns>
        /// The <see cref="VlanNumber"/> representation of <paramref name="value"/>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not a valid vlan number
        /// </exception>
        public static VlanNumber Parse(int value)
            => TryParse(value, out var result) ? result : throw new ArgumentOutOfRangeException(nameof(value));

        #endregion Parsing
        
        #region TryParse
        
        /// <summary>
        /// Converts an integer that represents a vlan number to an <see cref="VlanNumber"/> instance.
        /// </summary>
        /// <param name="value">
        /// An integer that represents a vlan number.
        /// </param>
        /// <param name="result">
        /// The <see cref="VlanNumber"/> representation of <paramref name="value"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was able to be converted to an
        ///     <see cref="VlanNumber"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(int value, out VlanNumber result)
        {
            if (value is >= 1 and <= 4095)
            {
                result = new ((ushort)value);
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Determines whether the specified string represents a valid vlan number
        /// </summary>
        /// <param name="text">
        /// A string that represents a vlan number
        /// </param>
        /// <param name="result">
        /// The <see cref="VlanNumber"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="VlanNumber"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out VlanNumber result)
        {
            result = default;
            return text is not null && VlanNumber.TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        }

        /// <summary>
        /// Determines whether the specified string represents a valid vlan number
        /// </summary>
        /// <param name="text">
        /// A string that represents a vlan number.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid vlan number.
        /// </param>
        /// <param name="result">
        /// The <see cref="VlanNumber"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="VlanNumber"/>; otherwise, <see langword="false"/>.
        ///     If <paramref name="text"/> begins with a valid <see cref="VlanNumber"/>, and is followed by other characters, this method will
        ///     return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="VlanNumber"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out VlanNumber result)
        {
            var span = (text ?? string.Empty).AsSpan();
            charsRead = default;
            return TryParse(ref span, ref charsRead, out result);
        }
        
        /// <summary>
        /// Determines whether the specified character span represents a valid vlan number.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid vlan number.
        /// </param>
        /// <param name="result">
        /// The <see cref="VlanNumber"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="VlanNumber"/>; otherwise, <see langword="false"/>.
        ///     If <paramref name="text"/> begins with a valid <see cref="VlanNumber"/>, and is followed by other characters, this method will
        ///     return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="VlanNumber"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out VlanNumber result)
        {
            result = default;
            charsRead = default;
            return TryParse(ref text, ref charsRead, out result);
        }
        
        /// <summary>
        /// Determines whether the specified character span represents a valid vlan number.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="VlanNumber"/> representation of <paramref name="text"/>
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="text"/> was able to be converted to an
        ///     <see cref="VlanNumber"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out VlanNumber result)
            => TryParse(text, out var charsRead, out result) && charsRead == text.Length;
        
        internal static bool TryParse(ref ReadOnlySpan<char> text, ref int charsRead, out VlanNumber result)
        {
            result = default;
            if (!text.TryParseVlanNumber(ref charsRead, out var value))
                return false;
            result = new (value);
            return true;
        }
        
        #endregion TryParse

    }
}