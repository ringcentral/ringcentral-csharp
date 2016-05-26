using NUnit.Framework;
using System;
using System.Net.Http;

namespace RingCentral.Test
{
    [TestFixture]
    public class BaseTest
    {
        protected const string AppKey = "AppKey";
        protected const string AppSecret = "AppSecret";

        protected const string Username = "147258369";
        protected const string Password = "963852741";
        protected const string Extension = "101";
        protected const string ToPhone = "258369741";

        protected SDK sdk;

        [TestFixtureSetUp]
        public void SetUp()
        {
            sdk = new SDK(AppKey, AppSecret, SDK.Server.Sandbox, "C Sharp Test Suite", "1.0.0");
            // mock test only, don't contact remote server
            sdk.Platform._client = new HttpClient(new MockHttpClient()) { BaseAddress = new Uri(SDK.SandboxServerUrl) };
            sdk.Platform.Login(Username, Extension, Password, true);
        }
    }
}