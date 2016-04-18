using NUnit.Framework;
using RingCentral.Http;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class APIVersionsTest:BaseTest
    {
        [Test]
        public void BaseURL()
        {
            var request = new Request("/restapi/");
            var response = platform.Get(request);
            var uriString = (string)response.GetJson().SelectToken("apiVersions").First().SelectToken("uriString");
            Assert.AreEqual("v1.0", uriString);
        }
    }
}
