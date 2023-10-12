
using System.Net.Http;
using RestSharp;

namespace Greg.Requests
{
    public class Downvote : Request
    {
        public Downvote(string id)
        {
            this._id = id;
        }

        public override string Path
        {
            get { return "downvote/" + this._id; }
        }

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Put; }
        }

        private readonly string _id;

        internal override void Build(ref RestRequest request)
        {

        }
    }
}
