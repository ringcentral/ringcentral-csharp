using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class AccountAndExtensionInformationTests : TestConfiguration
    {
        [Test]
        public void GetAccountInformation()
        {
            var result = RingCentralClient.GetRequest(AccountInformationEndPoint);

            JToken token = JObject.Parse(result);
            var mainNumber = (String)token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "***REMOVED***");

        }

        [Test]
        public void GetAccountExtensionInformation()
        {
            var result = RingCentralClient.GetRequest(AccountExtensionInformationEndPoint);

            Assert.IsNotNull(result);

        }

        [Test]
        public void GetExtensionInformation()
        {
            var result = RingCentralClient.GetRequest(AccountExtensionInformationEndPoint + "/~");

            Assert.IsNotNull(result);
        }
    }
}
