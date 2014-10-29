using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Greg.Utility;
using RestSharp;

namespace Greg.Requests
{
    public class PackageUpload : JsonRequest
    {

        public PackageUpload(string name, string version, string description, IEnumerable<string> keywords, string license,
                                                string contents, string engine, string engineVersion, string metadata, string group, List<string> files,
                                                IEnumerable<PackageDependency> dependencies, string siteUrl, string repositoryUrl)
        {
            this.Files = files;
            this.RequestBody = new PackageUploadRequestBody(name, version, description, keywords, license, contents, engine,
                                                    engineVersion, metadata, group, dependencies, siteUrl, repositoryUrl);

        }

        public PackageUpload(string name, string version, string description, IEnumerable<string> keywords, string license,
                                        string contents, string engine, string engineVersion, string metadata, string group, string zipFile,
                                        IEnumerable<PackageDependency> dependencies, string siteUrl, string repositoryUrl)
        {
            this.ZipFile = zipFile;
            this.RequestBody = new PackageUploadRequestBody(name, version, description, keywords, license, contents, engine,
                                                    engineVersion, metadata, group, dependencies, siteUrl, repositoryUrl);

        }

        public IEnumerable<string> Files { get; set; }
        public string ZipFile { get; set; }

        public override string Path
        {
            get { return "package"; }
        }

        public override Method HttpMethod
        {
            get { return Method.POST; }
        }

        public override void Build(ref RestRequest request)
        {
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

            var p = FileParameter.Create("pkg", bytes, ZipFile);
            request.Files.Add(p);
        }

    }

    public class PackageUploadRequestBody : RequestBody
    {
        public PackageUploadRequestBody()
        {
        }

        public PackageUploadRequestBody(string name, string version, string description,
                                    IEnumerable<string> keywords, string license,
                                    string contents, string engine, string engineVersion,
                                    string metadata, string group, IEnumerable<PackageDependency> dependencies,
                                    string siteUrl, string repositoryUrl)
        {
            this.name = name;
            this.version = version;
            this.description = description;
            this.keywords = keywords;
            this.dependencies = dependencies;
            this.license = license;
            this.contents = contents;
            this.engine = engine;
            this.group = group;
            this.engine_version = engineVersion;
            this.engine_metadata = metadata;
            this.site_url = siteUrl;
            this.repository_url = repositoryUrl;
        }

        public string file_hash { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string group { get; set; }
        public IEnumerable<string> keywords { get; set; }
        public IEnumerable<PackageDependency> dependencies { get; set; }
        public string license { get; set; }
        public string contents { get; set; }
        public string engine_version { get; set; }
        public string engine_metadata { get; set; }
        public string engine { get; set; }
        public string site_url { get; set; }
        public string repository_url { get; set; }
    }
}
