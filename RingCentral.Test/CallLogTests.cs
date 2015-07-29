using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;


namespace RingCentral.Test
{
    [TestFixture]
    public class CallLogTests : TestConfiguration
    {
        private const string CallLogEndPoint = "/restapi/v1.0/account/~";
        [TestFixtureSetUp]
        public void SetUp()
        {

            mockResponseHandler.AddGetMockResponse(
                new Uri(ApiEndPoint + CallLogEndPoint + "/active-calls"),
                new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                 "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                                "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/active-calls?page=1&perPage=100\" }}}"
                                                , Encoding.UTF8, "application/json")
                });
            mockResponseHandler.AddGetMockResponse(
               new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/active-calls"),
               new HttpResponseMessage(HttpStatusCode.OK)
               {
                   Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\",\"records\": [],\"paging\": {" +
                                                "\"page\": 1, \"perPage\": 100},\"navigation\": {\"firstPage\": {" +
                                               "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/active-calls?page=1&perPage=100\" }}}"
                                               , Encoding.UTF8, "application/json")
               });

            mockResponseHandler.AddGetMockResponse(
          new Uri(ApiEndPoint + CallLogEndPoint + "/call-log/"),
          new HttpResponseMessage(HttpStatusCode.OK)
          {
              Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/11/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
                "\"records\": [{ \"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abc?view=Simple\",\"id\": \"Abc\"," +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\", \"to\": { \"phoneNumber\": \"+19999999999\"}, \"from\": {" +
                    "\"phoneNumber\": \"+19999999999\" }},{" +
                  "\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/call-log/Abcd?view=Simple\",\"id\": \"Abcd\", " +
                  "\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:05.000Z\",\"duration\": 31,\"type\": \"Voice\",\"direction\": \"Inbound\"," +
                  "\"action\": \"Phone Call\",\"result\": \"Missed\",\"to\": {\"phoneNumber\": \"+19999999999\" },\"from\": {\"phoneNumber\": \"+19999999999\"}}]}", Encoding.UTF8, "application/json")
          });
            mockResponseHandler.AddGetMockResponse(
        new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/call-log"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log?view=Simple&dateFrom=2015-07-22T00:00:00.000Z&page=1&perPage=100\"," +
         "\"records\": [{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension/130076004/call-log/Abcdef?view=Simple\"," +
          "\"id\": \"Abcdef\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
          "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
           "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }," +
           "{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/130076004/extension/130076004/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\", \"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32,\"type\": \"Voice\"," +
           "\"direction\": \"Outbound\",\"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"," +
            "\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\" } }]}", Encoding.UTF8, "application/json")
        });
            mockResponseHandler.AddGetMockResponse(
        new Uri(ApiEndPoint + CallLogEndPoint + "/extension/~/call-log/Abcdefg"),
        new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/extension/1/call-log/Abcdefg?view=Simple\"," +
           "\"id\": \"Abcdefg\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
           "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
            "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }", Encoding.UTF8, "application/json")
        });
            mockResponseHandler.AddGetMockResponse(
            new Uri(ApiEndPoint + CallLogEndPoint + "/call-log/Abcdefgh"),
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"uri\": \"https://platform.devtest.ringcentral.com/restapi/v1.0/account/1/call-log/Abcdefg?view=Simple\"," +
               "\"id\": \"Abcdefgh\",\"sessionId\": \"1\",\"startTime\": \"2015-07-29T02:19:04.000Z\",\"duration\": 32, \"type\": \"Voice\"," +
               "\"direction\": \"Outbound\", \"action\": \"RingOut Web\",\"result\": \"Call connected\",\"to\": {\"phoneNumber\": \"+199999999999\"," +
                "\"name\": \"John Doe\",\"location\": \"Los Angeles, CA\"},\"from\": {\"phoneNumber\": \"+19999999999\",\"name\": \"John Doe\"} }", Encoding.UTF8, "application/json")
            });
        }
        [Test]
        public void GetActiveCalls()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/active-calls");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetActiveCallsByExtension()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/active-calls");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLog()
        {

            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/call-log/");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtension()
        {


            Response result = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/call-log");
            JToken token = JObject.Parse(result.GetBody());

            var uri = (string)token.SelectToken("uri");

            Assert.NotNull(uri);
        }

        [Test]
        public void GetCallLogByExtensionAndId()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/extension/~/call-log/Abcdefg");
            JToken token = JObject.Parse(response.GetBody());


            var id = (string)token.SelectToken("id");

            Assert.AreEqual(id, "Abcdefg");
        }

        [Test]
        public void GetCallLogById()
        {

            Response response = RingCentralClient.GetPlatform().GetRequest(CallLogEndPoint + "/call-log/Abcdefgh");
            JToken token = JObject.Parse(response.GetBody());

            var id = (string)token.SelectToken("id");

            Assert.AreEqual("Abcdefgh", id);
        }
    }
}