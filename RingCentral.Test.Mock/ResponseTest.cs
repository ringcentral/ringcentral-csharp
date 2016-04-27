using NUnit.Framework;
using RingCentral.Http;
using System;

namespace RingCentral.Test
{
    public class ResponseTest : BaseTest
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";

        [Test, ExpectedException(typeof(Exception), ExpectedMessage = @"Response is not JSON")]
        public void GetResponseNonJson()
        {
            Request request = new Request(AccountExtensionInformationEndPoint + "/6");
            Response result = sdk.Platform.Get(request);
            var jsonResult = result.GetJson();

        }

        [Test]
        public void GetErrorGoodCheckStatus()
        {
            Request request = new Request(AccountInformationEndPoint);
            Response result = sdk.Platform.Get(request);
            Assert.IsNull(result.GetError());
        }
    }
}
