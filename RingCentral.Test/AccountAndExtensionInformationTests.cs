using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class AccountAndExtensionInformationTests : TestConfiguration
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/1/extension";
       


        [Test]
        public void GetAccountExtensionInformation()
        {
            Request request = new Request(AccountExtensionInformationEndPoint);
            Response response = Platform.Get(request);

            Assert.AreEqual(response.GetStatus(),200);
        }

        [Test]
        public void GetAccountInformation()
        {
            Request request = new Request(AccountInformationEndPoint);
            Response response = Platform.Get(request);

            JToken token = response.GetJson();
            var mainNumber = (string) token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "19999999999");
        }

        [Test]
        public void GetExtensionInformation()
        {
            Request request = new Request(AccountExtensionInformationEndPoint + "/1");
            Response response = Platform.Get(request);

            Assert.IsNotNull(response);
        }
    }
}