[![Build status](https://ci.appveyor.com/api/projects/status/8m9is2hg8ma8f0ob?svg=true)](https://ci.appveyor.com/project/paulzolnierczyk/ringcentral-csharp)
[![Coverage Status](https://coveralls.io/repos/iinteractive/ringcentral-csharp/badge.svg?branch=develop&service=github&t=hWGUkw)](https://coveralls.io/github/iinteractive/ringcentral-csharp?branch=develop)
# RingCentral SDK for C&#35;

# Installation

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








