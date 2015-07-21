using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace RingCentralSDK.Test
{
    [TestFixture]
    public class AddressBookTests : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";

        [Test]
        public void GetAddressBook()
        {
            string result = RingCentralClient.GetRequest(AddressBookEndPoint);

            JToken token = JObject.Parse(result);
            var firstName = (string)token.SelectToken("records")[0].SelectToken("firstName");

            Assert.AreEqual(firstName, "Test");
        }

        [Test]
        public void CreateAddressBookContact()
        {


            string jsonData = "{\"firstName\": \"Vanessa\", " +
                              "\"lastName\": \"May\", " +
                              "\"businessAddress\": " +
                              "{ " +
                              "\"street\": \"2 Marina Blvd\", " +
                              "\"city\": \"San-Francisco\", " +
                              "\"state\": \"CA\", " +
                              "\"zip\": \"94123\"}" +
                              "}";
            RingCentralClient.SetJsonData(jsonData);

            string result = RingCentralClient.PostRequest(AddressBookEndPoint);

            JToken token = JObject.Parse(result);
            var firstNameResponse = (string)token.SelectToken("firstName");

            Assert.AreEqual(firstNameResponse, "Vanessa");
        }

        [Test]
        public void GetContactFromAddressBook()
        {
            const string contactId = "389441004";

            string result = RingCentralClient.GetRequest(AddressBookEndPoint + "/" + contactId);

            JToken token = JObject.Parse(result);
            var firstNameResponse = (string)token.SelectToken("firstName");

            Assert.AreEqual(firstNameResponse, "Vanessa");
        }

        [Test]
        public void UpdateContactInAddressbook()
        {
            string jsonData = "{\"firstName\": \"Vanessa\", " +
                              "\"lastName\": \"May\", " +
                              "\"businessAddress\": " +
                              "{ " +
                              "\"street\": \"3 Marina Blvd\", " +
                              "\"city\": \"San-Francisco\", " +
                              "\"state\": \"CA\", " +
                              "\"zip\": \"94123\"}" +
                              "}";
            RingCentralClient.SetJsonData(jsonData);

            const string contactId = "389441004";

            string result = RingCentralClient.PutRequest(AddressBookEndPoint + "/" + contactId);

            JToken token = JObject.Parse(result);
            var street = (string)token.SelectToken("businessAddress").SelectToken("street");

            Assert.AreEqual(street, "3 Marina Blvd");
        }

        [Test]
        public void DeleteContactFromAddressBook()
        {

            string jsonData = "{\"firstName\": \"Delete\", " +
                              "\"lastName\": \"Me\", " +
                              "\"businessAddress\": " +
                              "{ " +
                              "\"street\": \"2 Marina Blvd\", " +
                              "\"city\": \"San-Francisco\", " +
                              "\"state\": \"CA\", " +
                              "\"zip\": \"94123\"}" +
                              "}";
            RingCentralClient.SetJsonData(jsonData);

            string createResult = RingCentralClient.PostRequest(AddressBookEndPoint);

            JToken token = JObject.Parse(createResult);
            var id = (string)token.SelectToken("id");

            Assert.NotNull(id);

            RingCentralClient.DeleteRequest(AddressBookEndPoint + "/" + id);

            string getResult = RingCentralClient.GetRequest(AddressBookEndPoint + "/" + id);
            token = JObject.Parse(getResult);
            var message = (string)token.SelectToken("message");

            Assert.AreEqual(message, "Resource for parameter [contactId] is not found");

        }
    }
}
