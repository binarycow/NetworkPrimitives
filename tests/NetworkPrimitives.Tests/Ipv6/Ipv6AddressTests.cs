using System.Net;
using NetworkPrimitives.Ipv6;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv6
{
    [TestFixture]
    public class Ipv6AddressTests
    {
        [Test]
        [TestCase("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        public void TestParse(string input)
        {
            Assert.AreEqual(true, Ipv6Address.TryParse(input, out var address));
        }
        [Test]
        [TestCase("2001:db8:85a3:::8a2e:370:7334")]
        public void TestParseFailed(string input)
        {
            Assert.AreEqual(false, Ipv6Address.TryParse(input, out var address));
        }
        [Test]
        [TestCase("2001:db8:85a3::8a2e:370:7334")]
        public void TestToString(string input)
        {
            Ipv6Address.TryParse(input, out var address);
            var systemNetAddress = IPAddress.Parse(input);
            Assert.AreEqual(systemNetAddress.ToString(), address.ToString());
        }
    }
}