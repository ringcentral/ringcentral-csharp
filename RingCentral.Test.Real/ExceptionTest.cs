using NUnit.Framework;
using RingCentral.Http;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class ExceptionTest : BaseTest
    {
        [Test, ExpectedException(typeof(ApiException), ExpectedMessage = @"HTTP Status Code: NotFound")]
        public void NotFound()
        {
            // correct url should be: "/restapi/v1.0/account/~/extension/~/sms"
            var request = new Http.Request("/account/~/extension/~/sms", "{\"hello\": \"world\"}");
            sdk.Platform.Post(request);
        }
    }
}
