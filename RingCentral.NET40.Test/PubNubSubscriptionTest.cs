using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Helper;
using RingCentral.Http;
using RingCentral.Subscription;


namespace RingCentral.NET40.Test
{
    [TestFixture]
    public class PubNubSubscriptionTest : TestConfiguration
    {
        
        private const string Channel = "RCNETSDK-TEST";

        [Test]
        public void SubsricptionPubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel, "", _subscriptionServiceMock.SetMessage,_subscriptionServiceMock.SubscribeConnectStatusMessage, _subscriptionServiceMock.ErrorMessage);        
            Thread.Sleep(1000);
            if (_subscriptionServiceMock.Message.Count == 3)
            {
                Assert.AreEqual("Connected", _subscriptionServiceMock.Message[1]);
                Assert.AreEqual(Channel,_subscriptionServiceMock.Message[2]);
            }
            else if (_subscriptionServiceMock.Message.Count == 2)
            {
                Assert.AreEqual(Channel, _subscriptionServiceMock.Message[1]);
                var success = _subscriptionServiceMock.Message[0].ToString().Split(',');
                Assert.AreEqual("{\r\n  \"status\": 200", success[0]);
            }
            else
            {
                Assert.Fail("Returned unrecognized result on PubNub Subscription");
            }
        
        }

        [Test]
        public void UnsubscribePubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel+"2", "", null, null, null);  
            Thread.Sleep(1000);
           
            _subscriptionServiceMock.Unsubscribe(Channel+"2","",null,null,null,null);
            Thread.Sleep(1000);
           
            Assert.AreEqual("Channel Unsubscribed from RCNETSDK-TEST2",_subscriptionServiceMock.Message[1]);
        }

        [Test]
        public void ErrorMessagePubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
            Thread.Sleep(1000);
            _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
            Thread.Sleep(1000);
            Assert.AreEqual("Channel Already Subscribed. Duplicate channel subscription not allowed",_subscriptionServiceMock.Error.Description);
            Assert.AreEqual(112,_subscriptionServiceMock.Error.StatusCode);

        }

        [Test]
        public void SendMessagePubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel + "4","",null,null,null);
            Thread.Sleep(1000);
            _subscriptionServiceMock.PublishMessage("This is a test of the RingCentral C# SDK");
            Thread.Sleep(1000);
            Assert.AreEqual("This is a test of the RingCentral C# SDK", _subscriptionServiceMock.Message[0]);

        }


    }
}