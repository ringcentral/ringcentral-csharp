using System;
using System.Collections.Generic;

namespace RingCentral.Subscription
{
    public class Subscription
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public List<string> EventFilters { get; set; }
        public DateTime ExperationTime { get; set; }
        public long ExpiresIn { get; set; }
        public DeliveryMode DeliveryMode { get; set; }
        public string Status { get; set; }
        public string Uri { get; set; }
    }

    public class DeliveryMode
    {
        public string TransportType { get; set; }
        public bool Encryption { get; set; }
        public string Address { get; set; }
        public string SubscriberKey { get; set; }
        public string SecretKey { get; set; }
        public string EncryptionAlgorithm { get; set; }
        public string EncryptionKey { get; set; }
    }
}