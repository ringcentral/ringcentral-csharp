using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class PresenceTests : TestConfiguration
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";

        [Test]
        public void GetPresence()
        {
            Request request = new Request(PresenceEndPoint);
            Response response = RingCentralClient.GetPlatform().GetRequest(request);

            JToken token = response.GetJson();

            var presenceStatus = (string) token.SelectToken("presenceStatus");

            Assert.AreEqual(presenceStatus, "Available");
        }
    }
}