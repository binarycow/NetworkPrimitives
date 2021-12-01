#nullable enable

using System;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4.Cidr
{
    public class ParsingTests : Ipv4CidrTests
    {
        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestParsing(Ipv4CidrTestCase testCase)
        {
            Assert.That(Ipv4Cidr.TryParse(testCase.Cidr.ToString(), out _), Is.True);
        }
        
        
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("foo")]
        [TestCase("12foo")]
        public void TestInvalidStrings(string? input)
        {
            Assert.Multiple(() =>
            {
                Assert.Throws<FormatException>(() => Ipv4Cidr.Parse(input));
                Assert.IsFalse(Ipv4Cidr.TryParse(input, out _));
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> span = input;
                    Ipv4Cidr.Parse(span);
                });
                ReadOnlySpan<char> span = input;
                Assert.IsFalse(Ipv4Cidr.TryParse(span, out _));
#endif
            });
        }

        
        [Test]
        [TestCaseSource(nameof(InvalidCidrs))]
        public void TestInvalidBytes(byte cidr)
        {
            Assert.Multiple(() =>
            {
                Assert.False(Ipv4Cidr.TryParse(cidr, out _));
                Assert.False(Ipv4Cidr.TryParse(cidr.ToString(), out _));
                Assert.Throws<ArgumentOutOfRangeException>(() => Ipv4Cidr.Parse(cidr));
                Assert.Throws<FormatException>(() => Ipv4Cidr.Parse(cidr.ToString()));
            
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                Assert.Throws<FormatException>(() =>
                {
                    ReadOnlySpan<char> span = cidr.ToString();
                    Ipv4Cidr.Parse(span);
                });
                Assert.False(Ipv4Cidr.TryParse(cidr.ToString().AsSpan(), out _));
#endif
            });
        }
        
    }
}