using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greg.Requests;
using Greg.Responses;
using System.Net;
using System.IO;
using Greg.Utility;
using Newtonsoft.Json;

namespace Greg
{
    /// <summary>
    /// Package Manager REST Client for ACG API
    /// </summary>
    public class PackageManagerClient : IGregClient
    {
        private readonly RestClient _client;
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
            _client = new RestClient(packageManagerUrl);
            _fileClient = new RestClient(fileStorageUrl);
        }

        private IRestResponse ExecuteInternal(Request m)
        {
            var req = new RestRequest(m.Path, m.HttpMethod);

            m.Build(ref req);


            if (m.HttpMethod == Method.GET && !m.Path.Contains("members"))
                SignRequest(req);
            else
                AuthProvider.SignRequest(ref req, _client);

            if (((PackageManagerRequest)(m)).fileRequest)
                return _fileClient.Execute(req);
            else
                return _client.Execute(req);
        }

        public void SignRequest(RestRequest m)
        {
            m.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            m.AddHeader("X-AFC", "DY1ON1");
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
