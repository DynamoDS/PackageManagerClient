using Greg.Responses;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Greg.Converters
{
    /// <summary>
    /// Custom converter that supports deserializing a Dependency from either:
    /// - a string, which is interpreted as the id of the dependency
    /// - an object having the expected properties, which is the default behavior
    /// </summary>
    public class DependencyConverter : JsonConverter<Dependency>
    {
        public override Dependency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {   
            if (reader.TokenType == JsonTokenType.String)
            {
                // This is interpreted as the id of the dependency.
                var dep =new Dependency();
                dep._id = reader.GetString();
                return dep;
            }
            else
            {
                // Use the default deserialization behavior.
                //do not pass the same options here as this method is called with, that will result in the converter being called again and again.
                return JsonSerializer.Deserialize<Dependency>(ref reader);
            }
        }
        public override void Write(Utf8JsonWriter writer, Dependency value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
