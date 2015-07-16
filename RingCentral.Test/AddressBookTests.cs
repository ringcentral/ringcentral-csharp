using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var result = RingCentralClient.GetRequest(AddressBookEndPoint);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void CreateAddressBookContact()
        {
            var jsonData = "{firstName\": \"Vanessa\", " +
                           "\"lastName\": \"May\", " +
                           "\"businessAddress\": " +
                           "{ " +
                               "\"street\": \"2 Marina Blvd\", " +
                               "\"city\": \"San-Francisco\", " +
                               "\"state\": \"CA\", " +
                               "\"zip\": \"94123\"}" +
                           "}";
            RingCentralClient.SetJsonData(jsonData);

            var result = RingCentralClient.PostRequest(AddressBookEndPoint);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void GetContactFromAddressBook()
        {
            const string contactId = "";

            var result = RingCentralClient.GetRequest(AddressBookEndPoint + "/" + contactId);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void UpdateContactInAddressbook()
        {
            const string contactId = "";

            var result = RingCentralClient.PutRequest(AddressBookEndPoint + "/" + contactId);
        }

        //TODO: This isn't working, a known permissions related issue
        //[Test]
        public void DeleteContactFromAddressBook()
        {
            const string contactId = "";

            var result = RingCentralClient.DeleteRequest(AddressBookEndPoint + "/" + contactId);
        }
    }
}
