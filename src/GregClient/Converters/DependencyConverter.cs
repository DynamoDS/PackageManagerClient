using Greg.Responses;
using Newtonsoft.Json;
using System;

namespace Greg.Converters
{
    /// <summary>
    /// Custom converter that supports deserializing a Dependency from either:
    /// - a string, which is interpreted as the id of the dependency
    /// - an object having the expected properties, which is the default behavior
    /// </summary>
    public class DependencyConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dependency);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                // This is interpreted as the id of the dependency.
                Dependency dep = (Dependency)existingValue ?? new Dependency();
                dep._id = (string)reader.Value;
                return dep;
            }
            else
            {
                // Use the default deserialization behavior.
                existingValue = existingValue ?? serializer.ContractResolver.ResolveContract(objectType).DefaultCreator();
                serializer.Populate(reader, existingValue);
                return existingValue;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Not needed as CanWrite is false.
            throw new NotImplementedException();
        }
    }
}
