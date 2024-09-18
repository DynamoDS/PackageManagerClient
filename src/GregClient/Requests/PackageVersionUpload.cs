using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageVersionUpload : JsonRequest
    {
        public PackageVersionUpload(PackageVersionUploadRequestBody requestBody, IEnumerable<string> files)
        {
            this.Files = files;
            this.RequestBody = requestBody;
        }

        public PackageVersionUpload(PackageVersionUploadRequestBody requestBody, string zipFile)
        {
            this.ZipFile = zipFile;
            this.RequestBody = requestBody;
        }

        public IEnumerable<string> Files { get; set; }
        public string ZipFile { get; set; }

        public override string Path
        {
            get { return "package"; }
        }

        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Put; }
        }

        internal override void Build(ref RestRequest request)
        {
            InitReqParams(ref request);

            // zip up and get hash for zip
            if (Files != null)
                ZipFile = FileUtilities.Zip(Files);

            var zipInfo = new FileInfo(ZipFile);
            var zipHash = FileUtilities.GetFileHash(zipInfo);

            ((PackageVersionUploadRequestBody)this.RequestBody).file_hash = Convert.ToBase64String(zipHash);

            // pass pkg_header as parameter
            request.AddParameter("pkg_header", this.RequestBody.AsJson());

            // add files
            var fs = File.OpenRead(ZipFile);
            byte[] bytes = new byte[fs.Length];
            try
            {

                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
            }
            finally
            {
                fs.Close();
            }

            request.AddFile("pkg", bytes, ZipFile);
        }

    }

    public class PackageDependency
    {
        public PackageDependency(string name, string version)
        {
            this.name = name;
            this.version = version;
        }
        public string name { get; set; }
        public string version { get; set; }
    }
    public class PackageCompatibility
    {
        public PackageCompatibility(string name, List<string> versions, string min, string max)
        {
            this.name = name;
            this.versions = versions;
            this.min = min;
            this.max = max;
        }
        public string name { get; set; }
        public List<string> versions { get; set; }
        public string min { get; set; }
        public string max { get; set; }
    }
}
