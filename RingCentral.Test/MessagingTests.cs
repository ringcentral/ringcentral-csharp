using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;

namespace RingCentral.Test
{
    [TestFixture]
    public class MessagingTests : TestConfiguration
    {
        [Test]
        public void SendSms()
        {
            const string smsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";
            const string toPhone = "***REMOVED***"; //cellphone number of Paul

            var smsHelper = new SmsHelper(toPhone, UserName, smsText);
            var jsonObject = JsonConvert.SerializeObject(smsHelper);

            var result = RingCentralClient.PostRequest(SmsEndPoint, jsonObject);

            JToken token = JObject.Parse(result);
            var messageStatus = (String)token.SelectToken("messageStatus");

            Assert.AreEqual(messageStatus, "Sent");

        }
    }
}
