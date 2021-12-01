#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv6;

namespace NetworkPrimitives.JsonConverters.Ipv6
{
    public class Ipv6CidrJsonConverter : JsonConverter<Ipv6Cidr>
    {
        private Ipv6CidrJsonConverter() { }
        public static readonly Ipv6CidrJsonConverter Instance = new ();
        public override Ipv6Cidr Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv6Cidr.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv6Cidr value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}