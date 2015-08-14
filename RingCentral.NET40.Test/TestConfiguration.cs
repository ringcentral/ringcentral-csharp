using NUnit.Framework;

namespace RingCentral.NET40.Test
{
    
    [TestFixture]
    public class TestConfiguration 
    {
        protected SubscriptionServiceMock _subscriptionServiceMock;

        [TestFixtureSetUp]
        public void SetUp()
        {

            _subscriptionServiceMock = new SubscriptionServiceMock("demo-36", "demo-36", "demo-36", "", false);
        }
    }
}