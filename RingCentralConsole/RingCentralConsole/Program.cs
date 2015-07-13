using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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

            var result = ringCentral.Authenticate(userName, password, extension);
            Debug.WriteLine("Auth result is: " + result);

            //result = ringCentral.GetRequest("/restapi/v1.0/account/~");
            //Debug.WriteLine("Account information result is: " + result);

            //result = ringCentral.Refresh("/restapi/oauth/token");
            //Debug.WriteLine("Refresh account result: " + result);

            //result = ringCentral.GetRequest("/restapi/v1.0/account/~");
            //Debug.WriteLine("Account information result after refresh is: " + result);

            //result = ringCentral.DeleteRequest("/restapi/v1.0/account/~/extension/~/message-store/1152149004");
            //Debug.WriteLine("Delete Message request result: " + result);

            var fromPhoneNumber = userName;
            var smsText = "This is a test from the Debug Console for RingCentral";


            var sms = new SMS();
            var toPhone = "1***REMOVED***";

            var toNumbers = new List<String>();
            sms.To = toNumbers;
            sms.From = fromPhoneNumber;
            sms.Text = smsText;

            var jsonString = JsonConvert.SerializeObject(sms);
            result = ringCentral.PostRequest("/restapi/v1.0/account/~/extension/~/sms", null);
            Debug.WriteLine("SMS Result: " + result);

            //Debug.WriteLine(ringCentral.Revoke("/restapi/oauth/revoke"));

            //result = ringCentral.GetRequest("/restapi/v1.0/account/~");
            //Debug.WriteLine("This should fail due to account revoke" + result);
        }
    }
}
