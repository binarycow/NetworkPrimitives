#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv4;

namespace NetworkPrimitives.JsonConverters.Ipv4
{
    public class Ipv4NetworkMatchJsonConverter : JsonConverter<Ipv4NetworkMatch>
    {
        private Ipv4NetworkMatchJsonConverter() { }
        public static readonly Ipv4NetworkMatchJsonConverter Instance = new ();
        public override Ipv4NetworkMatch Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4NetworkMatch.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4NetworkMatch value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}