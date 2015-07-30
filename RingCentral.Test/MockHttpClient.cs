using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RingCentral.Test
{
    public class MockHttpClient : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage>
        _GetMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _PostMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _DeleteMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        private readonly Dictionary<Uri, HttpResponseMessage>
        _PutMockResponses = new Dictionary<Uri, HttpResponseMessage>();
        protected const string ApiEndPoint = "https://platform.devtest.ringcentral.com";
        

        public void AddGetMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _GetMockResponses.Add(uri, responseMessage);
        }
        public void AddDeleteMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _DeleteMockResponses.Add(uri, responseMessage);
        }
        public void AddPostMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _PostMockResponses.Add(uri, responseMessage);
        }
        public void AddPutMockResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _PutMockResponses.Add(uri, responseMessage);
        }



        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if(request.Method.Equals(HttpMethod.Get)){
                if((_GetMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_GetMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if(request.Method.Equals(HttpMethod.Post)){
                 if((_PostMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_PostMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if (request.Method.Equals(HttpMethod.Delete))
            {
                if ((_DeleteMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_DeleteMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            if (request.Method.Equals(HttpMethod.Put))
            {
                if ((_PutMockResponses.ContainsKey(request.RequestUri))) return TaskEx.FromResult(_PutMockResponses[request.RequestUri]);
                else return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
            else
            {
                return TaskEx.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
            }
        }

        public MockHttpClient()
        {
            AddAccountAndExtensionResponses();
            AddAddressBookResponses();
            AddAuthenticationResponses();
            AddCallLogResponses();
            AddGeographicalDicitonaryResponses();
            AddMessagingResponses();
            AddPresenceResponses();
            AddRingOutResponses();
            AddSubscriptionResponses();
        }

        public void AddAccountAndExtensionResponses()
        {
            string AccountInformationEndPoint = "/restapi/v1.0/account/~";
            string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";
            AddGetMockResponse(
                new Uri(ApiEndPoint + AccountExtensionInformationEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1\"," + "\"id\": 1," + "\"serviceInfo\": {" +
                                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/service-info\"," + "\"brand\": {" +
                                   "\"id\": \"1\"," + "\"name\": \"RingCentral\"," + "\"homeCountry\": {" + "\"id\": \"1\"," +
                                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"} }," +
                                 "\"servicePlan\": {" + "\"id\": \"1\"," + "\"name\": \"Sandbox Office 4 lines Enterprise Edition\"," + "\"edition\": \"Enterprise\"}," +
                                 "\"billingPlan\": {" + "\"id\": \"8853\"," + "\"name\": \"Monthly-109.98-Sandbox 4 Line\"," + "\"durationUnit\": \"Month\"," +
                                   "\"duration\": 1, " + "\"type\": \"Regular\"} }," +
                               "\"operator\": { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 1," +
                                 "\"extensionNumber\": \"101\" }," + "\"mainNumber\": \"19999999999\"," + "\"status\": \"Confirmed\"," + "\"setupWizardState\": \"Completed\"}", Encoding.UTF8, "application/json")
                }

                );
            AddGetMockResponse(
              new Uri(ApiEndPoint + AccountInformationEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent(
                      "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1\"," + "\"id\": 1," + "\"serviceInfo\": {" +
                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/service-info\"," + "\"brand\": {" +
                                 "\"id\": \"1\"," + "\"name\": \"RingCentral\"," + "\"homeCountry\": {" + "\"id\": \"1\"," +
                                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"} }," +
                               "\"servicePlan\": {" + "\"id\": \"1\"," + "\"name\": \"Sandbox Office 4 lines Enterprise Edition\"," + "\"edition\": \"Enterprise\"}," +
                               "\"billingPlan\": {" + "\"id\": \"8853\"," + "\"name\": \"Monthly-109.98-Sandbox 4 Line\"," + "\"durationUnit\": \"Month\"," +
                                 "\"duration\": 1, " + "\"type\": \"Regular\"} }," +
                             "\"operator\": { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 1," +
                               "\"extensionNumber\": \"101\" }," + "\"mainNumber\": \"19999999999\"," + "\"status\": \"Confirmed\"," + "\"setupWizardState\": \"Completed\"}", Encoding.UTF8, "application/json")
              }

              );

            AddGetMockResponse(
             new Uri(ApiEndPoint + AccountExtensionInformationEndPoint + "/1"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension?page=1&perPage=100\"," +
                "\"records\": [ { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 1," +
                "\"extensionNumber\": \"102\"," + "\"contact\": {\"firstName\": \"Alice\",\"lastName\": \"Keys\",\"email\": \"alice.keys@example.com\"}," +
                "\"name\": \"Alice Keys\",\"type\": \"User\",\"status\": \"NotActivated\",\"permissions\": { \"admin\": {\"enabled\": false }," +
                "\"internationalCalling\": {\"enabled\": true }},\"profileImage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/profile-image\"}}" +
                "],\"paging\": {\"page\": 1,\"totalPages\": 1,\"perPage\": 100,\"totalElements\": 1, \"pageStart\": 0,\"pageEnd\": 0},\"navigation\": {\"firstPage\": {" +
                "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension?page=1&perPage=100\" },\"lastPage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension?page=1&perPage=100\"}}}"
                 , Encoding.UTF8, "application/json")
             });
        }
        public void AddAddressBookResponses()
        {
            string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";
            AddGetMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint + "/1"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/123123\"," +
                       "\"availability\": \"Alive\"," + "\"id\": 123123 ," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," +
                       "\"businessAddress\": { " + "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")

                   });
           AddGetMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint),
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
            AddDeleteMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint + "/3"),
                   new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"message\": \"Deleted\" }", Encoding.UTF8, "application/json") });
            AddPostMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/3\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 3 ," + "\"firstName\": \"Jim\"," + "\"lastName\": \"Johns\"," + "\"businessAddress\": { " + "\"street\": \"5 Marina Blvd\", " + "\"city\": \"San-Francisco\"," +
                       "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")
                   });
            AddPutMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint + "/5"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/5\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 5 ," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"}" + "} ", Encoding.UTF8, "application/json")
                   });

        }
        public void AddAuthenticationResponses()
        {
           string RefreshEndPoint = "/restapi/oauth/token";
            string VersionEndPoint = "/restapi";
            AddGetMockResponse(
                new Uri(ApiEndPoint + VersionEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{ \"apiVersions\": { \"uriString\": \"v1.0\" } }" ,Encoding.UTF8, "application/json")
                });
            AddPostMockResponse(
              new Uri(ApiEndPoint + RefreshEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent(
                      "{\"access_token\": \"abcdefg\",\"token_type\": \"bearer\",\"expires_in\": 3599, \"refresh_token\": \"gfedcba\",\"refresh_token_expires_in\": 604799," +
                      "\"scope\": \"EditCustomData EditAccounts ReadCallLog EditPresence SMS Faxes ReadPresence ReadAccounts Contacts EditExtensions InternalMessages EditMessages ReadCallRecording ReadMessages EditPaymentInfo EditCallLog NumberLookup Accounts RingOut ReadContacts\"," +
                      "\"owner_id\": \"1\" }", Encoding.UTF8, "application/json")
              });
        }
        public void AddCallLogResponses()
        {
            string CallLogEndPoint = "/restapi/v1.0/account/~";
            AddGetMockResponse(
               new Uri(ApiEndPoint + CallLogEndPoint + "/active-calls"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\" }}}"
                                               , Encoding.UTF8, "application/json")
               });
            AddGetMockResponse(
               new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/active-calls"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\" }}}"
                                               , Encoding.UTF8, "application/json")
               });

            AddGetMockResponse(
          new Uri(ApiEndPoint + CallLogEndPoint + "/call-log/"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/11/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
                "\"records\": [{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abc?view=Simple\",\"id\": \"Abc\"," +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\", \"to\": { \"phoneNumber\": \"+19999999999\"}, \"from\": {" +
                    "\"phoneNumber\": \"+19999999999\" }},{" +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/call-log/Abcd?view=Simple\",\"id\": \"Abcd\", " +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\",\"to\": {\"phoneNumber\": \"+19999999999\" },\"from\": {\"phoneNumber\": \"+19999999999\"}}]}", Encoding.UTF8, "application/json")
          });
            AddGetMockResponse(
        new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/call-log"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
         "\"records\": [{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension/130076004/call-log/Abcdef?view=Simple\"," +
          "\"id\": \"Abcdef\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
          "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
           "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }," +
           "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension/130076004/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\", \"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32,\"type\": \"Voice\"," +
           "\"direction\": \"Outbound\",\"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"," +
            "\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\" } }]}", Encoding.UTF8, "application/json")
        });
            AddGetMockResponse(
        new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/call-log/Abcdefg"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
           "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
            "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }", Encoding.UTF8, "application/json")
        });
            AddGetMockResponse(
            new Uri(ApiEndPoint + CallLogEndPoint + "/call-log/Abcdefgh"),
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
            AddGetMockResponse(
               new Uri(ApiEndPoint + CountryEndPoint),
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
            AddGetMockResponse(
           new Uri(ApiEndPoint + CountryEndPoint + "/3"),
           new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(
                  "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/3\"," +
                    "\"id\": \"3\",\"name\": \"Albania\",\"isoCode\": \"AL\",\"callingCode\": \"355\"," +
                    "\"emergencyCalling\": false,\"numberSelling\": false}", Encoding.UTF8, "application/json")
           });
            AddGetMockResponse(
           new Uri(ApiEndPoint + LanguageEndPoint),
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
            AddGetMockResponse(
          new Uri(ApiEndPoint + LanguageEndPoint + "/1033"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent(
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language/1033\"," +
                 "\"id\": \"1033\",\"name\": \"English (United States)\",\"isoCode\": \"en\"," +
                 "\"localeCode\": \"en-US\",\"ui\": true,\"greeting\": true,\"formattingLocale\": true}",
                 Encoding.UTF8, "application/json")
          });
            AddGetMockResponse(
             new Uri(ApiEndPoint + LocationEndPoint + "?stateId=13"),
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
            AddGetMockResponse(
            new Uri(ApiEndPoint + StateEndPoint + "/13"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                  "{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                   "\"id\": \"13\",\"name\": \"Alaska\",\"isoCode\": \"AK\",\"country\": {" +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\",\"id\": \"1\" } }", Encoding.UTF8, "application/json")
            });
            AddGetMockResponse(
               new Uri(ApiEndPoint + StateEndPoint + "?countryId=1&withPhoneNumbers=True&perPage=2"),
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
            AddGetMockResponse(
            new Uri(ApiEndPoint + TimeZoneEndPoint + "/1"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                  "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/1\"," +
                  "\"id\": \"1\",\"name\": \"GMT\",\"description\": \"Casablanca, Monrovia, Reykjavik\" } ", Encoding.UTF8, "application/json")
            });
            AddGetMockResponse(
              new Uri(ApiEndPoint + TimeZoneEndPoint),
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
            AddDeleteMockResponse(
             new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/123123123"),
             new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            AddDeleteMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/123"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/1/content/1"),
              new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("This is a test from the the NUnit Test Suite of the RingCentral C# SDK") });
            AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/1,2,"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("--Boundary_0 Content-Type: application/json {" +
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
                   " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} --Boundary_0")
              });
            AddGetMockResponse(
                  new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/2"),
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
                       " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"}", Encoding.UTF8, "application/json")
                  });
            AddPostMockResponse(
             new Uri(ApiEndPoint + SmsEndPoint),
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
            AddPutMockResponse(
             new Uri(ApiEndPoint + ExtensionMessageEndPoint + "/3"),
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
            AddGetMockResponse(
              new Uri(ApiEndPoint + ExtensionMessageEndPoint),
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
                    " },\"lastModifiedTime\": \"2015-07-29T15:56:21.583Z\"} ]}", Encoding.UTF8, "application/json")
              });
        }
        public void AddPresenceResponses()
        {
            string PresenceEndPoint = "/restapi/v1.0/account/~/extension/~/presence";
            AddGetMockResponse(
               new Uri(ApiEndPoint + PresenceEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/presence\"," +
                    "\"extension\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," +
                    "\"id\": 130076004,\"extensionNumber\": \"101\"}," +
                    "\"presenceStatus\": \"Available\",\"telephonyStatus\": \"NoCall\",\"userStatus\": \"Available\"," +
                    "\"dndStatus\": \"TakeAllCalls\",\"allowSeeMyPresence\": true,\"ringOnMonitoredCall\": false,\"pickUpCallsOnHold\": false}"
                    , Encoding.UTF8, "application/json")
               });
        }

        public void AddRingOutResponses()
        {
            string RingOutEndPoint = "/restapi/v1.0/account/~/extension/~/ringout";
            AddPostMockResponse(
               new Uri(ApiEndPoint + RingOutEndPoint),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\"," +
                    "\"id\": 255,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
               });
            AddGetMockResponse(
            new Uri(ApiEndPoint + RingOutEndPoint + "/1"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/ringout/1?#\"," +
                 "\"id\": 1,\"status\": {\"callStatus\": \"InProgress\",\"callerStatus\": \"InProgress\",\"calleeStatus\": \"InProgress\"}}", Encoding.UTF8, "application/json")
            });
            //TODO: Correct response once API explore is working
            AddDeleteMockResponse(
              new Uri(ApiEndPoint + RingOutEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
        }
        public void AddSubscriptionResponses()
        {
            string SubscriptionEndPoint = "/restapi/v1.0/subscription";
            AddPostMockResponse(
            new Uri(ApiEndPoint + SubscriptionEndPoint),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                  "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                  "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                  "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                  "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
            });
            AddGetMockResponse(
             new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                   "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                   "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                   "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                   "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
             });
            AddDeleteMockResponse(
              new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.NoContent) { Content = new StringContent("") });
            AddPutMockResponse(
              new Uri(ApiEndPoint + SubscriptionEndPoint + "/1"),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent("{\"id\": \"1\",\"creationTime\": \"2015-07-30T00:58:37.818Z\",\"status\": \"Active\"," +
                    "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/1\",\"eventFilters\": [ " +
                    "\"/restapi/v1.0/account/1/extension/130076004/message-store\", \"/restapi/v1.0/account/1/extension/130076004/presence\" ]," +
                    "\"expirationTime\": \"2015-07-30T01:13:37.818Z\",\"expiresIn\": 899,\"deliveryMode\": {" +
                    "\"transportType\": \"PubNub\",\"encryption\": true,\"address\": \"2\"," +
                    "\"subscriberKey\": \"2\",\"secretKey\": \"sec2\",\"encryptionAlgorithm\": \"AES\", \"encryptionKey\": \"1=\" }}", Encoding.UTF8, "application/json")
              });
        }        
    }
}
