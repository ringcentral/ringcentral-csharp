using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Core;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    public class ResponseTest : TestConfiguration
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/";
        [Test]
        public void GetErrorMessageTest()
        {

            Request request = new Request(AccountInformationEndPoint + "5");
            Response result = RingCentralClient.GetPlatform().GetRequest(request);
            var errorResult =  result.GetError();
            Assert.IsNotNull(errorResult);
        }
    }
}
