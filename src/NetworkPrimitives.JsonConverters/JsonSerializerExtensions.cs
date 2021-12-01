#nullable enable

using System.Text.Json;
using NetworkPrimitives.JsonConverters.Ipv4;
using NetworkPrimitives.JsonConverters.Ipv6;

namespace NetworkPrimitives.JsonConverters
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions AddNetworkPrimitivesConverters(
            this JsonSerializerOptions options
        )
        {
            return options
                .AddIpv4Converters()
                .AddIpv6Converters();
        }


        public static JsonSerializerOptions AddIpv4Converters(
            this JsonSerializerOptions options
        )
        {
            options.Converters.Add(Ipv4AddressJsonConverter.Instance);
            options.Converters.Add(Ipv4AddressRangeJsonConverter.Instance);
            options.Converters.Add(Ipv4AddressRangeListJsonConverter.Instance);
            options.Converters.Add(Ipv4CidrJsonConverter.Instance);
            options.Converters.Add(Ipv4NetworkMatchJsonConverter.Instance);
            options.Converters.Add(Ipv4SubnetJsonConverter.Instance);
            options.Converters.Add(Ipv4SubnetMaskJsonConverter.Instance);
            options.Converters.Add(Ipv4WildcardMaskJsonConverter.Instance);
            return options;
        }
        
        public static JsonSerializerOptions AddIpv6Converters(
            this JsonSerializerOptions options
        )
        {
            options.Converters.Add(Ipv6AddressJsonConverter.Instance);
            options.Converters.Add(Ipv6CidrJsonConverter.Instance);
            options.Converters.Add(Ipv6SubnetJsonConverter.Instance);
            options.Converters.Add(Ipv6SubnetMaskJsonConverter.Instance);
            return options;
        }
        
        
    }
}