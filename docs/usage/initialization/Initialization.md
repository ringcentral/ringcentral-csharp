# Initialization

## Initializing the SDK

```cs
//import RingCentral
using RingCentral;

//Initialize Ring Central Client
var sdk = new SDK(
	"your appKey",
	"your appSecret",
	"RingCentral apiServer",
	"Application Name",
	"Application Version");
```

`Application Name` and `Application Version` are optional. They are used to generate your app's `User Agent`.

You don't need to memorize `RingCentral apiServer`'s address. There is another constructor for `SDK`:

```cs
var sdk = new SDK(
	"your appKey",
	"your appSecret",
	SDK.Server.Production,
	"Application Name",
	"Application Version");
```

`RingCentral apiServer` should be either `SDK.Server.Production` or `SDK.Server.Sandbox`.
