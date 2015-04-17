using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public class TermsOfUse : Request
    {
        private readonly Method httpMethod = Method.GET;

        public TermsOfUse(bool queryAcceptanceStatus)
        {
            // both endpoints require authentication by the user
            this.ForceAuthentication = true;

            httpMethod = queryAcceptanceStatus ? Method.GET : Method.PUT;
        }

        public override string Path
        {
            get { return "tou"; }
        }

        public override Method HttpMethod
        {
            get { return httpMethod; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
