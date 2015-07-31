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
   

        [Test]
        public void GetCountries()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(CountryEndPoint);

            JToken token = response.GetJson();

            var countryName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(countryName, "Afghanistan");
        }

        [Test]
        public void GetCountryById()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CountryEndPoint + "/3");

            JToken token = response.GetJson();

            var countryName = (string) token.SelectToken("name");

            Assert.AreEqual(countryName, "Albania");
        }

        [Test]
        public void GetLanguage()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(LanguageEndPoint);

            JToken token = response.GetJson();

            var languageName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(languageName, "English (United States)");
        }

        [Test]
        public void GetLanguagesById()
        {
            const string languageId = "1033";

            Response response = RingCentralClient.GetPlatform().GetRequest(LanguageEndPoint + "/" + languageId);

            JToken token = response.GetJson();

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "English (United States)");
        }

        [Test]
        public void GetLocation()
        {
            RingCentralClient.GetPlatform().AddQueryParameters("stateId", "13");

            Response response = RingCentralClient.GetPlatform().GetRequest(LocationEndPoint);

            JToken token = response.GetJson();

            var city = (string) token.SelectToken("records")[0].SelectToken("city");

            Assert.AreEqual(city, "Anchorage");
        }

        [Test]
        public void GetStateById()
        {
            const string stateId = "13";

            Response response = RingCentralClient.GetPlatform().GetRequest(StateEndPoint + "/" + stateId);

            JToken token = response.GetJson();

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

            JToken token = response.GetJson();

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "Alabama");
        }

        [Test]
        public void GetTimeZoneById()
        {
            
            Response response = RingCentralClient.GetPlatform().GetRequest(TimeZoneEndPoint + "/1");

            JToken token = response.GetJson();

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "GMT");
        }

        [Test]
        public void GetTimeZones()
        {
            Response response = RingCentralClient.GetPlatform().GetRequest(TimeZoneEndPoint);

            JToken token = response.GetJson();

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "GMT");
        }
    }
}