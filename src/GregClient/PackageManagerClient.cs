using System;
using System.IO;
using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Greg
{
    /// <summary>
    /// Package Manager REST Client for ACG API
    /// </summary>
    public class PackageManagerClient : IGregClient
    {
        private const string AFC_CODE = "DY1ON1";
        //Client for ACG REST API
        private readonly RestClient _client;
        //Client for file upload, the server address for file upload is different than ACG API
        private readonly RestClient _fileClient;

        public string BaseUrl { get { return _client.BaseUrl.ToString(); } }

        public readonly IAuthProvider _authProvider;
        public IAuthProvider AuthProvider
        {
            get { return _authProvider; }
        }

        public PackageManagerClient(IAuthProvider provider, string packageManagerUrl, string fileStorageUrl)
        {
            _authProvider = provider;
            _client = new RestClient(packageManagerUrl)
            {
                Authenticator = new RestSharp.Authenticators.NtlmAuthenticator()
            };
            _fileClient = new RestClient(fileStorageUrl);
        }

        /// <summary>
        /// Execute the REST request
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private IRestResponse ExecuteInternal(PackageManagerRequest m)
        {
            if (m == null)
                throw new Exception("Request object is not instance of type PackageManagerRequest.");

            var req = new RestRequest(m.Path, m.HttpMethod);

            m.Build(ref req);

            /// For GET request we need to pass AFC(Affiliation Code) in request header.
            /// For all other request and Member requests we need to pass additional X-Session in request header. 
            /// X-Session header should be added from AuthProvider.SignRequest (ACG Auth Provider which is part of ACG login implementation) method.
            /// If request is for upload file _flieClient is used, as URL for ACG API and File Upload server are different.
            if (m.HttpMethod == Method.GET && !m.Path.Contains("members"))
                SignRequest(req);
            else
                AuthProvider.SignRequest(ref req, _client);

            if (m.fileRequest)
                return _fileClient.Execute(req);
            else
                return _client.Execute(req);
        }

        /// <summary>
        /// Add request headers for GET requests. No need to go through AuthProvider
        /// </summary>
        /// <param name="m"></param>
        public void SignRequest(RestRequest m)
        {
            m.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            m.AddHeader("X-AFC", AFC_CODE);
        }

        /// <summary>
        /// Execute the REST request
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public Response Execute(Request m)
        {
            return new Response(ExecuteInternal((PackageManagerRequest)m));
        }

        /// <summary>
        /// Execute and deserialize the request
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public ResponseBody ExecuteAndDeserialize(Request m)
        {
            return Execute(m).Deserialize();
        }

        /// <summary>
        /// Execute the request and deserialize the content as dynamic type.
        /// </summary>
        /// <typeparam name="T">The Type of content</typeparam>
        /// <param name="m">The request.</param>
        /// <returns>A <see cref="ResponseWithContent{dynamic}"/> or null if there was an error
        /// in executing the message.</returns>
        public ResponseWithContentBody<dynamic> ExecuteAndDeserializeWithContent<dynamic>(Request m)
        {
            var response = this.Execute(m);

            return new ResponseWithContentBody<dynamic>()
            {
                message = response.InternalRestReponse.StatusDescription,
                content = JsonConvert.DeserializeObject<dynamic>(response.InternalRestReponse.Content),
                success = true
            };
        }

        /// <summary>
        /// Get the file stream from response and save locally.
        /// </summary>
        /// <param name="gregResponse"></param>
        /// <returns></returns>
        public string GetFileFromResponse(Response gregResponse)
        {
            var response = gregResponse.InternalRestReponse;

            if (!(response.ResponseUri != null && response.ResponseUri.AbsolutePath != null)) return "";

            var tempOutput = System.IO.Path.Combine(FileUtilities.GetTempFolder(), System.IO.Path.GetFileName(response.ResponseUri.AbsolutePath));
            using (var f = new FileStream(tempOutput, FileMode.Create))
            {
                f.Write(response.RawBytes, 0, (int)response.ContentLength);
            }

            //var md5HeaderResp = response.Headers.FirstOrDefault(x => x.Name == "ETag");
            //if (md5HeaderResp == null) throw new Exception("Could not check integrity of package download!");

            //var md5HeaderComputed =
            //    String.Join("", FileUtilities.GetMD5Checksum(tempOutput).Select(x => x.ToString("X"))).ToLower();

            //if (md5HeaderResp.Value.ToString() == md5HeaderComputed)
            //    throw new Exception("Could not validate package integrity!  Please try again!");

            return tempOutput;

        }
    }
}
