using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class PresenceTest : BaseTest
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";

        [Test]
        public void GetPresence()
        {
            Request request = new Request(PresenceEndPoint);
            ApiResponse response = sdk.Platform.Get(request);

            JToken token = response.Json;

            var presenceStatus = (string)token.SelectToken("presenceStatus");

            Assert.AreEqual(presenceStatus, "Available");
        }
    }
}