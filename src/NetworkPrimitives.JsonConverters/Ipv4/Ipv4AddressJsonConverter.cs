#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv4;

namespace NetworkPrimitives.JsonConverters.Ipv4
{
    public class Ipv4AddressJsonConverter : JsonConverter<Ipv4Address>
    {
        private Ipv4AddressJsonConverter() { }
        public static readonly Ipv4AddressJsonConverter Instance = new ();
        public override Ipv4Address Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4Address.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4Address value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}