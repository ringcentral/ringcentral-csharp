using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using PubNubMessaging;
using System.Diagnostics;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceImplementation : ISubscriptionService
    {
        private PubNubMessaging.Core.Pubnub _pubnub;

        public SubscriptionServiceImplementation(string publishKey, string subscribeKey)
        {
            _pubnub = new PubNubMessaging.Core.Pubnub(publishKey,subscribeKey);
        }

        public void Subscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<SubscriptionError> errorCallback)
        {
            _pubnub.Subscribe<string>(channel, channelGroup, DisplaySubscribeReturnMessage, DisplaySubscribeConnectStatusMessage, DisplayErrorMessage);
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<object> disconnectCallback, Action<SubscriptionError> errorCallback)
        {
            _pubnub.Unsubscribe(channel,DisplaySubscribeReturnMessage,DisplaySubscribeConnectStatusMessage,DisplayDisconnectMessage,DisplayErrorMessage);
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
