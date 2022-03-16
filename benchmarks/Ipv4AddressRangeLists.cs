using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NetworkPrimitives.Ipv4;

namespace NetworkPrimitives.Benchmarks;

public class Ipv4AddressRangeLists
{
    [Benchmark]
    public void NetworkPrimitives()
    {
        foreach (var rangeListString in TestData.RangeLists)
        {
            var rangeList = Ipv4AddressRangeList.Parse(rangeListString);
            foreach (var range in rangeList)
            {
                foreach (var address in range)
                {
                    _ = address;
                }
            }
        }
    }
}