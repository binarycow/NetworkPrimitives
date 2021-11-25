#nullable enable

using System;
using System.Text.Json;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public static class JsonSerializerExtensions
    {
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
    }
}