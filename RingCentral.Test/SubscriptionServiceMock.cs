using System;
using System.Collections.Generic;
using System.Diagnostics;
using PubNubMessaging.Core;


namespace RingCentral.Test
{
    public class SubscriptionServiceMock
    {
        private readonly Pubnub _pubnub;
        private Dictionary<string, object> Events = new Dictionary<string, object>
        {
            {"notification",""},
            {"errorMessage",""},
            {"connectMessage", ""},
            {"disconnectMessage",""}
        };

        public SubscriptionServiceMock(string publishKey, string subscribeKey, string secretKey, string cipherKey, bool sslOn)
            
        {
            _pubnub = new Pubnub(publishKey, subscribeKey, secretKey, cipherKey, sslOn);
            _pubnub.GrantAccess<string>("RCNETSDK-TEST", true, true, 20, SubscribeConnectStatusMessage, ErrorMessage);
        }

        public void Subscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> errorCallback)
        {
                       _pubnub.Subscribe<string>(channel, channelGroup, NotificationMessage,
                SubscribeConnectStatusMessage, ErrorMessage);
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> disconnectCallback, Action<object> errorCallback)
        {
            _pubnub.Unsubscribe<string>(channel, NotificationMessage, SubscribeConnectStatusMessage,
                DisconnectMessage, ErrorMessage);
        }

        public void PublishMessage(object message)
        {
            _pubnub.Publish<string>("RCNETSDK-TEST", message, true, NotificationMessage, ErrorMessage);
        }

 
        public void NotificationMessage(object message)
        {
            Events["notification"] = message;
            Debug.WriteLine("Subscribe Message: " + message);
        }

        public void SubscribeConnectStatusMessage(object message)
        {
            Events["connectMessage"] = message;
            Debug.WriteLine("Connect Message: " + message);
        }

        public void ErrorMessage(object message)
        {
            Events["errorMessage"] = message;
            Debug.WriteLine("Error Message: " + message);
        }

        public void DisconnectMessage(object message)
        {
            Events["disconnectMessage"] = message;
            Debug.WriteLine("Disconnect Message: " + message);
        }

        public object ReturnMessage(string requestedMessage)
        {
            if (Events.ContainsKey(requestedMessage)) return Events[requestedMessage];
            else return "Error: Message not found";
        }

    }
}
