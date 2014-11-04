using System.Collections.Generic;

namespace Greg.Requests
{
    public class PackageVersionUploadRequestBody : RequestBody
    {
        internal PackageVersionUploadRequestBody()
        {
            
        }

        public PackageVersionUploadRequestBody(string name, string version, string description, 
            IEnumerable<string> keywords, string contents, string engine, string engineVersion, 
            string metadata, string group, IEnumerable<PackageDependency> dependencies,
            string siteUrl, string repositoryUrl, bool containsBinaries,
            IEnumerable<string> nodeLibraryNames )
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
        public string contents { get; set; }
        public string engine_version { get; set; }
        public string engine { get; set; }
        public string engine_metadata { get; set; }
        public string site_url { get; set; }
        public string repository_url { get; set; }
        public bool contains_binaries { get; set; }
        public IEnumerable<string> node_libraries { get; set; }
    }
}