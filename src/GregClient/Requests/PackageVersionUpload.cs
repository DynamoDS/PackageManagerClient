using System;
using System.Collections.Generic;
using System.IO;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageVersionUpload : JsonRequest
    {

        public PackageVersionUpload(string name, string version, string description, IEnumerable<string> keywords,
                                                string contents, string engine, string engineVersion, string metadata, string group, 
                                                List<string> files, IEnumerable<PackageDependency> dependencies )
        {
            this.Files = files;
            this.RequestBody = new PackageVersionUploadRequestBody(name, version, description, keywords, contents, engine,
                                                    engineVersion, metadata, group, dependencies);
        }

        public PackageVersionUpload(string name, string version, string description, IEnumerable<string> keywords,
                                        string contents, string engine, string engineVersion, string metadata, string group, string zipFile,
                                        IEnumerable<PackageDependency> dependencies)
        {
            this.ZipFile = zipFile;
            this.RequestBody = new PackageVersionUploadRequestBody(name, version, description, keywords, contents, engine,
                                                    engineVersion, metadata, group, dependencies);

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

        public override void Build(ref RestRequest request)
        {
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

            var p = FileParameter.Create("pkg", bytes, ZipFile);
            request.Files.Add(p);
        }

    }

    public class PackageVersionUploadRequestBody : RequestBody
    {
        public PackageVersionUploadRequestBody(string name, string version, string description, 
                                    IEnumerable<string> keywords, string contents, string engine, string engineVersion, 
                                    string metadata, string group, IEnumerable<PackageDependency> dependencies)
        {
            this.name = name;
            this.version = version;
            this.description = description;
            this.dependencies = dependencies;
            this.keywords = keywords;
            this.contents = contents;
            this.engine = engine;
            this.group = group;
            this.engine_version = engineVersion;
            this.engine_metadata = metadata;
        }

        public string group { get; set; }
        public string name { get; set; }
        public string file_hash { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public IEnumerable<PackageDependency> dependencies { get; set; }
        public IEnumerable<string> keywords { get; set; }
        public string contents { get; set; }
        public string engine_version { get; set; }
        public string engine { get; set; }
        public string engine_metadata { get; set; }
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
