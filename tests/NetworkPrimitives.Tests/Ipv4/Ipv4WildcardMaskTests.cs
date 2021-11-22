#nullable enable

using System;
using System.Collections.Generic;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    
    
    [TestFixture]
    public class Ipv4WildcardMaskTests
    {

        
        
        [Test]
        [TestCase("0.0.0.255")]
        public void TestParse(string input)
        {
            Assert.Multiple(() =>
            {
                Assert.That(Ipv4WildcardMask.TryParse(input, out var mask));
                Assert.That(mask.ToString(), Is.EqualTo(input));
            });
        }

        [Test]
        [TestCase("0.0.0.255", (ulong)256)]
        [TestCase("0.0.0.235", (ulong)64)]
        [TestCase("0.0.0.15", (ulong)16)]
        public void TestCounts(string input, ulong hostCount)
        {
            Assume.That(Ipv4WildcardMask.TryParse(input, out var mask));

            Assert.That(mask.HostCount, Is.EqualTo(hostCount));
        }
        
        
    }
}