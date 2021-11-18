using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    [TestFixture]
    public class Ipv4SubnetTests
    {
        [Test]
        [TestCase("1.2.3.4/8", "1.0.0.0/8", "1.0.0.0 255.0.0.0", "1.0.0.0 0.255.255.255")]
        public void TestParse(string input, string cidr, string mask, string wildcard)
        {
            Assert.AreEqual(true, Ipv4Subnet.TryParse(input, out var subnet));
            Assert.AreEqual(cidr, subnet.ToString());
            Assert.AreEqual(cidr, subnet.ToString("C"));
            Assert.AreEqual(mask, subnet.ToString("M"));
            Assert.AreEqual(wildcard, subnet.ToString("W"));
        }
    }
}