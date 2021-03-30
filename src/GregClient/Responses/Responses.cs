using System;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using Greg.Converters;

namespace Greg.Responses
{

    public class Response
    {
        private static JsonSerializerSettings settings;

        internal Response(IRestResponse response)
        {
            InternalRestReponse = response;
        }

        public static JsonSerializerSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new JsonSerializerSettings
                    {
                        Error = (sender, args) =>
                        {
                            if (System.Diagnostics.Debugger.IsAttached)
                            {
                                System.Diagnostics.Debugger.Break();
                            }
                        },
                        Converters = new List<JsonConverter>()
                        {
                            new DependencyConverter()
                        }
                    };
                }
                return settings;
            }
        }

        public ResponseBody Deserialize()
        {
            try
            {
                //return jsonDeserializer.Deserialize<ResponseBody>(InternalRestReponse);
                return JsonConvert.DeserializeObject<ResponseBody>(InternalRestReponse.Content, Settings);
            }
            catch
            {
                return null;
            }
        }

        internal IRestResponse InternalRestReponse { get; set; }
    }

    public class ResponseWithContent<T> : Response
    {
        public ResponseWithContent(IRestResponse response) : base(response)
        {

        }

        public ResponseWithContentBody<T> DeserializeWithContent()
        {
            return JsonConvert.DeserializeObject<ResponseWithContentBody<T>>(InternalRestReponse.Content, Settings);
            //return jsonDeserializer.Deserialize<ResponseWithContentBody<T>>(InternalRestReponse);
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
}