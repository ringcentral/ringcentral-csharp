using Newtonsoft.Json;
using NUnit.Framework;
using RingCentral.Http;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace RingCentral.Test.Real
{
    [TestFixture]
    class AddressBookTest : BaseTest
    {
        [Test]
        public void AddressBook()
        {
            // create
            var requestBody = new
            {
                firstName = "Tyler",
                lastName = "Long",
                homePhone = "+15889546648"
            };
            var request1 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact", JsonConvert.SerializeObject(requestBody));
            var response1 = sdk.Platform.Post(request1);
            Assert.AreEqual(true, response1.OK);

            // list
            var request2 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact?startsWith=Long");
            var response2 = sdk.Platform.Get(request2);
            Assert.AreEqual(true, response2.OK);
            var count = response2.Json.SelectToken("records").Count();
            Assert.AreEqual(true, count > 0);
            var me = response2.Json.SelectToken("records").Where(jt => (string)jt.SelectToken("homePhone") == "+15889546648").FirstOrDefault();
            Assert.NotNull(me);

            // update
            var request25 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"),
                JsonConvert.SerializeObject(new { homePhone = "+18922849962" }));
            var response25 = sdk.Platform.Put(request25);
            Assert.AreEqual(true, response25.OK);

            // get
            var request255 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"));
            var response255 = sdk.Platform.Get(request255);
            Assert.AreEqual(true, response255.OK);
            Assert.AreEqual("+18922849962", (string)response255.Json.SelectToken("homePhone"));

            // delete
            var request3 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"));
            var response3 = sdk.Platform.Delete(request3);
            Assert.AreEqual(true, response3.OK);

            // list again
            var request4 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact?startsWith=Long");
            var response4 = sdk.Platform.Get(request4);
            Assert.AreEqual(true, response4.OK);
            Assert.AreEqual(count - 1, response4.Json.SelectToken("records").Count());
        }

        [Test]
        public void HttpMethodTunneling()
        {
            Thread.Sleep(40000);

            // create
            var requestBody = new
            {
                firstName = "Tyler",
                lastName = "Long",
                homePhone = "+15889546648"
            };
            var request1 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact", JsonConvert.SerializeObject(requestBody));
            var response1 = sdk.Platform.Post(request1);
            Assert.AreEqual(true, response1.OK);

            // list
            var request2 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact?startsWith=Long");
            var response2 = sdk.Platform.Get(request2);
            Assert.AreEqual(true, response2.OK);
            var count = response2.Json.SelectToken("records").Count();
            Assert.AreEqual(true, count > 0);
            var me = response2.Json.SelectToken("records").Where(jt => (string)jt.SelectToken("homePhone") == "+15889546648").FirstOrDefault();
            Assert.NotNull(me);

            // update
            var request25 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"),
                JsonConvert.SerializeObject(new { homePhone = "+18922849962" }));
            request25.HttpMethodTunneling = true;
            var response25 = sdk.Platform.Put(request25);
            Assert.AreEqual(HttpMethod.Post, response25.Request.Method);
            Assert.AreEqual(true, response25.OK);

            // get
            var request255 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"));
            var response255 = sdk.Platform.Get(request255);
            Assert.AreEqual(true, response255.OK);
            Assert.AreEqual("+18922849962", (string)response255.Json.SelectToken("homePhone"));

            // delete
            var request3 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + (string)me.SelectToken("id"));
            request3.HttpMethodTunneling = true;
            var response3 = sdk.Platform.Delete(request3);
            Assert.AreEqual(HttpMethod.Post, response3.Request.Method);
            Assert.AreEqual(true, response3.OK);

            // list again
            var request4 = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact?startsWith=Long");
            var response4 = sdk.Platform.Get(request4);
            Assert.AreEqual(true, response4.OK);
            Assert.AreEqual(count - 1, response4.Json.SelectToken("records").Count());
        }
    }
}
