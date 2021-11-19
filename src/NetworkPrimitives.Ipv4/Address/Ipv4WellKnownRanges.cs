#nullable enable

using System;

namespace NetworkPrimitives.Ipv4
{
    public static class Ipv4WellKnownRanges
    {
        public static readonly Ipv4Subnet Rfc1918A = Ipv4Address.Parse(0x0A000000) + Ipv4Cidr.Parse(8);
        public static readonly Ipv4Subnet Rfc1918B = Ipv4Address.Parse(0xAC100000) + Ipv4Cidr.Parse(12);
        public static readonly Ipv4Subnet Rfc1918C = Ipv4Address.Parse(0xC0A80000) + Ipv4Cidr.Parse(16);
        public static readonly Ipv4Subnet CgNat = Ipv4Address.Parse(0x64400000) + Ipv4Cidr.Parse(10);
        public static readonly Ipv4Subnet Doc1 = Ipv4Address.Parse(0xC0000200) + Ipv4Cidr.Parse(24);
        public static readonly Ipv4Subnet Doc2 = Ipv4Address.Parse(0xC6336400) + Ipv4Cidr.Parse(24);
        public static readonly Ipv4Subnet Doc3 = Ipv4Address.Parse(0xCB007100) + Ipv4Cidr.Parse(24);
        public static readonly Ipv4Subnet Apipa = Ipv4Address.Parse(0xA9FE0000) + Ipv4Cidr.Parse(16);
        public static readonly Ipv4Subnet Loopback = Ipv4Address.Parse(0x7F000000) + Ipv4Cidr.Parse(8);
        public static readonly Ipv4Subnet Current = Ipv4Address.Parse(0x00000000) + Ipv4Cidr.Parse(8);
        public static readonly Ipv4Subnet Broadcast = Ipv4Address.Parse(0xFFFFFFFF) + Ipv4Cidr.Parse(32);
        public static readonly Ipv4Subnet Multicast = Ipv4Address.Parse(0xE0000000) + Ipv4Cidr.Parse(4);
        public static readonly Ipv4Subnet Benchmark = Ipv4Address.Parse(0xC6120000) + Ipv4Cidr.Parse(15);
    }
}