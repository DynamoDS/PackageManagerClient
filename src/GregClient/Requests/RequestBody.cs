using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp.Serializers;

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
