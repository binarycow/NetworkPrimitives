#nullable enable

using System;

namespace NetworkPrimitives.Ipv4
{
    /// <summary>
    /// Provides access to many well-defined Ipv4 address ranges.
    /// </summary>
    public static class Ipv4WellKnownRanges
    {
        /// <summary>
        /// Private IP address, in the range 10.0.0.0/8; Allocated by RFC 1918
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1918">RFC 1918</seealso>
        public static readonly Ipv4Subnet Rfc1918A = Ipv4Address.Parse(0x0A000000) + Ipv4Cidr.Parse(8);
        
        /// <summary>
        /// Private IP address, in the range 172.16.0.0/12; Allocated by RFC 1918
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1918">RFC 1918</seealso>
        public static readonly Ipv4Subnet Rfc1918B = Ipv4Address.Parse(0xAC100000) + Ipv4Cidr.Parse(12);
        
        /// <summary>
        /// Private IP address, in the range 192.168.0.0/16; Allocated by RFC 1918
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1918">RFC 1918</seealso>
        public static readonly Ipv4Subnet Rfc1918C = Ipv4Address.Parse(0xC0A80000) + Ipv4Cidr.Parse(16);
        
        /// <summary>
        /// IP Range 100.64.0.0/10, used for carrier-grade NAT; Allocated by RFC 6598
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc6598">RFC 6598</seealso>
        public static readonly Ipv4Subnet CgNat = Ipv4Address.Parse(0x64400000) + Ipv4Cidr.Parse(10);
        
        /// <summary>
        /// IP Range 192.0.2.0/24, used for documentation; Allocated by RFC 5737.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5737">RFC 5737</seealso>
        public static readonly Ipv4Subnet Doc1 = Ipv4Address.Parse(0xC0000200) + Ipv4Cidr.Parse(24);
        
        /// <summary>
        /// IP Range 198.51.100.0/24, used for documentation; Allocated by RFC 5737.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5737">RFC 5737</seealso>
        public static readonly Ipv4Subnet Doc2 = Ipv4Address.Parse(0xC6336400) + Ipv4Cidr.Parse(24);
        
        /// <summary>
        /// IP Range 203.0.113.0/24, used for documentation; Allocated by RFC 5737.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc5737">RFC 5737</seealso>
        public static readonly Ipv4Subnet Doc3 = Ipv4Address.Parse(0xCB007100) + Ipv4Cidr.Parse(24);
        
        /// <summary>
        /// IP Range 169.254.0.0/16, used for link-local addresses[6] between two hosts on a single link
        /// when no IP address is otherwise specified, such as would have normally been retrieved from a DHCP server.
        /// Allocated by RFC 3927.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc3927">RFC 3927</seealso>
        public static readonly Ipv4Subnet Apipa = Ipv4Address.Parse(0xA9FE0000) + Ipv4Cidr.Parse(16);
        
        /// <summary>
        /// IP Range 127.0.0.0/8, used for loopback addresses to the host.
        /// Allocated by RFC 1122.
        /// </summary>
        /// <seealso href="https://datatracker.ietf.org/doc/html/rfc1122">RFC 1122</seealso>
        public static readonly Ipv4Subnet Loopback = Ipv4Address.Parse(0x7F000000) + Ipv4Cidr.Parse(8);
        
        /// <summary>
        /// IP range 0.0.0.0/8
        /// </summary>
        public static readonly Ipv4Subnet Current = Ipv4Address.Parse(0x00000000) + Ipv4Cidr.Parse(8);
        
        /// <summary>
        /// IP Address 255.255.255.255; The broadcast network
        /// </summary>
        public static readonly Ipv4Subnet Broadcast = Ipv4Address.Parse(0xFFFFFFFF) + Ipv4Cidr.Parse(32);
        
        /// <summary>
        /// IP Range 224.0.0.0/4; Multicast networks
        /// </summary>
        public static readonly Ipv4Subnet Multicast = Ipv4Address.Parse(0xE0000000) + Ipv4Cidr.Parse(4);
        
        /// <summary>
        /// IP Range 198.18.0.0/15, used for benchmark testing of inter-network communications between two separate subnets.
        /// </summary>
        public static readonly Ipv4Subnet Benchmark = Ipv4Address.Parse(0xC6120000) + Ipv4Cidr.Parse(15);
    }
}