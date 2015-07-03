using RestSharp;

namespace Greg.Requests
{
    public class BanPackage : Request
    {
        private enum Action { Ban, Unban }
        private readonly Action banAction;
        private readonly string packageId;

        public BanPackage(string packageId, bool banPackage)
        {
            // both endpoints require authentication by the user
            this.ForceAuthentication = true;
            this.packageId = packageId;
            this.banAction = banPackage ? Action.Ban : Action.Unban;
        }

        public override string Path
        {
            get
            {
                var root = banAction == Action.Ban ? "ban/" : "unban/";
                return root + packageId;
            }
        }

        public override Method HttpMethod
        {
            get { return Method.PUT; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
