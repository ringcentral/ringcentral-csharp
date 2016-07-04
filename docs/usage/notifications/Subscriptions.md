# Notifications: Subscription API

## Subscription

RingCentral provides the ability to subscribe for event data using PubNub.

### Create Subscription

#### About Pubnub SSL

Pubnub supports SSL. But we are not taking advantages of it. Before publishing data to Pubnub, we do encryption on our server side. And this SDK is responsible for decryption of data automatically. Users of this SDK don't need to do anything to enable SSL, still can be assured that data is being protected by encryption.

In the future, we might switch to Pubnub SSL. And it won't affect SDK users since we will keep the API identical.

#### Sample code

```cs
var subscription = sdk.CreateSubscription();
subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/message-store");
subscription.EventFilters.Add("/restapi/v1.0/account/~/extension/~/presence");
subscription.ConnectEvent += (sender, args) => {
    Console.WriteLine("Connected:");
    Console.WriteLine(args.Message);
};
subscription.NotificationEvent += (sender, args) => {
    Console.WriteLine("Notification:");
    Console.WriteLine(args.Message);
};
subscription.ErrorEvent += (sender, args) => {
    Console.WriteLine("Error:");
    Console.WriteLine(args.Message);
};
subscription.Register();
```

Alternatively you can set Event Filters by:
```cs
subscription.EventFilters = listOfEvents;
```
Where listOfEvents is a `List<string>` containing each event to subscribe to.


### Lifecyle management

Users of this SDK are responsible for managing the lifecyle of `subscription`. In order to keep it alive, you need to make sure that it will not be garbage collected. So most likely you want to make `subscription` a class variable instead of a local variable.


### Casting SubscriptionEventArgs

#### Casting on Notification

`args.Message` is an object that can easily be cast to a string or a JArray/JObject (Json.Net). See below for examples of JSON data.**

Use a JObject to grab a token

```cs
public void ActionOnMessage(object sender, SubscriptionEventArgs args) {
    var message = ((JObject)args.Message).SelectToken("body.changes[0].type").ToString();
}
```

Or string for other JSON parsing

```cs
public void ActionOnMessage(object sender, SubscriptionEventArgs args) {
    var message = args.Message.ToString();
}
```

#### Casting on Connect

Use a JArray to grab a token.

```cs
public void ActionOnConnect(object sender, SubscriptionEventArgs args){
    var message = ((JArray)args.Message).SelectToken("[1]").ToString();
}
```

Or string for other JSON parsing

```cs
public void ActionOnConnect(object sender, SubscriptionEventArgs args) {
    var message = args.Message.ToString();
}
```

#### Casting on Error

Note: PubNub error messages are not deserializable JSON.

```cs
public void ActionOnError(object sender, SubscriptionEventArgs args) {
    var error = args.Message.ToString();
}
```

### Example Notification Messages

`message-store` example:

```json
{
  "uuid": "32a089d4-bd5d-4259-8db4-feed54e8dcd9",
  "event": "/restapi/v1.0/account/~/extension/850957020/message-store",
  "timestamp": "2016-05-30T03:18:22.424Z",
  "subscriptionId": "a4b70629-ac59-4a0c-a479-bf81c591df7c",
  "body": {
    "extensionId": 850957020,
    "lastUpdated": "2016-05-30T11:18:10.624+08:00",
    "changes": [
      {
        "type": "SMS",
        "newCount": 1,
        "updatedCount": 0
      }
    ]
  }
}
```

`presence` example:

```json
{
  "uuid": "a78f2232-c627-48b8-9aa9-74a8adef63fa",
  "event": "/restapi/v1.0/account/~/extension/850957020/presence",
  "timestamp": "2016-05-30T03:18:42.184Z",
  "subscriptionId": "a4b70629-ac59-4a0c-a479-bf81c591df7c",
  "body": {
    "extensionId": 850957020,
    "telephonyStatus": "Ringing"
  }
}
```

### Delete Subscription

```cs
subscription.Remove();
```

## FAQ

### How can I subscribe to all extensions

To subscribe to presence events for all extensions, create a set of event filters including filters for every extension, and then create a subscription including all the event filters. A presence event filter includes the accound id and extension id.

Here is an example of a single presence event filter using the account id for the authorized session `["/restapi/v1.0/account/~/extension/111111/presence"]`.

A set of presence event filters looks like the following: `["/restapi/v1.0/account/~/extension/111111/presence". "/restapi/v1.0/account/~/extension/222222/presence"]`.

A full set of extension ids can be retrieved via the extension endpoint: `/restapi/v1.0/account/~/extension`. This has been tested with a single subscription API call and a set of over 2000 extensions.
