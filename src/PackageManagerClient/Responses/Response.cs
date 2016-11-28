using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManagerClient
{
    public class PackageManagerResponse
    {
        internal PackageManagerResponse(IRestResponse response)
        {
            InternalRestReponse = response;
        }

        public PackageManagerResponseBody Deserialize()
        {
            try
            {
                //return jsonDeserializer.Deserialize<PackageManagerResponseBody>(InternalRestReponse);
                return JsonConvert.DeserializeObject<PackageManagerResponseBody>(InternalRestReponse.Content);
            }
            catch
            {
                return null;
            }
        }

        public IRestResponse InternalRestReponse { get; set; }
    }

    public class PackageManagerResponseBody
    {
        public Boolean success { get; set; }
        public string message { get; set; }
    }

    public class PMResponseWithContentBody
    {
        public Boolean success { get; set; }
        public string message { get; set; }
        public dynamic content { get; set; }
    }

    public class ResponseWithContent : PackageManagerResponse
    {
        public ResponseWithContent(IRestResponse response) : base(response)
        {

        }

        public PMResponseWithContentBody DeserializeWithContent()
        {
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) =>
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            };

            return new PMResponseWithContentBody()
            {
                message = InternalRestReponse.StatusDescription,
                content = JsonConvert.DeserializeObject<dynamic>(InternalRestReponse.Content),
                success = true
            };
        }
    }

}