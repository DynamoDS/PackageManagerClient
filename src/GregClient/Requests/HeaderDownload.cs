using RestSharp;

namespace Greg.Requests
{
    public class HeaderDownload : Request
    {
        enum PackageHeaderDownloadRequestType { Id, EngineName };

        public HeaderDownload(string id)
        {
            this._type = PackageHeaderDownloadRequestType.Id;
            this._id = id;
        }

        public HeaderDownload(string engine, string name)
        {
            this._type = PackageHeaderDownloadRequestType.EngineName;
            this._name = name;
            this._engine = engine;
        }

        public override string Path
        {
            get
            {
                if (this._type == PackageHeaderDownloadRequestType.Id)
                {
                    return "package/" + this._id;
                }
                else // if (this._type == PackageHeaderDownloadRequestType.ENGINE_NAME)
                {
                    return "package/" + this._engine + "/" + this._name;
                }
            }
        }

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        private readonly PackageHeaderDownloadRequestType _type;
        private readonly string _id;
        private readonly string _name;
        private readonly string _engine;

        public override void Build(ref RestRequest request)
        {

        }
    }
}


