using System.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    public class PlatformTest : TestConfiguration
    {
        [Test]
        public void GetHttpCleint()
        {
            var httpClient = Platform.GetClient();
            Assert.IsNotNull(httpClient);
        }

        [Test]
        public void SetUserAgentHeader()
        {
            Platform.SetUserAgentHeader("Chrome/44.0.2403.125");
            var userAgentHeader = Platform.GetClient().DefaultRequestHeaders.GetValues("User-Agent").ToList();
            Assert.Contains("Chrome/44.0.2403.125", userAgentHeader);
        }

        [Test]
        public void SetXHttpOverRideHeader()
        {
            //TODO: can we mock response this
            const string xHttpOverRideHeader = "GET";

            var request = new Request("/restapi/v1.0/account/~");
            request.SetXhttpOverRideHeader(xHttpOverRideHeader);

            var requestHeader = request.GetXhttpOverRideHeader();

            Assert.AreEqual(requestHeader, xHttpOverRideHeader);
        }
    }
}