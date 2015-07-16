using NUnit.Framework;

namespace RingCentral.Test
{
    [TestFixture]
    public class AddressBookTests : TestConfiguration
    {
        private const string AddressBookEndPoint = "/restapi/v1.0/account/~/extension/~/address-book/contact";

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void GetAddressBook()
        {
            string result = RingCentralClient.GetRequest(AddressBookEndPoint);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void CreateAddressBookContact()
        {
            string jsonData = "{firstName\": \"Vanessa\", " +
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
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void GetContactFromAddressBook()
        {
            const string contactId = "";

            string result = RingCentralClient.GetRequest(AddressBookEndPoint + "/" + contactId);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void UpdateContactInAddressbook()
        {
            const string contactId = "";

            string result = RingCentralClient.PutRequest(AddressBookEndPoint + "/" + contactId);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void DeleteContactFromAddressBook()
        {
            const string contactId = "";

            string result = RingCentralClient.DeleteRequest(AddressBookEndPoint + "/" + contactId);
        }
    }
}