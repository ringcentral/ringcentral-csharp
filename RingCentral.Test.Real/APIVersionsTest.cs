using Newtonsoft.Json;
using NUnit.Framework;
using RingCentral.Http;
using RingCentral.Model;
using System.Linq;

namespace RingCentral.Test
{
    [TestFixture]
    class APIVersionsTest : BaseTest
    {
        [Test]
        public void BaseURL()
        {
            var request = new Request("/restapi/");
            var response = platform.Get(request);
            var uriString = (string)response.GetJson().SelectToken("apiVersions").First().SelectToken("uriString");
            Assert.AreEqual("v1.0", uriString);
            var apiVersions = JsonConvert.DeserializeObject<APIVersions>(response.GetBody());
            Assert.AreEqual("v1.0", apiVersions.ApiVersions[0].UriString);
        }
    }
}
