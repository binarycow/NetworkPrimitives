namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Indicates which special type of address (if any) that the address has.
    /// </summary>
    public enum Ipv4AddressRangeType
    {
        /// <summary>
        /// A normal address
        /// </summary>
        Normal,
        
        /// <summary>
        /// Private IP address, in the range 192.168.0.0/16
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1918">RFC 1918</seealso>
        Rfc1918,
        
        /// <summary>
        /// IP Range 100.64.0.0/10, used for carrier-grade NAT; Allocated by RFC 6598
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc6598">RFC 6598</seealso>
        CgNat,
        
        /// <summary>
        /// Documentation ranges; Allocated by RFC 5737.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5737">RFC 5737</seealso>
        Documentation,
        
        /// <summary>
        /// IP Range 169.254.0.0/16, used for link-local addresses[6] between two hosts on a single link
        /// when no IP address is otherwise specified, such as would have normally been retrieved from a DHCP server.
        /// Allocated by RFC 3927.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3927">RFC 3927</seealso>
        Apipa,
        
        /// <summary>
        /// IP Range 127.0.0.0/8, used for loopback addresses to the host.
        /// Allocated by RFC 1122.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1122">RFC 1122</seealso>
        Loopback,
        
        /// <summary>
        /// IP range 0.0.0.0/8
        /// </summary>
        Current,
        
        /// <summary>
        /// IP Address 255.255.255.255; The broadcast network
        /// </summary>
        Broadcast,
        
        /// <summary>
        /// IP Range 224.0.0.0/4; Multicast networks
        /// </summary>
        Multicast,
        
        /// <summary>
        /// IP Range 198.18.0.0/15, used for benchmark testing of inter-network communications between two separate subnets.
        /// </summary>
        Benchmark,
    }
}