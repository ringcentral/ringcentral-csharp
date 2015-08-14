[![Build status](https://ci.appveyor.com/api/projects/status/ka1g6n869rxw81g4?svg=true)](https://ci.appveyor.com/project/paulzolnierczyk/ringcentral-csharp)
[![Coverage Status](https://coveralls.io/repos/ringcentral/ringcentral-csharp/badge.svg?branch=develop&service=github)](https://coveralls.io/github/ringcentral/ringcentral-csharp?branch=develop)
# RingCentral SDK for C&#35;

# Installation

Via NuGet

```
Install-Package RingCentralSDK 
```

This will download the Ring Central Portable Class Library into your project as well as the [Pubnub](https://github.com/pubnub/c-sharp "PubNub") dependencies

## Additional Instructions for Xamarin Platforms

PubNub will need to manually be installed in your project.  Find the platform you are targeting at [Pubnub](https://github.com/pubnub/c-sharp "PubNub") and follow the instructions to include the library in your project.

# Basic Usage

## Initialization

```
//import RingCentral SDK
using RingCentral;
```

```
//Initialze Ring Central Client
var ringCentral = new RingCentralClient("your appKey", "your appSecret", "Ring Central apiEndPoint").GetPlatform();
```


## Recipes
### Set User Agent Header
```
ringCentral.SetUserAgentHeader("<YOUR USER AGENT HEADER>");
```

### Authenticate
```
Response response = ringCentral.Authenticate(userName, password, extension, true);
````

### Refresh
```
Response response = ringCentral.Refresh();
```

### Logout
```
ringCentral.Revoke();
```

### x-http-method-override
```
Request overRiderequest = new Request("/restapi/v1.0/account/~");
overRiderequest.SetXhttpOverRideHeader("GET");
Response overRideResponse = ringCentral.PostRequest(overRiderequest);
```

### Get Account Information
```
Request request = new Request("/restapi/v1.0/account/~");
Response response = ringCentral.GetRequest(request);
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
Response response = ringCentral.PostRequest(request);
```

### Send SMS
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonSmsString);
Response response = ringCentral.PostRequest(request);
```

### Get Address Book
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
Response response = ringCentral.GetRequest(request);
```

### Get Message Store
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
Response response = ringCentral.GetRequest(request);
```

### Get the First id in the Message Store
```
var messageId = response.GetJson().SelectToken("records")[0].SelectToken("id");
```

### Update Message Status
```
var messageStatusJson = "{\"readStatus\": \"Read\"}";
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
Response response = ringCentral.PutRequest(request);
```

### Update Message Status via x-http-ovverride-header
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.SetXhttpOverRideHeader("PUT");
Response response = ringCentral.PostRequest(request);
```

### Delete Message
```
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
Response response = ringCentral.DeleteRequest(request);
```

### Create Subscription
```
var jsonData = "{\"eventFilters\": [ \"/restapi/v1.0/account/~/extension/~/presence\",\"/restapi/v1.0/account/~/extension/~/message-store\" ], \"deliveryMode\": { \"transportType\": \"PubNub\", \"encryption\": \"false\" } }";
Request request = new Request("/restapi/v1.0/subscription" jsonData);
Response result = ringCentral.PostRequest(request);
```

### Delete Subscription
```
Request request = new Request("/restapi/v1.0/subscription{subscriptionId}");
Response result = RingCentralClient.GetPlatform().DeleteRequest(request);
```

### Subscribing for server events through PubNub
```
SubscriptionServiceImplementation subscriptionServiceImplementation = new SubscriptionServiceImplementation("", "Subscriber Key");

subscriptionServiceImplementation.Subscribe("Channel Name", "Channel Group", DisplaySubscribeReturnMessage,  DisplaySubscribeConnectStatusMessage, DisplayErrorMessage)

public void DisplaySubscribeReturnMessage(object message)
{
	Debug.WriteLine("Subscribe Message: " + message);
}

public void DisplaySubscribeConnectStatusMessage(object message)
{
	Debug.WriteLine("Connect Message: " + message);
}

public void DisplayErrorMessage(object message)
{
	Debug.WriteLine("Error Message: " + message);
}
```

Note: Channel Name and Subscriber key are both provided from Subscription result. Channel Name is the field "address" in the DeliveryMode of Subscription result.  

###Unsubscribe from PubNub 
```
subscriptionServiceImplementation.Unsubscribe("Channel Name", "Channel Group", DisplaySubscribeReturnMessage, DisplaySubscribeConnectStatusMessage, DisplayDisconnectMessage, DisplayErrorMessage)

public void DisplayDisconnectMessage(object message)
{
	Debug.WriteLine("Disconnect Message: " + message);
}

public void DisplaySubscribeReturnMessage(object message)
{
	Debug.WriteLine("Subscribe Message: " + message);
}

public void DisplaySubscribeConnectStatusMessage(object message)
{
	Debug.WriteLine("Connect Message: " + message);
}

public void DisplayErrorMessage(object message)
{
	Debug.WriteLine("Error Message: " + message);
}
```






