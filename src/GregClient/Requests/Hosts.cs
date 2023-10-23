using System.Net.Http;
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

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        internal override void Build(ref RestRequest request)
        {
        }
    }
}
