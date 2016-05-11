using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class CallLogTest : BaseTest
    {
        private const string CallLogEndPoint = "/restapi/v1.0/account/~";

        [Test]
        public void GetActiveCalls()
        {

            Request request = new Request(CallLogEndPoint + "/active-calls");
            ApiResponse result = sdk.Platform.Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetActiveCallsByExtension()
        {

            Request request = new Request(CallLogEndPoint + "/extension/~/active-calls");
            ApiResponse result = sdk.Platform.Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLog()
        {
            Request request = new Request(CallLogEndPoint + "/call-log/");
            ApiResponse result = sdk.Platform.Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtension()
        {

            Request request = new Request(CallLogEndPoint + "/extension/~/call-log");
            ApiResponse result = sdk.Platform.Get(request);
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtensionAndId()
        {
            Request request = new Request(CallLogEndPoint + "/extension/~/call-log/Abcdefg");
            ApiResponse response = sdk.Platform.Get(request);
            JToken token = response.GetJson();


            var id = (string)token.SelectToken("id");

            Assert.AreEqual(id, "Abcdefg");
        }

        [Test]
        public void GetCallLogById()
        {
            Request request = new Request(CallLogEndPoint + "/call-log/Abcdefgh");
            ApiResponse response = sdk.Platform.Get(request);
            JToken token = response.GetJson();

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("Abcdefgh", id);
        }
    }
}