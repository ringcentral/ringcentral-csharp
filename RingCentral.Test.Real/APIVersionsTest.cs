using NUnit.Framework;
using RingCentral.Http;
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
            var response = sdk.Platform.Get(request);
            var uriString = (string)response.Json.SelectToken("apiVersions").First().SelectToken("uriString");
            Assert.AreEqual("v1.0", uriString);
        }
    }
}
