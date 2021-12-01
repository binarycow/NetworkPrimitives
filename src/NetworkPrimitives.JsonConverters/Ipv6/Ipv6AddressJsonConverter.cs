#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv6;

namespace NetworkPrimitives.JsonConverters.Ipv6
{
    public class Ipv6AddressJsonConverter : JsonConverter<Ipv6Address>
    {
        private Ipv6AddressJsonConverter() { }
        public static readonly Ipv6AddressJsonConverter Instance = new ();
        public override Ipv6Address Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv6Address.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv6Address value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}