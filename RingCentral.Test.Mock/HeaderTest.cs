using NUnit.Framework;
using RingCentral.Http;
using System.Linq;
using System.Net.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class HeaderTest : BaseTest
    {
        const string AccountInformationEndPoint = "/restapi/v1.0/account/";
        const string AddressBookContactEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact/3";

        [Test]
        public void GetHeaders()
        {
            Request request = new Request(AccountInformationEndPoint);
            ApiResponse response = sdk.Platform.Get(request);
            Assert.IsNotNull(response.Headers);
            Assert.AreEqual("application/json; charset=utf-8", response.Headers.ContentType.ToString());
        }

        [Test]
        public void GetUrlEncoded()
        {
            Request request = new Request(AccountInformationEndPoint);
            ApiResponse response = sdk.Platform.Get(request);
            Assert.IsFalse(response.IsUrlEncoded());
        }

        [Test]
        public void HttpMethodTunneling()
        {
            var request = new Request(AddressBookContactEndPoint);
            var response = sdk.Platform.Delete(request);
            Assert.AreEqual(HttpMethod.Delete, response.Request.Method);
            request.HttpMethodTunneling = true;
            response = sdk.Platform.Delete(request);
            Assert.AreEqual(HttpMethod.Post, response.Request.Method);
            Assert.AreEqual("DELETE", response.Request.Headers.GetValues("X-HTTP-Method-Override").FirstOrDefault());
        }
    }
}
