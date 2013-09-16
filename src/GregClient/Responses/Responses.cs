using System;
using System.Collections.Generic;
using RestSharp;
using RestSharp.Deserializers;

namespace Greg.Responses
{

    public class Response
    {
        protected static JsonDeserializer jsonDeserializer = new JsonDeserializer();

        public Response(IRestResponse response)
        {
            RestReponse = response;
        }

        public ResponseBody Deserialize()
        {
            try
            {
                return jsonDeserializer.Deserialize<ResponseBody>(RestReponse);
            }
            catch
            {
                return null;
            }
        }

        protected IRestResponse RestReponse { get; set; }
    }

    public class ResponseWithContent<T> : Response
    {
        public ResponseWithContent(IRestResponse response) : base(response)
        {

        }

        public ResponseWithContentBody<T> DeserializeWithContent()
        {
            return jsonDeserializer.Deserialize<ResponseWithContentBody<T>>(RestReponse);
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
        public string version { get; set; }
        public List<Dependency> direct_dependency_ids { get; set; }
        public List<string> direct_dependency_versions { get; set; }
        public List<Dependency> full_dependency_ids { get; set; }
        public List<string> full_dependency_versions { get; set; }
        public string engine_version { get; set; }
        public string engine_metadata { get; set; }
        public string contents { get; set; }
        public string created { get; set; }
        public string url { get; set; }
    }

    public class User
    {
        public string username { get; set; }
        public string _id { get; set; }
    }

    public class Comment
    {
        public string text { get; set; }
        public string user { get; set; }
        public string created { get; set; }
    }

    public class PackageHeader
    {
        public string name { get; set; }
        public List<string> keywords { get; set; }
        public List<User> maintainers { get; set; }
        public string description { get; set; }
        public List<string> used_by { get; set; }
        public int votes { get; set; }
        public string license { get; set; }
        public string engine { get; set; }
        public int downloads { get; set; }
        public string group { get; set; }
        public Boolean deprecated { get; set; }
        public List<Comment> comments { get; set; }
        public List<PackageVersion> versions { get; set; }
        public string _id { get; set; }
    }

}