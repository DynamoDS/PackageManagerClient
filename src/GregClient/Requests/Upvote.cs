using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public class Upvote : Request
    {
        public Upvote(string id)
        {
            this._id = id;
        }

        public override string Path
        {
            get { return "upvote/" + this._id; }
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
