using Greg;
using Greg.Requests;
using Greg.Responses;
using Greg.Utility;
using RestSharp;
using System.Reflection;
using System.Text.Json;

namespace GregClientTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetPackageHeaderTest()
        {    
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            //meshtoolkit
            var req = new HeaderDownload("56273e18662bf0e908000278");
            var res = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(req);
            //assert that dependency objects were deserialzied correctly when they were json objects.
            Assert.That(res.content.used_by.All(x => x.name is not null));
            Assert.That(res.content.name == "MeshToolkit");
        }

        [Test]
        public void GetPackageVersionHeaderTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");

            var req = new HeaderVersionDownload("dynamo", "DynaShape", "0.8.0");
            var res = pmc.ExecuteAndDeserializeWithContent<PackageVersion>(req);
            //assert that dependency objects were deserialzied correctly when they were just strings, name should be null.
            Assert.That(res.content.full_dependency_ids.Count == 2);
            Assert.That(res.content.full_dependency_ids.All(x => x.name is null));
        }
        [Test]
        public void DownloadPackageByIdTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var nv = new PackageDownload("5225e7dde2f476ca05000057");
            var response = pmc.Execute(nv);
            var pathDl = PackageDownload.GetFileFromResponse(response);
            var output = FileUtilities.UnZip(pathDl);
            Assert.That(!string.IsNullOrEmpty(output));
            Assert.That(new DirectoryInfo(output).Exists);
        }

        [Test]
        public void UploadDynamoPackageVersionTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "",
                            false, new List<String>(), new List<String>(), "Dynamo Team", "2021");

            var files = new List<string>() { "../test/pedro.dyf", "../test/RootNode.dyf" };
            var request = new PackageVersionUpload(nv, files);
            Assert.That(request.RequestBody.AsJson().Equals("{\"file_hash\":null,\"name\":\"Third .NET Package\",\"version\":\"2.1.0\",\"description\":\"\",\"group\":\"group\",\"keywords\":[\"neat\",\"ok\"],\"dependencies\":[{\"name\":\"peter\",\"version\":\"0.1.0\"},{\"name\":\"stephen\",\"version\":\"0.1.0\"}],\"host_dependencies\":[],\"contents\":\"contents\",\"engine_version\":\"0.1.0\",\"engine\":\"dynamo\",\"engine_metadata\":\"metadata\",\"site_url\":\"\",\"repository_url\":\"\",\"contains_binaries\":false,\"node_libraries\":[],\"copyright_holder\":\"Dynamo Team\",\"copyright_year\":\"2021\"}"));
            Console.WriteLine(request.RequestBody.AsJson());
        }
        
        [Test]
        public void UploadDynamoPackageWithHostDependencyTest()
        {
            var keywords = new List<string>() { "Civil" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                            new List<PackageDependency>() { new PackageDependency("Ram", "0.1.0"), new PackageDependency("Ian", "0.1.0") }, "", "",
                            false, new List<String>(), new List<String>() { "Civil3D" }, "Dynamo Team", "2021");

            var files = new List<string>() { "../test/pedro.dyf", "../test/RootNode.dyf" };
            var request = new PackageVersionUpload(nv, files);
            Assert.That(request.RequestBody.AsJson().Equals("{\"file_hash\":null,\"name\":\"Third .NET Package\",\"version\":\"2.1.0\",\"description\":\"\",\"group\":\"group\",\"keywords\":[\"Civil\"],\"dependencies\":[{\"name\":\"Ram\",\"version\":\"0.1.0\"},{\"name\":\"Ian\",\"version\":\"0.1.0\"}],\"host_dependencies\":[\"Civil3D\"],\"contents\":\"contents\",\"engine_version\":\"0.1.0\",\"engine\":\"dynamo\",\"engine_metadata\":\"metadata\",\"site_url\":\"\",\"repository_url\":\"\",\"contains_binaries\":false,\"node_libraries\":[],\"copyright_holder\":\"Dynamo Team\",\"copyright_year\":\"2021\"}"));
            Console.WriteLine(request.RequestBody.AsJson());
        }
        
        [Test]
        public void UploadDynamoPackageVersionWithFilesTest()
        {
            var keywords = new List<string>() { "neat", "ok" };
            var nv = new PackageVersionUploadRequestBody("Third .NET Package", "2.1.0", "", keywords, "contents", "dynamo", "0.1.0", "metadata", "group",
                new List<PackageDependency>() { new PackageDependency("peter", "0.1.0"), new PackageDependency("stephen", "0.1.0") }, "", "", false, new List<String>(), new List<String>(), "", "");

            var files = new List<string>() {Assembly.GetExecutingAssembly().Location };

            var request = new PackageVersionUpload(nv, files);

            var rr = new RestRequest();
            request.Build(ref rr);
            Assert.That(request.Files.Count, Is.EqualTo(1));
            Console.WriteLine(request.RequestBody.AsJson());
        }

        [Test]
        public void DownloadDynamoPackageByEngineAndNameTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");

            var nv = new HeaderDownload("dynamo", "MeshToolkit");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            Assert.That(pkgResponse.content.name, Is.EqualTo("MeshToolkit"));
            Console.WriteLine(JsonSerializer.Serialize(pkgResponse.content)); // the package
        }

        [Test]
        public void DownloadAllPackagesTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var nv = HeaderCollectionDownload.All();
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Assert.That(pkgResponse.content.Count > 2000);
            Assert.That(pkgResponse.content.All(x => x.banned is false));
            Assert.That(pkgResponse.content.Any(x => x.num_versions is >1));
            //seems like package headers don't actually contain the host deps - that makes sense,each version is different.
            Assert.That(pkgResponse.content.All(x => x.host_dependencies is null));
            Assert.That(pkgResponse.content.Any(x => x.versions.Last().host_dependencies is not null));
            Assert.That(pkgResponse.content.Any(x => x.num_dependents is not 0));
            Assert.That(pkgResponse.content.Any(x=>x.site_url is not null));
            Assert.That(pkgResponse.content.Any(x => x.repository_url is not null));
        }
        [Test]
        public void DownloadByEngineTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var nv = HeaderCollectionDownload.ByEngine("dynamo");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<List<PackageHeader>>(nv);
            Assert.IsNotEmpty(pkgResponse.content);
        }

        [Test]
        public void DownloadDynamoPackageMaintainersByEngineAndNameTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var nv = new GetMaintainers("dynamo", "MeshToolkit");
            var pkgResponse = pmc.ExecuteAndDeserializeWithContent<PackageHeader>(nv);
            Assert.That(pkgResponse.content.maintainers.Count, Is.EqualTo(1));
        }
       
        [Test]

        public void ValidateAuthTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var nv = new ValidateAuth();
            //this will be null as the string Unauthorized is not valid json
            Assert.Null(pmc.ExecuteAndDeserialize(nv));
            Assert.That(pmc.Execute(nv).InternalRestResponse.Content, Is.EqualTo("Unauthorized"));
        }
        [Test]

        public void ListHostsTest()
        {
            GregClient pmc = new GregClient(null, "http://dynamopackages.com/");
            var hosts = new Hosts();
            var hostsResponse = pmc.ExecuteAndDeserializeWithContent<List<String>>(hosts);
            Console.WriteLine(JsonSerializer.Serialize(hostsResponse.content));
            Assert.That(hostsResponse.content.Count, Is.EqualTo(5));
        }
        
    }
}