using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public override Method HttpMethod
        {
            get { return Method.GET; }
        }

        private readonly string _id;
        private readonly string _version;

        public override void Build(ref RestRequest request)
        {

        }

        public static string GetFileFromResponse(IRestResponse response)
        {
           
            if ( !(response.ResponseUri != null && response.ResponseUri.AbsolutePath != null) ) return "";

            var tempOutput = System.IO.Path.Combine(FileUtilities.GetTempFolder(), System.IO.Path.GetFileName( response.ResponseUri.AbsolutePath ) );
            using (var f = new FileStream(tempOutput, FileMode.Create))
            {
                f.Write(response.RawBytes, 0, (int)response.ContentLength);
            }
            return tempOutput;

        }

    }
}
