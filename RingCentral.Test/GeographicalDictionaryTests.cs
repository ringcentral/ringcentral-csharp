using System;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

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
            string result = RingCentralClient.GetRequest(CountryEndPoint);

            JToken token = JObject.Parse(result);

            var countryName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(countryName, "Afghanistan");
        }

        [Test]
        public void GetCountryById()
        {
            const string countryId = "1";

            string result = RingCentralClient.GetRequest(CountryEndPoint + "/" + countryId);

            JToken token = JObject.Parse(result);

            var countryName = (string) token.SelectToken("name");

            Assert.AreEqual(countryName, "United States");
        }

        [Test]
        public void GetLanguage()
        {
            string result = RingCentralClient.GetRequest(LanguageEndPoint);

            JToken token = JObject.Parse(result);

            var languageName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(languageName, "English (United States)");
        }

        [Test]
        public void GetLanguagesById()
        {
            const string languageId = "1033";

            string result = RingCentralClient.GetRequest(LanguageEndPoint + "/" + languageId);

            JToken token = JObject.Parse(result);

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "English (United States)");
        }

        [Test]
        public void GetLocation()
        {
            RingCentralClient.AddQueryParameters("stateId", "13");

            string result = RingCentralClient.GetRequest(LocationEndPoint);

            JToken token = JObject.Parse(result);

            var city = (string) token.SelectToken("records")[0].SelectToken("city");

            Assert.AreEqual(city, "Anchorage");
        }

        [Test]
        public void GetStateById()
        {
            const string stateId = "13";

            string result = RingCentralClient.GetRequest(StateEndPoint + "/" + stateId);

            JToken token = JObject.Parse(result);

            var stateName = (string) token.SelectToken("name");

            Assert.AreEqual(stateName, "Alaska");
        }

        [Test]
        public void GetStates()
        {
            RingCentralClient.AddQueryParameters("countryId", "1");
            RingCentralClient.AddQueryParameters("withPhoneNumbers", "True");
            RingCentralClient.AddQueryParameters("perPage", "5");

            string result = RingCentralClient.GetRequest(StateEndPoint);

            JToken token = JObject.Parse(result);

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "Alabama");
        }

        [Test]
        public void GetTimeZoneById()
        {
            const string timeZoneId = "1";

            string result = RingCentralClient.GetRequest(TimeZoneEndPoint + "/" + timeZoneId);

            JToken token = JObject.Parse(result);

            var timeZoneName = (string) token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "GMT");
        }

        [Test]
        public void GetTimeZones()
        {
            string result = RingCentralClient.GetRequest(TimeZoneEndPoint);

            JToken token = JObject.Parse(result);

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "GMT");
        }
    }
}