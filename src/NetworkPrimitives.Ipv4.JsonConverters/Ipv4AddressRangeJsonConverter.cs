#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public class Ipv4AddressRangeJsonConverter : JsonConverter<Ipv4AddressRange>
    {
        private Ipv4AddressRangeJsonConverter() { }
        public static readonly Ipv4AddressRangeJsonConverter Instance = new ();
        public override Ipv4AddressRange Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4AddressRange.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4AddressRange value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}