#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    [TestFixture]
    public class Ipv4AddressTests
    {

        public static IReadOnlyList<Ipv4TestCase> GetTestCases() 
            => Ipv4TestCaseProvider.LoadTestCases("randomips.json");

#if CICD
        [Test]
        public void TestSuccessfulParse()
        {
            foreach (var testCase in Ipv4AddressTests.GetTestCases())
                TestSuccessfulParse(testCase);
        }
#else
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.GetTestCases))]
#endif
        public void TestSuccessfulParse(Ipv4TestCase testCase)
        {
            Assert.AreEqual(true, Ipv4Address.TryParse(testCase.IpString, out var address));
            Assert.AreEqual(testCase.Ip, address.Value);
            Assert.AreEqual(testCase.IpString, address.ToString());
        }
        
        
#if CICD
        [Test]
        public void TestEquality()
        {
            foreach (var testCase in Ipv4AddressTests.GetTestCases())
                TestEquality(testCase);
        }
#else
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.GetTestCases))]
#endif
        public void TestEquality(Ipv4TestCase testCase)
        {
            var address = Ipv4Address.Parse(testCase.IpString);
            var other = Ipv4Address.Parse(testCase.FirstUsable);
            Assert.AreEqual(testCase.Ip.GetHashCode(), address.GetHashCode());
            Assert.AreEqual(testCase.Cidr is 31 or 32, other == address);
            Assert.AreEqual(testCase.Cidr is not (31 or 32), other != address);
        }

        
#if CICD
        [Test]
        public void TestOctets()
        {
            foreach (var testCase in Ipv4AddressTests.GetTestCases())
                TestOctets(testCase);
        }
#else
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.GetTestCases))]
#endif
        public void TestOctets(Ipv4TestCase testCase)
        {
            var address = Ipv4Address.Parse(testCase.IpString);
            var expectedBytes = GetExpectedBytes(testCase.IpString);
            Span<byte> span = stackalloc byte[4];
            Assert.IsTrue(address.TryWriteBytes(span, out var bytesWritten));
            Assert.AreEqual(4, bytesWritten);
            
            for (var i = -5; i < 10; ++i)
            {
                if (i is >= 0 and <= 3)
                {
                    Assert.AreEqual(expectedBytes[i], span[i]);
                    Assert.AreEqual(expectedBytes[i], address[i]);
                    Assert.AreEqual(expectedBytes[i], address.GetOctet(i));
                }
                else
                {
                    Assert.Throws<ArgumentOutOfRangeException>(() => _ = address[i]);
                    Assert.Throws<ArgumentOutOfRangeException>(() => _ = address.GetOctet(i));
                }
            }


            static byte[] GetExpectedBytes(string text) 
                => text.Split('.').Select(byte.Parse).ToArray();
        }
    }
}