#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public class Ipv4SubnetJsonConverter : JsonConverter<Ipv4Subnet>
    {
        private Ipv4SubnetJsonConverter() { }
        public static readonly Ipv4SubnetJsonConverter Instance = new ();
        public override Ipv4Subnet Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4Subnet.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4Subnet value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}