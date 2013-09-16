using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using Greg;
using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using Greg.AuthProviders;
using RestSharp;

namespace GregClientSandbox
{
    internal class Samples
    {
        private static BasicProvider provider;
        private static Client pmc = new Client(provider, "http://107.20.146.184/");

        //private static ResponseBody UploadDynamoPackageTest()
        //{
        //    var keywords = new List<string>() {"neat", "ok"};

        //    var nv = PackageUpload.MakeDynamoPackage("Align & IronPython2", "1.1.0", "description", keywords, "MIT",
        //                                             "contents", "0.1.0", "", "group", new List<string>() { @"C:\Users\boyerp\Desktop\align.PNG", @"C:\Users\boyerp\Desktop\IronPython.dll" }, new List<PackageDependency>());
        //    var response = pmc.ExecuteAndDeserialize(nv);
        //    return response;
        //}

        private static string DownloadPackageByIdTest()
        {
            var nv = new PackageDownload("51d092b8e434798d18000005", "1.1.0");
            var response = pmc.Execute(nv);
            var pathDl = PackageDownload.GetFileFromResponse(response);
            var output = FileUtilities.UnZip(pathDl);
            return pathDl;
        }

        private static void UploadDynamoPackageVersionTest()
        {
            var keywords = new List<string>() {"neat", "ok"};
            var nv = new PackageVersionUpload("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group", 
                new List<string>() { "../test/pedro.dyf", "../test/RootNode.dyf" }, 
                new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") });
            //var response = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(nv.RequestBody.AsJson());
        }

        private static void UploadDynamoPackageVersionWithFilesTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUpload("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group", 
                new List<string>() { "../../../../test/pedro.dyf", "../../../../test/RootNode.dyf" }, 
                new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") });

            var rr = new RestRequest();
            nv.Build(ref rr);

            //var response = pmc.ExecuteAndDeserialize(nv);
            Console.WriteLine(nv.RequestBody.AsJson());
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
        

        static void Main(string[] args)
        {

            //var authUri = provider.GetRequestToken();
            //Process.Start(authUri.ToString());
            //Console.WriteLine();
            //provider.GetAccessToken();

            DownloadByEngineTest();

            //ValidateAuthTest();
            //var nv = UploadDynamoPackageTest();
            //SearchWithQueryTest();

            //DownloadPackageByIdTest();
            
            ////var nv = UploadDynamoPackageVersionTest();
            ////DownloadDynamoPackageTest();
            ////DownloadAllPackagesTest();
            //ValidateAuthTest();

            //var path = FileUtilities.Zip(@"C:\Users\boyerp\Dropbox\Github\Autodesk\Dynamo\doc\distrib\nodes");

            Console.Read();


            //var resetEvent = new ManualResetEvent(false);

            //var requestTokenCallback = new Action<Uri, string>(delegate(Uri uri, string s)
            //    {
            //        Console.WriteLine("This is the request token: " + s);
            //        Console.WriteLine(uri.ToString());
            //        Process.Start(uri.ToString());
            //        resetEvent.Set();
            //    });

            //var handle = pmc.GetRequestTokenAsync(requestTokenCallback, Client.AuthorizationPageViewMode.Desktop);

            //resetEvent.WaitOne();
            //resetEvent.Reset(); // set a breakpoint here, awaiting user login

            //var accessTokenCallback = new Action<string>(delegate(string s)
            //{
            //    Console.WriteLine("This is the access token: " + s);
            //    resetEvent.Set();
            //});

            //handle = pmc.GetAccessTokenAsync(accessTokenCallback);

            //resetEvent.WaitOne();
            //resetEvent.Reset();

            //pmc.ExecuteAndDeserializeWithContentAsync<List<PackageHeader>>(HeaderCollectionDownload.All(), (response)
            //                                                                                               =>
            //    {
            //        Console.WriteLine(response.message);
            //        Console.WriteLine(response.content);
            //    });

            //resetEvent.Reset();

            //Console.ReadLine(); // to hold visual studio command line window open
        }
    }




}




//var authUri = pmc.GetRequestToken();
//Process.Start( authUri.ToString() );

//pmc.GetAccessToken();

////var nv = UploadDynamoPackageTest();
////var nv = UploadDynamoPackageVersionTest();
////DownloadDynamoPackageTest();
////DownloadAllPackagesTest();
//ValidateAuthTest();

//Console.Read();