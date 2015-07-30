using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;
using RingCentral.Http;
using System;
using System.Net.Http;
using System.Net;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class MessagingTests : TestConfiguration
    {
        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        private const string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store";

        private readonly string[] _messageSentValues = {"Sent", "Queued"};
        [TestFixtureSetUp]
        public void SetUp()
        {
            mockResponseHandler.AddDeleteMockResponse(
               new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/123123123"),
               new HttpResponseMessage(HttpStatusCode.NoContent)
               { Content = new StringContent("")});
            mockResponseHandler.AddDeleteMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/123"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            mockResponseHandler.AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint +"/1/content/1"),
              new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("This is a test from the the NUnit Test Suite of the RingCentral C# SDK") });
            mockResponseHandler.AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/1,2,"),
              new HttpResponseMessage(HttpStatusCode.OK)
              { Content = new StringContent("--Boundary_0 Content-Type: application/json {" +
                "\"response\" : [ {\"href\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\","+
                "\"status\" : 200, \"responseDescription\" : \"OK\" }, {"+
                 "\"href\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2\",\"status\" : 200,"+
                 "\"responseDescription\" : \"OK\" } ] }" +
                "--Boundary_1 Content-Type: application/json {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                 "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                 "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                 "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\","+
                 "\"attachments\": [{\"id\": 1," +
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\","+
                 "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                 "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                 "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1,"+
                 "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\""+
                 " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}" +
                "--Boundary_2  Content-Type: application/json {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," + 
                 "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                 "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                 "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\","+
                 "\"attachments\": [{\"id\": 1,"+
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," + 
                 "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                 "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                 "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1,"+
                 "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\""+
                 " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} --Boundary_0") 
              });
                        mockResponseHandler.AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/2"),
              new HttpResponseMessage(HttpStatusCode.OK)
              { Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2\","+
                 "\"id\": 2,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                 "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                 "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\","+
                 "\"attachments\": [{\"id\": 2," +
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2/content/2\","+
                 "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                 "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                 "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 2, \"conversationId\": 2,"+
                 "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/2\",\"id\": \"2\""+
                 " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8,"application/json") 
              });
             mockResponseHandler.AddPostMockResponse(
              new Uri(ApiEndPoint + SmsEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              { Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3\","+
                 "\"id\": 3,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                 "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                 "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Unread\",\"priority\": \"Normal\","+
                 "\"attachments\": [{\"id\": 3," +
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3/content/3\","+
                 "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                 "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                 "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 3, \"conversationId\": 3,"+
                 "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/3\",\"id\": \"3\""+
                 " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8,"application/json") 
              });
             mockResponseHandler.AddPutMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint+ "/3"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3\"," +
                   "\"id\": 3,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                   "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                   "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                   "\"attachments\": [{\"id\": 3," +
                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3/content/3\"," +
                   "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                   "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                   "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 3, \"conversationId\": 3," +
                   "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/3\",\"id\": \"3\"" +
                   " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8, "application/json")
              });
             mockResponseHandler.AddGetMockResponse(
               new Uri(ApiEndPoint + ExtensionMessageEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store?availability=Alive&dateFrom=2015-07-23T00:00:00.000Z&page=1&perPage=100\","+
                    "\"records\": [ " +
                    "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\","+
                                     "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                                     "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                                     "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\","+
                                     "\"attachments\": [{\"id\": 1," +
                                     "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\","+
                                     "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                                     "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                                     "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1,"+
                                     "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\""+
                                     " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}, " + 
	                    " {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," + 
                     "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," + 
                     "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"},"+
                     "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\","+
                     "\"attachments\": [{\"id\": 1,"+
                     "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," +
                     "\"type\": \"Text\",\"contentType\": \"text/plain\" }],"+
                     "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\","+
                     "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1,"+
                     "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\""+
                     " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} ]}", Encoding.UTF8, "application/json")
               });

        }
        
        //TODO: need to find a valid conversationId to delete to pass this test
        [Test]
        public void DeleteConversationById()
        {

            Response result = RingCentralClient.GetPlatform().DeleteRequest(ExtensionMessageEndPoint + "/123123123");
            Assert.AreEqual(204, result.GetStatus());
           
        }

        [Test]
        public void DeleteMessage()
        {
          //TODO:API explorer not deleting correctly. Ensure following correct responses
            Response result = RingCentralClient.GetPlatform().DeleteRequest(ExtensionMessageEndPoint + "/123");
            Assert.AreEqual(204, result.GetStatus());
     
        }


        [Test]
        public void GetAttachmentFromExtension()
        {
            const string expectedMessage = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint +"/1/content/1" );

            Assert.AreEqual(response.GetBody(), expectedMessage);
        }


        [Test]
        public void GetBatchMessage()
        {
            var messages = new List<string> {"1", "2"};
            string batchMessages = messages.Aggregate("", (current, message) => current + (message + ","));

            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint +"/" + batchMessages);

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

            Assert.AreEqual("+19999999999",phoneNumber);
        }

        [Test]
        public void GetSingleMessage()
        {
            
            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint +"/2");

            JToken token = JObject.Parse(response.GetBody());

            var id = (string) token.SelectToken("id");

            Assert.AreEqual("2",id);
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

            Response result = RingCentralClient.GetPlatform().PostRequest(SmsEndPoint);

            JToken token = JObject.Parse(result.GetBody());
            var messageStatus = (string) token.SelectToken("messageStatus");
            Assert.Contains(messageStatus, _messageSentValues);
        }

        [Test]
        public void UpdateMessage()
        {
            

            Response result = RingCentralClient.GetPlatform().PutRequest(ExtensionMessageEndPoint +"/3");

            JToken token = JObject.Parse(result.GetBody());

            var availability = (string) token.SelectToken("availability");
            var readStatus = (string) token.SelectToken("readStatus");
            Assert.AreEqual("Read", readStatus);
            
        }
    }
}