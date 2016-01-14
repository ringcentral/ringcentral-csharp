# Initialization

## Initializing the SDK

```cs
//import RingCentral SDK
using RingCentral;

//Initialize Ring Central Client
var ringCentral = new SDK(
	"your appKey",
	"your appSecret",
	"Ring Central apiEndPoint",
	"Application Name",
	"Application Version").GetPlatform();
```

## Set User Agent Header

In additoin to setting the `Application Name` and `Application Version` via the constructor, these can be set using the `SetUserAgentHeader` method.

```cs
ringCentral.SetUserAgentHeader("Application Name", "Application Version");
```