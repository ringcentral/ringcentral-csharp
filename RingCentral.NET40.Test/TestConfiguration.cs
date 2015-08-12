using System;
using System.Net;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using RingCentral.Subscription;
using RingCentral.Test;

namespace RingCentral.NET40.Test
{
    
    [TestFixture]
    public class TestConfiguration : RingCentral.Test.TestConfiguration
    {
        protected SubscriptionServiceMock _subscriptionServiceMock;

        [TestFixtureSetUp]
        public void SetUp()
        {

            _subscriptionServiceMock = new SubscriptionServiceMock("demo-36", "demo-36", "demo-36", "", false);
        }
    }
}