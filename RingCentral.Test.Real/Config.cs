using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace RingCentral.Test
{
    class Config
    {
        private static Config instance = null;
        private Config() { }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    try
                    {
                        using (var sr = new StreamReader("config.json"))
                        {
                            var jsonData = sr.ReadToEnd();
                            instance = JsonConvert.DeserializeObject<Config>(jsonData);
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        Debug.WriteLine("config.json doesn't exist");
                    }
                }
                return instance;
            }
        }

        public string ServerUrl
        {
            get
            {
                if (Server == "Production")
                {
                    return "https://platform.ringcentral.com";
                }
                else
                {
                    return "https://platform.devtest.ringcentral.com";
                }
            }
        }

        [JsonProperty("RC_APP_KEY")]
        public string AppKey;

        [JsonProperty("RC_APP_SECRET")]
        public string AppSecret;

        [JsonProperty("RC_APP_SERVER")]
        public string Server;

        [JsonProperty("RC_USERNAME")]
        public string UserName;

        [JsonProperty("RC_EXTENSION")]
        public string Extension;

        [JsonProperty("RC_PASSWORD")]
        public string Password;
    }
}