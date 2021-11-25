#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public class Ipv4CidrJsonConverter : JsonConverter<Ipv4Cidr>
    {
        private Ipv4CidrJsonConverter() { }
        public static readonly Ipv4CidrJsonConverter Instance = new ();
        public override Ipv4Cidr Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4Cidr.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4Cidr value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}