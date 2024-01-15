using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Faerie.Core.Templates
{
    internal class ValueRuleConverter : JsonConverter<object>
    {
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            Console.WriteLine(reader.TokenType);

            return reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString()!,
                JsonTokenType.StartObject => JsonSerializer.Deserialize<List<string>>(ref reader),
                _ => new JsonException(),
            };
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
