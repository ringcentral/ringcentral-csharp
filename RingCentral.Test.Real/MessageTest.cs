using Newtonsoft.Json;
using NUnit.Framework;
using RingCentral.Http;
using System.Collections.Generic;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class MessageTest : BaseTest
    {
        [Test]
        public void SMS()
        {
            var subject = "hello world";
            var requestBody = new {
                text = subject,
                from = new { phoneNumber = Config.Instance.Username },
                to = new object[] { new { phoneNumber = Config.Instance.Receiver } }
            };
            var request = new Http.Request("/restapi/v1.0/account/~/extension/~/sms", JsonConvert.SerializeObject(requestBody));
            var response = sdk.Platform.Post(request);
            Assert.AreEqual(subject, response.Json["subject"].ToString());
            Assert.AreEqual("Outbound", response.Json["direction"].ToString());
            Assert.AreEqual("Sent", response.Json["messageStatus"].ToString());
        }

        [Test]
        public void Fax()
        {
            //var requestBody = JsonConvert.SerializeObject(new
            //{
            //    resolution = "High",
            //    //coverIndex = 0,
            //    to = new object[] { new { phoneNumber = Config.Instance.Receiver } }
            //});

            var requestBody = "{\"resolution\": \"High\", \"to\":[{\"phoneNumber\": \"" + Config.Instance.Receiver + "\"}]}";
            var attachments = new List<Attachment>();
            var textBytes = System.Text.Encoding.UTF8.GetBytes("hello fax");
            var attachment = new Attachment(@"test.txt", "application/octet-stream", textBytes);
            attachments.Add(attachment);
            var attachment2 = new Attachment(@"test2.txt", "text/plain", textBytes);
            attachments.Add(attachment2);
            var request = new Request("/restapi/v1.0/account/~/extension/~/fax", requestBody, attachments);
            //var body = request.HttpContent;
            //Assert.NotNull(body);
            var response = sdk.Platform.Post(request);
            Assert.AreEqual(true, response.OK);
        }
    }
}
