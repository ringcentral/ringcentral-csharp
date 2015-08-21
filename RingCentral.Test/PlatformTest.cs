using System.Linq;
using NUnit.Framework;
using RingCentral.SDK.Http;

namespace RingCentral.Test
{
    public class PlatformTest : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";
        [Test]
        public void GetHttpCleint()
        {
            var httpClient = Platform._client;
            Assert.IsNotNull(httpClient);
        }

        [Test]
        public void SetUserAgentHeader()
        {
            Platform.SetUserAgentHeader("Chrome/44.0.2403.125");
            var userAgentHeader = Platform._client.DefaultRequestHeaders.GetValues("User-Agent").ToList();
            Assert.Contains("Chrome/44.0.2403.125", userAgentHeader);
        }


        //[Test]
        //public void CheckPlatformXHttpOverRideHeader()
        //{
        //    string jsonData = "{\"firstName\": \"Vanessa\", " +
        //                       "\"lastName\": \"May\", " +
        //                       "\"businessAddress\": " +
        //                       "{ " +
        //                       "\"street\": \"5 Marina Blvd\", " +
        //                       "\"city\": \"San-Francisco\", " +
        //                       "\"state\": \"CA\", " +
        //                       "\"zip\": \"94123\"}" +
        //                       "}";

        //    Request request = new Request(AddressBookEndPoint, jsonData);
        //    request.SetXhttpOverRideHeader("POST");
        //    Assert.AreEqual("POST", request.GetXhttpOverRideHeader());
        //    Platform.Post(request);
        //    Assert.IsFalse(Platform.GetClient().DefaultRequestHeaders.Contains("X-HTTP-Method-Override"));


        //}
    }
}