# ringcentral-csharp

## Status

This documentation is for 0.1.1 of [ringcentral-csharp](https://github.com/ringcentral/ringcentral-csharp).

## Quickstart

### Initialization and Authorization

```cs
// Import RingCentral SDK
using RingCentral;

// Initialize Ring Central Client
var ringCentral = new SDK("your appKey", "your appSecret", "Ring Central apiEndPoint", "Application Name","Application Version").GetPlatform();

// Password Grant Authorization
Response response = ringCentral.Authorize(userName, extension, password, true);
```

### Send an SMS

```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonSmsString);
Response response = ringCentral.Post(request);
```

### Send Fax

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

### Get Account Information

```cs
Request request = new Request("/restapi/v1.0/account/~");
Response response = ringCentral.Get(request);
```

### Get Address Book

```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
Response response = ringCentral.Get(request);
```

### Using x-http-method-override Header

```cs
Request overRideRequest = new Request("/restapi/v1.0/account/~");
overRideRequest.SetXhttpOverRideHeader("GET");
Response overRideResponse = ringCentral.Post(overRideRequest);
```