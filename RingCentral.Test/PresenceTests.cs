using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class PresenceTests : TestConfiguration
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";

        [Test]
        public void GetPresence()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(PresenceEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var presenceStatus = (string) token.SelectToken("presenceStatus");

            Assert.AreEqual(presenceStatus, "Available");
        }
    }
}