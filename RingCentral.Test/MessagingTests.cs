using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class MessagingTests : TestConfiguration
    {
        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        private const string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store/";

        private readonly string[] _messageSentValues = {"Sent", "Queued"};

        //TODO: need to find a valid conversationId to delete to pass this test
        [Test]
        public void DeleteConversationById()
        {
            var smsHelper = new SmsHelper(ToPhone, UserName, SmsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.GetPlatform().SetJsonData(jsonObject);

            Response messageResult = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken messageToken = JObject.Parse(messageResult.GetBody());
            var conversationId = (string) messageToken.SelectToken("conversationId");

            Response result = RingCentralClient.GetPlatform().DeleteRequest(ExtensionMessageEndPoint + conversationId);
            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint + conversationId);
        }

        [Test]
        public void DeleteMessage()
        {
            var smsHelper = new SmsHelper(ToPhone, UserName, SmsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.GetPlatform().SetJsonData(jsonObject);

            Response messageResult = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken messageToken = JObject.Parse(messageResult.GetBody());
            var messageStore = (string) messageToken.SelectToken("id");
            Response result = RingCentralClient.GetPlatform().DeleteRequest(ExtensionMessageEndPoint + messageStore);

            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(response.GetBody());

            var availability = (string) token.SelectToken("availability");

            Assert.AreEqual("Deleted", availability);
        }


        [Test]
        public void GetAttachmentFromExtension()
        {
            const string messageId = "1152989004";
            const string contentId = "1152989004";
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint + messageId + "/content/" + contentId);

            Assert.AreEqual(response.GetBody(), expectedMessage);
        }


        [Test]
        public void GetBatchMessage()
        {
            var messages = new List<string> {"1180709004", "1180693004"};
            string batchMessages = messages.Aggregate("", (current, message) => current + (message + ","));

            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint + batchMessages);

            if (response.IsMultiPartResponse)
            {
                List<string> multiPartResult = response.GetMultiPartResponses();

                //We're interested in the response statuses and making sure the result was ok for each of the message id's sent.
                JToken responseStatuses = JObject.Parse(multiPartResult[0]);
                for (int i = 0; i < messages.Count; i++)
                {
                    var status = (string)responseStatuses.SelectToken("response")[i].SelectToken("status");
                    Assert.AreEqual(status, "200");
                }

                foreach (string res in multiPartResult.Skip(1))
                {
                    JToken token = JObject.Parse(res);
                    var id = (string)token.SelectToken("id");
                    Assert.IsNotNull(id);
                }
            }
        }

        [Test]
        public void GetMessagesFromExtension()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint);

            JToken token = JObject.Parse(response.GetBody());
            var phoneNumber = (string) token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual(phoneNumber.Replace("+", string.Empty), UserName);
        }

        [Test]
        public void GetSingleMessage()
        {
            var smsHelper = new SmsHelper(ToPhone, UserName, SmsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.GetPlatform().SetJsonData(jsonObject);

            Response messageResult = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken messageToken = JObject.Parse(messageResult.GetBody());
            var messageStore = (string) messageToken.SelectToken("id");
            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(response.GetBody());

            var id = (string) token.SelectToken("id");

            Assert.AreEqual(messageStore, id);
        }

        [Test]
        public void SendFax()
        {
            //TODO: unimplemented
        }

        [Test]
        public void SendPagerMessage()
        {
            //TODO unimplemented
        }

        [Test]
        public void SendSms()
        {
            var smsHelper = new SmsHelper(ToPhone, UserName, SmsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.GetPlatform().SetJsonData(jsonObject);

            Response result = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken token = JObject.Parse(result.GetBody());
            var messageStatus = (string) token.SelectToken("messageStatus");
            Assert.Contains(messageStatus, _messageSentValues);
        }

        [Test]
        public void UpdateMessage()
        {
            var smsHelper = new SmsHelper(ToPhone, UserName, SmsText);
            string jsonObject = JsonConvert.SerializeObject(smsHelper);

            RingCentralClient.GetPlatform().SetJsonData(jsonObject);

            Response messageResult = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken messageToken = JObject.Parse(messageResult.GetBody());
            var messageStore = (string) messageToken.SelectToken("id");

            RingCentralClient.GetPlatform().SetJsonData("{\"readStatus\": \"Read\"}");

            Response result = RingCentralClient.GetPlatform().PutRequest(ExtensionMessageEndPoint + messageStore);

            JToken token = JObject.Parse(result.GetBody());

            var availability = (string) token.SelectToken("availability");

            if (availability.Equals("Purged"))
            {
                Assert.AreEqual("Purged", availability);
            }
            else
            {
                var readStatus = (string) token.SelectToken("readStatus");
                Assert.AreEqual("Read", readStatus);
            }
        }
    }
}