using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    public class ValidateAuth : Request
    {
        public ValidateAuth()
        {
            this.ForceAuthentication = true;
        }

        public override string Path
        {
            get { return "validate"; }
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