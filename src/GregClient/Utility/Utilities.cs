using RestSharp;

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Greg.Utility
{
    public static class AppSettingMgr
    {
        private static XmlDocument debugDoc;

        static AppSettingMgr()
        {
            string configPath = $"{typeof(AppSettingMgr).Assembly.Location}.config";

            if (!File.Exists(configPath)) return;

            try
            {
                debugDoc = new XmlDocument();
                debugDoc.Load(configPath);
            }
            catch(Exception ex) 
            {
                Console.WriteLine("The referenced configuration file, {0}, could not be loaded", configPath);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Returns null if the requested key was not found.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static object GetConfigItem(String key)
        {
            string value = getItem(key);
            if (value == null) {
                return null;
            }

            try
            {
                switch (key)
                {
                    case "EnableDebugLogs":
                        return bool.Parse(value);
                    case "Timeout":
                        return int.Parse(value);
                    default:
                        return null;
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reads the requested filed from the debug configuration document.
        /// Returns null if the requested key does not exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string getItem(String key)
        {
            if (debugDoc == null)
                return null;

            try
            {
                XmlNode node = debugDoc.SelectSingleNode("//appSettings");
                if (node != null)
                {
                    XmlElement value = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
                    if (value != null)
                    {
                        return value.Attributes["value"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The referenced configuration item, {0}, could not be retrieved", key);
                Console.WriteLine(ex.Message);
            }
            return null;
        }
#if NETFRAMEWORK
        [Obsolete]
        public static KeyValueConfigurationElement GetItem(String key)
        {
            return new KeyValueConfigurationElement(key, getItem(key));
        }
#endif
    }

    public static class DebugLogger
    {
        private static readonly bool enabled = false;

        // Static constructor is called at most one time, before any
        // instance constructor is invoked or member is accessed.
        static DebugLogger()
        {
            try
            {
                var enableDebugLogsSetting = AppSettingMgr.GetConfigItem("EnableDebugLogs");
                if (enableDebugLogsSetting != null && Convert.ToBoolean(enableDebugLogsSetting))
                {
                    enabled = true;
                }
            }
            catch
            {
            }
        }

        public static void LogResponse(IRestResponse restResp)
        {
            if (!enabled)
            {
                return;
            }

            try
            {
                var logDirPath = Path.Combine(Path.GetTempPath(), "DynamoClientLogs");
                if (!Directory.Exists(logDirPath))
                {
                    Directory.CreateDirectory(logDirPath);
                }
                string ts = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(logDirPath, "DynamoClientLog " + ts + ".txt")))
                {
                    var logRespObj = new
                    {
                        requestResource = restResp.Request.Resource,
                        respContent = restResp.Content,
                        statusCode = restResp.StatusCode,
                        statusDesc = restResp.StatusDescription,
                        headers = restResp.Headers,
                        responseStatus = restResp.ResponseStatus,
                        errMsg = restResp.ErrorMessage,
                        errException = restResp.ErrorException,
                        logtimeStamp = ts
                    };
                    outputFile.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(logRespObj));
                }
            }
            catch
            {
            }
        }
    }

    public static class UrlEncoding
    {
        /// <summary>
        /// The set of characters that are unreserved in RFC 2396 but are NOT unreserved in RFC 3986.
        /// </summary>
        /// <seealso cref="http://stackoverflow.com/questions/846487/how-to-get-uri-escapedatastring-to-comply-with-rfc-3986" />
        private static readonly string[] UriRfc3986CharsToEscape = new[] { "!", "*", "'", "(", ")" };

        private static readonly string[] UriRfc3968EscapedHex = new[] { "%21", "%2A", "%27", "%28", "%29" };

        public static string Relaxed(string value)
        {
            // Start with RFC 2396 escaping by calling the .NET method to do the work.
            // This MAY sometimes exhibit RFC 3986 behavior (according to the documentation).
            // If it does, the escaping we do that follows it will be a no-op since the
            // characters we search for to replace can't possibly exist in the string.
            StringBuilder escaped = new StringBuilder(Uri.EscapeDataString(value));

            // Upgrade the escaping to RFC 3986, if necessary.
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                string t = UriRfc3986CharsToEscape[i];
                escaped.Replace(t, UriRfc3968EscapedHex[i]);
            }

            // Return the fully-RFC3986-escaped string.
            return escaped.ToString();
        }
    }

    public class StringValueAttribute : System.Attribute
    {

        private string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public class StringEnum
    {
        private static Hashtable _stringValues = new Hashtable();
 
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...
            if (_stringValues.ContainsKey(value))
                output = (_stringValues[value] as StringValueAttribute).Value;
            else
            {
                //Look for our 'StringValueAttribute' 
                //in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                StringValueAttribute[] attrs =
                   fi.GetCustomAttributes(typeof(StringValueAttribute),
                                           false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    _stringValues.Add(value, attrs[0]);
                    output = attrs[0].Value;
                }
            }

            return output;
        }
    }

}
