using NUnit.Framework;
using RingCentral.Http;
using System;

namespace RingCentral.Test
{
    public class ResponseTest : BaseTest
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";

        [Test]
        public void GetResponseNonJson()
        {
            var request = new Request(AccountExtensionInformationEndPoint + "/6");
            var result = sdk.Platform.Get(request);
            var jsonResult = result.Json;
            Assert.IsNull(jsonResult);
        }

        [Test]
        public void GetErrorGoodCheckStatus()
        {
            var request = new Request(AccountInformationEndPoint);
            var result = sdk.Platform.Get(request);
            Assert.IsNull(result.Error);
        }

        [Test, ExpectedException(typeof(ApiException), ExpectedMessage = @"Unsupported grant type")]
        public void ErrorResponse()
        {
            var request = new Request(AccountExtensionInformationEndPoint + "/7");
            var result = sdk.Platform.Get(request);
        }
    }
}
