#nullable enable

using System;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4.Cidr
{
    public class ValueTests : Ipv4CidrTests
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestValues(Ipv4CidrTestCase testCase)
        {
            var (value, totalHosts, usableHosts, mask) = testCase;
            Assume.That(Ipv4Cidr.TryParse(value.ToString(), out var cidr), Is.True);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(value, (byte)cidr);
                Assert.AreEqual(totalHosts, cidr.TotalHosts);
                Assert.AreEqual(usableHosts, cidr.UsableHosts);
                Assert.AreEqual(value.ToString(), cidr.ToString());
                Assert.AreEqual(mask, cidr.ToSubnetMask().Value);
            });
        }



        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestWildcardMasks(Ipv4CidrTestCase testCase)
        {
            var (value, _, _, maskValue) = testCase;
            Assume.That(Ipv4Cidr.TryParse(value.ToString(), out var cidr));
            Assume.That(Ipv4SubnetMask.TryParse(maskValue, out var mask));
            Assert.Multiple(() =>
            {
                Assert.That(cidr.ToWildcardMask().Value, Is.EqualTo(~maskValue));
                Assert.That(cidr.Equals(mask));
                Assert.That(cidr.CompareTo(value), Is.EqualTo(0));
            });
        }
    }
}