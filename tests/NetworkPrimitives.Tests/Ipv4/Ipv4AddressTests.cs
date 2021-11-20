#nullable enable

using System;
using System.Collections.Generic;
using System.Net;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
#if CICD
    [TestFixture]
#endif
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
// #else
        [Test]
        [TestCaseSource(nameof(Ipv4AddressTests.GetTestCases))]
// #endif
        public void TestSuccessfulParse(Ipv4TestCase testCase)
        {
            Assert.AreEqual(true, Ipv4Address.TryParse(testCase.IpString, out var address));
            Assert.AreEqual(testCase.Ip, address.Value);
            Assert.AreEqual(testCase.IpString, address.ToString());
        }
#endif
    }
}