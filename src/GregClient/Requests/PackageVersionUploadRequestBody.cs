using System;
using System.Collections.Generic;

namespace Greg.Requests
{
    public class PackageVersionUploadRequestBody : RequestBody
    {

        //public parameterless constructor used for system.text.json deserialization.
        public PackageVersionUploadRequestBody()
        {

        }


        /// <summary>
        /// Constructor which can be used to set hostDependencies
        /// </summary>
        /// <param name="name">Package name</param>
        /// <param name="version">Package version</param>
        /// <param name="description">Package description</param>
        /// <param name="keywords">Package keywords for quick identification</param>
        /// <param name="contents">Package content description</param>
        /// <param name="engine">Package engine name, usually is set to Dynamo</param>
        /// <param name="engineVersion">Package engine version, usually is set to use Dynamo version</param>
        /// <param name="metadata"></param>
        /// <param name="group"></param>
        /// <param name="dependencies">Package dependencies</param>
        /// <param name="siteUrl"></param>
        /// <param name="repositoryUrl"></param>
        /// <param name="containsBinaries">boolean flag indicating if the package contains binaries</param>
        /// <param name="nodeLibraryNames"></param>
        /// <param name="hostDependencies"> external programs this package depends on.</param
        /// <param name="copyright_holder">Copyright Holder's name</param>
        /// <param name="copyright_year">Year the copyright was put into effect.</param>
        public PackageVersionUploadRequestBody(string name, string version, string description,
          IEnumerable<string> keywords,
          string contents, string engine, string engineVersion,
          string metadata, string group, IEnumerable<PackageDependency> dependencies,
          string siteUrl, string repositoryUrl, bool containsBinaries,
          IEnumerable<string> nodeLibraryNames, IEnumerable<string> hostDependencies,
          string copyright_holder, string copyright_year)

        {
            this.host_dependencies = hostDependencies;
            this.copyright_holder = copyright_holder;
            this.copyright_year = copyright_year;
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
            this.site_url = siteUrl;
            this.repository_url = repositoryUrl;
            this.contains_binaries = containsBinaries;
            this.node_libraries = nodeLibraryNames;
        }



        public string file_hash { get; set; }

        public string name { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string group { get; set; }
        public IEnumerable<string> keywords { get; set; }
        public IEnumerable<PackageDependency> dependencies { get; set; }
        public IEnumerable<string> host_dependencies { get; set; }
        public string contents { get; set; }
        public string engine_version { get; set; }
        public string engine { get; set; }
        public string engine_metadata { get; set; }
        public string site_url { get; set; }
        public string repository_url { get; set; }
        public bool contains_binaries { get; set; }
        public IEnumerable<string> node_libraries { get; set; }
        public string copyright_holder { get; set; }
        public string copyright_year { get; set; }
    }
}