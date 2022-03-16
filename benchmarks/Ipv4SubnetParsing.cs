extern alias Lib_IPN2;
// ReSharper disable InconsistentNaming
#nullable enable

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NetworkPrimitives.Ipv4;

using IPN2 = Lib_IPN2::System.Net.IPNetwork;

namespace NetworkPrimitives.Benchmarks;

public class Ipv4SubnetParsing
{
    
    [Benchmark]
    public void NetworkPrimitives()
    {
        foreach (var address in TestData.RandomSubnets)
        {
            _ = Ipv4Subnet.Parse(address);
        }
    }

    [Benchmark]
    public void IpNetwork2()
    {
        foreach (var address in TestData.RandomSubnets)
        {
            _ = IPN2.Parse(address);
        }
    }
}