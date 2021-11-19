using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using NetworkPrimitives.Ipv4;

namespace NetworkPrimitives.Tests.Ipv4
{
    public record Ipv4TestCaseStrings(
        string IpString,
        string MaskString,
        
        string Ip,
        string Cidr,
        string Mask,
        string TotalHosts,
        string UsableHosts,
        string Network,
        string FirstUsable,
        string LastUsable,
        string Broadcast
    );

    public static class Ipv4TestCaseProvider
    {
        public static IReadOnlyList<Ipv4TestCase> LoadTestCases(string path)
        {
            var fullPath = Path.GetFullPath(Path.Combine("../../../..",path));
            var jsonString = File.ReadAllText(fullPath);
            var strings = JsonSerializer.Deserialize<Ipv4TestCaseStrings[]>(jsonString)
                ?? Array.Empty<Ipv4TestCaseStrings>();
            return strings.Select(ParseTestCase).ToList();
        }
        private static Ipv4TestCase ParseTestCase(Ipv4TestCaseStrings arg)
        {
            ;
            return new Ipv4TestCase(
                IpString: arg.IpString,
                MaskString: arg.MaskString,
                Ip: uint.Parse(arg.Ip, NumberStyles.HexNumber, CultureInfo.CurrentCulture),
                Cidr: byte.Parse(arg.Cidr),
                Mask: uint.Parse(arg.Mask, NumberStyles.HexNumber, CultureInfo.CurrentCulture),
                TotalHosts: ulong.Parse(arg.TotalHosts),
                UsableHosts: uint.Parse(arg.UsableHosts),
                Network: uint.Parse(arg.Network, NumberStyles.HexNumber, CultureInfo.CurrentCulture),
                FirstUsable: uint.Parse(arg.FirstUsable, NumberStyles.HexNumber, CultureInfo.CurrentCulture),
                LastUsable: uint.Parse(arg.LastUsable, NumberStyles.HexNumber, CultureInfo.CurrentCulture),
                Broadcast: uint.Parse(arg.Broadcast, NumberStyles.HexNumber, CultureInfo.CurrentCulture)
            );
        }
    }

    public record Ipv4TestCase(
        string IpString,
        string MaskString,

        uint Ip,
        byte Cidr,
        uint Mask,
        ulong TotalHosts,
        uint UsableHosts,
        uint Network,
        uint FirstUsable,
        uint LastUsable,
        uint Broadcast
    )
    {
        public string SubnetInput => $"{IpString}/{Cidr.ToString()}";
        public string SubnetExpected => $"{new IPAddress(Network.SwapEndianIfLittleEndian())}/{Cidr.ToString()}";
    }
}