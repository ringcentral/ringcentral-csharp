using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class GeographicalDictionaryTests : TestConfiguration
    {
        private const string DictionaryEndPoint = "/restapi/v1.0/dictionary";
        private const string CountryEndPoint = DictionaryEndPoint + "/country";
        private const string StateEndPoint = DictionaryEndPoint + "/state";
        private const string LocationEndPoint = DictionaryEndPoint + "/location";
        private const string TimeZoneEndPoint = DictionaryEndPoint + "/timezone";
        private const string LanguageEndPoint = DictionaryEndPoint + "/language";
        [TestFixtureSetUp]
        public void SetUp()
        {
            mockResponseHandler.AddGetMockResponse(
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
            mockResponseHandler.AddGetMockResponse(
           new Uri(ApiEndPoint + CountryEndPoint + "/3"),
           new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(
                  "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/3\"," +
                    "\"id\": \"3\",\"name\": \"Albania\",\"isoCode\": \"AL\",\"callingCode\": \"355\"," +
                    "\"emergencyCalling\": false,\"numberSelling\": false}", Encoding.UTF8, "application/json")
           });
            mockResponseHandler.AddGetMockResponse(
           new Uri(ApiEndPoint + LanguageEndPoint),
           new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(
                 "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\","+
                  "\"records\": [ {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language/1033\","+
                  "\"id\": \"1033\",\"name\": \"English (United States)\",\"isoCode\": \"en\","+
                  "\"localeCode\": \"en-US\",\"ui\": true,\"greeting\": true,\"formattingLocale\": true} ]," + 
                  "\"paging\": { \"page\": 1,\"totalPages\": 1,\"perPage\": 100,\"totalElements\": 1,\"pageStart\": 0,"+
                  "\"pageEnd\": 0},\"navigation\": {\"firstPage\": {"+
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\"},"+
                  "\"lastPage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language?page=1&perPage=100\"}}}", 
                  Encoding.UTF8, "application/json")
           });
            mockResponseHandler.AddGetMockResponse(
          new Uri(ApiEndPoint + LanguageEndPoint + "/1033"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent(
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/language/1033\"," +
                 "\"id\": \"1033\",\"name\": \"English (United States)\",\"isoCode\": \"en\"," +
                 "\"localeCode\": \"en-US\",\"ui\": true,\"greeting\": true,\"formattingLocale\": true}",
                 Encoding.UTF8, "application/json")
          });
         mockResponseHandler.AddGetMockResponse(
          new Uri(ApiEndPoint + LocationEndPoint+ "?stateId=13"),
            new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent(
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/location?stateId=13&withNxx=true&orderBy=City&page=1&perPage=100\","+
                "\"records\": [{\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"268\",\"state\": {"+
                "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\",\"id\": \"13\" } },"+
                "{\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"312\",\"state\": {" + 
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                 "\"id\": \"13\"} }, {\"city\": \"Anchorage\",\"npa\": \"907\",\"nxx\": \"331\",\"state\": {" + 
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," + 
                  "\"id\": \"13\" } } ] }",Encoding.UTF8, "application/json")
          });
         mockResponseHandler.AddGetMockResponse(
         new Uri(ApiEndPoint + StateEndPoint + "/13"),
         new HttpResponseMessage(HttpStatusCode.OK)
         {
             Content = new StringContent(
               "{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/state/13\"," +
                "\"id\": \"13\",\"name\": \"Alaska\",\"isoCode\": \"AK\",\"country\": {" +
                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\",\"id\": \"1\" } }", Encoding.UTF8, "application/json")
         });
         mockResponseHandler.AddGetMockResponse(
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
        mockResponseHandler.AddGetMockResponse(
        new Uri(ApiEndPoint + TimeZoneEndPoint + "/1"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
              "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/1\"," +
              "\"id\": \"1\",\"name\": \"GMT\",\"description\": \"Casablanca, Monrovia, Reykjavik\" } ", Encoding.UTF8, "application/json")
        });
        mockResponseHandler.AddGetMockResponse(
          new Uri(ApiEndPoint + TimeZoneEndPoint ),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent(
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone?page=1&perPage=100\"," +
                "\"records\": [" +"{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/1\"," +
                "\"id\": \"1\",\"name\": \"GMT\",\"description\": \"Casablanca, Monrovia, Reykjavik\" }," +
                "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/timezone/2\"," +
                "\"id\": \"2\",\"name\": \"WET\",\"description\": \"Dublin, Edinburgh, Lisbon, London\" } ]}", Encoding.UTF8, "application/json")
          });
        }
        [Test]
        public void GetCountries()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(CountryEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var countryName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(countryName, "Afghanistan");
        }

        [Test]
        public void GetCountryById()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CountryEndPoint + "/3");

            JToken token = JObject.Parse(response.GetBody());

            var countryName = (string) token.SelectToken("name");

            Assert.AreEqual(countryName, "Albania");
        }

        [Test]
        public void GetLanguage()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(LanguageEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var languageName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(languageName, "English (United States)");
        }

        [Test]
        public void GetLanguagesById()
        {
            const string languageId = "1033";

            Response response = RingCentralClient.GetPlatform().GetRequest(LanguageEndPoint + "/" + languageId);

            JToken token = JObject.Parse(response.GetBody());

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "English (United States)");
        }

        [Test]
        public void GetLocation()
        {
            RingCentralClient.GetPlatform().AddQueryParameters("stateId", "13");

            Response response = RingCentralClient.GetPlatform().GetRequest(LocationEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var city = (string) token.SelectToken("records")[0].SelectToken("city");

            Assert.AreEqual(city, "Anchorage");
        }

        [Test]
        public void GetStateById()
        {
            const string stateId = "13";

            Response response = RingCentralClient.GetPlatform().GetRequest(StateEndPoint + "/" + stateId);

            JToken token = JObject.Parse(response.GetBody());

            var stateName = (string) token.SelectToken("name");

            Assert.AreEqual(stateName, "Alaska");
        }

        [Test]
        public void GetStates()
        {
            RingCentralClient.GetPlatform().AddQueryParameters("countryId", "1");
            RingCentralClient.GetPlatform().AddQueryParameters("withPhoneNumbers", "True");
            RingCentralClient.GetPlatform().AddQueryParameters("perPage", "2");

            Response response = RingCentralClient.GetPlatform().GetRequest(StateEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "Alabama");
        }

        [Test]
        public void GetTimeZoneById()
        {
            
            Response response = RingCentralClient.GetPlatform().GetRequest(TimeZoneEndPoint + "/1");

            JToken token = JObject.Parse(response.GetBody());

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "GMT");
        }

        [Test]
        public void GetTimeZones()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(TimeZoneEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "GMT");
        }
    }
}