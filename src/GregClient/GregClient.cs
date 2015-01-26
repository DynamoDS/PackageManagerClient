using System;
using Greg.Requests;
using Greg.Responses;
using RestSharp;

namespace Greg
{
    public class GregClient : IGregClient
    {
        private readonly RestClient _client;

        public string BaseUrl { get { return _client.BaseUrl; } }

        public readonly IAuthProvider _authProvider;
        public IAuthProvider AuthProvider
        {
            get { return _authProvider; }
        }

        public GregClient(IAuthProvider provider, string packageManagerUrl)
        {
            _authProvider = provider;
            _client = new RestClient(packageManagerUrl);
        }

        private IRestResponse ExecuteInternal(Request m)
        {
            var req = new RestRequest(m.Path, m.HttpMethod);
            m.Build(ref req);

            if (m.RequiresAuthorization)
            {
                AuthProvider.SignRequest(ref req, _client);
            }
            return _client.Execute(req);
        }

        public Response Execute(Request m)
        {
            return new Response(ExecuteInternal(m));
        }

        public ResponseBody ExecuteAndDeserialize(Request m)
        {
            return Execute(m).Deserialize();
        }

        public ResponseWithContentBody<T> ExecuteAndDeserializeWithContent<T>(Request m)
        {
            return new ResponseWithContent<T>( this.ExecuteInternal(m) ).DeserializeWithContent();
        }

    }
}


