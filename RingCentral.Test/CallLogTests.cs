using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class CallLogTests : TestConfiguration
    {
        private const string CallLogEndPoint = "/restapi/v1.0/account/~";

        [Test]
        public void GetActiveCalls()
        {

            Request request = new Request(CallLogEndPoint + "/active-calls");
            Response result = RingCentralClient.GetPlatform().Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetActiveCallsByExtension()
        {

            Request request = new Request(CallLogEndPoint + "/extension/~/active-calls");
            Response result = RingCentralClient.GetPlatform().Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLog()
        {
            Request request = new Request(CallLogEndPoint + "/call-log/");
            Response result = RingCentralClient.GetPlatform().Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtension()
        {

            Request request = new Request(CallLogEndPoint + "/extension/~/call-log");
            Response result = RingCentralClient.GetPlatform().Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtensionAndId()
        {
            Request request = new Request(CallLogEndPoint + "/extension/~/call-log/Abcdefg");
            Response response = RingCentralClient.GetPlatform().Get(request);
            JToken token = response.GetJson();


            var id = (string)token.SelectToken("id");

            Assert.AreEqual(id, "Abcdefg");
        }

        [Test]
        public void GetCallLogById()
        {
            Request request = new Request(CallLogEndPoint + "/call-log/Abcdefgh");
            Response response = RingCentralClient.GetPlatform().Get(request);
            JToken token = response.GetJson();

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("Abcdefgh", id);
        }
    }
}