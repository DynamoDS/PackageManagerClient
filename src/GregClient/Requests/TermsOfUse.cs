using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public class TermsOfUse : Request
    {
        private readonly HttpMethod httpMethod = System.Net.Http.HttpMethod.Get;

        public TermsOfUse(bool queryAcceptanceStatus)
        {
            // both endpoints require authentication by the user
            this.ForceAuthentication = true;

            httpMethod = queryAcceptanceStatus ? System.Net.Http.HttpMethod.Get: System.Net.Http.HttpMethod.Put;
        }

        public override string Path
        {
            get { return "tou"; }
        }

        public override HttpMethod HttpMethod
        {
            get { return httpMethod; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
