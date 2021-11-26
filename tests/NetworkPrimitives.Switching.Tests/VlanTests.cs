using System;
using System.Collections.Generic;
using System.Linq;
using NetworkPrimitives.Switching;
using NUnit.Framework;

namespace NetworkPrimitives.Tests
{
    [TestFixture]
    public class VlanTests
    {
        public static IReadOnlyList<ushort> ValidVlans = Enumerable.Range(0, 4095).Select(x => (ushort)x).ToList();


        [Test]
        [TestCaseSource(nameof(VlanTests.ValidVlans))]
        public void TestParse(ushort vlan)
        {
            var vlanString = vlan.ToString();
            Assert.That(VlanNumber.TryParse(vlanString, out var vlanNumber));
            Assert.That(vlanNumber.Value, Is.EqualTo(vlan));
        }
        
        
        [Test]
        [TestCaseSource(nameof(VlanTests.ValidVlans))]
        public void TestToString(ushort vlan)
        {
            var vlanString = vlan.ToString();
            Assume.That(VlanNumber.TryParse(vlanString, out var vlanNumber));
            Assert.That(vlanNumber.ToString(), Is.EqualTo(vlanString));
        }
        
        

        [Test]
        [TestCase("")]
        [TestCase("4095")]
        [TestCase("4096")]
        public void TestInvalidParse(string input)
        {
            Assert.Throws<FormatException>(() => VlanNumber.Parse(input));
        }
    }
}