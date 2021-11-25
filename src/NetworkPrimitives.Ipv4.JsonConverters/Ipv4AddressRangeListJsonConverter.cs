#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public class Ipv4AddressRangeListJsonConverter : JsonConverter<Ipv4AddressRangeList>
    {
        private Ipv4AddressRangeListJsonConverter() { }
        public static readonly Ipv4AddressRangeListJsonConverter Instance = new ();
        public override Ipv4AddressRangeList Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4AddressRangeList.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4AddressRangeList dateTimeValue,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(dateTimeValue.ToString());
    }
}