using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ringcentral;

namespace ringcentral.test
{
    using NUnit.Framework;

    [TestFixture]
    class RingCentralTest
    {

        const string AppKey = "***REMOVED***";
        const string AppSecret = "***REMOVED***";
        const string ApiEndPoint = "https://platform.devtest.ringcentral.com";

        const string UserName = "***REMOVED***";
        const string Password = "***REMOVED***";
        const string Extension = "101";

        [Test]
        public void TestAuthentication()
        {
            var ringCentral = new RingCentral(AppKey, AppSecret, ApiEndPoint);
            var result = ringCentral.Authenticate(UserName, Password, Extension);

            JToken token = JObject.Parse(result);
            var accessToken = (String)token.SelectToken("access_token");
            var refreshToken = (String)token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);

        }

    }
}
