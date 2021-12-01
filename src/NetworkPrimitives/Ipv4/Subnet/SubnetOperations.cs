#nullable enable

using System;
using System.Collections.Generic;

namespace NetworkPrimitives.Ipv4
{
    [ExcludeFromCodeCoverage("Internal")]
    internal static class SubnetOperations
    {
        public static Ipv4Subnet GetContainingSupernet(IEnumerable<Ipv4Address> addresses)
        {
            using var enumerator = addresses.GetEnumerator();
            if (enumerator.MoveNext() == false) return default;
            var subnet = new Ipv4Subnet(enumerator.Current, Ipv4Cidr.Slash32);
            while(enumerator.MoveNext())
                subnet = GetContainingSupernet(subnet, new Ipv4Subnet(enumerator.Current, Ipv4Cidr.Slash32));
            return subnet;
        }

        public static Ipv4Subnet GetContainingSupernet(IReadOnlyList<Ipv4Address> addresses)
        {
            if (addresses.Count == 0) return default;
            if (addresses.Count == 1) return new (addresses[0], Ipv4Cidr.Slash32);
            var subnet = new Ipv4Subnet(addresses[0], Ipv4Cidr.Slash32);
            for (var i = 1; i < addresses.Count; ++i)
            {
                subnet = GetContainingSupernet(subnet, new Ipv4Subnet(addresses[i], Ipv4Cidr.Slash32));
            }
            return subnet;
        }

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
            _ = SubnetOperations.TryGetParent(subnet, out var parent);
            return parent;
        }
        
        public static bool TryGetParent(Ipv4Subnet subnet, out Ipv4Subnet parent)
        {
            parent = default;
            if (subnet.Mask.Value is 0) return false;
            parent = new Ipv4Subnet(subnet.NetworkAddress, Ipv4Cidr.Parse(subnet.Mask.ToCidr().Value - 1));
            return true;
        }

        
        public static Ipv4Subnet GetContainingSupernet(Ipv4Address a, Ipv4Address b) 
            => GetContainingSupernet(new Ipv4Subnet(a, Ipv4Cidr.Slash32), new Ipv4Subnet(b, Ipv4Cidr.Slash32));
        public static Ipv4Subnet GetContainingSupernet(Ipv4Address a, Ipv4Subnet b) 
            => GetContainingSupernet(new Ipv4Subnet(a, Ipv4Cidr.Slash32), b);
        public static Ipv4Subnet GetContainingSupernet(Ipv4Subnet a, Ipv4Address b) 
            => GetContainingSupernet(a, new Ipv4Subnet(b, Ipv4Cidr.Slash32));

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