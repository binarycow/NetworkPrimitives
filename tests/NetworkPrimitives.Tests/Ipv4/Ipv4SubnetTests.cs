using System.Collections;
using System.Collections.Generic;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    [TestFixture]
    public class Ipv4SubnetTests
    {
        public static IReadOnlyList<Ipv4TestCase> GetTestCases() 
            => Ipv4TestCaseProvider.LoadTestCases("randomips.json");
        
        [Test]
        [TestCaseSource(nameof(Ipv4SubnetTests.GetTestCases))]
        public void TestParse(Ipv4TestCase testCase)
        {
            Assume.That(Ipv4Subnet.TryParse(testCase.SubnetInput, out var subnet), Is.True);
            
            Assert.Multiple(() =>
            {
                Assert.That(subnet.TotalHosts, Is.EqualTo(testCase.TotalHosts));
                Assert.That(subnet.UsableHosts, Is.EqualTo(testCase.UsableHosts));
                Assert.That(subnet.NetworkAddress.Value, Is.EqualTo(testCase.Network));
                Assert.That(subnet.BroadcastAddress.Value, Is.EqualTo(testCase.Broadcast));
                Assert.That(subnet.FirstUsable.Value, Is.EqualTo(testCase.FirstUsable));
                Assert.That(subnet.LastUsable.Value, Is.EqualTo(testCase.LastUsable));
                Assert.That(subnet.ToString(), Is.EqualTo(testCase.SubnetExpected));
            });
        }


        [Test]
        [TestCase("10.0.0.0/24", "10.0.0.0/25", "10.0.0.128/25")]
        public void TestTrySplit(string subnetString, string lowString, string highString)
        {
            Assume.That(Ipv4Subnet.TryParse(subnetString, out var subnet), Is.True);
            
            Assert.Multiple(() =>
            {
                Assert.That(subnet.TrySplit(out var low, out var high), Is.True);
                Assert.That(low.ToString(), Is.EqualTo(lowString));
                Assert.That(high.ToString(), Is.EqualTo(highString));
            });
        }
        
        
        [Test]
        [TestCase("10.0.0.0/25", "10.0.0.128/25", "10.0.0.0/24")]
        [TestCase("255.255.255.255/32", "0.0.0.0/32", "0.0.0.0/0")]
        [TestCase("10.10.10.10/0", "10.10.10.10/32", "0.0.0.0/0")]
        [TestCase("10.0.0.0/24", "10.0.0.50/32", "10.0.0.0/24")]
        public void TestGetContainingSupernet(string aString, string bString, string expectedString)
        {
            Assume.That(Ipv4Subnet.TryParse(aString, out var a), Is.True);
            Assume.That(Ipv4Subnet.TryParse(bString, out var b), Is.True);
            
            Assert.Multiple(() =>
            {
                var supernet = Ipv4Subnet.GetContainingSupernet(a, b);
                Assert.That(supernet.ToString(), Is.EqualTo(expectedString));
                supernet = Ipv4Subnet.GetContainingSupernet(b, a);
                Assert.That(supernet.ToString(), Is.EqualTo(expectedString));
            });
        }
        
        [Test]
        [TestCase("10.0.0.0/24", "10.0.1.0/25", "10.0.2.0/25", "10.0.0.0/22")]
        public void TestGetContainingSupernet(string aString, string bString, string cString, string expectedString)
        {
            Assume.That(Ipv4Subnet.TryParse(aString, out var a), Is.True);
            Assume.That(Ipv4Subnet.TryParse(bString, out var b), Is.True);
            Assume.That(Ipv4Subnet.TryParse(cString, out var c), Is.True);
            
            Assert.Multiple(() =>
            {
                IReadOnlyList<Ipv4Subnet> list = new[] { a, b, c };
                IEnumerable<Ipv4Subnet> enumerable = list;
                var supernet = Ipv4Subnet.GetContainingSupernet(list);
                Assert.That(supernet.ToString(), Is.EqualTo(expectedString));
                var supernet2 = Ipv4Subnet.GetContainingSupernet(enumerable);
                Assert.That(supernet2, Is.EqualTo(supernet));
            });
        }
        
        
        
        [Test]
        [TestCase("10.200.40.128/25", "10.200.40.128", "255.255.255.128")]
        public void TestDeconstruct(string subnetString, string networkString, string maskString)
        {
            Assume.That(Ipv4Subnet.TryParse(subnetString, out var subnet));
            Assert.Multiple(() =>
            {
                var (network, mask) = subnet;
                Assert.That(network.ToString(), Is.EqualTo(networkString));
                Assert.That(mask.ToString(), Is.EqualTo(maskString));
            });
        }
        

        [Test]
        [TestCase("10.0.0.0/24", "10.0.0.0", "10.0.0.255")]
        [TestCase("10.0.0.0/30", "10.0.0.0", "10.0.0.3")]
        [TestCase("10.0.0.0/31", "10.0.0.0", "10.0.0.1")]
        [TestCase("10.0.0.0/32", "10.0.0.0", "10.0.0.0")]
        public void TestGetAllAddresses(string subnetString, string startAddressString, string lastAddressString)
        {
            Assume.That(Ipv4Subnet.TryParse(subnetString, out var subnet));
            Assume.That(Ipv4Address.TryParse(startAddressString, out var startAddress));
            Assume.That(Ipv4Address.TryParse(lastAddressString, out var lastAddress));
            
            var range = subnet.GetAllAddresses();
            Assert.Multiple(() =>
            {
                this.TestRangeRefStructEnumerator(range.GetEnumerator(), startAddress, lastAddress);
                this.TestRangeClassEnumerator(range.ToEnumerable().GetEnumerator(), startAddress, lastAddress);
            });
        }


        [Test]
        [TestCase("10.0.0.0/24", "10.0.0.1", "10.0.0.254")]
        [TestCase("10.0.0.0/30", "10.0.0.1", "10.0.0.2")]
        [TestCase("10.0.0.0/31", "10.0.0.0", "10.0.0.1")]
        [TestCase("10.0.0.0/32", "10.0.0.0", "10.0.0.0")]
        public void TestGetUsableAddresses(string subnetString, string startAddressString, string lastAddressString)
        {
            Assume.That(Ipv4Subnet.TryParse(subnetString, out var subnet));
            Assume.That(Ipv4Address.TryParse(startAddressString, out var startAddress));
            Assume.That(Ipv4Address.TryParse(lastAddressString, out var lastAddress));
            
            var range = subnet.GetUsableAddresses();
            Assert.Multiple(() =>
            {
                this.TestRangeRefStructEnumerator(range.GetEnumerator(), startAddress, lastAddress);
                this.TestRangeClassEnumerator(range.ToEnumerable().GetEnumerator(), startAddress, lastAddress);
            });
        }
        

        private void TestRangeRefStructEnumerator(Ipv4AddressEnumerator range, Ipv4Address start, Ipv4Address end)
        {
            ulong expected = start.Value;
            Assert.That(range.MoveNext());
            do
            {
                Assert.That(range.Current, Is.EqualTo(Ipv4Address.Parse((uint)expected)));
                ++expected;
            } while (range.MoveNext());
            --expected;
            Assert.That(Ipv4Address.Parse((uint)expected), Is.EqualTo(end));
        }
        private void TestRangeClassEnumerator(IEnumerator<Ipv4Address> range, Ipv4Address start, Ipv4Address end)
        {
            ulong expected = start.Value;
            Assert.That(range.MoveNext());
            do
            {
                Assert.That(range.Current, Is.EqualTo(Ipv4Address.Parse((uint)expected)));
                ++expected;
            } while (range.MoveNext());
            --expected;
            Assert.That(Ipv4Address.Parse((uint)expected), Is.EqualTo(end));
        }
    }
}