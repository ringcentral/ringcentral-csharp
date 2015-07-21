using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RingCentral.Subscription;

namespace RingCentral.NET40
{
    public class SubscriptionServiceImplementation : ISubscriptionService
    {
        public void Subscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<SubscriptionError> errorCallback)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(string channel, string channelGroup, Action<object> userCallback, Action<object> connectCallback, Action<object> disconnectCallback, Action<SubscriptionError> errorCallback)
        {
            throw new NotImplementedException();
        }
    }
}
