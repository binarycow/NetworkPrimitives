using System;
using System.Linq;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace NetworkPrimitives.Benchmarks
{
    internal static class Program
    {
        private static void Main()
        {
            _ = BenchmarkSwitcher.FromTypes(new[]{typeof(Ipv4AddressParsing)})?.RunAll()
                ?? Enumerable.Empty<Summary>();
        }
    }
}