﻿#nullable enable

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetworkPrimitives.Ipv4.JsonConverters
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
            Ipv4Address dateTimeValue,
            JsonSerializerOptions options
        ) => writer.WriteStringValue(dateTimeValue.ToString());
    }
}