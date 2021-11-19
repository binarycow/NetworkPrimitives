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
            Assert.AreEqual(true, Ipv4Subnet.TryParse(testCase.SubnetInput, out var subnet));
            Assert.AreEqual(testCase.TotalHosts, subnet.TotalHosts);
            Assert.AreEqual(testCase.UsableHosts, subnet.UsableHosts);
            Assert.AreEqual(testCase.Network, subnet.NetworkAddress.Value);
            Assert.AreEqual(testCase.Broadcast, subnet.BroadcastAddress.Value);
            Assert.AreEqual(testCase.FirstUsable, subnet.FirstUsable.Value);
            Assert.AreEqual(testCase.LastUsable, subnet.LastUsable.Value);
            Assert.AreEqual(testCase.SubnetExpected, subnet.ToString());
        }
    }
}