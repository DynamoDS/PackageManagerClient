using System;
using System.Collections.Generic;
using System.Linq;
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
                return HttpMethod == Method.POST || HttpMethod == Method.PUT || ForceAuthentication;
            }
        }

        public abstract string Path { get; }

        public abstract Method HttpMethod { get; }

        public RequestBody RequestBody { get; set; }

        public bool ForceAuthentication { get; set; }
        
        public abstract void Build(ref RestRequest request);
    }
}
