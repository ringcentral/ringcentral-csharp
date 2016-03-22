# Authorization

The RingCentral SDK for C# has several helper methods for managing OAuth authorization.

## Authorization via Password Grant

The OAuth 2.0 resource owner password credentials (RPOC) grant allows apps that have the user's password credentials to entire them directly on the user's behalf. This is useful for server applications and does not support IdPs for SSO.

```cs
Response response = ringCentral.Authorize(userName, extension, password, true);
```

## Token Refresh

```cs
Response response = ringCentral.Refresh();
```

## Logout

```cs
ringCentral.Logout();
```