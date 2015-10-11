[![Build status](https://ci.appveyor.com/api/projects/status/ka1g6n869rxw81g4?svg=true)](https://ci.appveyor.com/project/paulzolnierczyk/ringcentral-csharp)
[![Coverage Status](https://coveralls.io/repos/ringcentral/ringcentral-csharp/badge.svg?branch=develop&service=github)](https://coveralls.io/github/ringcentral/ringcentral-csharp?branch=develop)
# RingCentral SDK for C&#35;

# Installation

Via NuGet

```
Install-Package RingCentralSDK 
```

This will download the Ring Central Portable Class Library into your project as well as the [PubNub](https://github.com/pubnub/c-sharp "PubNub") dependencies

## Additional Instructions for PubNub

PubNub will need to manually be installed in your project.  Find the platform you are targeting at [PubNub](https://github.com/pubnub/c-sharp "PubNub") and follow the instructions to include the library in your project.

# Basic Usage

## Initialization

```
//import RingCentral SDK
using RingCentral;
```

```
//Initialze Ring Central Client
var ringCentral = new SDK("your appKey", "your appSecret", "Ring Central apiEndPoint", "Application Name","Application Version").GetPlatform();
```


## Recipes
### Set User Agent Header
```
ringCentral.SetUserAgentHeader("Application Name", "Application Version");
```

### Authorize
```
Response response = ringCentral.Authorize(userName, extension, password, true);
````

### Refresh
```
Response response = ringCentral.Refresh();
```

### Logout
```
ringCentral.Logout();
```

### x-http-method-override
```
Request overRiderequest = new Request("/restapi/v1.0/account/~");
overRiderequest.SetXhttpOverRideHeader("GET");
Response overRideResponse = ringCentral.Post(overRiderequest);
```

### Get Account Information
```
Request request = new Request("/restapi/v1.0/account/~");
Response response = ringCentral.Get(request);
```

### Send Fax
```
const string text = "Hello world!";

var byteArrayText = System.Text.Encoding.UTF8.GetBytes(text);
var attachment = new Attachment("test.txt", "application/octet-stream", byteArrayText);
var attachment2 = new Attachment("test2.txt", "text/plain", byteArrayText);
var pdfFile = File.ReadAllBytes("<PATH TO YOUR PDF>");
var attachment3 = new Attachment("<NAME OF YOUR PDF.pdf", "application/pdf", pdfFile);
var attachments = new List<Attachment> { attachment,attachment2, attachment3 };
var json = "{\"to\":[{\"phoneNumber\":\"<YOUR TARGET NUMBER>\"}],\"faxResolution\":\"High\"}";

Request request = new Request("/restapi/v1.0/account/~/extension/~/fax", json, attachments);
Response response = ringCentral.Post(request);
```

### Send SMS
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonSmsString);
Response response = ringCentral.Post(request);
```

### Get Address Book
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
Response response = ringCentral.Get(request);
```

### Get Message Store
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
Response response = ringCentral.Get(request);
```

### Get the First id in the Message Store
```
var messageId = response.GetJson().SelectToken("records")[0].SelectToken("id");
```

### Update Message Status
```
var messageStatusJson = "{\"readStatus\": \"Read\"}";
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
Response response = ringCentral.Put(request);
```

### Update Message Status via x-http-ovverride-header
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.SetXhttpOverRideHeader("PUT"); 
Response response = ringCentral.Post(request);
```

### Delete Message
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
Response response = ringCentral.Delete(request);
```

### Create Subscription with default call backs
``` 
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral};
subscription.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
var response = subscription.Subscribe(null,null,null);
```

Alternatively you can set Event Filters by:
```
subscription.SetEvent(listOfEvents);
```
Where listOfEvents is a List<string> containing each event to subscribe to.

### Create Subscription with Callbacks 

```
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral};
subscription.AddEvent("/restapi/v1.0/account/~/extension/~/presence");
var response = subscription.Subscribe(ActionOnNotification,ActionOnConnect,ActionOnError);
```
Note: You can assign the callback action for disconnect on initialization. Disconnect Action fired upon PubNub disconnect

```
var subscription = new SubscriptionServiceImplementation(){ _platform = ringCentral, disconnectAction = ActionOnDisconnect};
```

Or after initialization

```
subscription.disconnectAction =  ActionOnDisconnect;
```


### All callbacks must take only one parameter of type object.  See below for proper casting on actions.
### Casting on Notification from PubNub

**This will return an object that can easily be cast to a string or a JArray (Json.Net)**
**Messages will be decrypted, if required, before being passed to Actions. See below for an example of JSON returned.**
Use a JArray to grab a token
```
public void ActionOnMessage(object message) {
	var ReceivedMessage = ((JArray)message).SelectToken("[0].body.changes[0].type");  
}
```
Or string for other JSON parsing
```
public void ActionOnMessage(object message) {
	var ReceivedMessage = message.ToString();  
}
```

##### Casting on Connect
Use a JArray to grab a token. 
```
public void ActionOnConnect(object message){
	var receivedMessage = ((JArray)receivedMessage).SelectToken("[1]");
}
```
Or string for other JSON parsing
```
public void ActionOnConnect(object message) {
	var ReceivedMessage = message.ToString();  
}
```
##### Casting on Disconnect 
Note: Disconnect messages are not deserializable JSON. 
```
public void ActionOnDisconnect(object message) {
	var receivedMessage = message.ToString();
}
```

##### Casting on Error Message
Note: PubNub error messages are not deserializable JSON. 
```
public void ActionOnError(object error) {
	var receivedMessage = message.ToString();
}
```


### Example JSON returned from PubNub Notification Message
This example provides some possible tokens for JSON parsing of PubNub Notification message
```
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

### Delete Subscription

```
var response = subscription.Remove();
```

### Unsubscribe from Subscription
Note: If you provided a callback action for PubNub disconnect it will fire once during unsubscribe.
```
subscription.Unsubscribe();
```

### Access PubNub message from subscription
```
var notificationMessage = subscription.ReturnMessage("notification");
var connectMessage = subscription.ReturnMessage("connectMessage");
var disconnectMessage = subscriptionS.ReturnMessage("disconnectMessage");
var errorMessage = subscription.ReturnMessage("errorMessage");
```





