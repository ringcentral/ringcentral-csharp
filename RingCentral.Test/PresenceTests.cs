using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class PresenceTests : TestConfiguration
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";

        [Test]
        public void GetPresence()
        {
            string result = RingCentralClient.GetRequest(PresenceEndPoint);

            JToken token = JObject.Parse(result);

            var presenceStatus = (string) token.SelectToken("presenceStatus");

            Assert.AreEqual(presenceStatus, "Available");
        }
    }
}