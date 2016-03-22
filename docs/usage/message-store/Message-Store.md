# Message Store

## Get Message Store
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store");
Response response = ringCentral.Get(request);
```

## Get Message Store First ID
```cs
var messageId = response.GetJson().SelectToken("records")[0].SelectToken("id");
```

## Update Message Status
```cs
var messageStatusJson = "{\"readStatus\": \"Read\"}";
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
Response response = ringCentral.Put(request);
```

### Update Message Status using x-http-ovverride-header
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId, messageStatusJson);
request.SetXhttpOverRideHeader("PUT"); 
Response response = ringCentral.Post(request);
```

## Delete Message
```cs
Request request = new Request("/restapi/v1.0/account/~/extension/~/message-store/" + messageId);
Response response = ringCentral.Delete(request);
```