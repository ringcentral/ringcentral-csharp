# RingCentral SDK for C&#35;

[![NuGet][nuget-version-svg]][nuget-version-link]
[![NuGet][nuget-count-svg]][nuget-count-link]
[![Build Status][build-status-svg]][build-status-link]
[![Coverage Status][coverage-status-svg]][coverage-status-link]
[![License][license-svg]][license-link]

## Table of contents

1. [Installation](#installation)
  1. [Additional Instructions for PubNub](#additional-instructions-for-pubnub)
1. [Basic Usage](#basic-usage)
  1. [API Developer Guide](#api-developer-guide)
  1. [Initialization](#initialization)
    1. [Set User Agent Header](#set-user-agent-header)
  2. [OAuth 2.0 Authorization](#oauth-2.0-authorization)
    1. [Authorize](#authorize)
    1. [Refresh](#refresh)
    1. [Logout](#logout)
  3. [Quick Recipes](#quick-recipes)
    1. [Send SMS](#send-sms)
    1. [Send Fax](#send-fax)
    1. [Get Account Information](#get-account-information)
    1. [Get Address Book](#get-address-book)
    1. [Using x-http-method-override Header](#using-x-http-method-override-header)
  4. [Message Store](#message-store)
    1. [Get Message Store](#get-message-store)
    1. [Get Message Store First ID](#get-message-store-first-id)
    1. [Update Message Status](#update-message-status)
      1. [Update Message Status using x-http-override-header](#update-message-using-x-http-override-header)
    1. [Delete Message](#delete-message)
  1. [Subscription](#subscription)
    1. [Create Subscription](#create-subscription)
      1. [Using Default Callbacks](#create-subscription-using-default-callbacks)
      1. [Using Explicit Callbacks](#create-subscription-using-explicit-callbacks)
    1. [Casting on PubNub Notification](#casting-on-pubnub-notification)
      1. [Casting on Connect](#casting-on-connect)
      1. [Casting on Disconnect](#casting-on-disconnect)
      1. [Casting on Error](#casting-on-error)
    1. [Example PubNub Notification Message](#example-pubnub-notification-message)
    1. [Delete Subscription](#delete-subscription)
    1. [Unsubscribe from Subscription](#unsubscribe-from-subscription)
    1. [Access PubNub Message from Subscription](#access-pubnub-message-from-subscription)
1. [Support](#support)
1. [Contributions](#contributions)
1. [License](#license)

## Installation

Via NuGet

```
PM> Install-Package RingCentralSDK 
```

This will download the Ring Central Portable Class Library into your project as well as the [PubNub](https://github.com/pubnub/c-sharp "PubNub") dependencies

### Additional Instructions for PubNub

PubNub will need to manually be installed in your project.  Find the platform you are targeting at [PubNub](https://github.com/pubnub/c-sharp "PubNub") and follow the instructions to include the library in your project.

## Basic Usage

### API Developer Guide

This SDK wraps the RingCentral Connect Platform API which is documented in the [RingCentral Connect Platform Developer Guide](https://developer.ringcentral.com/api-docs/latest/index.html). For additional information, please refer to the Developer Guide.

### Initialization

```cs
//import RingCentral SDK
using RingCentral;
```

```cs
//Initialize Ring Central Client
var ringCentral = new SDK("your appKey", "your appSecret", "Ring Central apiEndPoint", "Application Name","Application Version").GetPlatform();
```

#### Set User Agent Header

```cs
ringCentral.SetUserAgentHeader("Application Name", "Application Version");
```

### OAuth 2.0 Authorization

#### Authorize

```cs
Response response = ringCentral.Authorize(userName, extension, password, true);
```

#### Refresh

```cs
Response response = ringCentral.Refresh();
```

#### Logout

```cs
ringCentral.Logout();
```

### Quick Recipes

#### Send SMS
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonSmsString);
Response response = ringCentral.Post(request);
```

#### Send Fax
```cs
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

#### Get Account Information
```cs
Request request = new Request("/restapi/v1.0/account/~");
Response response = ringCentral.Get(request);
```

### Get Address Book
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
Response response = ringCentral.Get(request);
```

#### Using x-http-method-override Header
```cs
Request overRideRequest = new Request("/restapi/v1.0/account/~");
overRideRequest.SetXhttpOverRideHeader("GET");
Response overRideResponse = ringCentral.Post(overRideRequest);
```

### Message Store

#### Get Message Store
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
Response response = ringCentral.Get(request);
```

#### Get Message Store First ID
```cs
var messageId = response.GetJson().SelectToken("records")[0].SelectToken("id");
```

#### Update Message Status
```cs
var messageStatusJson = "{\"readStatus\": \"Read\"}";
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
Response response = ringCentral.Put(request);
```

##### Update Message Status using x-http-ovverride-header
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.SetXhttpOverRideHeader("PUT"); 
Response response = ringCentral.Post(request);
```

#### Delete Message
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
Response response = ringCentral.Delete(request);
```

### Subscription

RingCentral provides the ability to subscribe for event data using PubNub.

#### Create Subscription

##### Create Subscription using Default Callbacks

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

##### Create Subscription using Explicit Callbacks 

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

#### Casting PubNub Notifications

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

##### Casting on Connect

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

##### Casting on Disconnect

Note: Disconnect messages are not deserializable JSON.

```cs
public void ActionOnDisconnect(object message) {
	var receivedMessage = message.ToString();
}
```

##### Casting on Error

Note: PubNub error messages are not deserializable JSON.

```cs
public void ActionOnError(object error) {
	var receivedMessage = message.ToString();
}
```

#### Example PubNub Notification Message

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

#### Delete Subscription

```cs
var response = subscription.Remove();
```

#### Unsubscribe from Subscription

Note: If you provided a callback action for PubNub disconnect it will fire once during unsubscribe.

```cs
subscription.Unsubscribe();
```

#### Access PubNub Message from Subscription

```cs
var notificationMessage = subscription.ReturnMessage("notification");
var connectMessage = subscription.ReturnMessage("connectMessage");
var disconnectMessage = subscriptionS.ReturnMessage("disconnectMessage");
var errorMessage = subscription.ReturnMessage("errorMessage");
```

## Links

Project Repo

* https://github.com/ringcentral/ringcentral-csharp

RingCentral Developer Site

* https://developer.ringcentral.com

RingCentral API Docs

* https://developer.ringcentral.com/library.html

RingCentral API Explorer

* http://ringcentral.github.io/api-explorer

RingCentral GitHub Organization

* https://github.com/ringcentral

## Support

For support using this SDK, please use the following resources:

1. [RingCentral Developer Community](https://devcommunity.ringcentral.com)
1. [RingCentral C# SDK GitHub repo](https://github.com/ringcentral/ringcentral-csharp)
1. [Stack Overflow](http://stackoverflow.com/)

## Contributions

Any reports of problems, comments or suggestions are most welcome.

Please report these on [Github](https://github.com/ringcentral/ringcentral-csharp)

## License

RingCentral SDK is available under an MIT-style license. See [LICENSE](LICENSE) for details.

RingCentral SDK &copy; 2015 by RingCentral.

 [nuget-version-svg]: https://img.shields.io/nuget/v/RingCentralSDK.svg
 [nuget-version-link]: http://www.nuget.org/packages/RingCentralSDK/
 [nuget-count-svg]: https://img.shields.io/nuget/dt/RingCentralSDK.svg
 [nuget-count-link]: http://www.nuget.org/packages/RingCentralSDK/
 [build-status-svg]: https://ci.appveyor.com/api/projects/status/ka1g6n869rxw81g4?svg=true
 [build-status-link]: https://ci.appveyor.com/project/paulzolnierczyk/ringcentral-csharp
 [coverage-status-svg]: https://coveralls.io/repos/ringcentral/ringcentral-csharp/badge.svg?branch=develop&service=github
 [coverage-status-link]: https://coveralls.io/github/ringcentral/ringcentral-csharp
 [license-svg]: https://img.shields.io/badge/license-MIT-blue.svg
 [license-link]: https://github.com/ringcentral/ringcentral-csharp/blob/master/LICENSE