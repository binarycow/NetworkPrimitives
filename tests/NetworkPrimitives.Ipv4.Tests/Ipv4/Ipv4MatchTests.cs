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

        [Test]
        [TestCaseSource(nameof(RangeTestCases))]
        public void TestParse(Ipv4WildcardMaskRangeTestCase testCase)
        {
            Assert.That(Ipv4NetworkMatch.TryParse(testCase.Input, out _));
        }
        
  
    }
}