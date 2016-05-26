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

/*

{
  "uri" : "https://platform.devtest.ringcentral.com/restapi/v1.0/subscription/40eeffda-e425-47f8-8127-d26dff22253c",
  "id" : "40eeffda-e425-47f8-8127-d26dff22253c",
  "creationTime" : "2016-05-26T10:38:15.804Z",
  "status" : "Active",
  "eventFilters" : [ "/restapi/v1.0/account/130829004/extension/130829004/message-store", "/restapi/v1.0/account/130829004/extension/130829004/presence" ],
  "expirationTime" : "2016-05-26T10:53:15.808Z",
  "expiresIn" : 899,
  "deliveryMode" : {
    "transportType" : "PubNub",
    "encryption" : true,
    "address" : "4132724593900141_7526c66b",
    "subscriberKey" : "sub-c-b8b9cd8c-e906-11e2-b383-02ee2ddab7fe",
    "encryptionAlgorithm" : "AES",
    "encryptionKey" : "KfpwGgFQiUUZmsGA1VkCqw=="
  }
}

*/
