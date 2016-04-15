using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store";
        private const string FaxEndPoint = "/restapi/v1.0/account/~/extension/~/fax";
        private const string PagerEndPoint = "/restapi/v1.0/account/~/extension/~/company-pager";

        private readonly string[] _messageSentValues = { "Sent", "Queued" };



        [Test]
        public void DeleteConversationById()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/123123123");
            Response result = RingCentralClient.GetPlatform().Delete(request);
            Assert.AreEqual(204, result.GetStatus());

        }

        [Test]
        public void DeleteMessage()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/123");
            Response result = RingCentralClient.GetPlatform().Delete(request);
            Assert.AreEqual(204, result.GetStatus());

        }


        [Test]
        public void GetAttachmentFromExtension()
        {
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            Request request = new Request(ExtensionMessageEndPoint + "/1/content/1");
            Response response = RingCentralClient.GetPlatform().Get(request);

            Assert.AreEqual(response.GetBody(), expectedMessage);
        }


        [Test]
        public void GetBatchMessage()
        {
            var messages = new List<string> { "1", "2" };
            string batchMessages = messages.Aggregate("", (current, message) => current + (message + ","));

            Request request = new Request(ExtensionMessageEndPoint + "/" + batchMessages);
            Response response = RingCentralClient.GetPlatform().Get(request);
            Assert.IsTrue(response.IsMultiPartResponse());
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

        [Test]
        public void GetMessagesFromExtension()
        {
            Request request = new Request(ExtensionMessageEndPoint);
            Response response = RingCentralClient.GetPlatform().Get(request);

            JToken token = response.GetJson();
            var phoneNumber = (string)token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual("+19999999999", phoneNumber);
        }

        [Test]
        public void GetSingleMessage()
        {
            Request request = new Request(ExtensionMessageEndPoint + "/2");
            Response response = RingCentralClient.GetPlatform().Get(request);

            JToken token = response.GetJson();

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("2", id);
        }


        [Test]
        public void SendFax()
        {
            const string text = "Hello world!";
            var json = "{\"to\":[{\"phoneNumber\":\"" + ToPhone + "\"}],\"faxResolution\":\"High\"}";

            var byteArrayText = Encoding.UTF8.GetBytes(text);

            var attachment = new Attachment("test.txt", "application/octet-stream", byteArrayText);
            var attachment2 = new Attachment("test2.txt", "text/plain", byteArrayText);

            var attachments = new List<Attachment> { attachment, attachment2 };

            Request request = new Request(FaxEndPoint, json, attachments);
            Response response = RingCentralClient.GetPlatform().Post(request);

            JToken token = response.GetJson();
            var availability = (string)token.SelectToken("availability");

            Assert.AreEqual("Alive", availability);
        }


        [Test]
        public void SendPagerMessage()
        {
            List<string> pagerToList = new List<string> {"102", "103"};
            PagerHelper pagerHelper = new PagerHelper(pagerToList,"101","Hello");
            var jsonData = "{\"to\": [{\"extensionNumber\": \""+ pagerHelper.to[0] + "\"}, {\"extensionNumber\": \"" + pagerHelper.to[1] + "\"}]," +
                            "\"from\": {\"extensionNumber\": \""+ pagerHelper.from +"\"},\"text\": \""+ pagerHelper.text +"\"}";
            Request request = new Request(PagerEndPoint, jsonData);
            Response result = RingCentralClient.GetPlatform().Post(request);
            JToken token = result.GetJson();
            var id = (int)token.SelectToken("id");
            Assert.AreEqual(8, id);
        }

        [Test]
        public void SendSms()
        {
            SmsHelper smsHelper= new SmsHelper("19999999999", "19999999999","Test SMS message");
            var jsonData = "{\"to\": [{\"phoneNumber\": \""  +smsHelper.to + "\"}]," +
                           "\"from\": {\"phoneNumber\": \"" + smsHelper.from + "\"}," +
                          "\"text\": \"" + smsHelper.text + "\" }";
            Request request = new Request(SmsEndPoint, jsonData);
            Response result = RingCentralClient.GetPlatform().Post(request);

            JToken token = JObject.Parse(result.GetBody());
            var messageStatus = (string)token.SelectToken("messageStatus");
            Assert.Contains(messageStatus, _messageSentValues);
        }


        [Test]
        public void UpdateMessage()
        {
            var jsonData = "{\"readStatus\": \"Read\"}";
            Request request = new Request(ExtensionMessageEndPoint + "/3", jsonData);
            Response result = RingCentralClient.GetPlatform().Put(request);
            JToken token = JObject.Parse(result.GetBody());
            var availability = (string)token.SelectToken("availability");
            var readStatus = (string)token.SelectToken("readStatus");
            Assert.AreEqual("Read", readStatus);

        }
    }
}