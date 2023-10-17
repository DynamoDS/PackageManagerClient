using System.Text.Json;

namespace Greg.Requests
{
    public abstract class RequestBody
    {
        public virtual string AsJson()
        {
            return JsonSerializer.Serialize<object>(this);
        }
    }

}
