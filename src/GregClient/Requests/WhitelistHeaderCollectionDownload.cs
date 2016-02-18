using RestSharp;

namespace Greg.Requests
{
    public class WhitelistHeaderCollectionDownload : HeaderCollectionDownload
    {
        public override string Path
        {
            get
            {
                return "whitelist";
            }
        }

        public static new WhitelistHeaderCollectionDownload All()
        {
            return new WhitelistHeaderCollectionDownload(CollectionDownloadType.All);
        }

        protected WhitelistHeaderCollectionDownload(CollectionDownloadType type) : base(type)
        {
        }

        protected WhitelistHeaderCollectionDownload(string engine) : base(engine)
        {
        }
    }
}
