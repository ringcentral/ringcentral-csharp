using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using RingCentral.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class AccountAndExtensionInformationTests : TestConfiguration
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/~";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";
       


        [Test]
        public void GetAccountExtensionInformation()
        {
            Response response = Platform.GetRequest(AccountExtensionInformationEndPoint);

            Assert.AreEqual(response.GetStatus(),200);
        }

        [Test]
        public void GetAccountInformation()
        {
            Response response = Platform.GetRequest(AccountInformationEndPoint);

            JToken token = response.GetJson();
            var mainNumber = (string) token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "19999999999");
        }

        [Test]
        public void GetExtensionInformation()
        {
            Response response = Platform.GetRequest(AccountExtensionInformationEndPoint + "/1");

            Assert.IsNotNull(response);
        }
    }
}