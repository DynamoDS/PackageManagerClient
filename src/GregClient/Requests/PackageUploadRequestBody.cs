using System;
using System.Collections.Generic;

namespace Greg.Requests
{
    public class PackageUploadRequestBody : PackageVersionUploadRequestBody
    {

        //!!!! it is important to keep this in mind:
        //https://stackoverflow.com/questions/33107789/json-net-deserialization-constructor-vs-property-rules

        /// <summary>
        /// Default constructor - should only be used for deserialization with json.net.
        /// json.net will construct an empty object and fill it with properties with matching names.
        /// </summary>
        public PackageUploadRequestBody()
        {

        }

        public PackageUploadRequestBody(string name, string version, string description,
        IEnumerable<string> keywords, string license,
        string contents, string engine, string engineVersion,
        string metadata, string group, IEnumerable<PackageDependency> dependencies,
        string siteUrl, string repositoryUrl, bool containsBinaries,
        IEnumerable<string> nodeLibraryNames, IEnumerable<string> hostDependencies,
        string copyright_holder, string copyright_year) :

        this(name, version, description,
            keywords, license,
            contents, engine, engineVersion,
            metadata, group, dependencies,
            siteUrl, repositoryUrl, containsBinaries,
            nodeLibraryNames)
        {
            this.host_dependencies = hostDependencies;
            this.copyright_holder = copyright_holder;
            this.copyright_year = copyright_year;
        }

        [Obsolete("This constructor will be removed in a future release of packageManagerClient.")]
        public PackageUploadRequestBody(string name, string version, string description,
            IEnumerable<string> keywords, string license,
            string contents, string engine, string engineVersion,
            string metadata, string group, IEnumerable<PackageDependency> dependencies,
            string siteUrl, string repositoryUrl, bool containsBinaries, 
            IEnumerable<string> nodeLibraryNames, IEnumerable<string> hostDependencies) :

            this(name,version,description,
                keywords,license,
                contents,engine,engineVersion, 
                metadata,group,dependencies,
                siteUrl,repositoryUrl,containsBinaries,
                nodeLibraryNames)
        {
            this.host_dependencies = hostDependencies;
            this.copyright_holder = copyright_holder;
            this.copyright_year = copyright_year;
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