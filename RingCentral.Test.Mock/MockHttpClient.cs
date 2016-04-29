using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RingCentral.Test
{
    public partial class MockHttpClient : DelegatingHandler
    {
        private const string ServerUrl = SDK.SandboxServerUrl;

        private readonly Dictionary<HttpMethod, Dictionary<Uri, HttpResponseMessage>> mockResponses
            = new Dictionary<HttpMethod, Dictionary<Uri, HttpResponseMessage>> {
                { HttpMethod.Get, new Dictionary<Uri, HttpResponseMessage>() },
                { HttpMethod.Post, new Dictionary<Uri, HttpResponseMessage>() },
                { HttpMethod.Delete, new Dictionary<Uri, HttpResponseMessage>() },
                { HttpMethod.Put, new Dictionary<Uri, HttpResponseMessage>() },
            };

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (mockResponses.ContainsKey(request.Method) && mockResponses[request.Method].ContainsKey(request.RequestUri))
            {
                return TaskEx.FromResult(mockResponses[request.Method][request.RequestUri]);
            }
            else
            {
                return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
        }

        public MockHttpClient()
        {
            AddAuthenticationMockData();
            AddAccountAndExtensionResponses();
            AddAddressBookResponses();
            AddCallLogResponses();
            AddGeographicalDicitonaryResponses();
            AddMessagingResponses();
            AddPresenceResponses();
            AddRingOutResponses();
            AddSubscriptionResponses();
            AddResponseTestResponses();
        }

        public void AddAddressBookResponses()
        {
            string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";
            mockResponses[HttpMethod.Get].Add(
                   new Uri(ServerUrl + AddressBookEndPoint + "/1"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/123123\"," +
                       "\"availability\": \"Alive\"," + "\"id\": 123123 ," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," +
                       "\"businessAddress\": { " + "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")

                   });
            mockResponses[HttpMethod.Get].Add(
                 new Uri(ServerUrl + AddressBookEndPoint),
                 new HttpResponseMessage(HttpStatusCode.OK)
                 {
                     Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact?sortBy=FirstName\"," +
                      "\"records\": [ " + "{" + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/1\"," +
                          "\"availability\": \"Alive\"," + "\"id\": 1," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," + "\"businessAddress\": { " +
                            "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\", " + "\"zip\": \"94123\" " + "}" + "}," + "{" +
                          "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/2/extension/2/address-book/contact/2\"," + "\"availability\": \"Alive\"," +
                          "\"id\": 2," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                              "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"" + "}" + "}" +
                        "], \"paging\" : { \"page\": 1, \"totalPages\": 1, \"perPage\": 100, \"totalElements\": 2, \"pageStart\": 0, \"pageEnd\": 1 }, " +
                        "\"navigation\": {  \"firstPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" },  " +
                        "\"lastPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" } }," +
                        "\"groups\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/group\" } }"
                       , Encoding.UTF8, "application/json")
                 });
            mockResponses[HttpMethod.Delete].Add(
                   new Uri(ServerUrl + AddressBookEndPoint + "/3"),
                   new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"message\": \"Deleted\" }", Encoding.UTF8, "application/json") });
            mockResponses[HttpMethod.Post].Add(
                   new Uri(ServerUrl + AddressBookEndPoint),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/3\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 3 ," + "\"firstName\": \"Jim\"," + "\"lastName\": \"Johns\"," + "\"businessAddress\": { " + "\"street\": \"5 Marina Blvd\", " + "\"city\": \"San-Francisco\"," +
                       "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")
                   });
            mockResponses[HttpMethod.Put].Add(
                new Uri(ServerUrl + AddressBookEndPoint + "/5"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/5\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 5 ," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"}" + "} ", Encoding.UTF8, "application/json")
                   });

        }
        public void AddCallLogResponses()
        {
            string CallLogEndPoint = "/restapi/v1.0/account/~";
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + CallLogEndPoint + "/active-calls"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\" }}}"
                                               , Encoding.UTF8, "application/json")
               });
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + CallLogEndPoint + "/extension/~/active-calls"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\" }}}"
                                               , Encoding.UTF8, "application/json")
               });

            mockResponses[HttpMethod.Get].Add(
          new Uri(ServerUrl + CallLogEndPoint + "/call-log/"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/11/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
                "\"records\": [{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abc?view=Simple\",\"id\": \"Abc\"," +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\", \"to\": { \"phoneNumber\": \"+19999999999\"}, \"from\": {" +
                    "\"phoneNumber\": \"+19999999999\" }},{" +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abcd?view=Simple\",\"id\": \"Abcd\", " +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\",\"to\": {\"phoneNumber\": \"+19999999999\" },\"from\": {\"phoneNumber\": \"+19999999999\"}}]}", Encoding.UTF8, "application/json")
          });
            mockResponses[HttpMethod.Get].Add(
        new Uri(ServerUrl + CallLogEndPoint + "/extension/~/call-log"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
         "\"records\": [{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log/Abcdef?view=Simple\"," +
          "\"id\": \"Abcdef\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
          "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
           "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }," +
           "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\", \"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32,\"type\": \"Voice\"," +
           "\"direction\": \"Outbound\",\"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"," +
            "\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\" } }]}", Encoding.UTF8, "application/json")
        });
            mockResponses[HttpMethod.Get].Add(
        new Uri(ServerUrl + CallLogEndPoint + "/extension/~/call-log/Abcdefg"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
           "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
            "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }", Encoding.UTF8, "application/json")
        });
            mockResponses[HttpMethod.Get].Add(
            new Uri(ServerUrl + CallLogEndPoint + "/call-log/Abcdefgh"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abcdefg?view=Simple\"," +
               "\"id\": \"Abcdefgh\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
               "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
                "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }", Encoding.UTF8, "application/json")
            });
        }
        public void AddGeographicalDicitonaryResponses()
        {
            string DictionaryEndPoint = "/restapi/v1.0/dictionary";
            string CountryEndPoint = DictionaryEndPoint + "/country";
            string StateEndPoint = DictionaryEndPoint + "/state";
            string LocationEndPoint = DictionaryEndPoint + "/location";
            string TimeZoneEndPoint = DictionaryEndPoint + "/timezone";
            string LanguageEndPoint = DictionaryEndPoint + "/language";
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + CountryEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(
                      "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country?page=1&perPage=100\"," +
                        "\"records\": [{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/89\"," +
                        "\"id\": \"89\",\"name\": \"Afghanistan\",\"isoCode\": \"AF\",\"callingCode\": \"93\", \"emergencyCalling\": false," +
                        "\"numberSelling\": false},{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/3\"," +
                        "\"id\": \"3\",\"name\": \"Albania\",\"isoCode\": \"AL\",\"callingCode\": \"355\"," +
                        "\"emergencyCalling\": false,\"numberSelling\": false}]}", Encoding.UTF8, "application/json")
               });
            mockResponses[HttpMethod.Get].Add(
           new Uri(ServerUrl + CountryEndPoint + "/3"),
           new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(
                  "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/3\"," +
                    "\"id\": \"3\",\"name\": \"Albania\",\"isoCode\": \"AL\",\"callingCode\": \"355\"," +
                    "\"emergencyCalling\": false,\"numberSelling\": false}", Encoding.UTF8, "application/json")
           });
            mockResponses[HttpMethod.Get].Add(
           new Uri(ServerUrl + LanguageEndPoint),
           new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(
                 "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\"," +
                  "\"records\": [ {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language/1033\"," +
                  "\"id\": \"1033\",\"name\": \"English (United States)\",\"isoCode\": \"en\"," +
                  "\"localeCode\": \"en-US\",\"ui\": true,\"greeting\": true,\"formattingLocale\": true} ]," +
                  "\"paging\": { \"page\": 1,\"totalPages\": 1,\"perPage\": 100,\"totalElements\": 1,\"pageStart\": 0," +
                  "\"pageEnd\": 0},\"navigation\": {\"firstPage\": {" +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\"}," +
                  "\"lastPage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\"}}}",
                  Encoding.UTF8, "application/json")
           });
            mockResponses[HttpMethod.Get].Add(
          new Uri(ServerUrl + LanguageEndPoint + "/1033"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent(
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language/1033\"," +
                 "\"id\": \"1033\",\"name\": \"English (United States)\",\"isoCode\": \"en\"," +
                 "\"localeCode\": \"en-US\",\"ui\": true,\"greeting\": true,\"formattingLocale\": true}",
                 Encoding.UTF8, "application/json")
          });
            mockResponses[HttpMethod.Get].Add(
             new Uri(ServerUrl + LocationEndPoint + "?stateId=13"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(
                     "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/location?stateId=13&withNxx=true&orderBy=City&page=1&perPage=100\"," +
                     "\"records\": [{\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"268\",\"state\": {" +
                     "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\",\"id\": \"13\" } }," +
                     "{\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"312\",\"state\": {" +
                      "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                      "\"id\": \"13\"} }, {\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"331\",\"state\": {" +
                      "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                       "\"id\": \"13\" } } ] }", Encoding.UTF8, "application/json")
               });
            mockResponses[HttpMethod.Get].Add(
            new Uri(ServerUrl + StateEndPoint + "/13"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                  "{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                   "\"id\": \"13\",\"name\": \"Alaska\",\"isoCode\": \"AK\",\"country\": {" +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\",\"id\": \"1\" } }", Encoding.UTF8, "application/json")
            });
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + StateEndPoint + "?countryId=1&withPhoneNumbers=True&perPage=2"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(
                     "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state?countryId=1&withPhoneNumbers=true&page=1&perPage=2\"," +
                        "\"records\": [ {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/12\"," +
                         "\"id\": \"12\",\"name\": \"Alabama\",\"isoCode\": \"AL\",\"country\": {" +
                         "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\",\"id\": \"1\"}}," +
                         "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\",\"id\": \"13\"," +
                         "\"name\": \"Alaska\",\"isoCode\": \"AK\",\"country\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"," +
                          "\"id\": \"1\"}}]}", Encoding.UTF8, "application/json")
               });
            mockResponses[HttpMethod.Get].Add(
            new Uri(ServerUrl + TimeZoneEndPoint + "/1"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                  "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/1\"," +
                  "\"id\": \"1\",\"name\": \"GMT\",\"description\": \"Casablanca, Monrovia, Reykjavik\" } ", Encoding.UTF8, "application/json")
            });
            mockResponses[HttpMethod.Get].Add(
              new Uri(ServerUrl + TimeZoneEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent(
                    "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone?page=1&perPage=100\"," +
                    "\"records\": [" + "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/1\"," +
                    "\"id\": \"1\",\"name\": \"GMT\",\"description\": \"Casablanca, Monrovia, Reykjavik\" }," +
                    "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/2\"," +
                    "\"id\": \"2\",\"name\": \"WET\",\"description\": \"Dublin, Edinburgh, Lisbon, London\" } ]}", Encoding.UTF8, "application/json")
              });
        }
        public void AddMessagingResponses()
        {
            string SmsEndPoint = "/restapi/v1.0/account/~/extension/~/sms";
            string ExtensionMessageEndPoint = "/restapi/v1.0/account/~/extension/~/message-store";
            string FaxEndPoint = "/restapi/v1.0/account/~/extension/~/fax";
            string PagerEndPoint = "/restapi/v1.0/account/~/extension/~/company-pager";
            mockResponses[HttpMethod.Delete].Add(
             new Uri(ServerUrl + ExtensionMessageEndPoint + "/123123123"),
             new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            mockResponses[HttpMethod.Delete].Add(
              new Uri(ServerUrl + ExtensionMessageEndPoint + "/123"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            mockResponses[HttpMethod.Get].Add(
              new Uri(ServerUrl + ExtensionMessageEndPoint + "/1/content/1"),
              new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("This is a test from the the NUnit Test Suite of the RingCentral C# SDK") });
            mockResponses[HttpMethod.Get].Add(
              new Uri(ServerUrl + ExtensionMessageEndPoint + "/1,2,"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("boundary=Boundary_0 --Boundary_0 Content-Type: application/json {" +
                  "\"response\" : [ {\"href\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                  "\"status\" : 200, \"responseDescription\" : \"OK\" }, {" +
                   "\"href\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2\",\"status\" : 200," +
                   "\"responseDescription\" : \"OK\" } ] }" +
                  "--Boundary_1 Content-Type: application/json {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                   "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                   "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                   "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                   "\"attachments\": [{\"id\": 1," +
                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," +
                   "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                   "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                   "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1," +
                   "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\"" +
                   " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}" +
                  "--Boundary_2  Content-Type: application/json {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                   "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                   "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                   "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                   "\"attachments\": [{\"id\": 1," +
                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," +
                   "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                   "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                   "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1," +
                   "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\"" +
                   " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} --Boundary_0"
                   , Encoding.UTF8, "multipart/mixed")
              });
            mockResponses[HttpMethod.Get].Add(
                  new Uri(ServerUrl + ExtensionMessageEndPoint + "/2"),
                  new HttpResponseMessage(HttpStatusCode.OK)
                  {
                      Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2\"," +
                       "\"id\": 2,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                       "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                       "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                       "\"attachments\": [{\"id\": 2," +
                       "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/2/content/2\"," +
                       "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                       "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                       "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 2, \"conversationId\": 2," +
                       "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/2\",\"id\": \"2\"" +
                       " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8, "application/json"),

                  });
            mockResponses[HttpMethod.Post].Add(
             new Uri(ServerUrl + SmsEndPoint),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3\"," +
                  "\"id\": 3,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                  "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                  "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Unread\",\"priority\": \"Normal\"," +
                  "\"attachments\": [{\"id\": 3," +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3/content/3\"," +
                  "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                  "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                  "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 3, \"conversationId\": 3," +
                  "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/3\",\"id\": \"3\"" +
                  " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8, "application/json")
             });
            mockResponses[HttpMethod.Put].Add(
             new Uri(ServerUrl + ExtensionMessageEndPoint + "/3"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3\"," +
                  "\"id\": 3,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                  "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                  "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:58:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                  "\"attachments\": [{\"id\": 3," +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/3/content/3\"," +
                  "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                  "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                  "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 3, \"conversationId\": 3," +
                  "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/3\",\"id\": \"3\"" +
                  " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8, "application/json")
             });
            mockResponses[HttpMethod.Get].Add(
              new Uri(ServerUrl + ExtensionMessageEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              {

                  Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store?availability=Alive&dateFrom=2015-07-23T00:00:00.000Z&page=1&perPage=100\"," +
                   "\"records\": [ " +
                   "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                                    "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                                    "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                                    "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                                    "\"attachments\": [{\"id\": 1," +
                                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," +
                                    "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                                    "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                                    "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1," +
                                    "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\"" +
                                    " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}, " +
                       " {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1\"," +
                    "\"id\": 1,\"to\": [{ \"phoneNumber\": \"+19999999999\"}]," +
                    "\"from\": {\"phoneNumber\": \"+19999999999\",\"location\": \"South San Francisco, CA\"}," +
                    "\"type\": \"SMS\",\"creationTime\": \"2015-07-29T15:56:21.000Z\",\"readStatus\": \"Read\",\"priority\": \"Normal\"," +
                    "\"attachments\": [{\"id\": 1," +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/1/content/1\"," +
                    "\"type\": \"Text\",\"contentType\": \"text/plain\" }]," +
                    "\"direction\": \"Outbound\",\"availability\": \"Alive\",\"subject\": \"This is a test from the Debug Console for RingCentral\"," +
                    "\"messageStatus\": \"Sent\",\"smsSendingAttemptsCount\": 1, \"conversationId\": 1," +
                    "\"conversation\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/conversation/1035491849837189700\",\"id\": \"1\"" +
                    " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} ]}", Encoding.UTF8, "application/json"),

              });
            mockResponses[HttpMethod.Post].Add(
                new Uri(ServerUrl + FaxEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                     "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/5\", \"id\": 5, \"to\": [{\"phoneNumber\": \"19999999999\"}]," +
                     "\"type\": \"Fax\",\"creationTime\": \"2015-07-26T08:38:45.000Z\",\"readStatus\": \"Unread\",\"priority\": \"Normal\"," +
                     "\"attachments\": [{\"id\": 1, \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/5/content/1\"," +
                     "\"contentType\": \"image/tiff\"}],\"direction\": \"Outbound\",\"availability\": \"Alive\",\"messageStatus\": \"Queued\"," +
                     "\"faxResolution\": \"High\",\"faxPageCount\": 0,\"lastModifiedTime\": \"2013-07-26T08:38:45.000Z\"}", Encoding.UTF8, "application/json")
                });
            mockResponses[HttpMethod.Post].Add(
                new Uri(ServerUrl + PagerEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        "{\"uri\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1346632010/extension/1346632010/message-store/315458412010\"," +
                       "\"id\" : 8,\"to\" : [{\"extensionNumber\" : \"2\"},{\"extensionNumber\" : \"3\"} ]," +
                       "\"from\" : { \"extensionNumber\" : \"1\"},\"type\" : \"Pager\",\"creationTime\" : \"2015-07-26T16:03:04.000Z\"," +
                       "\"readStatus\" : \"Unread\",\"priority\" : \"Normal\",\"attachments\" : [ {\"id\" : 1," +
                       "\"uri\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/message-store/8/content/1\",\"contentType\" : \"text/plain\"} ]," +
                       "\"direction\" : \"Outbound\",\"availability\" : \"Alive\",\"subject\" : \"Hello!\",\"messageStatus\" : \"Sent\"," +
                       "\"conversationId\" : 1,\"lastModifiedTime\" : \"2015-07-26T16:03:04.000Z\"}", Encoding.UTF8, "application/json")
                });
        }
        public void AddPresenceResponses()
        {
            string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + PresenceEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/presence\"," +
                    "\"extension\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," +
                    "\"id\": 1,\"extensionNumber\": \"101\"}," +
                    "\"presenceStatus\": \"Available\",\"telephonyStatus\": \"NoCall\",\"userStatus\": \"Available\"," +
                    "\"dndStatus\": \"TakeAllCalls\",\"allowSeeMyPresence\": true,\"ringOnMonitoredCall\": false,\"pickUpCallsOnHold\": false}"
                    , Encoding.UTF8, "application/json")
               });
        }

        public void AddRingOutResponses()
        {
            string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";
            mockResponses[HttpMethod.Post].Add(
               new Uri(ServerUrl + RingOutEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\"," +
                    "\"id\": 255,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
               });
            mockResponses[HttpMethod.Get].Add(
            new Uri(ServerUrl + RingOutEndPoint + "/1"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\"," +
                 "\"id\": 1,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
            });

            mockResponses[HttpMethod.Delete].Add(
              new Uri(ServerUrl + RingOutEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
        }
        public void AddSubscriptionResponses()
        {
            string SubscriptionEndPoint = "/restapi/v1.0/subscription";
            mockResponses[HttpMethod.Post].Add(
                new Uri(ServerUrl + "/restapi/v1.0/subscription"),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        "{\r\n  \"id\" : \"1\",\r\n  \"creationTime\" : \"2015-09-17T23:59:39.150Z\",\r\n  \"status\" : \"Active\",\r\n  \"uri\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/7ffd447a-0d0c-47f0-a313-0d53851293da\",\r\n" +
                        "\"eventFilters\" : [ \"/restapi/v1.0/account/1/extension/1/message-store\", \"/restapi/v1.0/account/1/extension/1/presence\" ],\r\n  \"expirationTime\" : \"2015-09-18T00:14:39.150Z\",\r\n  \"expiresIn\" : 899,\r\n  \"deliveryMode\" :" +
                        "{\r\n    \"transportType\" : \"PubNub\",\r\n    \"encryption\" : true,\r\n    \"address\" : \"demo-36\",\r\n    \"subscriberKey\" : \"demo-36\",\r\n    \"secretKey\" : \"demo-36\",\r\n" +
                        "\"encryptionAlgorithm\" : \"AES\",\r\n    \"encryptionKey\" : \"6hTFP4B94ZNI+IvgxPLY7g==\"\r\n  }\r\n}", Encoding.UTF8, "application/json")
                }
                );
            mockResponses[HttpMethod.Get].Add(
             new Uri(ServerUrl + SubscriptionEndPoint + "/1"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent(
                        "{\r\n  \"id\" : \"1\",\r\n  \"creationTime\" : \"2015-09-17T23:59:39.150Z\",\r\n  \"status\" : \"Active\",\r\n  \"uri\" : \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/7ffd447a-0d0c-47f0-a313-0d53851293da\",\r\n" +
                        "\"eventFilters\" : [ \"/restapi/v1.0/account/1/extension/1/message-store\", \"/restapi/v1.0/account/1/extension/1/presence\" ],\r\n  \"expirationTime\" : \"2015-09-18T00:14:39.150Z\",\r\n  \"expiresIn\" : 899,\r\n  \"deliveryMode\" :" +
                        "{\r\n    \"transportType\" : \"PubNub\",\r\n    \"encryption\" : true,\r\n    \"address\" : \"demo-36\",\r\n    \"subscriberKey\" : \"demo-36\",\r\n    \"secretKey\" : \"demo-36\",\r\n" +
                        "\"encryptionAlgorithm\" : \"AES\",\r\n    \"encryptionKey\" : \"6hTFP4B94ZNI+IvgxPLY7g==\"\r\n  }\r\n}", Encoding.UTF8, "application/json")
             });
            mockResponses[HttpMethod.Delete].Add(
              new Uri(ServerUrl + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            mockResponses[HttpMethod.Put].Add(
              new Uri(ServerUrl + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                    "\"/restapi/v1.0/account/1/extension/1/presence\" ]," +
                    "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                    "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                    "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
              });
        }

        public void AddResponseTestResponses()
        {
            string AccountInformationEndPoint = "/restapi/v1.0/account/";
            string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";

            mockResponses[HttpMethod.Get].Add(new Uri(ServerUrl + AccountInformationEndPoint + "5"),
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"errorCode\": \"CMN-201\",\"message\": \"Service Temporary Unavailable\"," +
                                         "\"errors\": [{\"errorCode\": \"CMN-201\"," +
                                         "\"message\": \"Service Temporary Unavailable\" }]}", Encoding.UTF8, "application/json")
                });
            mockResponses[HttpMethod.Get].Add(
               new Uri(ServerUrl + AccountExtensionInformationEndPoint + "/6"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent(
                       "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/6\"," + "\"id\": 6," + "\"serviceInfo\": {" +
                                "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/6/service-info\"," + "\"brand\": {" +
                                  "\"id\": \"6\"," + "\"name\": \"RingCentral\"," + "\"homeCountry\": {" + "\"id\": \"6\"," +
                                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"} }," +
                                "\"servicePlan\": {" + "\"id\": \"6\"," + "\"name\": \"Sandbox Office 4 lines Enterprise Edition\"," + "\"edition\": \"Enterprise\"}," +
                                "\"billingPlan\": {" + "\"id\": \"8853\"," + "\"name\": \"Monthly-109.98-Sandbox 4 Line\"," + "\"durationUnit\": \"Month\"," +
                                  "\"duration\": 1, " + "\"type\": \"Regular\"} }," +
                              "\"operator\": { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 6," +
                                "\"extensionNumber\": \"101\" }," + "\"mainNumber\": \"19999999999\"," + "\"status\": \"Confirmed\"," + "\"setupWizardState\": \"Completed\"}", Encoding.UTF8, "text/plain")
               });
            mockResponses[HttpMethod.Get].Add(new Uri(ServerUrl + AccountExtensionInformationEndPoint + "/7"),
                new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{ \"error\": \"invalid_request\", \"error_description\": \"Unsupported grant type\" }", Encoding.UTF8, "application/json")
                });

        }
    }
}
