using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageUpload : JsonRequest
    {

        public PackageUpload(PackageUploadRequestBody requestBody, IEnumerable<string> files)
        {
            this.Files = files;
            this.RequestBody = requestBody;
        }

        public PackageUpload(PackageUploadRequestBody requestBody, string zipFile )
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
            get { return HttpMethod.Post; }
        }

        internal override void Build(ref RestRequest request)
        {
            InitReqParams(ref request);

            // zip up and get hash for zip
            if (Files != null)
                ZipFile = FileUtilities.Zip(Files);

            var zipInfo = new FileInfo(ZipFile);
            var zipHash = FileUtilities.GetFileHash(zipInfo);

            ((PackageUploadRequestBody)this.RequestBody).file_hash = Convert.ToBase64String(zipHash);

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
}
