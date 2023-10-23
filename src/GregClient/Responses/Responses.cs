using System;
using System.Collections.Generic;
using RestSharp;
using Greg.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Greg.Responses
{

    public class Response
    {
        private static JsonSerializerOptions settings;

        internal Response(RestResponse response)
        {
            InternalRestResponse = response;
        }

        public static JsonSerializerOptions Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                    settings.Converters.Add(new DependencyConverter());
                }
                return settings;
            }
        }

        public ResponseBody Deserialize()
        {
            try
            {
                return JsonSerializer.Deserialize<ResponseBody>(InternalRestResponse.Content, Settings);
            }
            catch
            {
                return null;
            }
        }

        internal RestSharp.RestResponse InternalRestResponse { get; set; }
    }

    public class ResponseWithContent<T> : Response
    {
        //TODO does this need to be public? How to avoid leaking RestSharp types?
        public ResponseWithContent(RestSharp.RestResponse response) : base(response)
        {

        }

        public ResponseWithContentBody<T> DeserializeWithContent()
        {
            return JsonSerializer.Deserialize<ResponseWithContentBody<T>>(InternalRestResponse.Content, Settings);
        }
    }

    public class ResponseBody
    {
        public Boolean success { get; set; }
        public string message { get; set; }
    }

    public class ResponseWithContentBody<T>
    {
        public Boolean success { get; set; }
        public string message { get; set; }
        public T content { get; set; }
    }

    public class Dependency
    {
        public string name { get; set; }

        public string _id { get; set; }
    }

    public class PackageVersion
    {
        public string url_with_deps { get; set; }

        public string url { get; set; }

        public string contents { get; set; }

        public string engine_metadata { get; set; }

        public string engine_version { get; set; }

        public string created { get; set; }

        public List<string> full_dependency_versions { get; set; }

        public List<Dependency> full_dependency_ids { get; set; }

        public List<string> direct_dependency_versions { get; set; }

        public List<Dependency> direct_dependency_ids { get; set; }

        public IEnumerable<string> host_dependencies { get; set; }

        public string change_log { get; set; }

        public string version { get; set; }

        public bool contains_binaries { get; set; }

        public List<string> node_libraries { get; set; }

        public string name { get; set; }

        public string id { get; set; }

        public string copyright_holder { get; set; }

        public string copyright_year { get; set; }

        public string scan_status { get; set; }

        public string latest_version_update { get; set; }        
    }

    public class User
    {
        public string username { get; set; }

        public string _id { get; set; }
    }

    public class TermsOfUseStatus
    {
        public string user_id { get; set; }
        public Boolean accepted { get; set; }
    }

    public class Comment
    {
        public string text { get; set; }
        public string user { get; set; }
        public string created { get; set; }
    }

    public class PackageHeader
    {
        public string _id { get; set; }

        public string name { get; set; }

        public List<PackageVersion> versions { get; set; }

        public DateTime latest_version_update { get; set; }

        public int num_versions { get; set; }

        public List<Comment> comments { get; set; }

        public int num_comments { get; set; }

        public string latest_comment { get; set; }

        public int votes { get; set; }

        public int downloads { get; set; }

        public string repository_url { get; set; }

        public string site_url { get; set; }

        public bool banned { get; set; }

        public bool deprecated { get; set; }

        public string group { get; set; }

        public string engine { get; set; }

        public string license { get; set; }

        public List<Dependency> used_by { get; set; }

        public List<string> host_dependencies { get; set; }

        public int num_dependents { get; set; }

        public string description { get; set; }

        public List<User> maintainers { get; set; }

        public List<string> keywords { get; set; }

    }
    public class UserPackages
    {
        public string username { get; set; }

        public string _id { get; set; }

        public DateTime last_updated_package_date { get; set; }

        public PackageVersion last_updated_package { get; set; }

        public List<PackageVersion> maintains { get; set; }
    }
    /// <summary>
    /// Used to handle response from /user/votes route
    /// to fetch all packages the current user has upvoted
    /// </summary>
    public class UserVotes
    {
        public string username { get; set; }

        public string _id { get; set; }

        /// <summary>
        /// List of package ids that the current user has upvoted
        /// </summary>
        public List<string> has_upvoted { get; set; }
    }
}