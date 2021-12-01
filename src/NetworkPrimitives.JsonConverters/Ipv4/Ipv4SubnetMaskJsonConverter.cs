#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NetworkPrimitives.Ipv4;

namespace NetworkPrimitives.JsonConverters.Ipv4
{
    public class Ipv4SubnetMaskJsonConverter : JsonConverter<Ipv4SubnetMask>
    {
        private Ipv4SubnetMaskJsonConverter() { }
        public static readonly Ipv4SubnetMaskJsonConverter Instance = new ();
        public override Ipv4SubnetMask Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => Ipv4SubnetMask.Parse(reader.GetString());

        public override void Write(
            Utf8JsonWriter writer,
            Ipv4SubnetMask value,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(value.ToString());
    }
}