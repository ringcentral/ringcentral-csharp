using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
            Assert.Contains("GET", overrideHeader);
        }

        [Test]
        public void SetUserAgentHeader()
        {   
          
            Platform.SetUserAgentHeader("Chrome/44.0.2403.125");
            var userAgentHeader = Platform.GetClient().DefaultRequestHeaders.GetValues("User-Agent").ToList();
            Assert.Contains("Chrome/44.0.2403.125", userAgentHeader);
           
            
            

        }
    }



}
