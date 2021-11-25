

using System;
using Demo;
using NetworkPrimitives.Ipv4;

const int iterations = 1_000_000;

for (var i = 0; i < iterations; ++i)
{
    foreach (var address in TestData.RandomIpAddresses)
    {
        _ = Ipv4Address.Parse(address);
    }
}