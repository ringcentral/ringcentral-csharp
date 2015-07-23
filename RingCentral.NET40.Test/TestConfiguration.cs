using NUnit.Framework;
using RingCentral.Subscription;

namespace RingCentral.NET40.Test
{
    [TestFixture]
    public class TestConfiguration : RingCentral.Test.TestConfiguration
    {
        protected const string PublishKey = "";
        protected const string SubscribeKey = "";
        public SubscriptionServiceImplementation SubscriptionServiceImplementation;

        [TestFixtureSetUp]
        public void Setup()
        {
            SubscriptionServiceImplementation = new SubscriptionServiceImplementation(PublishKey, SubscribeKey);
        }
    }
}