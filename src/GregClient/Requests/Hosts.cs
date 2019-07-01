using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
	/// <summary>
    /// Hosts request. Returns a fixed list of all supported host applications.
	/// Ex. ["Revit", "Autocad", ...]
    /// </summary>
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
