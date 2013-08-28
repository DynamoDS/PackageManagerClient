using RestSharp;

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

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        private readonly string _query;

        public override void Build(ref RestRequest request)
        {

        }
    }
}


