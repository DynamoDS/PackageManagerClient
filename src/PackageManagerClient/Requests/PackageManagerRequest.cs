using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManagerClient.Requests
{
    public class PackageManagerRequest: CefRequest
    {
        public PackageManagerRequest(string path, RestSharp.Method httpMethod, bool fileRequest = false, string fileToUpload = ""): base(fileRequest, fileToUpload)
        {
            _path = path;
            _httpMethod = httpMethod;
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

        private RestSharp.Method _httpMethod;
        public override RestSharp.Method HttpMethod
        {
            get { return _httpMethod; }
        }

        internal override void Build(ref RestRequest request)
        {
            if(fileRequest && !string.IsNullOrEmpty(fileToUpload))
            {
                request.AddFile(new FileInfo(fileToUpload).Name, fileToUpload);
            }
        }

    }
}
