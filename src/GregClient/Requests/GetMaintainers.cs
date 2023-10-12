using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    /// <summary>
    /// Request for getting maintainers of a specific package.
    /// </summary>
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

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        internal override void Build(ref RestRequest request)
        {

        }
    }
}
