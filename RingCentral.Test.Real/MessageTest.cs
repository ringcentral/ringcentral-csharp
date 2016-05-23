using Newtonsoft.Json;
using NUnit.Framework;

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
    }
}
