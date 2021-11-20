using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    [TestFixture]
    public class Ipv4SubnetMaskTests
    {
        [Test]
        [TestCase("0.0.0.0")]
        [TestCase("128.0.0.0")]
        [TestCase("192.0.0.0")]
        [TestCase("224.0.0.0")]
        [TestCase("240.0.0.0")]
        [TestCase("248.0.0.0")]
        [TestCase("252.0.0.0")]
        [TestCase("254.0.0.0")]
        [TestCase("255.0.0.0")]
        [TestCase("255.128.0.0")]
        [TestCase("255.192.0.0")]
        [TestCase("255.224.0.0")]
        [TestCase("255.240.0.0")]
        [TestCase("255.248.0.0")]
        [TestCase("255.252.0.0")]
        [TestCase("255.254.0.0")]
        [TestCase("255.255.0.0")]
        [TestCase("255.255.128.0")]
        [TestCase("255.255.192.0")]
        [TestCase("255.255.224.0")]
        [TestCase("255.255.240.0")]
        [TestCase("255.255.248.0")]
        [TestCase("255.255.252.0")]
        [TestCase("255.255.254.0")]
        [TestCase("255.255.255.0")]
        [TestCase("255.255.255.128")]
        [TestCase("255.255.255.192")]
        [TestCase("255.255.255.224")]
        [TestCase("255.255.255.240")]
        [TestCase("255.255.255.248")]
        [TestCase("255.255.255.252")]
        [TestCase("255.255.255.254")]
        [TestCase("255.255.255.255")]
        public void TestParse(string input)
        {
            Assert.AreEqual(true, Ipv4SubnetMask.TryParse(input, out var mask));
            Assert.AreEqual(input, mask.ToString());
        }
    }
}