using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class AddressBookTests : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";

        [Test]
        public void GetContactFromAddressBook()
        {
            Request request = new Request(AddressBookEndPoint + "/1");
            Response response = Platform.Get(request);
            JToken token = response.GetJson();
            var firstNameResponse = (string)token.SelectToken("firstName");

            Assert.AreEqual(firstNameResponse, "Delete");
        }
        [Test]
        public void GetAddressBook()
        {
            Request request = new Request(AddressBookEndPoint);
            Response response = Platform.Get(request);
            JToken token = response.GetJson();
            var firstName = (string)token.SelectToken("records")[0].SelectToken("firstName");

            Assert.AreEqual("Delete", firstName);

            firstName = (string)token.SelectToken("records")[1].SelectToken("firstName");

            Assert.AreEqual("Vanessa", firstName);

        }
        [Test]
        public void DeleteContactFromAddressBook()
        {
            Request request = new Request(AddressBookEndPoint + "/3");
            Response response = Platform.Delete(request);
            JToken token = response.GetJson();
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

            Request request = new Request(AddressBookEndPoint,jsonData);

            Response response = Platform.Post(request);
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
            Request request = new Request(AddressBookEndPoint + "/5", jsonData);

            Response response = Platform.Put(request);

            JToken token = response.GetJson();
            var street = (string)token.SelectToken("businessAddress").SelectToken("street");

            Assert.AreEqual(street, "3 Marina Blvd");
        }
    }
}