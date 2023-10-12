using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    public class HeaderCollectionDownload : Request
    {
        protected enum CollectionDownloadType
        {
            ByEngine,
            All
        };

        public static HeaderCollectionDownload All()
        {
            return new HeaderCollectionDownload(CollectionDownloadType.All);
        }

        public static HeaderCollectionDownload ByEngine(string engine)
        {
            return new HeaderCollectionDownload(engine);
        }

        protected HeaderCollectionDownload(CollectionDownloadType type)
        {
            this._type = type;
        }

        protected HeaderCollectionDownload(string engine)
        {
            this._engine = engine;
            this._type = CollectionDownloadType.ByEngine;
        }

        protected string _engine;
        protected CollectionDownloadType _type;

        public override string Path
        {
            get
            {
                if (this._type == CollectionDownloadType.ByEngine)
                {
                    return "packages/" + this._engine;
                }
                else // if (this._type == CollectionDownloadType.All)
                {
                    return "packages";
                }
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
