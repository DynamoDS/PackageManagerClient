using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public abstract class JsonRequest : Request
    {
        public override void Build(ref RestRequest request)
        {
            request.RequestFormat = DataFormat.Json;
            request.AddBody(this.RequestBody);
        }
    }
}
