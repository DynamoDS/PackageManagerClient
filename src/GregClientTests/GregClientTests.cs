using Greg;
using Greg.Requests;
using Greg.Responses;
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

        //TODO move all the "tests" over from gregSandbox program.cs to this file.
    }
}