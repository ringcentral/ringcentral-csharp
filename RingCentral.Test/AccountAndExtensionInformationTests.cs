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
    public class AccountAndExtensionInformationTests : TestConfiguration
    {
        protected const string AccountInformationEndPoint = "/restapi/v1.0/account/~";
        protected const string AccountExtensionInformationEndPoint = "/restapi/v1.0/account/~/extension";
       [TestFixtureSetUp]
        public void Setup()
        {
            mockResponseHandler.AddGetMockResponse(
                new Uri(ApiEndPoint + AccountExtensionInformationEndPoint),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1\"," + "\"id\": 1," + "\"serviceInfo\": {" +
                                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/service-info\"," + "\"brand\": {" +
                                   "\"id\": \"1\"," + "\"name\": \"RingCentral\"," + "\"homeCountry\": {" + "\"id\": \"1\"," +
                                   "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"} }," +
                                 "\"servicePlan\": {" + "\"id\": \"1\"," + "\"name\": \"Sandbox Office 4 lines Enterprise Edition\"," + "\"edition\": \"Enterprise\"}," +
                                 "\"billingPlan\": {" + "\"id\": \"8853\"," + "\"name\": \"Monthly-109.98-Sandbox 4 Line\"," + "\"durationUnit\": \"Month\"," +
                                   "\"duration\": 1, " + "\"type\": \"Regular\"} }," +
                               "\"operator\": { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 1," +
                                 "\"extensionNumber\": \"101\" }," + "\"mainNumber\": \"19999999999\"," + "\"status\": \"Confirmed\"," + "\"setupWizardState\": \"Completed\"}", Encoding.UTF8, "application/json")
                }

                );
            mockResponseHandler.AddGetMockResponse(
              new Uri(ApiEndPoint + AccountInformationEndPoint),
              new HttpResponseMessage(HttpStatusCode.OK)
              {
                  Content = new StringContent(
                      "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1\"," + "\"id\": 1," + "\"serviceInfo\": {" +
                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/service-info\"," + "\"brand\": {" +
                                 "\"id\": \"1\"," + "\"name\": \"RingCentral\"," + "\"homeCountry\": {" + "\"id\": \"1\"," +
                                 "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/dictionary/country/1\"} }," +
                               "\"servicePlan\": {" + "\"id\": \"1\"," + "\"name\": \"Sandbox Office 4 lines Enterprise Edition\"," + "\"edition\": \"Enterprise\"}," +
                               "\"billingPlan\": {" + "\"id\": \"8853\"," + "\"name\": \"Monthly-109.98-Sandbox 4 Line\"," + "\"durationUnit\": \"Month\"," +
                                 "\"duration\": 1, " + "\"type\": \"Regular\"} }," +
                             "\"operator\": { " + "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," + "\"id\": 1," +
                               "\"extensionNumber\": \"101\" }," + "\"mainNumber\": \"19999999999\"," + "\"status\": \"Confirmed\"," + "\"setupWizardState\": \"Completed\"}", Encoding.UTF8, "application/json")
              }

              );

            mockResponseHandler.AddGetMockResponse(
             new Uri(ApiEndPoint + AccountExtensionInformationEndPoint + "/1"),
             new HttpResponseMessage(HttpStatusCode.OK)
             {
                 Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension?page=1&perPage=100\"," + 
                "\"records\": [ { "+ "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1\"," +"\"id\": 1,"+
                "\"extensionNumber\": \"102\"," + "\"contact\": {\"firstName\": \"Alice\",\"lastName\": \"Keys\",\"email\": \"alice.keys@example.com\"}," +
                "\"name\": \"Alice Keys\",\"type\": \"User\",\"status\": \"NotActivated\",\"permissions\": { \"admin\": {\"enabled\": false }," + 
                "\"internationalCalling\": {\"enabled\": true }},\"profileImage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/profile-image\"}}" +
                "],\"paging\": {\"page\": 1,\"totalPages\": 1,\"perPage\": 100,\"totalElements\": 1, \"pageStart\": 0,\"pageEnd\": 0},\"navigation\": {\"firstPage\": {" +
                "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension?page=1&perPage=100\" },\"lastPage\": {\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension?page=1&perPage=100\"}}}"
                 , Encoding.UTF8, "application/json")
             });
        }


        [Test]
        public void GetAccountExtensionInformation()
        {
            Response response = Platform.GetRequest(AccountExtensionInformationEndPoint);

            Assert.AreEqual(response.GetStatus(),200);
        }

        [Test]
        public void GetAccountInformation()
        {
            Response response = Platform.GetRequest(AccountInformationEndPoint);

            JToken token = response.GetJson();
            var mainNumber = (string) token.SelectToken("mainNumber");

            Assert.AreEqual(mainNumber, "19999999999");
        }

        [Test]
        public void GetExtensionInformation()
        {
            Response response = Platform.GetRequest(AccountExtensionInformationEndPoint + "/1");

            Assert.IsNotNull(response);
        }
    }
}