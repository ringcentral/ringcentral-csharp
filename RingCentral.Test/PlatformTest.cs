using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

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
        public void SetXHttpOverRideHeader()
        {
            Platform.SetXhttpOverRideHeader("get");
            var overrideHeader = Platform.GetClient().DefaultRequestHeaders.GetValues("X-HTTP-Method-Override").ToList();
            Assert.Contains("GET",overrideHeader);
        }

    }


}
