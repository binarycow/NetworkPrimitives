#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
{
    public class Ipv4WildcardMaskJsonConverter : JsonConverter<Ipv4WildcardMask>
    {
        private Ipv4WildcardMaskJsonConverter() { }
        public static readonly Ipv4WildcardMaskJsonConverter Instance = new ();
        public override Ipv4WildcardMask Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4WildcardMask.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4WildcardMask value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}