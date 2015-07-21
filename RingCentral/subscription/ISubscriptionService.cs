using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingCentral.Subscription
{
    public interface ISubscriptionService
    {
        void Subscribe(string channel, string channelGroup, Action<object> userCallback,Action<object> connectCallback, Action<SubscriptionError> errorCallback);

        void Unsubscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<object> disconnectCallback, Action<SubscriptionError> errorCallback);
    }
}
