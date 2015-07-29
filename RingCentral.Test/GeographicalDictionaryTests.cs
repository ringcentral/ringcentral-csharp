using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

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

            JToken token = JObject.Parse(response.GetBody());

            var countryName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(countryName, "Afghanistan");
        }

        [Test]
        public void GetCountryById()
        {
            const string countryId = "1";

            Response response = RingCentralClient.GetPlatform().GetRequest(CountryEndPoint + "/" + countryId);

            JToken token = JObject.Parse(response.GetBody());

            var countryName = (string) token.SelectToken("name");

            Assert.AreEqual(countryName, "United States");
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
            RingCentralClient.GetPlatform().AddQueryParameters("perPage", "5");

            Response response = RingCentralClient.GetPlatform().GetRequest(StateEndPoint);

            JToken token = JObject.Parse(response.GetBody());

            var stateName = (string) token.SelectToken("records")[0].SelectToken("name");

            Assert.AreEqual(stateName, "Alabama");
        }

        [Test]
        public void GetTimeZoneById()
        {
            const string timeZoneId = "1";

            Response response = RingCentralClient.GetPlatform().GetRequest(TimeZoneEndPoint + "/" + timeZoneId);

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