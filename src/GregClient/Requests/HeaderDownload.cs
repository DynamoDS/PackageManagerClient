using RestSharp;

namespace Greg.Requests
{

    public class HeaderDownload : PackageReferenceRequest
    {

        public HeaderDownload(string id)
        {
            this._type = PackageReferenceStyle.Id;
            this._id = id;
        }

        public HeaderDownload(string engine, string name)
        {
            this._type = PackageReferenceStyle.EngineAndName;
            this._name = name;
            this._engine = engine;
        }

        public override string Path
        {
            get
            {
                if (this._type == PackageReferenceStyle.Id)
                {
                    return "package/" + this._id;
                }
                else 
                {
                    return "package/" + this._engine + "/" + this._name;
                }
            }
        }

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        public override void Build(ref RestRequest request)
        {

        }
    }
}


