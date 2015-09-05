using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;

namespace RingCentral.NET40.Test
{
    [TestFixture]
    public class PubNubSubscriptionTest : TestConfiguration
    {
        
        private const string Channel = "RCNETSDK-TEST";

        [Test]
        public void SubsricptionPubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel, "", null,null,null);        
            Thread.Sleep(500);
            string subscribeResult = _subscriptionServiceMock.ReturnMessage("connectMessage").ToString();
            if (subscribeResult[1].Equals('{'))
            {
                
                Assert.IsTrue(subscribeResult.Contains(Channel));
                Assert.IsTrue(subscribeResult.Contains("\"status\":200"));
            }
            else
            {
                Assert.IsTrue(subscribeResult.Contains("\"Connected\""));
                Assert.IsTrue(subscribeResult.Contains("\"RCNETSDK-TEST\""));
            }

        }

        [Test]
        public void UnsubscribePubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel+"2", "", null, null, null);  
            Thread.Sleep(500);
           
            _subscriptionServiceMock.Unsubscribe(Channel+"2","",null,null,null,null);
            Thread.Sleep(500);
           Assert.IsTrue(_subscriptionServiceMock.ReturnMessage("disconnectMessage").ToString().Contains("Channel Unsubscribed from RCNETSDK-TEST2"));
           
        }

        [Test]
        public void ErrorMessagePubNubTest()
        {
            _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
            Thread.Sleep(500);
            _subscriptionServiceMock.Subscribe(Channel + "3", "", null, null, null);
            Thread.Sleep(500);
            Assert.IsTrue(_subscriptionServiceMock.ReturnMessage("errorMessage").ToString().Contains("Channel Already Subscribed. Duplicate channel subscription not allowed"));

        }

        [Test]
        public void SendMessagePubNubTest()
        { 
            Thread.Sleep(500);
            _subscriptionServiceMock.Subscribe(Channel,"",null,null,null);
           Thread.Sleep(500);
            _subscriptionServiceMock.PublishMessage("This is a test of the RingCentral C# SDK");
            Thread.Sleep(500);
            Assert.IsTrue(
                _subscriptionServiceMock.ReturnMessage("notification")
                    .ToString()
                    .Contains("This is a test of the RingCentral C# SDK"));
        }
 


    }
}