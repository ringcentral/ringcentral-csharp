using System.Runtime.InteropServices;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;

namespace RingCentral.Test
{
    [TestFixture]
    public class TestConfiguration
    {
        protected const string AppKey = "***REMOVED***";
        protected const string AppSecret = "***REMOVED***";
        protected const string UserName = "***REMOVED***";
        protected const string Password = "***REMOVED***";
        protected const string Extension = "101";

        protected const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        protected const string RevokeEndPoint = "/restapi/oauth/revoke";

        protected const string AuthenticateEndPoint = "/restapi/oauth/token";

        protected const string smsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";

        protected const string toPhone = "***REMOVED***";
        
        protected string AuthResult;

        public RingCentralClient RingCentralClient;

        [TestFixtureSetUp]
        public void SetUp()
        {
            RingCentralClient = new RingCentralClient(AppKey, AppSecret, ApiEndPoint);
            AuthResult = RingCentralClient.Authenticate(UserName, Password, Extension, AuthenticateEndPoint);
        }

        [TestFixtureTearDown]
        public  void TearDown()
        {
            RingCentralClient.Revoke(RevokeEndPoint);
            RingCentralClient = null;
            //Due to Request limitions a wait of 25 second is needed to sure not to exceed the maximum requst rate / minute
            Thread.Sleep(25000);
        }


    }
}