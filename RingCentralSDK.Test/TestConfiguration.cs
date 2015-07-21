using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace RingCentralSDK.Test
{
    [TestFixture]
    public class TestConfiguration
    {
        protected const string AppKey = "***REMOVED***";
        protected const string AppSecret = "***REMOVED***";
        protected const string UserName = "***REMOVED***";
        protected const string Password = "***REMOVED***";
        protected const string Extension = "101";

        protected const string SmsNumber = "***REMOVED***"; // cellphone number of Paul
        //protected const string SmsNumber = "***REMOVED***"; //cellphone number of Nate

        protected const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        protected const string RevokeEndPoint = "/restapi/oauth/revoke";
        protected const string AuthenticateEndPoint = "/restapi/oauth/token";

        protected string AuthResult;

        public RingCentralClient RingCentralClient;

        [TestFixtureSetUp]
        public void SetUp()
        {
            RingCentralClient = new RingCentralClient(AppKey, AppSecret, ApiEndPoint);
            AuthResult = RingCentralClient.Authenticate(UserName, Password, Extension, AuthenticateEndPoint);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            RingCentralClient.Revoke(RevokeEndPoint);
            RingCentralClient = null;
            //Due to Request limitions a wait of 15 second is needed to sure not to exceed the maximum requst rate / minute
            Thread.Sleep(15000);
        }
    }
}
