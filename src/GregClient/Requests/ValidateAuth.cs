using RestSharp;

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

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        internal override void Build(ref RestRequest request)
        {
        }

    }
}