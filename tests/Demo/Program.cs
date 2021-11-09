using System;
using NetworkPrimitives.Ipv4;

namespace Demo
{
    internal static class Program
    {
        private static void Main()
        {
            ParseIpv4Subnet();
            ParseIpv4Address();
            ParseIpv4Cidr();
            ParseIpv4SubnetMask();
            CreateIpv4Subnets();
            InSubnet();
            ParseRange();
            ParseRangeList();
        }

        public static void ParseRangeList()
        {
            var text = @"
10.0.0.2-5
10.100.0.100/29
";
            var ranges = Ipv4AddressRangeList.Parse(text);
            var allAddresses = ranges.GetAllAddresses();
            Console.WriteLine($"  Range Count: {ranges.Count}");
            Console.WriteLine($"Address Count: {allAddresses.Length}");
            foreach(var ip in allAddresses)
                Console.WriteLine($"    {ip}");
        }
        
        public static void ParseRange()
        {
            var range = Ipv4AddressRange.Parse("10.0.0.2-15");
            Console.WriteLine($"Range: {range}");
            Console.WriteLine($"Length: {range.Length}");
            foreach (var address in range)
            {
                Console.WriteLine($"  {address}");
            }
            Console.WriteLine();
            range = range.Slice(start: 3, length: 3);
            Console.WriteLine($"Length: {range.Length}");
            foreach (var address in range)
            {
                Console.WriteLine($"  {address}");
            }
        }
        
        public static void InSubnet()
        {
            var networkAddress = Ipv4Address.Parse("10.0.0.0");
            var mask = Ipv4SubnetMask.Parse("255.255.255.192");
            var address = Ipv4Address.Parse("10.0.0.47");
            var subnet = networkAddress + mask;
            Console.WriteLine($"In Subnet: {address.IsInSubnet(subnet)}");
            Console.WriteLine($" Contains: {subnet.Contains(address)}");
        }

        public static void CreateIpv4Subnets()
        {
            var address = Ipv4Address.Parse("10.0.0.0");
            var mask = Ipv4SubnetMask.Parse("255.255.255.192");
            var cidr1 = Ipv4Cidr.Parse("30");
            var cidr2 = Ipv4Cidr.Parse("28");
            var subnet = address + mask;
            Console.WriteLine(subnet);
            subnet = address + cidr1;
            Console.WriteLine(subnet);
            subnet = address / cidr2;
            Console.WriteLine(subnet);
        }
        
        public static void ParseIpv4SubnetMask()
        {
            var text = "255.255.255.224";
            var mask = Ipv4SubnetMask.Parse(text);
            Console.WriteLine($"        Mask: {mask}");
            Console.WriteLine($" Total Hosts: {mask.TotalHosts}");
            Console.WriteLine($"Usable Hosts: {mask.UsableHosts}");
        }

        public static void ParseIpv4Cidr()
        {
            var text = "29";
            var cidr = Ipv4Cidr.Parse(text);
            Console.WriteLine($"        Cidr: {cidr}");
            Console.WriteLine($" Total Hosts: {cidr.TotalHosts}");
            Console.WriteLine($"Usable Hosts: {cidr.UsableHosts}");
        }
        
        public static void ParseIpv4Address()
        {
            var text = "10.0.0.50";
            var address = Ipv4Address.Parse(text);
            Console.WriteLine($"Address: {address}");
            Console.WriteLine($"  Class: {address.GetAddressClass()}");
            Console.WriteLine($"   Type: {address.GetRangeType()}");
            Console.WriteLine($" Octet1: {address.GetOctet(0)}");
            Console.WriteLine($" Octet2: {address.GetOctet(1)}");
            Console.WriteLine($" Octet3: {address.GetOctet(2)}");
            Console.WriteLine($" Octet4: {address.GetOctet(3)}");
        }

        public static void ParseIpv4Subnet()
        {
            var text = "10.0.0.0/29";
            var subnet = Ipv4Subnet.Parse(text);
            Console.WriteLine($"      Total Hosts: {subnet.TotalHosts}");
            Console.WriteLine($"     Usable Hosts: {subnet.UsableHosts}");
            Console.WriteLine($"  Network Address: {subnet.NetworkAddress}");
            Console.WriteLine($"     First Usable: {subnet.FirstUsable}");
            Console.WriteLine($"      Last Usable: {subnet.LastUsable}");
            Console.WriteLine($"Broadcast Address: {subnet.BroadcastAddress}");

            Console.WriteLine($"         Format as CIDR: {subnet:C}");
            Console.WriteLine($"  Format as Subnet Mask: {subnet:M}");
            Console.WriteLine($"Format as Wildcard Mask: {subnet:W}");
            
            foreach (var address in subnet.GetUsableAddresses())
            {
                Console.WriteLine($"   {address}");
            }
        }
    }
}