using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using RestSharp;
using System;
using System.Net;

namespace Greg
{
    public class GregClient : IGregClient
    {
        private readonly RestClient _client;
        public string BaseUrl { get { return _client.Options.BaseUrl.ToString(); } }
        public readonly IAuthProvider _authProvider;
        public IAuthProvider AuthProvider
        {
            get { return _authProvider; }
        }

        public GregClient(IAuthProvider provider, string packageManagerUrl)
        {


            // https://stackoverflow.com/questions/2819934/detect-windows-version-in-net
            // if the current OS is windows 7 or lower
            // set TLS to 1.2.
            // else do nothing and let the OS decide the version of TLS to support. (.net 4.7 required)
            if (System.Environment.OSVersion.Version.Major <= 6 && System.Environment.OSVersion.Version.Minor <= 1)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            _authProvider = provider;
            _client = new RestClient(packageManagerUrl);
        }

        private RestResponse ExecuteInternal(Request m)
        {

            var req = new RestRequest(m.Path, m.HttpMethod.ToRestSharpHTTPMethod());
            m.Build(ref req);

            if (m.RequiresAuthorization)
            {
                if (AuthProvider is IOAuth2AuthProvider)
                {
                    AuthProvider.SignRequest(ref req, _client);
                }
            }

            try
            {
                // Allow users to override the default Timeout setting in RestRequests
                var timeoutSetting = Utility.AppSettingMgr.GetConfigItem("Timeout");
                if (timeoutSetting != null)
                {
                    // Timeout App Settings is in seconds.
                    // RestRequest.Timeout is now timespan object for clarity from RestSharp 112.0.0
                    var userVal = Convert.ToInt32(timeoutSetting);
                    // Sanity check.
                    if (userVal >= 0 && userVal < 86400/*24 hours*/)
                    {
                        req.Timeout = TimeSpan.FromSeconds(userVal);
                    }
                }
            }
            catch
            {
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


