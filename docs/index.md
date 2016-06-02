# ringcentral-csharp

## Status

This documentation is for 1.0.0 of [ringcentral-csharp](https://github.com/ringcentral/ringcentral-csharp).

## Quickstart

### Initialization and Authorization

```cs
// Import RingCentral
using RingCentral;

// Initialize Ring Central Client
var sdk = new SDK("your appKey", "your appSecret", "Ring Central apiServer", "Application Name","Application Version");

// Password Grant Authorization
var response = sdk.Platform.Login(username, extension, password, true);
```

### Send an SMS

```cs
using Newtonsoft.Json;

var requestBody = new {
    text = "hello world",
    from = new { phoneNumber = phoneNumber },
    to = new object[] { new { phoneNumber = phoneNumber } }
};
var jsonString = JsonConvert.SerializeObject(requestBody);
var request = new Request("/restapi/v1.0/account/~/extension/~/sms", jsonString);
var response = sdk.Platform.Post(request);
```

### Send Fax

```cs
const string text = "Hello world!";

var byteArrayText = System.Text.Encoding.UTF8.GetBytes(text);
var attachment = new Attachment("test.txt", "application/octet-stream", byteArrayText);
var attachment2 = new Attachment("test2.txt", "text/plain", byteArrayText);
var pdfFile = File.ReadAllBytes("<PATH TO YOUR PDF>");
var attachment3 = new Attachment("<NAME OF YOUR PDF>.pdf", "application/pdf", pdfFile);
var attachments = new List<Attachment> { attachment, attachment2, attachment3 };
var json = "{\"to\":[{\"phoneNumber\":\"<YOUR TARGET NUMBER>\"}],\"faxResolution\":\"High\"}";

var request = new Request("/restapi/v1.0/account/~/extension/~/fax", json, attachments);
var response = sdk.Platform.Post(request);
```

### Get Account Information

```cs
var request = new Request("/restapi/v1.0/account/~");
var response = sdk.Platform.Get(request);
```

### Get Address Book

```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact");
var response = sdk.Platform.Get(request);
```

### Using HTTP method tunneling

Sometimes, due to different technical limitations, API clients cannot issue all HTTP methods. In the most severe case a client may be restricted to GET and POST methods only. To work around this situation the RingCentral API provides a mechanism for tunneling PUT and DELETE methods through POST.

```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/address-book/contact/" + contactId);
request.HttpMethodTunneling = true;
var response = sdk.Platform.Delete(request);
Assert.AreEqual(HttpMethod.Post, response.Request.Method);
```