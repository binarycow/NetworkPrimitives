using System;
using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Utilities;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Represents an IPv4 Subnet
    /// </summary>
    public readonly struct Ipv4Subnet : INetworkPrimitive<Ipv4Subnet>
    {
        /// <summary>
        /// The network address of the subnet
        /// </summary>
        public Ipv4Address NetworkAddress { get; }
        
        /// <summary>
        /// The subnet mask
        /// </summary>
        public Ipv4SubnetMask Mask { get; }

        /// <summary>
        /// The subnet mask, in CIDR notation
        /// </summary>
        public Ipv4Cidr Cidr => Mask.ToCidr();
        
        /// <summary>
        /// Create a subnet with an <see cref="Ipv4Address"/> and <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="address">An IP address</param>
        /// <param name="mask">A subnet mask</param>
        public Ipv4Subnet(Ipv4Address address, Ipv4SubnetMask mask)
        {
            this.NetworkAddress = address & mask;
            this.Mask = mask;
        }

        /// <summary>
        /// Create a subnet with an <see cref="Ipv4Address"/> and <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="address">An IP address</param>
        /// <param name="cidr">A CIDR</param>
        public Ipv4Subnet(Ipv4Address address, Ipv4Cidr cidr) : this(address, cidr.ToSubnetMask())
        {
            
        }
        
        /// <summary>
        /// Deconstruct an <see cref="Ipv4Subnet"/> into an <see cref="Ipv4Address"/>, <see cref="Ipv4SubnetMask"/>, and <see cref="Ipv4Cidr"/>
        /// </summary>
        /// <param name="networkAddress">The network address</param>
        /// <param name="mask">The subnet mask</param>
        /// <param name="cidr">The CIDR</param>
        public void Deconstruct(out Ipv4Address networkAddress, out Ipv4SubnetMask mask, out Ipv4Cidr cidr)
        {
            networkAddress = this.NetworkAddress;
            mask = this.Mask;
            cidr = this.Cidr;
        }
        
        /// <summary>
        /// Deconstruct an <see cref="Ipv4Subnet"/> into an <see cref="Ipv4Address"/> and <see cref="Ipv4SubnetMask"/>
        /// </summary>
        /// <param name="networkAddress">The network address</param>
        /// <param name="mask">The subnet mask</param>
        public void Deconstruct(out Ipv4Address networkAddress, out Ipv4SubnetMask mask)
        {
            networkAddress = this.NetworkAddress;
            mask = this.Mask;
        }

        #region Subnetting

        /// <summary>
        /// The total number of hosts allowed by this <see cref="Ipv4Cidr"/>.
        /// Includes the broadcast and network addresses.
        /// </summary>
        public ulong TotalHosts => Mask.TotalHosts;
        
        /// <summary>
        /// The number of usable hosts allowed by this <see cref="Ipv4Cidr"/>.
        /// Does not include the broadcast or network address.
        /// RFC 3021 states that for a /31 network, the number of usable hosts is 2.
        /// The number of usable hosts for a /32 network is 1.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3021">RFC3021</seealso>
        public uint UsableHosts => Mask.UsableHosts;
        
        /// <summary>
        /// The broadcast address of the subnet.
        /// </summary>
        public Ipv4Address BroadcastAddress => Mask.Value switch
        {
            0xFFFFFFFF => this.NetworkAddress,
            0xFFFFFFFE => this.NetworkAddress.AddInternal(1),
            _ => NetworkAddress.AddInternal((uint)(Mask.TotalHosts - 1)),
        };
        
        /// <summary>
        /// The first usable address in the subnet.
        /// </summary>
        /// <remarks>
        /// This property respects RFC 3021.
        /// </remarks>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3021">RFC3021</seealso>
        public Ipv4Address FirstUsable => Mask.Value switch
        {
            0xFFFFFFFF => this.NetworkAddress,
            0xFFFFFFFE => this.NetworkAddress,
            _ => this.NetworkAddress.AddInternal(1),
        };
        
        /// <summary>
        /// The last usable address in the subnet.
        /// </summary>
        /// <remarks>
        /// This property respects RFC 3021.
        /// </remarks>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3021">RFC3021</seealso>
        public Ipv4Address LastUsable => Mask.Value switch
        {
            0xFFFFFFFF => this.NetworkAddress,
            0xFFFFFFFE => this.NetworkAddress.AddInternal(1),
            _ => NetworkAddress.AddInternal((uint)(Mask.TotalHosts - 2)),
        };

        /// <summary>
        /// Calculate the largest subnet that contains all of the given subnets
        /// </summary>
        /// <param name="subnets">
        /// A collection of subnets to supernet.
        /// </param>
        /// <returns>
        /// A subnet that contains all of the subnets in the collection <paramref name="subnets"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(params Ipv4Subnet[] subnets)
            => SubnetOperations.GetContainingSupernet(subnets);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given subnets
        /// </summary>
        /// <param name="subnets">
        /// A collection of subnets to supernet.
        /// </param>
        /// <returns>
        /// A subnet that contains all of the subnets in the collection <paramref name="subnets"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(IEnumerable<Ipv4Subnet> subnets)
            => SubnetOperations.GetContainingSupernet(subnets);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given subnets
        /// </summary>
        /// <param name="a">
        /// The first subnet
        /// </param>
        /// <param name="b">
        /// The second subnet
        /// </param>
        /// <returns>
        /// A subnet that contains subnets <paramref name="a"/> and <paramref name="b"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(Ipv4Subnet a, Ipv4Subnet b)
            => SubnetOperations.GetContainingSupernet(a, b);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given subnets
        /// </summary>
        /// <param name="a">
        /// The first subnet
        /// </param>
        /// <param name="b">
        /// The second subnet
        /// </param>
        /// <param name="c">
        /// The second subnet
        /// </param>
        /// <returns>
        /// A subnet that contains subnets <paramref name="a"/>, <paramref name="b"/>, and <paramref name="c"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(Ipv4Subnet a, Ipv4Subnet b, Ipv4Subnet c)
            => SubnetOperations.GetContainingSupernet(a, SubnetOperations.GetContainingSupernet(b, c));
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given addresses
        /// </summary>
        /// <param name="addresses">
        /// A collection of addresses to supernet.
        /// </param>
        /// <returns>
        /// A subnet that contains all of the addresses in the collection <paramref name="addresses"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(params Ipv4Address[] addresses)
            => SubnetOperations.GetContainingSupernet(addresses);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given addresses
        /// </summary>
        /// <param name="addresses">
        /// A collection of addresses to supernet.
        /// </param>
        /// <returns>
        /// A subnet that contains all of the addresses in the collection <paramref name="addresses"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(IEnumerable<Ipv4Address> addresses)
            => SubnetOperations.GetContainingSupernet(addresses);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given addresses
        /// </summary>
        /// <param name="a">
        /// The first address
        /// </param>
        /// <param name="b">
        /// The second address
        /// </param>
        /// <returns>
        /// A subnet that contains addresses <paramref name="a"/> and <paramref name="b"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(Ipv4Address a, Ipv4Address b)
            => SubnetOperations.GetContainingSupernet(a, b);
        
        /// <summary>
        /// Calculate the largest subnet that contains all of the given addresses
        /// </summary>
        /// <param name="a">
        /// The first address
        /// </param>
        /// <param name="b">
        /// The second address
        /// </param>
        /// <param name="c">
        /// The second address
        /// </param>
        /// <returns>
        /// A subnet that contains addresses <paramref name="a"/>, <paramref name="b"/>, and <paramref name="c"/>
        /// </returns>
        public static Ipv4Subnet GetContainingSupernet(Ipv4Address a, Ipv4Address b, Ipv4Address c)
            => SubnetOperations.GetContainingSupernet(a, SubnetOperations.GetContainingSupernet(b, c));

        /// <summary>
        /// Determine if a given <see cref="Ipv4Address"/> is part of this <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="address">
        /// The address to check.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> is contained within this instance, otherwise, <see langword="false"/>
        /// </returns>
        public bool Contains(Ipv4Address address) => (address & this.Mask) == NetworkAddress;
        
        /// <summary>
        /// Determine if a given <see cref="Ipv4Subnet"/> is part of this <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="subnet">
        /// The subnet to check.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="subnet"/> is entirely contained within this instance, otherwise, <see langword="false"/>
        /// </returns>
        public bool Contains(Ipv4Subnet subnet)
        {
            if (subnet.Mask.ToCidr() < this.Mask.ToCidr())
                return false;
            return (subnet.NetworkAddress & this.Mask) == (this.NetworkAddress & this.Mask);
        }
        
        /// <summary>
        /// Attempt to split this subnet into two smaller subnets.
        /// </summary>
        /// <param name="low">
        /// If successful, an <see cref="Ipv4Subnet"/> that is exactly one half the size of this instance,
        /// with a network address the same as this instance's network address.
        /// </param>
        /// <param name="high">
        /// If successful, an <see cref="Ipv4Subnet"/> that is exactly one half the size of this instance,
        /// with a broadcast address the same as this instance's broadcast address.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if successful.
        /// This method will only fail if this subnet's subnet mask is 255.255.255.255 (or 32 in CIDR notation).
        /// </returns>
        public bool TrySplit(out Ipv4Subnet low, out Ipv4Subnet high)
        {
            low = default;
            high = default;
            var cidr = Mask.ToCidr();
            if (cidr.Value == 32) return false;
            var highAddress = NetworkAddress.AddInternal((uint)(TotalHosts / 2));
            cidr = Ipv4Cidr.Parse((byte)(cidr.Value + 1));
            low = new (NetworkAddress, cidr);
            high = new (highAddress, cidr);
            return true;
        }

        /// <summary>
        /// Attempt to get the parent subnet of this subnet.
        /// </summary>
        /// <param name="parentSubnet">
        /// If successful, an <see cref="Ipv4Subnet"/> that is exactly twice the size of this instance,
        /// with a network address the same as this instance's network address.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if successful.
        /// This method will only fail if this subnet's subnet mask is 0.0.0.0 (or 0 in CIDR notation).
        /// </returns>
        public bool TryGetParentSubnet(out Ipv4Subnet parentSubnet) 
            => SubnetOperations.TryGetParent(this, out parentSubnet);

        #endregion Subnetting
        

        #region Formatting

        int ITryFormat.MaximumLengthRequired => Ipv4Address.MAXIMUM_LENGTH * 2 + 1;

        /// <summary>
        /// Converts an <see cref="Ipv4Subnet"/> to its <see cref="string"/> representation (in CIDR notation)
        /// </summary>
        /// <returns>
        /// A string that contains this subnet in CIDR notation
        /// </returns>
        public override string ToString() => this.GetString();

        
        /// <summary>
        /// Tries to format the current subnet into the provided span.
        /// </summary>
        /// <param name="destination">
        /// When this method returns, the subnet, as a span of characters.
        /// </param>
        /// <param name="charsWritten">
        /// When this method returns, the number of characters written into the span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the formatting was successful; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryFormat(Span<char> destination, out int charsWritten) 
            => Ipv4Formatting.TryFormat(this, destination, out charsWritten, null, null);
        
        #endregion Formatting


        #region Equality

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Subnet"/>.
        /// </summary>
        /// <param name="obj">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Ipv4Subnet"/> and
        /// equals the value of this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) => obj is Ipv4Subnet other && Equals(other);
        
        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified <see cref="Ipv4Subnet"/>.
        /// </summary>
        /// <param name="other">
        /// A value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same value as this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(Ipv4Subnet other) => this.NetworkAddress.Equals(other.NetworkAddress) && this.Mask.Equals(other.Mask);
        
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer hash code.
        /// </returns>
        public override int GetHashCode() => HashCode.Combine(this.NetworkAddress, this.Mask);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Subnet"/> are equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Subnet"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Subnet"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator ==(Ipv4Subnet left, Ipv4Subnet right) => left.Equals(right);
        
        /// <summary>
        /// Returns a value indicating whether two instances of <see cref="Ipv4Subnet"/> are not equal.
        /// </summary>
        /// <param name="left">
        /// The first <see cref="Ipv4Subnet"/> to compare.
        /// </param>
        /// <param name="right">
        /// The second <see cref="Ipv4Subnet"/> to compare.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise <see langword="false"/>.
        /// </returns>
        public static bool operator !=(Ipv4Subnet left, Ipv4Subnet right) => !left.Equals(right);

        #endregion Equality
        
        #region Enumeration

        
        /// <summary>
        /// Create an instance of <see cref="Ipv4AddressRange"/> that reflects the usable addresses in this subnet.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="Ipv4AddressRange"/> that reflects the usable addresses in this subnet.
        /// </returns>
        /// <remarks>
        /// This method respects RFC 3021.
        /// </remarks>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3021">RFC3021</seealso>
        public Ipv4AddressRange GetUsableAddresses() => new(this.FirstUsable, this.UsableHosts - 1);
        
        /// <summary>
        /// Create an instance of <see cref="Ipv4AddressRange"/> that reflects all addresses in this subnet.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="Ipv4AddressRange"/> that reflects all addresses in this subnet.
        /// </returns>
        public Ipv4AddressRange GetAllAddresses() => new (NetworkAddress, (uint)(TotalHosts - 1));


        #endregion Enumeration
        

        #region Parse

        
        /// <summary>
        /// Parse a subnet into an <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="address">
        /// A string that contains an IPv4 address in dotted decimal form.
        /// </param>
        /// <param name="mask">
        /// A string that contains a subnet mask in dotted decimal form.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4Subnet"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address"/> is <see langword="null"/>.
        /// <paramref name="mask"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="address"/> is not a valid IPv4 address.
        /// <paramref name="mask"/> is not a valid IPv4 subnet mask.
        /// </exception>
        public static Ipv4Subnet Parse(string? address, string? mask)
        {
            address = address ?? throw new ArgumentNullException(nameof(address));
            mask = mask ?? throw new ArgumentNullException(nameof(mask));
            return Ipv4Subnet.TryParse(address, mask, out var result)
                ? result
                : throw new FormatException();
        }


        /// <summary>
        /// Parse a subnet into an <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="address">
        /// A string that contains an IPv4 address in dotted decimal form.
        /// </param>
        /// <param name="cidr">
        /// An IPv4 CIDR string that contains a subnet mask in dotted decimal form.
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4Subnet"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="address"/> is not a valid IPv4 address.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="cidr"/> is not a valid IPv4 CIDR.
        /// </exception>
        public static Ipv4Subnet Parse(string? address, int cidr)
        {
            address = address ?? throw new ArgumentNullException(nameof(address));
            if (!Ipv4Address.TryParse(address, out var add))
                throw new FormatException();
            if (cidr is < 0 or > 32)
                throw new ArgumentOutOfRangeException(nameof(cidr));
            return new (add, Ipv4Cidr.Parse(cidr));
        }

        /// <summary>
        /// Parse a subnet into an <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="text">
        /// A string that contains a subnet in any of the supported formats (see examples).
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4Subnet"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid <see cref="Ipv4Subnet"/>.
        /// </exception>
        /// <example>10.0.0.0/24</example>
        /// <example>10.0.0.5</example>
        /// <example>10.0.0.0 255.255.255.0</example>
        public static Ipv4Subnet Parse(string? text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            return Ipv4Subnet.TryParse(text, out var result)
                ? result
                : throw new FormatException();
        }

        /// <summary>
        /// Parse a subnet into an <see cref="Ipv4Subnet"/> instance.
        /// </summary>
        /// <param name="text">
        /// A span that contains a subnet in any of the supported formats (see examples).
        /// </param>
        /// <returns>
        /// An <see cref="Ipv4Subnet"/> instance.
        /// </returns>
        /// <exception cref="FormatException">
        /// <paramref name="text"/> is not a valid <see cref="Ipv4Subnet"/>.
        /// </exception>
        /// <example>10.0.0.0/24</example>
        /// <example>10.0.0.5</example>
        /// <example>10.0.0.0 255.255.255.0</example>
        public static Ipv4Subnet Parse(ReadOnlySpan<char> text)
            => Ipv4Subnet.TryParse(text, out var result)
                ? result
                : throw new FormatException();

        private static bool ParseExact(Ipv4Address address, out Ipv4Subnet result, out bool implicitSlash32)
        {
            result = new (address, Ipv4Cidr.Parse(32));
            implicitSlash32 = true;
            return true;
        }
        

        #endregion Parse
        
        
        #region TryParse

        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 subnet.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4Subnet"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Subnet"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Subnet result)
            => TryParse(text, out charsRead, out result, out _);


        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 subnet.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <param name="implicitSlash32">
        /// <see langword="true"/> if <paramref name="text"/> contained a valid IPv4 address, with no subnet mask or CIDR.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an IP address; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4Subnet"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Subnet"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            charsRead = default;
            return Ipv4Subnet.TryParseInternal(ref text, ref charsRead, out result, out implicitSlash32);
        }
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Subnet result)
            => TryParse(text, out result, out _);
        
        /// <summary>
        /// Determines whether the specified character span represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The character span to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <param name="implicitSlash32">
        /// <see langword="true"/> if <paramref name="text"/> contained a valid IPv4 address, with no subnet mask or CIDR.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> text, out Ipv4Subnet result, out bool implicitSlash32)
            => TryParse(text, out var charsRead, out result, out implicitSlash32) && charsRead == text.Length;

        private static bool TryParseWithCidr(ref ReadOnlySpan<char> text, ref int charsRead, Ipv4Address address, out Ipv4Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, '/'))
                return false;
            if (!Ipv4Cidr.TryParse(ref textCopy, ref charsReadCopy, out var cidr))
                return false;
            result = new (address, cidr);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }
        
        private static bool TryParseWithMask(ref ReadOnlySpan<char> text, ref int charsRead, Ipv4Address address, out Ipv4Subnet result)
        {
            result = default;
            var textCopy = text;
            var charsReadCopy = charsRead;
            if (!textCopy.TryReadCharacter(ref charsReadCopy, ' '))
                return false;
            if (!Ipv4SubnetMask.TryParse(ref textCopy, ref charsReadCopy, out var mask))
                return false;
            result = new (address, mask);
            charsRead = charsReadCopy;
            text = textCopy;
            return true;
        }
        
        
        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="address">
        /// The string to validate.
        /// </param>
        /// <param name="cidr">
        /// An IPv4 CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> and <paramref name="cidr"/> were able to be
        /// parsed into an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? address, int cidr, out Ipv4Subnet result)
        {
            result = default;
            if (!Ipv4Address.TryParse(address, out var parsedAddress))
                return false;
            if (!Ipv4Cidr.TryParse(cidr, out var parsedCidr))
                return false;
            result = new Ipv4Subnet(parsedAddress, parsedCidr);
            return true;
        }


        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="address">
        /// The string to validate.
        /// </param>
        /// <param name="maskOrCidr">
        /// A subnet mask in dotted decimal notation, or a valid IPv4 CIDR.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="address"/> and <paramref name="maskOrCidr"/> were able to be
        /// parsed into an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? address, string? maskOrCidr, out Ipv4Subnet result)
        {
            result = default;
            if (!Ipv4Address.TryParse(address, out var parsedAddress))
                return false;
            if (Ipv4SubnetMask.TryParse(maskOrCidr, out var mask))
            {
                result = new Ipv4Subnet(parsedAddress, mask);
                return true;
            }
            if (!Ipv4Cidr.TryParse(maskOrCidr, out var cidr))
                return false;
            result = new Ipv4Subnet(parsedAddress, cidr);
            return true;
        }
        
        
        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? text, out Ipv4Subnet result)
            => Ipv4Subnet.TryParseInternal(text, out _, out result, out _);
        

        /// <summary>
        /// Determines whether the specified string represents a valid IPv4 subnet.
        /// </summary>
        /// <param name="text">
        /// The string to validate.
        /// </param>
        /// <param name="charsRead">
        /// The length of the valid Ipv4 subnet.
        /// </param>
        /// <param name="result">
        /// The <see cref="Ipv4Subnet"/> version of the character span.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="text"/> was able to be parsed as an <see cref="Ipv4Subnet"/>; otherwise, <see langword="false"/>.
        /// 
        /// If <paramref name="text"/> begins with a valid <see cref="Ipv4Subnet"/>, and is followed by other characters, this method will
        /// return <see langword="true"/>.  <paramref name="charsRead"/> will contain the length of the valid <see cref="Ipv4Subnet"/>.
        /// </returns>
        public static bool TryParse(string? text, out int charsRead, out Ipv4Subnet result)
            => Ipv4Subnet.TryParseInternal(text, out charsRead, out result, out _);
        
        internal static bool TryParseInternal(string? text, out int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            var span = (text ?? string.Empty).AsSpan();
            charsRead = default;
            return Ipv4Subnet.TryParseInternal(ref span, ref charsRead, out result, out implicitSlash32);
        }

        internal static bool TryParseInternal(ref ReadOnlySpan<char> text, ref int charsRead, out Ipv4Subnet result, out bool implicitSlash32)
        {
            implicitSlash32 = default;
            result = default;
            if (!Ipv4Address.TryParse(ref text, ref charsRead, out var address))
                return false;
            return TryParseWithCidr(ref text, ref charsRead, address, out result)
                || TryParseWithMask(ref text, ref charsRead, address, out result)
                || ParseExact(address, out result, out implicitSlash32);
        }
        #endregion TryParse
    }
}