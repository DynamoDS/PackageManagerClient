using System;
using Greg.Requests;
using Greg.Responses;
using RestSharp;

namespace Greg
{
    public class Client
    {
        public IAuthProvider Provider { get; set; }
        private readonly RestClient _pmClient;

        public Client(IAuthProvider provider, string packageManagerUrl)
        {
            Provider = provider;
            _pmClient = new RestClient(packageManagerUrl);
        }

        public string BaseUrl { get { return _pmClient.BaseUrl; } }

        public IRestResponse Execute(Request m)
        {
            var req = new RestRequest(m.Path, m.HttpMethod);
            m.Build( ref req );
            if (m.RequiresAuthorization)
            {
                Provider.SignRequest(ref req, _pmClient);
            }
            return _pmClient.Execute(req);
        }

        public ResponseBody ExecuteAndDeserialize(Request m)
        {
            var res = this.Execute(m);
            var resp = new Response(res);
            var des_resp = resp.Deserialize();
            return des_resp;
        }

        public ResponseWithContentBody<T> ExecuteAndDeserializeWithContent<T>(Request m)
        {
            return new ResponseWithContent<T>( this.Execute(m) ).DeserializeWithContent();
        }

        public bool IsAuthenticated()
        {
            try
            {
                var response = this.ExecuteAndDeserialize(new ValidateAuth());
                return response.success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


