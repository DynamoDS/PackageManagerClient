using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.IO;

namespace Greg.Requests
{
    public class PackageManagerRequest : Request
    {
        private Method _httpMethod;
        public readonly bool fileRequest;
        internal string fileToUpload;

        public PackageManagerRequest(string path, RestSharp.Method httpMethod, bool fileRequest = false, string fileToUpload = "") : base()
        {
            _path = path;
            _httpMethod = httpMethod;
            this.fileRequest = fileRequest;
            this.fileToUpload = fileToUpload;
        }

        public PackageManagerRequest(string path, RestSharp.Method httpMethod)
        {
            _path = path;
            _httpMethod = httpMethod;
        }

        private string _path;
        public override string Path
        {
            get
            {
                return _path;
            }
        }

        public override Method HttpMethod
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
