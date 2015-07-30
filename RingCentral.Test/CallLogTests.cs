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
    public class CallLogTests : TestConfiguration
    {
        private const string CallLogEndPoint = "/restapi/v1.0/account/~";

        [Test]
        public void GetActiveCalls()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/active-calls");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetActiveCallsByExtension()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/active-calls");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLog()
        {

            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/call-log/");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtension()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/call-log");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtensionAndId()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/call-log/Abcdefg");
            JToken token = JObject.Parse(response.GetBody());


            var id = (string)token.SelectToken("id");

            Assert.AreEqual(id, "Abcdefg");
        }

        [Test]
        public void GetCallLogById()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/call-log/Abcdefgh");
            JToken token = JObject.Parse(response.GetBody());

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("Abcdefgh", id);
        }
    }
}