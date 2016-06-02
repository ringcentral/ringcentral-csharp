# Message Store

## Get Message Store
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
var response = sdk.Platform.Get(request);
```

## Get Message Store First ID
```cs
var messageId = response.Json.SelectToken("records")[0].SelectToken("id");
```

## Update Message Status
```cs
var messageStatusJson = "{\"readStatus\": \"Read\"}";
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
var response = sdk.Platform.Put(request);
```

### Update Message Status using HTTP method tunneling
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.HttpMethodTunneling = true;
var response = sdk.Platform.Put(request);
```

## Delete Message
```cs
var request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
var response = sdk.Platform.Delete(request);
```