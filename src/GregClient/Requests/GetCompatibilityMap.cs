using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Greg.Requests
{
    public class GetCompatibilityMap : Request
    {
        public GetCompatibilityMap()
        {
        }

        public override string Path
        {
            get { return "host_map"; }
        }

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
