#nullable enable

using System;
using System.Collections.Generic;
using NetworkPrimitives.Switching;
using NUnit.Framework;

namespace NetworkPrimitives.Tests
{
    [TestFixture]
    public class MacAddressTests
    {
        public record MacFormatProviderTestCase(
            MacAddressFormat FormatProvider,
            string Expected
        );
        
        public static IReadOnlyList<MacFormatProviderTestCase> MacFormatProviderTestCases = new MacFormatProviderTestCase[]
        {
            new (MacAddressFormats.Cisco, "feed.dead.beef"),
        };
        
        [Test]
        [TestCase((ushort)0xFEED, (ushort)0xDEAD, (ushort)0xBEEF, "feed.dead.beef")]
        public void TestToString(ushort high, ushort mid, ushort low, string expected)
        {
            var macAddress = new MacAddress(high, mid, low);
            Assert.That(macAddress.ToString(), Is.EqualTo(expected));
        }
        
        
        [Test]
        [TestCase(null, "feed.dead.beef")]
        [TestCase("", "feed.dead.beef")]
        [TestCase("G", "feed.dead.beef")]
        [TestCase("4.", "feed.dead.beef")]
        [TestCase("2.", "fe.ed.de.ad.be.ef")]
        [TestCase("2-", "fe-ed-de-ad-be-ef")]
        [TestCase("4-", "feed-dead-beef")]
        [TestCase("4:", "feed:dead:beef")]
        [TestCase("4:X", "FEED:DEAD:BEEF")]
        [TestCase("4:x", "feed:dead:beef")]
        [TestCase("6:X", "FEEDDE:ADBEEF")]
        [TestCase("6:x", "feedde:adbeef")]
        [TestCase("12X", "FEEDDEADBEEF")]
        [TestCase("12x", "feeddeadbeef")]
        public void TestFormatToString(string? format, string expected)
        {
            var macAddress = new MacAddress(0xFEED, 0xDEAD, 0xBEEF);
            Assert.That(macAddress.ToString(format), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("6")]
        public void TestFormatExceptions(string? format)
        {
            var macAddress = new MacAddress(0xFEED, 0xDEAD, 0xBEEF);
            Assert.Throws<FormatException>(() => _ = macAddress.ToString(format));
        }


        [Test]
        [TestCaseSource(nameof(MacAddressTests.MacFormatProviderTestCases))]
        public void TestTypedFormatters(MacFormatProviderTestCase testCase)
        {
            var (formatProvider, expected) = testCase;
            var macAddress = new MacAddress(0xFEED, 0xDEAD, 0xBEEF);
            Assert.That(macAddress.ToString(formatProvider, null), Is.EqualTo(expected));
            Assert.That(macAddress.ToString(formatProvider, "G"), Is.EqualTo(expected));
            Assert.That(macAddress.ToString(formatProvider, string.Empty), Is.EqualTo(expected));
        }
    }
}