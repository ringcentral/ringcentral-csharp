using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PubNubMessaging.Core;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceImplementation : ISubscriptionService
    {
        private readonly Pubnub _pubnub;
        private const string Tag = "RingCentral Android SDK";

        public SubscriptionServiceImplementation(string publishKey, string subscribeKey)
        {
            _pubnub = new Pubnub(publishKey, subscribeKey);
        }

        public void Subscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<object> errorCallback)
        {
            _pubnub.Subscribe<string>(channel, channelGroup, DisplaySubscribeReturnMessage,
                DisplaySubscribeConnectStatusMessage, DisplayErrorMessage);
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback,
            Action<object> disconnectCallback, Action<object> errorCallback)
        {
            _pubnub.Unsubscribe(channel, DisplaySubscribeReturnMessage, DisplaySubscribeConnectStatusMessage,
                DisplayDisconnectMessage, DisplayErrorMessage);
        }

        public void DisplaySubscribeReturnMessage(object message)
        {
            Log.Debug(Tag, "Subscribe Message: " + message);
        }

        public void DisplaySubscribeConnectStatusMessage(object message)
        {
            Log.Debug(Tag, "Connect Message: " + message);
        }

        public void DisplayErrorMessage(object message)
        {
            Log.Debug(Tag, "Error Message: " + message);
        }

        public void DisplayDisconnectMessage(object message)
        {
            Log.Debug(Tag, "Disconnect Message: " + message);
        }

        
    }
}