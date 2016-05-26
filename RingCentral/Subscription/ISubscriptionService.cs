using System;

namespace RingCentral.Subscription
{
    public interface ISubscriptionService
    {
        void Subscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback,
            Action<object> errorCallback);

        void Unsubscribe(string channel, string channelGroup, Action<object> userCallback,
            Action<object> connectCallback, Action<object> disconnectCallback, Action<object> errorCallback);
    }
}