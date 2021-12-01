#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv6;

namespace NetworkPrimitives.JsonConverters.Ipv6
{
    public class Ipv6SubnetJsonConverter : JsonConverter<Ipv6Subnet>
    {
        private Ipv6SubnetJsonConverter() { }
        public static readonly Ipv6SubnetJsonConverter Instance = new ();
        public override Ipv6Subnet Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv6Subnet.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv6Subnet value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}