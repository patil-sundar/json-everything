﻿using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema
{
	[SchemaKeyword(Name)]
	[JsonConverter(typeof(MinLengthKeywordJsonConverter))]
	public class MinLengthKeyword : IJsonSchemaKeyword
	{
		internal const string Name = "minLength";

		public decimal Value { get; }

		public MinLengthKeyword(decimal value)
		{
			Value = value;
		}

		public void Validate(ValidationContext context)
		{
			if (context.LocalInstance.ValueKind != JsonValueKind.String)
			{
				context.IsValid = true;
				return;
			}

			var length = new StringInfo(context.LocalInstance.GetString()).LengthInTextElements;
			context.IsValid = Value <= length;
			if (!context.IsValid)
				context.Message = $"Value is not longer than or equal to {Value} characters";
		}
	}

	public class MinLengthKeywordJsonConverter : JsonConverter<MinLengthKeyword>
	{
		public override MinLengthKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.Number)
				throw new JsonException("Expected number");

			var number = reader.GetDecimal();

			return new MinLengthKeyword(number);
		}
		public override void Write(Utf8JsonWriter writer, MinLengthKeyword value, JsonSerializerOptions options)
		{
			writer.WriteNumber(MinLengthKeyword.Name, value.Value);
		}
	}
}