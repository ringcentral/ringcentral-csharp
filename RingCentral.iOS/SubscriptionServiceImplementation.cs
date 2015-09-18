using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PubNubMessaging.Core;
using Newtonsoft.Json.Linq;
using RingCentral.SDK.Http;
using System.Timers;
using RingCentral.SDK;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceImplementation : ISubscriptionService
    {
        private const string Tag = "RingCentral Android SDK";
        private Pubnub _pubnub;
        private bool _encrypted;
        private ICryptoTransform _decrypto;
        public Platform _platform;
        private Subscription _subscription;
        private Timer timeout;
        private bool subscribed;
        private List<string> eventFilters = new List<string>();
        private const string SubscriptionEndPoint = "/restapi/v1.0/subscription";
        private const int RenewHandicap = 60;
        private Dictionary<string, object> _events = new Dictionary<string, object>
        {
            {"notification",""},
            {"errorMessage",""},
            {"connectMessage", ""},
            {"disconnectMessage",""}

        };

        private void SetTimeout()
        {

            timeout = new Timer((_subscription.ExpiresIn * 1000) - RenewHandicap);
            timeout.Elapsed += OnTimedExpired;
            timeout.AutoReset = false;
            //Keep garbage collection from removing this on extended time
            GC.KeepAlive(timeout);
            timeout.Start();
        }
        public bool IsSubsribed()
        {
            return subscribed;
        }
        private void ClearTimeout()
        {
            if (timeout != null)
            {
                timeout.Stop();
                timeout.Dispose();
            }
        }
        public List<string> GetEvents()
        {
            return eventFilters;
        }
        public void ClearEvents()
        {
            eventFilters.Clear();
        }
        private void OnTimedExpired(Object source, System.Timers.ElapsedEventArgs e)
        {
            timeout.Stop();
            if (subscribed) Renew();
            else Subscribe();

        }
        public void SetEvents(List<string> newEventFilters)
        {
            eventFilters = newEventFilters;
        }

        public void AddEvent(string eventToAdd)
        {
            eventFilters.Add(eventToAdd);
        }
        public Response Renew()
        {
            ClearTimeout();
            try
            {
                if (_subscription == null || string.IsNullOrEmpty(_subscription.Id)) throw new Exception("Subscription ID is required");
                if (eventFilters.Count == 0) throw new Exception("Events are undefined");
                var jsonData = GetFullEventsFilter();
                Request request = new Request(SubscriptionEndPoint + "/" + _subscription.Id, jsonData);
                Response response = _platform.Put(request);
                UpdateSubscription(JsonConvert.DeserializeObject<Subscription>(response.GetBody()));
                return response;
            }
            catch (Exception e)
            {
                Unsubscribe();
                throw e;
            }
        }
        public Response Remove()
        {
            if (_subscription == null || string.IsNullOrEmpty(_subscription.Id)) throw new Exception("Subscription ID is required");
            try
            {
                Request request = new Request(SubscriptionEndPoint + "/" + _subscription.Id);
                Response response = _platform.Delete(request);
                Unsubscribe();
                return response;
            }
            catch (Exception e)
            {
                Unsubscribe();
                throw e;
            }

        }

        public void UpdateSubscription(Subscription subscription)
        {
            ClearTimeout();
            _subscription = subscription;
            subscribed = true;
            SetTimeout();
        }

        public Response Subscribe()
        {
            if (eventFilters.Count == 0)
            {
                throw new Exception("Event filters are undefined");
            }
            try
            {
                var jsonData = GetFullEventsFilter();
                Request request = new Request(SubscriptionEndPoint, jsonData);
                Response response = _platform.Post(request);
                _subscription = JsonConvert.DeserializeObject<Subscription>(response.GetBody());
                if (_subscription.DeliveryMode.Encryption)
                {
                    PubNubServiceImplementation("", _subscription.DeliveryMode.SubscriberKey, _subscription.DeliveryMode.SecretKey, _subscription.DeliveryMode.EncryptionKey, false);
                }
                else
                {
                    PubNubServiceImplementation("", _subscription.DeliveryMode.SubscriberKey);
                }
                Subscribe(_subscription.DeliveryMode.Address, "", NotificationReturnMessage, SubscribeConnectStatusMessage, ErrorMessage);
                subscribed = true;
                SetTimeout();
                return response;
            }
            catch (Exception e)
            {
                Unsubscribe();
                throw e;
            }

        }
        public void Unsubscribe()
        {
            ClearTimeout();
            if (_pubnub != null) Unsubscribe(_subscription.DeliveryMode.Address, "", NotificationReturnMessage, SubscribeConnectStatusMessage, DisconnectMessage, ErrorMessage);
            _subscription = new Subscription();
            ClearEvents();
            subscribed = false;

        }
        private string GetFullEventsFilter()
        {
            var fullEventsFilter = "{ \"eventFilters\": ";
            string eventFiltersToString = "[ ";
            foreach (string filter in eventFilters)
            {
                eventFiltersToString += ("\"" + filter + "\",");
            }
            eventFiltersToString = eventFiltersToString.TrimEnd(',');
            eventFiltersToString += "]";
            fullEventsFilter += (eventFiltersToString + ", \"deliveryMode\" : { \"transportType\" : \"PubNub\" } }");
            return fullEventsFilter;
        }

        private void PubNubServiceImplementation(string publishKey, string subscribeKey)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey);
        }

        public void PubNubServiceImplementation(string publishKey, string subscribeKey, string secretKey, string cipherKey, bool sslOn)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey);
            _encrypted = true;
            var aes = new AesManaged { Key = Convert.FromBase64String(cipherKey), Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
            _decrypto = aes.CreateDecryptor();
        }

        public void Subscribe(string channel, string channelGroup, Action<object> userCallback,
              Action<object> connectCallback, Action<object> errorCallback)
        {
            _pubnub.Subscribe<string>(channel, channelGroup, userCallback,
                connectCallback, errorCallback);
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> disconnectCallback, Action<object> errorCallback)
        {
            _pubnub.Unsubscribe(channel, userCallback, connectCallback,
                disconnectCallback, errorCallback);
        }
        private void NotificationReturnMessage(object message)
        {
            if (_encrypted) _events["notification"] = DecryptMessage(message);
            else _events["notification"] = JObject.Parse(JsonConvert.DeserializeObject<List<string>>(message.ToString())[0]);
             Debug.WriteLine("Subscribe Message: " + message);
        }

        private void SubscribeConnectStatusMessage(object message)
        {
            _events["connectMessage"] = message;
             Debug.WriteLine("Connect Message: " + message);
        }

        private void ErrorMessage(object message)
        {
            _events["errorMessage"] = message;
            Debug.WriteLine("Error Message: " + message);
        }

        private void DisconnectMessage(object message)
        {
            _events["disconnectMessage"] = message;
            Debug.WriteLine("Disconnect Message: " + message);
        }
        private JObject DecryptMessage(object message)
        {

            var deserializedMessage = JsonConvert.DeserializeObject<List<string>>(message.ToString());
            byte[] decoded64Message = Convert.FromBase64String(deserializedMessage[0]);
            byte[] decryptedMessage = _decrypto.TransformFinalBlock(decoded64Message, 0, decoded64Message.Length);
            deserializedMessage[0] = Encoding.UTF8.GetString(decryptedMessage);
            return JObject.Parse(deserializedMessage[0]);
        }

    }
}