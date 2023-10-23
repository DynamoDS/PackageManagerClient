using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    public class Search : Request
    {

        public Search(string query)
        {
            this._query = query;
        }

        public override string Path
        {
            get
            {
                return "search/" + this._query;
            }
        }

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        private readonly string _query;

        internal override void Build(ref RestRequest request)
        {

        }
    }
}


