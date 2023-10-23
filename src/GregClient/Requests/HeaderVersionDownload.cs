using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    /// <summary>
    /// Request for getting a specific package version.
    /// </summary>
    public class HeaderVersionDownload : PackageReferenceRequest
    {
        private string _versionId;

        public HeaderVersionDownload(string id, string versionId)
        {
            this._type = PackageReferenceStyle.Id;
            this._id = id;
            this._versionId = versionId;
        }

        public HeaderVersionDownload(string engine, string name, string versionId)
        {
            this._type = PackageReferenceStyle.EngineAndName;
            this._name = name;
            this._engine = engine;
            this._versionId = versionId;
        }

        public override string Path
        {
            get
            {
                if (this._type == PackageReferenceStyle.Id)
                {
                    return "package_version/" + this._id + "/" + this._versionId;
                }
                else
                {
                    return "package_version/" + this._engine + "/" + this._name + "/" + this._versionId;
                }
            }
        }

        public override HttpMethod HttpMethod
        {
            get
            {
                return HttpMethod.Get;
            }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
