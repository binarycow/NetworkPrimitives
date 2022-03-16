extern alias Lib_IPN2;
// ReSharper disable InconsistentNaming
#nullable enable

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NetworkPrimitives.Ipv4;

using IPN2 = Lib_IPN2::System.Net.IPNetwork;

namespace NetworkPrimitives.Benchmarks;

public class Ipv4AddressRange
{
    [Benchmark]
    public void NetworkPrimitives()
    {
        foreach (var subnetString in TestData.RandomSubnets)
        {
            var subnet = Ipv4Subnet.Parse(subnetString);
            foreach (var address in subnet.GetAllAddresses())
            {
                _ = address;
            }
        }
    }

    [Benchmark]
    public void IpNetwork2()
    {
        foreach (var subnetString in TestData.RandomSubnets)
        {
            var subnet = IPN2.Parse(subnetString);
            foreach (var address in subnet.ListIPAddress())
            {
                _ = address;
            }
        }
    }
}