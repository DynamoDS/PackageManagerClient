using System;
using System.Collections.Generic;
using System.IO;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageVersionUpload : JsonRequest
    {
        public PackageVersionUpload(PackageVersionUploadRequestBody requestBody, IEnumerable<string> files )
        {
            this.Files = files;
            this.RequestBody = requestBody;
        }

        public PackageVersionUpload(PackageVersionUploadRequestBody requestBody, string zipFile )
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

        public override Method HttpMethod
        {
            get { return Method.PUT; }
        }

        internal override void Build(ref RestRequest request)
        {
            // zip up and get hash for zip
            if (Files != null)
                ZipFile = FileUtilities.Zip(Files);

            var zipInfo = new FileInfo(ZipFile);
            var zipHash = FileUtilities.GetFileHash(zipInfo);

            ((PackageVersionUploadRequestBody)this.RequestBody).file_hash = Convert.ToBase64String(zipHash);

            // pass pkg_header to the request body. Do not pass as param to header (some params can be large and thus exceed the header size limits on certain servers).
            request.AddObject(new { pkg_header = this.RequestBody.AsJson() });

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

            var p = FileParameter.Create("pkg", bytes, ZipFile);
            request.Files.Add(p);
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
}
