using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Greg;
using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using Greg.AuthProviders;
using RestSharp;

namespace GregClientSandbox
{
    internal class TestAuthrovider : IAuthProvider
    {

        public TestAuthrovider()
        {
        }

        public void SignRequest(ref RestRequest m, RestClient client)
        {
            string auth = "";
            foreach (var param in m.Parameters)
            {
                auth += param.Value as string;
            }
            m.Resource = m.Resource + "?" + auth; // append as string to url
        }

        public void Logout()
        {
        }

        public bool Login()
        {
            return true;
        }

        public LoginState LoginState
        {
            get { return LoginState.LoggedIn; }
        }

        public string Username
        {
            get { return "test"; }
        }

        public event Func<object, bool> RequestLogin;
        public event Action<LoginState> LoginStateChanged;
    }

    internal class TesteVersionUpload : JsonRequest
    {
        public TesteVersionUpload(PackageVersionUploadRequestBody requestBody, IEnumerable<string> files)
        {
            this.Files = files;
            this.RequestBody = requestBody;
        }

        public IEnumerable<string> Files { get; set; }
        public string ZipFile { get; set; }

        public override string Path
        {
            get { return "package"; }
        }

        public override Method HttpMethod
        {
            get { return Method.PUT; }
        }


        internal override void Build(ref RestRequest request)
        {
            // zip up and get hash for zip
            if (Files != null)
            {
                ZipFile = FileUtilities.Zip(Files);
                var fs = File.OpenRead(ZipFile);
                byte[] bytes = new byte[fs.Length];
                try
                {

                    fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                }
                finally
                {
                    fs.Close();
                }
                var p = FileParameter.Create("pkg", bytes, ZipFile);
                request.Files.Add(p);
            }
                
            // pass pkg_header as parameter
            request.AddParameter("pkg_header1", this.RequestBody.AsJson());
            request.AddParameter("pkg_header2", "something something", ParameterType.QueryString);
            request.AddParameter("pkg_header3", "something something", ParameterType.RequestBody);
        }

    }

    internal class Samples
    {
        /// <summary>
        /// A basic auth provider specifying the test user
        /// name and password that correspond to the test environment on Greg.
        /// </summary>
        private static BasicProvider provider = new BasicProvider("test", "e0jlZfJfKS");
        private static TestAuthrovider testProvider = new TestAuthrovider();

        /// <summary>
        /// A GregClient specifying the local host. The alternate IP provided is for 
        /// production Reach. 
        /// </summary>
        private static GregClient pmc = new GregClient(testProvider, "http://107.20.146.184/");
        //private static GregClient pmc = new GregClient(provider, "http://localhost:8080/");
        //private static GregClient pmc = new GregClient(provider, "http://dynamopackages.com/");

        private static string DownloadPackageByIdTest()
        {
            var nv = new PackageDownload("5225e7dde2f476ca05000057");
            var response = pmc.Execute(nv);
            var pathDl = PackageDownload.GetFileFromResponse(response);
            var output = FileUtilities.UnZip(pathDl);
            return pathDl;
        }

        private static void UploadDynamoPackageVersionTest()
        {
            var keywords = new List<string>() {"neat", "ok"};
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "", 
                            false, new List<String>());

            var files = new List<string>() {"../test/pedro.dyf", "../test/RootNode.dyf"};
            var request = new PackageVersionUpload(nv, files);
            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void UploadWithFilesAuthTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            null, "", "", false, new List<String>());

            var files = new List<string>() { "../test/pedro.dyf", "../test/RootNode.dyf" };
            var request = new TesteVersionUpload(nv, files);

            var restReq = pmc.BuildRestRequest(request);
  

            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void UploadNoFilesAuthTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            null, "", "", false, new List<String>());

            var request = new TesteVersionUpload(nv, null);

            var restReq = pmc.BuildRestRequest(request);
   
            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void UploadDynamoPackageVersionWithFilesTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group", 
                new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "", false, new List<String>() );

            var files = new List<string>() {"../../../../test/pedro.dyf", "../../../../test/RootNode.dyf"};

            var request = new PackageVersionUpload(nv, files);

            var rr = new RestRequest();
            request.Build(ref rr);

            //var response = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(request.RequestBody.AsJson());
        }

        private static void DownloadDynamoPackageByEngineAndNameTest()
        {
            var nv = new HeaderDownload("dynamo", "Third .NET Package");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            Console.WriteLine(pkgResponse.content); // the package
        }

        private static void DownloadDynamoPackageByIdTest()
        {
            var nv = new HeaderDownload("51eccaac7fa5b0146b000005");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            //var pkgResponse = pmc.Execute(nv);
            
            Console.WriteLine(pkgResponse.content.name); // the package
        }

        private static void DownloadAllPackagesTest()
        {
            var nv = HeaderCollectionDownload.All();
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }

        private static void DownloadByEngineTest()
        {
            var nv = HeaderCollectionDownload.ByEngine("dynamo");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }

        private static void SearchWithQueryTest()
        {
            var nv = new Search("*ython");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Console.WriteLine(pkgResponse.content);
        }
        
        private static void ValidateAuthTest()
        {
            var nv = new ValidateAuth();
            var pkgResponse = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(pkgResponse.message);
        }

        private static void GetWhitelistTest()
        {
            var nv = WhitelistHeaderCollectionDownload.All();
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);

            if (pkgResponse == null)
            {
                Console.WriteLine("There was an error with the whitelist request.");
                return;
            }

            if (pkgResponse.content == null)
            {
                Console.WriteLine("The package response content was null.");
                return;
            }

            Console.WriteLine(pkgResponse.message);

            var libs = pkgResponse.content.SelectMany(p => p.versions.Last().node_libraries).Distinct();
            foreach (var lib in libs)
            {
                Console.WriteLine(lib);
            }
        }
        
        static void Main(string[] args)
        {
            UploadWithFilesAuthTest();
            UploadNoFilesAuthTest();
            //DownloadPackageByIdTest();
            //DownloadAllPackagesTest();
            //GetWhitelistTest();
            //Console.Read();
        }
    }
}

