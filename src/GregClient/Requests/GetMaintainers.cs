using RestSharp;

namespace Greg.Requests
{
    public class GetMaintainers : PackageReferenceRequest
    {

        public GetMaintainers(string engine, string name)
        {
            this._name = name;
            this._engine = engine;
        }

        public override string Path
        {
            get
            {
                return "package/" + this._engine + "/" + this._name + "/maintainers";
            }
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
