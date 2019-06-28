using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public class Hosts : Request
    {
        public Hosts()
        {
        }

        public override string Path
        {
            get { return "hosts"; }
        }

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
