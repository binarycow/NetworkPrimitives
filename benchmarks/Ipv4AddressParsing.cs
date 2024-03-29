﻿extern alias Lib_IPN2;
// ReSharper disable InconsistentNaming
#nullable enable
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Attributes;
using NetworkPrimitives.Ipv4;

using IPN2 = Lib_IPN2::System.Net.IPNetwork;

namespace NetworkPrimitives.Benchmarks;

public class Ipv4AddressParsing
{
    [Benchmark]
    public void NetworkPrimitives()
    {
        foreach (var address in TestData.RandomIpAddresses)
        {
            _ = Ipv4Address.Parse(address);
        }
    }
        
    [Benchmark]
    public void DotNet()
    {
        foreach (var address in TestData.RandomIpAddresses)
        {
            _ = System.Net.IPAddress.Parse(address);
        }
    }

    [Benchmark]
    public void IpNetwork2()
    {
        foreach (var address in TestData.RandomIpAddresses)
        {
            _ = IPN2.Parse(address);
        }
    }
}