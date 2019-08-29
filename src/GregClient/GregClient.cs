using System.Net;
using Greg.Requests;
using Greg.Responses;
using RestSharp;

namespace Greg
{
    public class GregClient : IGregClient
    {
        private readonly RestClient _client;
   
        public string BaseUrl { get { return _client.BaseUrl.ToString(); } }

        public readonly IAuthProvider _authProvider;
        public IAuthProvider AuthProvider
        {
            get { return _authProvider; }
        }

        public GregClient(IAuthProvider provider, string packageManagerUrl)
        {

#if LT_NET47
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
#else
            // https://stackoverflow.com/questions/2819934/detect-windows-version-in-net
            // if the current OS is windows 7 or lower
            // set TLS to 1.2.
            // else do nothing and let the OS decide the version of TLS to support. (.net 4.7 required)
            if (System.Environment.OSVersion.Version.Major <= 6 && System.Environment.OSVersion.Version.Minor <= 1)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
#endif
            _authProvider = provider;
            _client = new RestClient(packageManagerUrl);
        }

        private IRestResponse ExecuteInternal(Request m)
        {
            var req = new RestRequest(m.Path, m.HttpMethod);
            m.Build(ref req);

            if (m.RequiresAuthorization)
            {
                // Issue: auth api was adding body params to the query string.
                // Details: https://jira.autodesk.com/browse/DYN-1795
                // Build a subset of the original request, with only specific parameters that we need to authenticate.
                var reqToSign = new RestRequest(req.Resource, req.Method);
                var authParams = m.GetParamsToSign(ref req);
                foreach (var par in authParams)
                {
                    reqToSign.AddParameter(par);
                }

                // All reqToSign.Parameters will be added in the reqToSign.Resource.
                AuthProvider.SignRequest(ref reqToSign, _client);
                req.Resource = reqToSign.Resource;
            }
            var restResp = _client.Execute(req);
            Utility.DebugLogger.LogResponse(restResp);
            return restResp;
        }

        public Response Execute(Request m)
        {
            return new Response(ExecuteInternal(m));
        }

        public ResponseBody ExecuteAndDeserialize(Request m)
        {
            return Execute(m).Deserialize();
        }

        /// <summary>
        /// Execute the request and deserialize the content.
        /// </summary>
        /// <typeparam name="T">The Type of content</typeparam>
        /// <param name="m">The request.</param>
        /// <returns>A <see cref="ResponseWithContent{T}"/> or null if there was an error
        /// in executing the message.</returns>
        public ResponseWithContentBody<T> ExecuteAndDeserializeWithContent<T>(Request m)
        {
            var response = this.ExecuteInternal(m);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return new ResponseWithContent<T>(response).DeserializeWithContent();
        }
    }
}


