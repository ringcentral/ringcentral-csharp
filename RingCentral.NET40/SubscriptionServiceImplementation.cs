using System;
using System.Diagnostics;
using PubNubMessaging.Core;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceImplementation : ISubscriptionService
    {
        private readonly Pubnub _pubnub;


        public SubscriptionServiceImplementation(string publishKey, string subscribeKey)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey);
        }

        public SubscriptionServiceImplementation(string publishKey, string subscribeKey, string secretKey)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey, secretKey);
        }

        public SubscriptionServiceImplementation(string publishKey, string subscribeKey, string secretKey,string cipherKey,bool sslOn)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey, secretKey, cipherKey, sslOn);
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

        public void DisplaySubscribeReturnMessage(object message)
        {
            Debug.WriteLine("Subscribe Message: " + message);
        }

        public void DisplaySubscribeConnectStatusMessage(object message)
        {
            Debug.WriteLine("Connect Message: " + message);
        }

        public void DisplayErrorMessage(object message)
        {
            Debug.WriteLine("Error Message: " + message);
        }

        public void DisplayDisconnectMessage(object message)
        {
            Debug.WriteLine("Disconnect Message: " + message);
        }
    }
}