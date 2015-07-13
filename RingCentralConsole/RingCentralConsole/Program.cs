using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

using ringcentral;

namespace RingCentralConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            

            const string appKey = "***REMOVED***";
            const string appSecret = "***REMOVED***";
            const string apiEndPoint = "https://platform.devtest.ringcentral.com";

            const string userName = "***REMOVED***";
            const string password = "***REMOVED***";
            const string extension = "101";

            var ringCentral = new RingCentral(appKey,appSecret,apiEndPoint);

            var result = ringCentral.Authenticate(userName, password, extension,"password");

            Debug.WriteLine(result);

            JToken token = JObject.Parse(result);

            var accessToken = (String)token.SelectToken("access_token");

            result = ringCentral.GetRequest("/restapi/v1.0/account/~");

            Debug.WriteLine(result);

            Debug.WriteLine("Access Token is: " + accessToken);

            Debug.WriteLine(ringCentral.Revoke());

            result = ringCentral.GetRequest("/restapi/v1.0/account/~");

            Debug.WriteLine(result);
        }
    }
}
