#nullable enable

using System;
using System.Collections.Generic;

namespace NetworkPrimitives.Ipv4
{
    internal static class SubnetOperations
    {
        public static Ipv4Subnet GetContainingSupernet(IEnumerable<Ipv4Subnet> subnets)
        {
            using var enumerator = subnets.GetEnumerator();
            if (enumerator.MoveNext() == false) return default;
            var subnet = enumerator.Current;
            while(enumerator.MoveNext())
                subnet = GetContainingSupernet(subnet, enumerator.Current);
            return subnet;
        }
        
        public static Ipv4Subnet GetContainingSupernet(IReadOnlyList<Ipv4Subnet> subnets)
        {
            if (subnets.Count == 0) return default;
            if (subnets.Count == 1) return subnets[0];
            var subnet = subnets[0];
            for (var i = 1; i < subnets.Count; ++i)
            {
                subnet = GetContainingSupernet(subnet, subnets[i]);
            }
            return subnet;
        }

        public static Ipv4Subnet GetParent(Ipv4Subnet subnet)
        {
            if (subnet.Mask.Value is 0) return default;
            return subnet.NetworkAddress / Ipv4Cidr.Parse(subnet.Mask.ToCidr().Value - 1);
        }

        public static Ipv4Subnet GetContainingSupernet(Ipv4Subnet a, Ipv4Subnet b)
        {
            if (a == b) return a;
            if (a.Contains(b)) return a;
            if (b.Contains(a)) return b;
            if (a.Mask.Value is 0 || b.Mask.Value is 0) return default;
            do
            {
                a = GetParent(a);
            } while (a.Contains(b) == false);
            return a;
        }
    }
}