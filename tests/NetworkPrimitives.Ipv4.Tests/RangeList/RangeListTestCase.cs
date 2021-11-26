using System.Collections.Generic;

namespace NetworkPrimitives.Ipv4.Tests.RangeList
{
    public record RangeListTestCase(
        string Input,
        IReadOnlyList<string> ExpectedRanges,
        IReadOnlyList<string> ExpectedIps
    );
}