namespace RingCentral.SDK
{
    public class SDK
    {
        private string AppKey { get; set; }
        private string AppSecret { get; set; }
        private string ApiEndpoint { get; set; }

        protected Platform Platform;

        /// <summary>
        ///     Constructor that sets up RingCentralClient
        /// </summary>
        /// <param name="appKey">RingCentral Application Key</param>
        /// <param name="appSecret">RingCentral Application Secret</param>
        /// <param name="apiEndPoint">RingCentral API Endpoint</param>
        public SDK(string appKey, string appSecret, string apiEndPoint)
        {
            AppKey = appKey;
            AppSecret = appSecret;
            ApiEndpoint = apiEndPoint;

            Platform = new Platform(appKey,appSecret,apiEndPoint);
        }

        public Platform GetPlatform()
        {
            return Platform;
        }

    }
}