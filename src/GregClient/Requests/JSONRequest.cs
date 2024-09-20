using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.Requests
{
    public abstract class JsonRequest : Request
    {
        static protected int Timeout = 300000;// Default timeout for JsonRequests, in milliseconds

        // Initialize the common RestRequest properties.
        // Ex: Timeout property
        internal void InitReqParams(ref RestRequest request)
        {
            request.Timeout = TimeSpan.FromMilliseconds(Timeout);
        }

        internal override void Build(ref RestRequest request)
        {
            InitReqParams(ref request);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(this.RequestBody);
        }
    }
}
