using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentralSDK.Test
{
    [TestFixture]
    public class AccountAndExtensionInformationTests : TestConfiguration
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/~";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";

        [Test]
        public void GetAccountExtensionInformation()
        {
            string result = RingCentralClient.GetRequest(AccountExtensionInformationEndPoint);

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetAccountInformation()
        {
            string result = RingCentralClient.GetRequest(AccountInformationEndPoint);

            JToken token = JObject.Parse(result);
            var mainNumber = (string)token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "***REMOVED***");
        }

        [Test]
        public void GetExtensionInformation()
        {
            string result = RingCentralClient.GetRequest(AccountExtensionInformationEndPoint + "/~");

            Assert.IsNotNull(result);
        }
    }
}
