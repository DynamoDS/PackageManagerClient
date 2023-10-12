using System;
using System.Collections.Generic;

using RestSharp;
using System.IO;
using System.Net.Http;

namespace Greg.Requests
{
    public class PackageManagerRequest : Request
    {
        private HttpMethod _httpMethod;
        public readonly bool fileRequest;
        internal string fileToUpload;

        public PackageManagerRequest(string path, HttpMethod httpMethod, bool fileRequest = false, string fileToUpload = "") : base()
        {
            _path = path;
            _httpMethod = httpMethod;
            this.fileRequest = fileRequest;
            this.fileToUpload = fileToUpload;
        }

        private string _path;
        public override string Path
        {
            get
            {
                return _path;
            }
        }

        public override HttpMethod HttpMethod
        {
            get { return _httpMethod; }
        }

        internal override void Build(ref RestRequest request)
        {
            if (fileRequest && !string.IsNullOrEmpty(fileToUpload))
            {
                request.AddFile(new FileInfo(fileToUpload).Name, fileToUpload);
            }
        }
    }
}
