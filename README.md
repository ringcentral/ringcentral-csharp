# RingCentral SDK for C&#35;

[![NuGet][nuget-version-svg]][nuget-version-link]
[![Build Status][build-status-svg]][build-status-link]
[![Coverage Status][coverage-status-svg]][coverage-status-link]
[![Docs][docs-readthedocs-svg]][docs-readthedocs-link]
[![License][license-svg]][license-link]

## Table of contents

1. [Installation](#installation)
1. [Basic Usage](#basic-usage)
    1. [API Developer Guide](#api-developer-guide)
    1. [Initialization](#initialization)
    1. [OAuth 2.0 Authorization](#oauth-20-authorization)
        1. [Login](#login)
        1. [Refresh](#refresh)
        1. [Logout](#logout)
    1. [Quick Recipes](#quick-recipes)
        1. [Send SMS](#send-sms)
        1. [Send Fax](#send-fax)
        1. [Get Account Information](#get-account-information)
        1. [Get Address Book](#get-address-book)
        1. [Using HTTP method tunneling](#using-http-method-tunneling)
    1. [Message Store](#message-store)
        1. [Get Message Store](#get-message-store)
        1. [Get Message Store First ID](#get-message-store-first-id)
        1. [Update Message Status](#update-message-status)
            1. [Update Message Status using HTTP method tunneling](#update-message-status-using-http-method-tunneling)
        1. [Delete Message](#delete-message)
    1. [Subscription](#subscription)
        1. [Create Subscription](#create-subscription)
            1. [About Pubnub SSL](#about-pubnub-ssl)
            1. [Sample code](#sample-code)
        1. [Casting SubscriptionEventArgs](#casting-subscriptioneventargs)
            1. [Casting on Notification](#casting-on-notification)
            1. [Casting on Connect](#casting-on-connect)
            1. [Casting on Error](#casting-on-error)
        1. [Example Notification Messages](#example-notification-messages)
        1. [Delete Subscription](#delete-subscription)
1. [Links](#links)
1. [Support](#support)
1. [Contributions](#contributions)
1. [License](#license)

## Installation

Via NuGet

```
PM> Install-Package RingCentralSDK
```

This will download the RingCentral Portable Class Library into your project as well as all the dependencies. The NuGet package is compabible with all popular platforms including but not limited to .NET 4.0+, Xamarin.iOS, Xamarin.Android and Xamarin.Mac.


## Basic Usage

### API Developer Guide

This SDK wraps the RingCentral Connect Platform API which is documented in the [RingCentral Connect Platform Developer Guide](https://developer.ringcentral.com/api-docs/latest/index.html). For additional information, please refer to the Developer Guide.

### Initialization

```cs
//import RingCentral
using RingCentral;

//Initialize RingCentral Client
var sdk = new SDK("your appKey", "your appSecret", "RingCentral server", "Application Name", "Application Version");
var ringCentral = sdk.Platform;
```

### OAuth 2.0 Authorization

#### Login

```cs
var response = ringCentral.Login(username, extension, password, true);
```

#### Refresh

```cs
var response = ringCentral.Refresh();
```

#### Logout

```cs
ringCentral.Logout();
```

### Quick Recipes

#### Send SMS
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonSmsString);
var response = ringCentral.Post(request);
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

var request = new Request("/restapi/v1.0/account/~/extension/~/fax", json, attachments);
var response = ringCentral.Post(request);
```

#### Get Account Information
```cs
var request = new Request("/restapi/v1.0/account/~");
var response = ringCentral.Get(request);
```

### Get Address Book
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
var response = ringCentral.Get(request);
```

#### Using HTTP method tunneling

Sometimes, due to different technical limitations, API clients cannot issue all HTTP methods. In the most severe case a client may be restricted to GET and POST methods only. To work around this situation the RingCentral API provides a mechanism for tunneling PUT and DELETE methods through POST.

```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + contactId);
request.HttpMethodTunneling = true;
var response = sdk.Platform.Delete(request);
Assert.AreEqual(HttpMethod.Post, response.Request.Method);
```

### Message Store

#### Get Message Store
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
var response = ringCentral.Get(request);
```

#### Get Message Store First ID
```cs
var messageId = response.GetJson().SelectToken("records")[0].SelectToken("id");
```

#### Update Message Status
```cs
var messageStatusJson = "{\"readStatus\": \"Read\"}";
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
var response = ringCentral.Put(request);
```

##### Update Message Status using HTTP method tunneling
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.HttpMethodTunneling = true;
var response = ringCentral.Put(request);
```

#### Delete Message
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
var response = ringCentral.Delete(request);
```

### Subscription

RingCentral provides the ability to subscribe for event data using PubNub.

#### Create Subscription

##### About Pubnub SSL

Pubnub supports SSL. But we are not taking advantages of it. Before publishing data to Pubnub, we do encryption on our server side. And this SDK is responsible for decryption of data automatically. Users of this SDK don't need to do anything to enable SSL, still can be assured that data is being protected by encryption.

In the future, we might switch to Pubnub SSL. And it won't affect SDK users since we will keep the API identical.

##### Sample code

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

#### Casting SubscriptionEventArgs

##### Casting on Notification

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

##### Casting on Connect

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

##### Casting on Error

Note: PubNub error messages are not deserializable JSON.

```cs
public void ActionOnError(object sender, SubscriptionEventArgs args) {
    var error = args.Message.ToString();
}
```

#### Example Notification Messages

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

#### Delete Subscription

```cs
subscription.Remove();
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

RingCentral SDK &copy; 2015-2016 by RingCentral, Inc.

[nuget-version-svg]: https://img.shields.io/nuget/v/RingCentralSDK.svg
[nuget-version-link]: http://www.nuget.org/packages/RingCentralSDK/
[nuget-count-svg]: https://img.shields.io/nuget/dt/RingCentralSDK.svg
[nuget-count-link]: http://www.nuget.org/packages/RingCentralSDK/
[build-status-svg]: https://ci.appveyor.com/api/projects/status/ka1g6n869rxw81g4?svg=true
[build-status-link]: https://ci.appveyor.com/project/paulzolnierczyk/ringcentral-csharp
[coverage-status-svg]: https://coveralls.io/repos/ringcentral/ringcentral-csharp/badge.svg?branch=develop&service=github
[coverage-status-link]: https://coveralls.io/github/ringcentral/ringcentral-csharp
[docs-readthedocs-svg]: https://img.shields.io/badge/docs-readthedocs-blue.svg
[docs-readthedocs-link]: http://ringcentral-csharp.readthedocs.org/
[license-svg]: https://img.shields.io/badge/license-MIT-blue.svg
[license-link]: https://github.com/ringcentral/ringcentral-csharp/blob/master/LICENSE
