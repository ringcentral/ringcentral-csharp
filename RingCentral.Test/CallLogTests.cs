using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class CallLogTests : TestConfiguration
    {
        private const string CallLogEndPoint = "/restapi/v1.0/account/~";

        [Test]
        public void GetActiveCalls()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/active-calls");
            JToken token = JObject.Parse(result);

            var uri = (string) token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetActiveCallsByExtension()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/extension/~/active-calls");
            JToken token = JObject.Parse(result);

            var uri = (string) token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLog()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/call-log/");
            JToken token = JObject.Parse(result);

            var uri = (string) token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtension()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/extension/~/call-log");
            JToken token = JObject.Parse(result);

            var uri = (string) token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtensionAndId()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.
            const string callLogId = "AP6Xrj3McwzI79c";

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/extension/~/call-log/" + callLogId);
            JToken token = JObject.Parse(result);

            var id = (string) token.SelectToken("id");

            Assert.AreEqual(id, callLogId);
        }

        [Test]
        public void GetCallLogById()
        {
            //this ID needs to be a call log id that exists in your RingCentral account otherwise this will fail.
            const string callLogId = "AP6Xsmec3CVm7_U";

            string result = RingCentralClient.GetRequest(CallLogEndPoint + "/call-log/" + callLogId);
            JToken token = JObject.Parse(result);

            var id = (string) token.SelectToken("id");

            Assert.AreEqual(id, callLogId);
        }
    }
}