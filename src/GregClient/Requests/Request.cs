using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public abstract class Request
    {
        public Request()
        {
            this.ForceAuthentication = false;
        }

        public bool RequiresAuthorization
        {
            get
            {
                return HttpMethod == HttpMethod.Post || HttpMethod == HttpMethod.Put || ForceAuthentication;
            }
        }

        public abstract string Path { get; }

        public abstract HttpMethod HttpMethod { get; }

        public RequestBody RequestBody { get; set; }

        public bool ForceAuthentication { get; set; }

        internal virtual IEnumerable<Parameter> GetParamsToSign(ref RestRequest request)
        {
            return request.Parameters;// By default send all params to sign.
        }

        internal abstract void Build(ref RestRequest request);
    }
}
