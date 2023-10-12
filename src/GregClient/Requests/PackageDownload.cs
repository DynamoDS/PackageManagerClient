using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Greg.Responses;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageDownload : Request
    {
        public PackageDownload(string id)
        {
            this._id = id;
        }

        public PackageDownload(string id, string version)
        {
            this._id = id;
            this._version = version;
        }

        public override string Path
        {
            get { return _version != null ? "download/" + this._id + "/" + _version : "download/" + this._id; }
        }

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        private readonly string _id;
        private readonly string _version;

        internal override void Build(ref RestRequest request)
        {

        }

        public static string GetFileFromResponse(Response gregResponse)
        {
            var response = gregResponse.InternalRestResponse;
           
            if ( !(response.ResponseUri != null && response.ResponseUri.AbsolutePath != null) ) return "";

            var tempOutput = System.IO.Path.Combine(FileUtilities.GetTempFolder(), System.IO.Path.GetFileName( response.ResponseUri.AbsolutePath ) );
            using (var f = new FileStream(tempOutput, FileMode.Create))
            {
                f.Write(response.RawBytes, 0, (int)response.ContentLength);
            }

            var md5HeaderResp = response.Headers.FirstOrDefault(x => x.Name == "ETag");
            if (md5HeaderResp == null) throw new Exception("Could not check integrity of package download!");

            var md5HeaderComputed =
                String.Join("", FileUtilities.GetMD5Checksum(tempOutput).Select(x => x.ToString("X"))).ToLower();

            if (md5HeaderResp.Value.ToString() == md5HeaderComputed )
                throw new Exception("Could not validate package integrity!  Please try again!");

            return tempOutput;

        }

    }
}
