extern alias Lib_IPN2;
// ReSharper disable InconsistentNaming
#nullable enable

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NetworkPrimitives.Ipv4;

using IPN2 = Lib_IPN2::System.Net.IPNetwork;

namespace NetworkPrimitives.Benchmarks
{
    [MemoryDiagnoser]
    [MarkdownExporter]
    [CsvExporter]
    [CsvMeasurementsExporter]
    [HtmlExporter]
    [MarkdownExporterAttribute.GitHub]
    [SimpleJob(RuntimeMoniker.Net50, launchCount: 3, warmupCount: 10, targetCount: 30)]
    public class Ipv4AddressRange
    {
        [Benchmark]
        public void NetworkPrimitives()
        {
            _ = Ipv4Subnet.TryParse("10.0.0.0/24", out var subnet);
            foreach (var address in subnet.GetAllAddresses())
            {

            }
        }

        [Benchmark]
        public void IpNetwork2()
        {
            var subnet = IPN2.Parse("10.0.0.0/24");
            foreach (var address in subnet.ListIPAddress())
            {
                
            }
        }
    }
}