#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    [TestFixture]
    public class Ipv4AddressTests
    {

        public static IReadOnlyList<Ipv4TestCase> TestCases { get; }
            = Ipv4TestCaseProvider.LoadTestCases("randomips.json");


        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.TestCases))]
        public void TestSuccessfulParse(Ipv4TestCase testCase)
        {
            Assert.That(IPAddress.TryParse(testCase.IpString, out var netIp));
            Assert.Multiple(() =>
            {
                Assert.That(Ipv4Address.TryParse(testCase.IpString, out var address));
                
                Assert.That(Ipv4Address.TryParse(netIp, out var address2));
                Assert.That(address2, Is.EqualTo(address));
                
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
                Assert.That(Ipv4Address.TryParse((ReadOnlySpan<char>)testCase.IpString, out address2));
                Assert.That(address2, Is.EqualTo(address));
#endif
                
                Assert.That(address.Value, Is.EqualTo(testCase.Ip));
                Assert.That(address.ToString(), Is.EqualTo(testCase.IpString));
            });
        }


        [Test]
        [TestCase("2001:db8:85a3::8a2e:370:7334")]
        public void TestFailedSystemNetParse(string input)
        {
            Assume.That(IPAddress.TryParse(input, out var netAddress));
            
            Assert.Multiple(() =>
            {
                Assert.Throws<ArgumentException>(() => _ = Ipv4Address.Parse(netAddress));
                Assert.That(Ipv4Address.TryParse(netAddress, out _), Is.False);
            });
        }
        
        
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.TestCases))]
        public void TestEquality(Ipv4TestCase testCase)
        {
            Assert.That(Ipv4Address.TryParse(testCase.IpString, out var address));
            Assert.That(Ipv4Address.TryParse(testCase.FirstUsable, out var _));
            
            Assert.AreEqual(testCase.Ip.GetHashCode(), address.GetHashCode());
        }


        [Test]
        [TestCase("38.221.101.187")]
        public void TestOctets2(string ipString)
        {
            Assert.That(Ipv4Address.TryParse(ipString, out var address));
            Assert.Multiple(() =>
            {
                var expectedBytes = GetExpectedBytes(ipString);
                Span<byte> span = stackalloc byte[4];
                var actualOctetArray = address.GetBytes();
                Assert.IsTrue(address.TryWriteBytes(span, out var bytesWritten));
                Assert.AreEqual(4, bytesWritten);
                Assert.IsTrue(span.EqualToArray(actualOctetArray));
            
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
            });


            static byte[] GetExpectedBytes(string text) 
                => text.Split('.').Select(byte.Parse).ToArray();
        }
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.TestCases))]
        public void TestOctets(Ipv4TestCase testCase)
        {
            Assert.That(Ipv4Address.TryParse(testCase.IpString, out var address));
            Assert.Multiple(() =>
            {
                var expectedBytes = GetExpectedBytes(testCase.IpString);
                Span<byte> span = stackalloc byte[4];
                var actualOctetArray = address.GetBytes();
                Assert.IsTrue(address.TryWriteBytes(span, out var bytesWritten));
                Assert.AreEqual(4, bytesWritten);
                Assert.IsTrue(span.EqualToArray(actualOctetArray));
            
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
            });


            static byte[] GetExpectedBytes(string text) 
                => text.Split('.').Select(byte.Parse).ToArray();
        }
    }
}