namespace RingCentral.SDK
{
    public class SDK
    {
        private string AppKey { get; set; }
        private string AppSecret { get; set; }
        private string ApiEndpoint { get; set; }
        private string AppName { get; set; }
        private string AppVersion { get; set; }

        public const string VERSION = "1.0.0";

        protected Platform Platform;

        /// <summary>
        ///     Constructor that sets up RingCentralClient
        /// </summary>
        /// <param name="appKey">RingCentral Application Key</param>
        /// <param name="appSecret">RingCentral Application Secret</param>
        /// <param name="apiEndPoint">RingCentral API Endpoint</param>
        public SDK(string appKey, string appSecret, string apiEndPoint, string appName, string appVersion)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;
            AppName = appName;
            AppVersion = appVersion;

            Platform = new Platform(appKey,appSecret,apiEndPoint,appName,appVersion);
        }

        public Platform GetPlatform()
        {
            return Platform;
        }

    }
}