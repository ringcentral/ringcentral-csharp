using NUnit.Framework;

namespace RingCentral.Test
{
    public class TestConfiguration
    {
        protected const string AppKey = "***REMOVED***";
        protected const string AppSecret = "***REMOVED***";
        protected const string UserName = "***REMOVED***";
        protected const string Password = "***REMOVED***";
        protected const string Extension = "101";

        protected const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        protected const string RevokeEndPoint = "/restapi/oauth/revoke";


        public RingCentralClient RingCentralClient;

        [TestFixtureSetUp]
        public void SetUp()
        {
            RingCentralClient = new RingCentralClient(AppKey, AppSecret, ApiEndPoint);
            RingCentralClient.Authenticate(UserName, Password, Extension);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            RingCentralClient.Revoke(RevokeEndPoint);
            RingCentralClient = null;
        }
    }
}