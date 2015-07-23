using RestSharp;

namespace Greg.Requests
{
    public class BanPackage : Request
    {
        public enum Action { Ban, Unban }
        private readonly Action banAction;
        private readonly string packageId;

        public BanPackage(string packageId, BanPackage.Action banAction)
        {
            this.packageId = packageId;
            this.banAction = banAction;
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
