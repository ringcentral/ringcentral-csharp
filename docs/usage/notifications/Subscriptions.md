# Notifications: Subscription API

# Subscription

RingCentral provides the ability to subscribe for event data using PubNub.

## Create Subscription

### Create Subscription using Default Callbacks

```cs
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral};
subscription.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
var response = subscription.Subscribe(null,null,null);
```

Alternatively you can set Event Filters by:
```cs
subscription.SetEvent(listOfEvents);
```
Where listOfEvents is a List<string> containing each event to subscribe to.

### Create Subscription using Explicit Callbacks 

```cs
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral};
subscription.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
var response = subscription.Subscribe(ActionOnNotification,ActionOnConnect,ActionOnError);
```
Note: You can assign the callback action for disconnect on initialization. Disconnect Action fired upon PubNub disconnect

```cs
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral, disconnectAction = ActionOnDisconnect};
```

Or after initialization

```cs
subscription.disconnectAction =  ActionOnDisconnect;
```

All callbacks must take only one parameter of type object.  See below for proper casting on actions.

## Casting PubNub Notifications

**This will return an object that can easily be cast to a string or a JArray (Json.Net)**
**Messages will be decrypted, if required, before being passed to Actions. See below for an example of JSON returned.**

Use a JArray to grab a token

```cs
public void ActionOnMessage(object message) {
	var ReceivedMessage = ((JArray)message).SelectToken("[0].body.changes[0].type");  
}
```

Or string for other JSON parsing

```cs
public void ActionOnMessage(object message) {
	var ReceivedMessage = message.ToString();  
}
```

### Casting on Connect

Use a JArray to grab a token.

```cs
public void ActionOnConnect(object message){
	var receivedMessage = ((JArray)receivedMessage).SelectToken("[1]");
}
```

Or string for other JSON parsing

```cs
public void ActionOnConnect(object message) {
	var ReceivedMessage = message.ToString();  
}
```

### Casting on Disconnect

Note: Disconnect messages are not deserializable JSON.

```cs
public void ActionOnDisconnect(object message) {
	var receivedMessage = message.ToString();
}
```

### Casting on Error

Note: PubNub error messages are not deserializable JSON.

```cs
public void ActionOnError(object error) {
	var receivedMessage = message.ToString();
}
```

## Example PubNub Notification Message

This example provides some possible tokens for JSON parsing of PubNub Notification message

```json
[
	{
	  "event": "/restapi/v1.0/account/~/extension/111/message-store",
	  "body": {
	    "lastUpdated": "2015-10-10T21:28:43.094-07:00",
	    "changes": [
	      {
	        "newCount": 0,
	        "updatedCount": 1,
	        "type": "Fax"
	      },
	      {
	        "newCount": 2,
	        "updatedCount": 0,
	        "type": "SMS"
	      }
	    ],
	    "extensionId": 1111
	  },
	  "uuid": "111-222-333-444",
	  "timestamp": "2015-10-11T04:28:51.821Z"
	}
]
```

## Delete Subscription

```cs
var response = subscription.Remove();
```

## Unsubscribe from Subscription

Note: If you provided a callback action for PubNub disconnect it will fire once during unsubscribe.

```cs
subscription.Unsubscribe();
```

## Access PubNub Message from Subscription

```cs
var notificationMessage = subscription.ReturnMessage("notification");
var connectMessage = subscription.ReturnMessage("connectMessage");
var disconnectMessage = subscriptionS.ReturnMessage("disconnectMessage");
var errorMessage = subscription.ReturnMessage("errorMessage");
```

## FAQ

### How can I subscribe to all extensions

To subscribe to presence events for all extensions, create a set of event filters including filters for every extension, and then create a subscription including all the event filters. A presence event filter includes the accound id and extension id. Here is an example of a single presence event filter using the account id for the authorized session `["/restapi/v1.0/account/~/extension/111111/presence"]`. A set of presence event filters looks like the following: `["/restapi/v1.0/account/~/extension/111111/presence". "/restapi/v1.0/account/~/extension/222222/presence"]`. A full set of extension ids can be retrieved via the extension endpoint: `/restapi/v1.0/account/~/extension`. This has been tested with a single subscription API call and a set of over 2000 extensions.