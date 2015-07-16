using System.Diagnostics;
using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class PresenceTests : TestConfiguration
    {
        private const string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";


        //TODO: this isn't working in the online api tool so it doesn't assert properly here, seems to be permissions related
        //[Test]
        public void GetPresence()
        {
            string result = RingCentralClient.GetRequest(PresenceEndPoint);
            Debug.WriteLine(result);
        }
    }
}