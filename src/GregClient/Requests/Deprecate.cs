using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public class Deprecate : Request
    {
        public Deprecate(string id)
        {
            this._id = id;
        }

        public override string Path
        {
            get { return "deprecate/" + this._id; }
        }

        public override Method HttpMethod
        {
            get { return Method.PUT; }
        }

        private readonly string _id;

        public override void Build(ref RestRequest request)
        {

        }
    }
}
