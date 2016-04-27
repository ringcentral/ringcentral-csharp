using NUnit.Framework;
using RingCentral.Http;
using System;
using System.Net.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class TestConfiguration
    {
        protected const string Server = SDK.SandboxServer;
        protected const string AppKey = "AppKey";
        protected const string AppSecret = "AppSecret";

        protected const string Username = "147258369";
        protected const string Password = "963852741";
        protected const string Extension = "101";

        protected const string ToPhone = "258369741";

        protected Response AuthResult;
        protected Platform Platform;
        protected SDK RingCentralClient;
        protected MockHttpClient MockResponseHandler = new MockHttpClient();

        [TestFixtureSetUp]
        public void SetUp()
        {
            RingCentralClient = new SDK(AppKey, AppSecret, Server, "C Sharp Test Suite", "1.0.0");
            Platform = RingCentralClient.GetPlatform();
            Platform._client = new HttpClient(MockResponseHandler) { BaseAddress = new Uri(Server) };
            AuthResult = Platform.Authorize(Username, Extension, Password, true);
        }
    }
}