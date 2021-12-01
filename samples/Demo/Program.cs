


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using NetworkPrimitives.Ipv4;
using NetworkPrimitives.JsonConverters;


namespace Demo
{
    internal static class Program
    {
        private static void Main()
        {
            DemoSubnetInfo();
            DemoJsonConverters();
            DemoSubnetOperations();
            DemoRangeLists();
        }

        private static void DemoRangeLists()
        {
            var rangeText = @"
10.0.0.0-10
10.0.1.0/29
10.1.0.40
";
            var rangeList = Ipv4AddressRangeList.Parse(rangeText);
            
            foreach (var range in rangeList)
            {
                Console.WriteLine(range);
                foreach (var address in range)
                {
                    Console.WriteLine(address);
                }
            }
            
            foreach (var address in rangeList.GetAllAddresses())
            {
                Console.WriteLine(address);
            }
        }

        private static void DemoSubnetOperations()
        {
            var slash24 = Ipv4Subnet.Parse("10.0.0.0/24");
            Console.WriteLine($"Splitting subnet {slash24}");   // 10.0.0.0/24
            var success = slash24.TrySplit(out var lowHalf, out var highHalf);
            Console.WriteLine($"Success: {success}");           // true
            Console.WriteLine($"Low subnet: {lowHalf}");        // 10.0.0.0/25
            Console.WriteLine($"High subnet: {highHalf}");      // 10.0.0.128/25

            var supernet = Ipv4Subnet.GetContainingSupernet(lowHalf, highHalf);
            Console.WriteLine($"Supernet: {supernet}");         // 10.0.0.0/24
        }

        private static void DemoSubnetInfo()
        {
            var subnet = Ipv4Subnet.Parse("10.0.0.0/29");
            Console.WriteLine($"  Network Address: {subnet.NetworkAddress}"); // 10.0.0.0
            Console.WriteLine($"     First Usable: {subnet.FirstUsable}"); // 10.0.0.1
            Console.WriteLine($"      Last Usable: {subnet.LastUsable}"); // 10.0.0.6
            Console.WriteLine($"Broadcast Address: {subnet.BroadcastAddress}"); // 10.0.0.7
            Console.WriteLine($"      Total Hosts: {subnet.TotalHosts}"); // 8
            Console.WriteLine($"     Usable Hosts: {subnet.UsableHosts}"); // 6
            foreach (var address in subnet.GetUsableAddresses())
            {
                Console.WriteLine(address);
            }
        }

        private static void DemoJsonConverters()
        {
            var json = @" 
[
    '10.0.0.0/24',
    '10.0.1.0/24',
    '10.1.0.0 255.255.255.0',
    '10.2.0.5'
]".Replace("'", "\"");
            var jsonSerializerOptions = new JsonSerializerOptions()
                .AddNetworkPrimitivesConverters();
            var subnets = JsonSerializer.Deserialize<Ipv4Subnet[]>(json, jsonSerializerOptions);
            foreach (var subnet in subnets)
            {
                Console.WriteLine(subnet);
            }
        }
    }
}