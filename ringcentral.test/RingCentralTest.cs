using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ringcentral.helper;

namespace RingCentral.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class RingCentralTest
    {

        private const string AppKey = "***REMOVED***";
        private const string AppSecret = "***REMOVED***";
        private const string UserName = "***REMOVED***";
        private const string Password = "***REMOVED***";
        private const string Extension = "101";

        private const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        private const string RevokeEndPoint = "/restapi/oauth/revoke";
        private const string RefreshEndPoint = "/restapi/oauth/token";
        private const string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
        private const string AccountInformationEndPoint = "/restapi/v1.0/account/~";
        private const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";

        private RingCentralClient _ringCentralClient;

        [SetUp]
        public void SetUp()
        {
            _ringCentralClient = new RingCentralClient(AppKey, AppSecret, ApiEndPoint);
        }

        [TearDown]
        public void TearDown()
        {
            _ringCentralClient.Revoke(RevokeEndPoint);
            _ringCentralClient = null;
        }

        [Test]
        public void TestAuthentication()
        {
            var result = _ringCentralClient.Authenticate(UserName, Password, Extension);
            
            Assert.NotNull(result);

            JToken token = JObject.Parse(result);
            var accessToken = (String)token.SelectToken("access_token");
            var refreshToken = (String) token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);

            _ringCentralClient.Revoke(RevokeEndPoint);
        }

        [Test]
        public void TestRefresh()
        {
            var authenticateResult = _ringCentralClient.Authenticate(UserName, Password, Extension);

            Assert.NotNull(authenticateResult);

            JToken token = JObject.Parse(authenticateResult);
            var accessTokenBeforeRefresh = (String)token.SelectToken("access_token");
            var refreshTokenBeforeFresh = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenBeforeRefresh);
            Assert.NotNull(refreshTokenBeforeFresh);

            var refreshResult = _ringCentralClient.Refresh(RefreshEndPoint);

            Assert.NotNull(refreshResult);

            token = JObject.Parse(refreshResult);
            var accessTokenAfterRefresh = (String)token.SelectToken("access_token");
            var refreshTokenAfterFresh = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessTokenAfterRefresh);
            Assert.NotNull(refreshTokenAfterFresh);

            Assert.AreNotEqual(accessTokenBeforeRefresh,accessTokenAfterRefresh);
            Assert.AreNotEqual(refreshTokenBeforeFresh,refreshTokenAfterFresh);

            _ringCentralClient.Revoke(RevokeEndPoint);
        }

        [Test]
        public void SendSms()
        {
            _ringCentralClient.Authenticate(UserName, Password, Extension);

            const string smsText = "This is a test from the the NUnit Test Suite of the RingCentral C# SDK";
            const string toPhone = "***REMOVED***"; //cellphone number of Paul

            var smsHelper = new SMSHelper(toPhone, UserName, smsText);
            var jsonObject = JsonConvert.SerializeObject(smsHelper);

            var result = _ringCentralClient.PostRequest(SmsEndPoint, jsonObject);

            JToken token = JObject.Parse(result);
            var messageStatus = (String)token.SelectToken("messageStatus");

            Assert.AreEqual(messageStatus,"Sent");

            _ringCentralClient.Revoke(RevokeEndPoint);

        }

        [Test]
        public void GetAccountInformation()
        {
            _ringCentralClient.Authenticate(UserName, Password, Extension);

            var result = _ringCentralClient.GetRequest(AccountInformationEndPoint);

            JToken token = JObject.Parse(result);
            var mainNumber = (String)token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "***REMOVED***");

            _ringCentralClient.Revoke(RevokeEndPoint);

        }

        [Test]
        public void GetAccountExtensionInformation()
        {
            _ringCentralClient.Authenticate(UserName, Password, Extension);

            var result = _ringCentralClient.GetRequest(AccountExtensionInformationEndPoint);

            Assert.IsNotNull(result);

            _ringCentralClient.Revoke(RevokeEndPoint);
        }

        [Test]
        public void GetExtensionInformation()
        {
            _ringCentralClient.Authenticate(UserName, Password, Extension);

            var result = _ringCentralClient.GetRequest(AccountExtensionInformationEndPoint+"/~");

            Assert.IsNotNull(result);

            _ringCentralClient.Revoke(RevokeEndPoint);
        }
    }
}
