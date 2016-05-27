using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;
using PubNubMessaging.Core;
using RingCentral.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RingCentral.Pubnub
{
    public class Subscription
    {
        private Platform platform;
        private SubscriptionModel _subscriptionModel;
        private SubscriptionModel SubscriptionModel
        {
            get
            {
                return _subscriptionModel;
            }
            set
            {
                _subscriptionModel = value;
                TaskEx.Delay((_subscriptionModel.ExpiresIn - 120) * 1000).ContinueWith((action)=> {
                    Renew(); // 2 minutes before the expiration
                });
            }
        }
        private Action<string> subscribeCallback;
        private Action<string> connectCallback;
        private Action<PubnubClientError> errorCallback;
        private List<string> eventFilters = new List<string>();
        private PubNubMessaging.Core.Pubnub pubnub;

        internal Subscription(Platform platform, Action<object> subscribeCallback, Action<object> connectCallback, Action<object> errorCallback)
        {
            this.platform = platform;
            this.subscribeCallback = subscribeCallback;
            this.connectCallback = connectCallback;
            this.errorCallback = errorCallback;
        }

        public void AddEventFilter(string eventFilter)
        {
            eventFilters.Add(eventFilter);
        }

        public void Register()
        {
            var request = new Request("/restapi/v1.0/subscription", JsonConvert.SerializeObject(new
            {
                eventFilters = eventFilters,
                deliveryMode = new { transportType = "PubNub", encryption = true }
            }));
            var response = platform.Post(request);
            SubscriptionModel = JsonConvert.DeserializeObject<SubscriptionModel>(response.Body);
            pubnub = new PubNubMessaging.Core.Pubnub(null, SubscriptionModel.DeliveryMode.SubscriberKey);
            pubnub.Subscribe<string>(SubscriptionModel.DeliveryMode.Address, OnSubscribe, OnConnect, onError);
        }

        public void UnRegister()
        {
            var request = new Request("/restapi/v1.0/subscription/" + SubscriptionModel.Id);
            var response = platform.Delete(request);
        }

        private void Renew()
        {
            var request = new Request("/restapi/v1.0/subscription/" + SubscriptionModel.Id);
            var response = platform.Put(request);
            SubscriptionModel = JsonConvert.DeserializeObject<SubscriptionModel>(response.Body);
        }

        private void OnSubscribe(string result)
        {
            var message = JsonConvert.DeserializeObject<string[]>(result)[0];
            message = Decrypt(message, SubscriptionModel.DeliveryMode.EncryptionKey);
            subscribeCallback?.Invoke(message);
        }

        private void OnConnect(string connectMessage)
        {
            connectCallback?.Invoke(connectMessage);
        }

        private void onError(PubnubClientError pubnubError)
        {
            errorCallback?.Invoke(pubnubError);
        }

        private string Decrypt(string dataString, string keyString)
        {
            var key = Convert.FromBase64String(keyString);
            var keyParameter = ParameterUtilities.CreateKeyParameter("AES", key);
            var cipher = CipherUtilities.GetCipher("AES/ECB/PKCS7Padding");
            cipher.Init(false, keyParameter);

            var data = Convert.FromBase64String(dataString);
            var memoryStream = new MemoryStream(data, false);
            var cipherStream = new CipherStream(memoryStream, cipher, null);

            var bufferSize = 1024;
            var buffer = new byte[bufferSize];
            var length = 0;
            var resultStream = new MemoryStream();
            while ((length = cipherStream.Read(buffer, 0, bufferSize)) > 0)
            {
                resultStream.Write(buffer, 0, length);
            }
            var resultBytes = resultStream.ToArray();
            var result = Encoding.UTF8.GetString(resultBytes, 0, resultBytes.Length);
            return result;
        }
    }
}

/*
        
{
  "uri" : "https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/d538cd7b-ec42-46fe-b79a-81f59cda3803",
  "id" : "d538cd7b-ec42-46fe-b79a-81f59cda3803",
  "creationTime" : "2016-05-27T02:31:50.684Z",
  "status" : "Active",
  "eventFilters" : [ "/restapi/v1.0/account/130829004/extension/130829004/message-store", "/restapi/v1.0/account/130829004/extension/130829004/presence" ],
  "expirationTime" : "2016-05-27T02:46:50.688Z",
  "expiresIn" : 899,
  "deliveryMode" : {
    "transportType" : "PubNub",
    "encryption" : true,
    "address" : "4189939474110475_68c23e20",
    "subscriberKey" : "sub-c-b8b9cd8c-e906-11e2-b383-02ee2ddab7fe",
    "encryptionAlgorithm" : "AES",
    "encryptionKey" : "vn8WGfVYhOgNbYhSJN/XJA=="
  }
}

*/
