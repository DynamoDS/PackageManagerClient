using System;
using System.Collections.Generic;

namespace Greg.Requests
{
    public class PackageUploadRequestBody : PackageVersionUploadRequestBody
    {
        //public parameterless constructor used for system.text.json deserialization.
        public PackageUploadRequestBody()
        {
        }

        public PackageUploadRequestBody(string name, string version, string description,
        IEnumerable<string> keywords, string license,
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
            this.keywords = keywords;
            this.dependencies = dependencies;
            this.contents = contents;
            this.engine = engine;
            this.group = group;
            this.engine_version = engineVersion;
            this.engine_metadata = metadata;
            this.site_url = siteUrl;
            this.repository_url = repositoryUrl;
            this.contains_binaries = containsBinaries;
            this.node_libraries = nodeLibraryNames;

            this.license = license;
        }




        public string license { get; set; }
    }
}