#nullable enable

using System;
using System.Net;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests
{
    [TestFixture]
    public class Ipv4AddressTests
    {
        [Test]
        [TestCase("244.214.215.163", true)]
        [TestCase("84.191.228.40", true)]
        [TestCase("200.97.83.142", true)]
        [TestCase("209.120.106.210", true)]
        [TestCase("154.104.32.33", true)]
        [TestCase("189.143.127.145", true)]
        [TestCase("198.196.67.154", true)]
        [TestCase("10.21.52.235", true)]
        [TestCase("234.165.69.77", true)]
        [TestCase("17.174.198.38", true)]
        [TestCase("111.38.88.75", true)]
        [TestCase("36.26.215.6", true)]
        [TestCase("153.215.18.167", true)]
        [TestCase("120.117.45.112", true)]
        [TestCase("191.161.189.19", true)]
        [TestCase("16.42.88.57", true)]
        [TestCase("122.216.252.17", true)]
        [TestCase("79.43.175.125", true)]
        [TestCase("79.72.122.87", true)]
        [TestCase("248.63.20.23", true)]
        [TestCase("225.42.42.6", true)]
        [TestCase("134.46.117.32", true)]
        [TestCase("72.160.179.55", true)]
        [TestCase("12.77.241.248", true)]
        [TestCase("152.203.98.79", true)]
        [TestCase("130.37.28.130", true)]
        [TestCase("204.177.125.37", true)]
        [TestCase("135.217.252.52", true)]
        [TestCase("101.133.135.77", true)]
        [TestCase("110.211.130.187", true)]
        [TestCase("239.192.104.125", true)]
        [TestCase("197.54.111.18", true)]
        [TestCase("248.190.112.173", true)]
        [TestCase("198.175.170.240", true)]
        [TestCase("180.223.130.136", true)]
        [TestCase("252.25.206.232", true)]
        [TestCase("84.35.86.17", true)]
        [TestCase("193.223.102.94", true)]
        [TestCase("139.254.81.101", true)]
        [TestCase("171.57.135.68", true)]
        public void TestTryParse(string input, bool valid)
        {
            var success = Ipv4Address.TryParse(input, out var charsRead, out var address);
            Assert.AreEqual(valid, success);
            if (valid == false) return;
#pragma warning disable 618
            var expectedValue = (uint)IPAddress.Parse(input).Address;
#pragma warning restore 618

            
            Assert.AreEqual(input.Length, charsRead);
            Assert.AreEqual(expectedValue, address.Value.SwapEndianIfLittleEndian());
        }
        
        
        
    }
}