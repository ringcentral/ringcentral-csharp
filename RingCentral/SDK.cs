using RingCentral.Pubnub;

namespace RingCentral
{
    public class SDK
    {
        public const string Version = "1.0.0";
        public const string SandboxServerUrl = "https://platform.devtest.ringcentral.com";
        public const string ProductionServerUrl = "https://platform.ringcentral.com";
        public enum Server
        {
            Sandbox,
            Production
        }

        /// <summary>
        /// Constructor that sets up RingCentral Client
        /// </summary>
        /// <param name="appKey">Application Key</param>
        /// <param name="appSecret">Application Secret</param>
        /// <param name="serverUrl">Server Url, either SDK.SandboxServerUrl or SDK.ProductionServerUrl</param>
        /// <param name="appName">Application name, will be used in user agent</param>
        /// <param name="appVersion">Application Version, will be used in user agent</param>
        public SDK(string appKey, string appSecret, string serverUrl, string appName = "", string appVersion = "")
        {
            Platform = new Platform(appKey, appSecret, serverUrl, appName, appVersion);
        }

        /// <summary>
        /// Constructor that sets up RingCentral Client
        /// </summary>
        /// <param name="appKey">Application Key</param>
        /// <param name="appSecret">Application Secret</param>
        /// <param name="server">Server.Sandbox or Server.Production</param>
        /// <param name="appName">Application name, will be used in user agent</param>
        /// <param name="appVersion">Application Version, will be used in user agent</param>
        public SDK(string appKey, string appSecret, Server server, string appName = "", string appVersion = "") : this(appKey, appSecret,
            (server == Server.Production ? ProductionServerUrl : SandboxServerUrl), appName, appVersion)
        {
        }

        public Platform Platform { get; private set; }

        public Subscription CreateSubscription()
        {
            return new Subscription(this.Platform);
        }
    }
}