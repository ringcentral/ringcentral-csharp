using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using RingCentral.Http;
using System.Text;

namespace RingCentral.Test
{
    [TestFixture]
    public class AddressBookTests : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";

        [Test]
        public void GetContactFromAddressBook()
        {
            Response response = Platform.GetRequest(AddressBookEndPoint + "/1");
            JToken token = JObject.Parse(response.GetBody());
            var firstNameResponse = (string)token.SelectToken("firstName");

            Assert.AreEqual(firstNameResponse, "Delete");
        }
        [Test]
        public void GetAddressBook()
        {
            Response response = Platform.GetRequest(AddressBookEndPoint);
            JToken token = JObject.Parse(response.GetBody());
            var firstName = (string)token.SelectToken("records")[0].SelectToken("firstName");

            Assert.AreEqual("Delete", firstName);

            firstName = (string)token.SelectToken("records")[1].SelectToken("firstName");

            Assert.AreEqual("Vanessa", firstName);

        }
        [Test]
        public void DeleteContactFromAddressBook()
        {
            Response response = Platform.DeleteRequest(AddressBookEndPoint + "/3");
            JToken token = JObject.Parse(response.GetBody());
            var message = (string)token.SelectToken("message");
            Assert.AreEqual("Deleted", message);
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
            Platform.SetJsonData(jsonData);
            Response response = Platform.PostRequest(AddressBookEndPoint);
            JToken token = response.GetJson();

            var firstName = (string)token.SelectToken("firstName");
            Assert.AreEqual("Jim", firstName);


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
            Platform.SetJsonData(jsonData);

            Response response = Platform.PutRequest(AddressBookEndPoint + "/5");

            JToken token = JObject.Parse(response.GetBody());
            var street = (string)token.SelectToken("businessAddress").SelectToken("street");

            Assert.AreEqual(street, "3 Marina Blvd");
        }
    }
}