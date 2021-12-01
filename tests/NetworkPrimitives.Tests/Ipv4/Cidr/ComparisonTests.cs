#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4.Cidr
{
    public class ComparisonTests : Ipv4CidrTests
    {

        [Test]
        public void TestNullEquality()
        {
            Assert.That(Ipv4Cidr.TryParse(24, out var cidr));
            Assert.That(cidr.CompareTo(null), Is.EqualTo(1));
            Assert.That(cidr.Equals(null), Is.EqualTo(false));
        }

        [Test]
        public void TestInvalidEquals()
        {
            object foo = "Foo";
            Assert.That(Ipv4Cidr.TryParse(24, out var cidr));
            Assert.Throws<ArgumentException>(() => _ = cidr.CompareTo(foo));
            Assert.That(_ = cidr.Equals(foo), Is.EqualTo(false));
        }
        
        [Test]
        [TestCase(null)]
        [TestCase("Foo")]
        [TestCase((ulong)24)]
        public void TestEquality(object? value)
        {
            var cidr = Ipv4Cidr.Parse(24);
            Assert.IsFalse(cidr.Equals(value));
        }
        
        
        [Test]
        [TestCaseSource(nameof(TestCases))]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void TestEquality(Ipv4CidrTestCase testCase)
        {
            var (value, _, _, _) = testCase;
            object objValue = value;
            Assume.That(Ipv4Cidr.TryParse(value.ToString(), out var cidr), Is.True);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(cidr.Equals(objValue));
                Assert.IsTrue(cidr.Equals(cidr));
                Assert.IsTrue(cidr == value);
                Assert.IsFalse(cidr != value);
                Assert.IsTrue(value == cidr);
                Assert.IsFalse(value != cidr);
#pragma warning disable CS1718 // Comparison made to same variable
                Assert.IsTrue(cidr == cidr);
                Assert.IsFalse(cidr != cidr);
#pragma warning restore CS1718 // Comparison made to same variable
                Assert.AreEqual(value.GetHashCode(), cidr.GetHashCode());
            });
        }
        
        
        [Test]
        [TestCaseSource(nameof(TestCases))]
        [SuppressMessage("ReSharper", "EqualExpressionComparison")]
        public void TestOperators(Ipv4CidrTestCase testCase)
        {
            var (value, _, _, _) = testCase;
            Assume.That(Ipv4Cidr.TryParse(value.ToString(), out var cidr));
            Assert.Multiple(() =>
            {
                Assert.That(Ipv4Cidr.Parse(0) <= cidr);
                Assert.That(Ipv4Cidr.Parse(0).CompareTo(cidr) <= 0);
                
                Assert.That(cidr >= Ipv4Cidr.Parse(0));
                Assert.That(cidr.CompareTo(Ipv4Cidr.Parse(0)) >= 0);
                
                Assert.That(Ipv4Cidr.Parse(32) >= cidr);
                Assert.That(Ipv4Cidr.Parse(32).CompareTo(cidr) >= 0);
                
                Assert.That(cidr <= Ipv4Cidr.Parse(32));
                Assert.That(cidr.CompareTo(Ipv4Cidr.Parse(32)) <= 0);
            
                Assert.That(0 <= cidr);
                
                Assert.That(cidr >= 0);
                Assert.That(cidr.CompareTo(0) >= 0);
                
                Assert.That(32 >= cidr);
                
                Assert.That(cidr <= 32);
                Assert.That(cidr.CompareTo(32) <= 0);
                
                if (value > 0)
                {
                    Assert.That(Ipv4Cidr.Parse(0).CompareTo(cidr) < 0);
                    Assert.That(Ipv4Cidr.Parse(0) < cidr);
                    
                    Assert.That(cidr.CompareTo(Ipv4Cidr.Parse(0)) > 0);
                    Assert.That(cidr > Ipv4Cidr.Parse(0));
                    
                    Assert.That(0 < cidr);
                    
                    Assert.That(cidr.CompareTo(0) > 0);
                    Assert.That(cidr > 0);
                }
                if (value < 32)
                {
                    Assert.That(cidr.CompareTo(Ipv4Cidr.Parse(32)) < 0);
                    Assert.That(cidr < Ipv4Cidr.Parse(32));
                    
                    Assert.That(Ipv4Cidr.Parse(32).CompareTo(cidr) > 0);
                    Assert.That(Ipv4Cidr.Parse(32) > cidr);
                    
                    Assert.That(cidr.CompareTo(32) < 0);
                    Assert.That(cidr < 32);
                    
                    Assert.That(32 > cidr);
                }
            });
        }
        
        

        [Test]
        [Pairwise]
        public void TestObjectComparisons(
            [ValueSource(nameof(TestCases))] Ipv4CidrTestCase a, 
            [ValueSource(nameof(TestCases))] Ipv4CidrTestCase b
        )
        {
            var (aByte, _, _, _) = a;
            var (bByte, _, _, bMaskInt) = b;
            Assert.That(Ipv4Cidr.TryParse(aByte, out var aCidr));
            Assert.That(Ipv4Cidr.TryParse(bByte, out var bCidr));
            Assert.That(Ipv4SubnetMask.TryParse(bMaskInt, out var bMask));

            object bCidrAsObject = bCidr;
            object bMaskAsObject = bMask;
            object bByteAsObject = bByte;

            var expectedEqual = aByte.Equals(bByte);
            var expectedCompare = aByte.CompareTo(bByte);
            
            Assert.That(aCidr.Equals(bCidrAsObject), Is.EqualTo(expectedEqual));
            Assert.That(aCidr.Equals(bMaskAsObject), Is.EqualTo(expectedEqual));
            Assert.That(aCidr.Equals(bByteAsObject), Is.EqualTo(expectedEqual));
            
            Assert.That(aCidr.CompareTo(bCidrAsObject), Is.EqualTo(expectedCompare));
            Assert.That(aCidr.CompareTo(bMaskAsObject), Is.EqualTo(expectedCompare));
            Assert.That(aCidr.CompareTo(bByteAsObject), Is.EqualTo(expectedCompare));
        }


    }
}