using RestSharp;

namespace Greg.Requests
{
    public class GetUserPackages : PackageReferenceRequest
    {
        private readonly Method httpMethod = Method.GET;

        public GetUserPackages()
        {
            this.ForceAuthentication = true;

            httpMethod = Method.GET;
        }

        public override string Path
        {
            get { return "user/latest_packages"; }
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
