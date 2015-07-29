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




        [TestFixtureSetUp]
        public void Setup()
        {
            mockResponseHandler.AddGetMockResponse(
                    new Uri(ApiEndPoint + AddressBookEndPoint + "/1"),
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/123123\"," +
                        "\"availability\": \"Alive\"," + "\"id\": 123123 ," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," +
                        "\"businessAddress\": { " + "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")

                    });
            mockResponseHandler.AddGetMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact?sortBy=FirstName\"," +
                     "\"records\": [ " + "{" + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/1\"," +
                         "\"availability\": \"Alive\"," + "\"id\": 1," + "\"firstName\": \"Delete\"," + "\"lastName\": \"Me\"," + "\"businessAddress\": { " +
                           "\"street\": \"2 Marina Blvd\", " + "\"city\": \"San-Francisco\"," + "\"state\": \"CA\", " + "\"zip\": \"94123\" " + "}" + "}," + "{" +
                         "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/2/extension/2/address-book/contact/2\"," + "\"availability\": \"Alive\"," +
                         "\"id\": 2," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"" + "}" + "}" +
                       "], \"paging\" : { \"page\": 1, \"totalPages\": 1, \"perPage\": 100, \"totalElements\": 2, \"pageStart\": 0, \"pageEnd\": 1 }, " +
                       "\"navigation\": {  \"firstPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" },  " +
                       "\"lastPage\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/contact?sortBy=FirstName&page=1&perPage=100\" } }," +
                       "\"groups\": { \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/123123/extension/123123/address-book/group\" } }"
                      , Encoding.UTF8, "application/json")
                });
            mockResponseHandler.AddDeleteMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint + "/3"),
                   new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{ \"message\": \"Deleted\" }", Encoding.UTF8, "application/json") });
            mockResponseHandler.AddPostMockResponse(
                   new Uri(ApiEndPoint + AddressBookEndPoint),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/3\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 3 ," + "\"firstName\": \"Jim\"," + "\"lastName\": \"Johns\"," + "\"businessAddress\": { " + "\"street\": \"5 Marina Blvd\", " + "\"city\": \"San-Francisco\"," +
                       "\"state\": \"CA\"," + "\"zip\": \"94123\" } }", Encoding.UTF8, "application/json")
                   });
            mockResponseHandler.AddPutMockResponse(
                new Uri(ApiEndPoint + AddressBookEndPoint + "/5"),
                   new HttpResponseMessage(HttpStatusCode.OK)
                   {
                       Content = new StringContent("{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/address-book/contact/5\"," + "\"availability\": \"Alive\"," +
                       "\"id\": 5 ," + "\"firstName\": \"Vanessa\", " + "\"lastName\": \"May\", " + "\"businessAddress\": " + "{ " +
                             "\"street\": \"3 Marina Blvd\", " + "\"city\": \"San-Francisco\", " + "\"state\": \"CA\", " + "\"zip\": \"94123\"}" + "} ", Encoding.UTF8, "application/json")
                   });




        }

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