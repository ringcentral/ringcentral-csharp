using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PubNubMessaging.Core;
using RingCentral.Subscription;

namespace RingCentral.NET40.Test
{
    public class SubscriptionServiceMock : ISubscriptionService
    {
        private readonly Pubnub _pubnub;
        public string Channel,ChannelGroup;
        public List<object> Message;
        public PubnubClientError Error;

        public SubscriptionServiceMock(string publishKey, string subscribeKey, string secretKey, string cipherKey, bool sslOn)
            
        {
            _pubnub = new Pubnub(publishKey, subscribeKey, secretKey, cipherKey, sslOn);
            _pubnub.GrantAccess<string>("RCNETSDK-TEST", true, true, 20, SubscribeConnectStatusMessage, ErrorMessage);
        }

        public void Subscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> errorCallback)
        {
            Channel = channel;
            ChannelGroup = channelGroup;
            _pubnub.Subscribe<string>(channel, channelGroup, SetMessage,
                SubscribeConnectStatusMessage, ErrorMessage);
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> disconnectCallback, Action<object> errorCallback)
        {
            _pubnub.Unsubscribe<string>(channel, SubscribeReturnMessage, SubscribeConnectStatusMessage,
                DisconnectMessage, ErrorMessage);
        }

        public void PublishMessage(object message)
        {
            _pubnub.Publish<string>(Channel, message, true, SetMessage, ErrorMessage);
        }

        public void SetMessage(object message)
        {
            Message = _pubnub.JsonPluggableLibrary.DeserializeToListOfObject((string)message);
        }

        public void SubscribeReturnMessage(object message)
        {
            
            Message = _pubnub.JsonPluggableLibrary.DeserializeToListOfObject((string)message);
            Debug.WriteLine("Subscribe Message: " + message);
        }

        public void ErrorMessage(object message)
        {

            Error = (PubnubClientError)message;
            Debug.WriteLine("Error Message: " + message);
        }

        public void SubscribeConnectStatusMessage(object message)
        {
            Message = _pubnub.JsonPluggableLibrary.DeserializeToListOfObject((string)message);
            Debug.WriteLine("Connect Message: " + message);
        }
        public void DisconnectMessage(object message)
        {
            Message = _pubnub.JsonPluggableLibrary.DeserializeToListOfObject((string)message);
            Debug.WriteLine("Disconnect Message: " + message);
        }


    }
}
