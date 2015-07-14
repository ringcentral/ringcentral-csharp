using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RingCentral.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class RingCentralTest
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
            var ringCentralClient = new RingCentralClient(AppKey, AppSecret, ApiEndPoint);
            var result = ringCentralClient.Authenticate(UserName, Password, Extension);
            
            Assert.NotNull(result);

            JToken token = JObject.Parse(result);
            var accessToken = (String)token.SelectToken("access_token");
            var refreshToken = (String) token.SelectToken("refresh_token");

            Assert.NotNull(accessToken);
            Assert.NotNull(refreshToken);

        }

    }
}
