# Authorization

The RingCentral SDK for C# has several helper methods for managing OAuth authorization.

## Resource Owner Password Credentials (ROPC) Flow

It's further named Password Flow — a 2-legged authorization flow which is more suitable for server apps used by a single user account.

```cs
var response = sdk.Platform.Login(userName, extension, password, true);
```

### Token Refresh

```cs
var response = sdk.Platform.Refresh();
```

### Logout

```cs
sdk.Platform.Logout();
```


## Authorization Code Flow

A 3-legged authorization flow and is a preferred flow for your app if it is a web, mobile or desktop application and is intended to work with multiple RingCentral user accounts.

### Step 1: generate authorize uri

```cs
var authroizeUri = sdk.Platform.AuthorizeUri(redirectUri, myState);
```

Open `authroizeUri` in browser and let user login and authrize your app. Then user will be redirected to `redirectUri` where you can get the `authCode` from its url.

`myState` is an opaque value used by the client to maintain state between the request and callback. The authorization server includes this value when redirecting the user-agent back to the client. The parameter should be used for preventing cross-site request forgery.

Please note that you have to configure `redirectUri` in your [RingCentral apps](https://developer.ringcentral.com/my-account.html#/applications).



### Step 2: authenticate by authCode

```cs
sdk.Platform.Authenticate(authCode, redirectUri)
```

`authCode` is obtained in last step and `redirectUri` should be identical to the one used in last step.
