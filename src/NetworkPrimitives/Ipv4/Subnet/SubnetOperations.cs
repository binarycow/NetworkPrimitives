#nullable enable

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

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

        public static bool TrySubnetBasedOnCount(
            Ipv4Subnet ipv4Subnet, 
            uint numberOfNetworks,
            [NotNullWhen(true)] out IReadOnlyList<Ipv4Subnet>? subnets
        )
        {
            var (networkAddress, mask) = ipv4Subnet;
            numberOfNetworks = BitOperationsEx.RoundUpToPowerOf2(
                numberOfNetworks
            );
            subnets = default;
            var popCount = BitOperations.PopCount(numberOfNetworks - 1);
            var cidrValue = (byte)mask.ToCidr() + popCount;
            if (!Ipv4Cidr.TryParse(cidrValue, out var cidr))
                return false;
            subnets = SubnetBasedOnCidr(networkAddress, cidr, numberOfNetworks);
            return true;
        }
        
        public static bool TrySubnetBasedOnSize(
            Ipv4Subnet ipv4Subnet, 
            Ipv4Cidr cidr,
            [NotNullWhen(true)] out IReadOnlyList<Ipv4Subnet>? subnets
        )
        {
            subnets = default;
            if (cidr < ipv4Subnet.Mask) return false;
            if (cidr == ipv4Subnet.Mask)
            {
                subnets = new[] { ipv4Subnet };
                return true;
            }
            subnets = SubnetBasedOnCidr(ipv4Subnet.NetworkAddress, cidr, (uint)(cidr.TotalHosts / ipv4Subnet.TotalHosts));
            return true;
        }
        
        private static Ipv4Subnet[] SubnetBasedOnCidr(
            Ipv4Address networkAddress, 
            Ipv4Cidr cidr,
            uint numberOfNetworks
        )
        {
            var delta = (uint)cidr.TotalHosts;
            var subnets = new Ipv4Subnet[numberOfNetworks];
            for (var i = 0; i < numberOfNetworks; ++i)
            {
                subnets[i] = new (networkAddress, cidr);
                networkAddress = networkAddress.AddInternal(delta);
            }
            return subnets;
        }

        public static bool TrySubnetBasedOnSize(
            Ipv4Subnet ipv4Subnet,
            uint sizeOfSubnets,
            [NotNullWhen(true)] out IReadOnlyList<Ipv4Subnet>? subnets
        )
        {
            subnets = default;
            var (networkAddress, mask) = ipv4Subnet;
            var minimumCidrNeeded = SubnetMaskLookups.GetCidrNeededForUsableHosts(sizeOfSubnets);
            if (minimumCidrNeeded < ipv4Subnet.Cidr) return false;
            if (!Ipv4Cidr.TryParse(minimumCidrNeeded, out var cidr)) return false;
            var numberOfNetworks = (uint)(mask.TotalHosts / cidr.TotalHosts);
            subnets = SubnetBasedOnCidr(networkAddress, cidr, numberOfNetworks);
            return true;
        }

        public static bool TryVariableLengthSubnet(
            Ipv4Subnet masterSubnet, 
            IEnumerable<uint> numberOfHosts, 
            [NotNullWhen(true)] out IReadOnlyList<Ipv4Subnet>? subnets
        )
        {
            subnets = default;
            var (networkAddress, mask) = masterSubnet;
            var sortedRequirements = numberOfHosts.OrderByDescending(x => x);
            List<Ipv4Subnet>? subnetList = null;
            var remainingAddresses = mask.TotalHosts;

            foreach (var requirement in sortedRequirements)
            {
                var minimumCidrNeeded = SubnetMaskLookups.GetCidrNeededForUsableHosts(requirement);
                if (!Ipv4Cidr.TryParse(minimumCidrNeeded, out var cidr)) return false;
                if(remainingAddresses < cidr.TotalHosts) return false;
                var subnet = new Ipv4Subnet(networkAddress, cidr);
                networkAddress = networkAddress.AddInternal((uint)cidr.TotalHosts);
                remainingAddresses -= cidr.TotalHosts;
                subnetList ??= new ();
                subnetList.Add(subnet);
            }
            if (subnetList is null) return false;
            subnets = subnetList.AsReadOnly();
            return true;
        }

    }
}