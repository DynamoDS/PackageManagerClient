using System;
using System.Collections.Generic;

namespace Greg.Requests
{
    public class PackageUploadRequestBody : PackageVersionUploadRequestBody
    {
        public PackageUploadRequestBody(string name, string version, string description,
            IEnumerable<string> keywords, string license,
            string contents, string engine, string engineVersion,
            string metadata, string group, IEnumerable<PackageDependency> dependencies,
            string siteUrl, string repositoryUrl, bool containsBinaries, 
            IEnumerable<string> nodeLibraryNames, IEnumerable<string> hostDependencies):

            this(name,version,description,
                keywords,license,
                contents,engine,engineVersion, 
                metadata,group,dependencies,
                siteUrl,repositoryUrl,containsBinaries,
                nodeLibraryNames)
        {
            this.host_dependencies = hostDependencies;
        }

        [Obsolete("This constructor will be removed in a future release of packageManagerClient.")]
        public PackageUploadRequestBody(string name, string version, string description,
           IEnumerable<string> keywords, string license,
           string contents, string engine, string engineVersion,
           string metadata, string group, IEnumerable<PackageDependency> dependencies,
           string siteUrl, string repositoryUrl, bool containsBinaries,
           IEnumerable<string> nodeLibraryNames)
        {
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