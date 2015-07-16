using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void GetCountryById()
        {
            const string countryId = "1";

            var result = RingCentralClient.GetRequest(CountryEndPoint + "/" + countryId);

            JToken token = JObject.Parse(result);

            var countryName = (String)token.SelectToken("name");

            Assert.AreEqual(countryName, "United States");
        }
        [Test]
        public void GetCountries()
        {
            var result = RingCentralClient.GetRequest(CountryEndPoint);

            JToken token = JObject.Parse(result);

            var countryName = (String)token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(countryName, "Afghanistan");
        }

        [Test]
        public void GetStateById()
        {
            const string stateId = "13";

            var result = RingCentralClient.GetRequest(StateEndPoint + "/" + stateId);

            JToken token = JObject.Parse(result);

            var stateName = (String)token.SelectToken("name");

            Assert.AreEqual(stateName, "Alaska");
        }

        [Test]
        public void GetStates()
        {

            RingCentralClient.AddQueryParameters("countryId", "1");
            RingCentralClient.AddQueryParameters("withPhoneNumbers", "True");
            RingCentralClient.AddQueryParameters("perPage", "5");

            var result = RingCentralClient.GetRequest(StateEndPoint, RingCentralClient.GetQueryString());
            
            RingCentralClient.ClearQueryParameters();
            
            JToken token = JObject.Parse(result);

            var stateName = (String)token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "Alabama");
        }

        //TODO: need to modify GetRequest to handle query parameters
        [Test]
        public void GetLocation()
        {
            RingCentralClient.AddQueryParameters("stateId", "13");

            var result = RingCentralClient.GetRequest(LocationEndPoint,RingCentralClient.GetQueryString());

            RingCentralClient.ClearQueryParameters();

            JToken token = JObject.Parse(result);

            var city = (String)token.SelectToken("records")[0].SelectToken("city");

            Assert.AreEqual(city, "Anchorage");
        }

        [Test]
        public void GetTimeZoneById()
        {
            const string timeZoneId = "1";

            var result = RingCentralClient.GetRequest(TimeZoneEndPoint + "/" + timeZoneId);

            JToken token = JObject.Parse(result);

            var timeZoneName = (String)token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "GMT");
        }
        [Test]
        public void GetTimeZones()
        {
            var result = RingCentralClient.GetRequest(TimeZoneEndPoint);

            JToken token = JObject.Parse(result);

            var stateName = (String)token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "GMT");
        }
        [Test]
        public void GetLanguage()
        {
            var result = RingCentralClient.GetRequest(LanguageEndPoint);

            JToken token = JObject.Parse(result);

            var languageName = (String)token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(languageName, "English (United States)");
        }
        [Test]
        public void GetLanguagesById()
        {
            const string languageId = "1033";

            var result = RingCentralClient.GetRequest(LanguageEndPoint + "/" + languageId);

            JToken token = JObject.Parse(result);

            var timeZoneName = (String)token.SelectToken("name");

            Assert.AreEqual(timeZoneName, "English (United States)");
        }
    }
}
