using RestSharp.Serialization.Json;

namespace Greg.Requests
{
    public abstract class RequestBody
    {
        public static JsonSerializer jsonSerializer = new JsonSerializer();

        public virtual string AsJson()
        {
            return jsonSerializer.Serialize(this);
        }
    }

}
