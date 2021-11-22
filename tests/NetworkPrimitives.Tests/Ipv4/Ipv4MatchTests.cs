#nullable enable

using System;
using System.Collections.Generic;
using NetworkPrimitives.Ipv4;
using NUnit.Framework;

namespace NetworkPrimitives.Tests.Ipv4
{
    public record Ipv4WildcardMaskRangeTestCase(string Input);

    
    [TestFixture]
    public class Ipv4MatchTests
    {
        public static IReadOnlyList<Ipv4WildcardMaskRangeTestCase> RangeTestCases { get; } = new[]
        {
            new Ipv4WildcardMaskRangeTestCase("10.0.0.0 0.0.0.5"),
        };

        
#if CICD
        [Test]
        public void TestParse()
        {
            foreach (var testCase in RangeTestCases)
                TestParse(testCase);
        }
#else
        [Test]
        [TestCaseSource(nameof(RangeTestCases))]
#endif
        public void TestParse(Ipv4WildcardMaskRangeTestCase testCase)
        {
            Assert.That(Ipv4NetworkMatch.TryParse(testCase.Input, out _));
        }
        
  
    }
}