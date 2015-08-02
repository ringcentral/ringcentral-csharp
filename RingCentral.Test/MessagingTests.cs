﻿using System.Collections.Generic;
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
        private const string FaxEndPoint = "/restapi/v1.0/account/~/extension/~/fax";
        private const string PagerEndPoint = "/restapi/v1.0/account/~/extension/~/company-pager";

        private readonly string[] _messageSentValues = {"Sent", "Queued"};
        
        
      \
        [Test]
        public void DeleteConversationById()
        {

            Response result = RingCentralClient.GetPlatform().DeleteRequest(ExtensionMessageEndPoint + "/123123123");
            Assert.AreEqual(204, result.GetStatus());
           
        }

        [Test]
        public void DeleteMessage()
        {
          
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

            JToken token = response.GetJson();
            var phoneNumber = (string) token.SelectToken("records")[0].SelectToken("from").SelectToken("phoneNumber");

            Assert.AreEqual("+19999999999",phoneNumber);
        }

        [Test]
        public void GetSingleMessage()
        {
            
            Response response = RingCentralClient.GetPlatform().GetRequest(ExtensionMessageEndPoint +"/2");

            JToken token = response.GetJson();

            var id = (string) token.SelectToken("id");

            Assert.AreEqual("2",id);
        }

        [Test]
        public void SendFax()
        {
            Response result = RingCentralClient.GetPlatform().PostRequest(FaxEndPoint);
            JToken token = result.GetJson();
            var id = (int)token.SelectToken("id");
            Assert.AreEqual(5, id);
        }

        [Test]
        public void SendPagerMessage()
        {
            Response result = RingCentralClient.GetPlatform().PostRequest(PagerEndPoint);
            JToken token = result.GetJson();
            var id = (int)token.SelectToken("id");
            Assert.AreEqual(8, id);
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