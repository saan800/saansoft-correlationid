# SaanSoft.CorrelationId.Web

Use with [SaanSoft.CorrelationId](https://github.com/saan800/saansoft-correlationid/blob/main/src/SaanSoft.CorrelationId/README.md) in web or api projects.

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` (or some systems call it `TraceId`) is metadata than can be used to bundle each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

Use `WebCorrelationIdMiddleware` to extract the `CorrelationId` from HTTP context and/or headers, and set the `ICorrelationIdProvider` to use that value.

Use `WebCorrelationIdOptions` to configure how the `CorrelationId` is extracted from the HTTP request. The first valid match is used.

- `HttpContext.TraceIdentifier`: The default ASP.NET Core trace identifier
- `traceparent`: From the [W3C Tract Context](https://www.w3.org/TR/trace-context-2/#traceparent-header) spec. Extract the `trace-id` value for use as the `CorrelationId`
- The value of other header names (eg `x-correlation-id`)

## Use in your Api or Website

### Prerequisite

Configure [SaanSoft.CorrelationId](https://github.com/saan800/saansoft-correlationid/blob/main/src/SaanSoft.CorrelationId/README.md).

### Register middleware

Then in your `Program.cs` or `Startup.cs` add:

```csharp
var app = builder.Build();

app.UseWebCorrelationIdMiddleware();
// OR
app.UseWebCorrelationIdMiddleware(new WebCorrelationIdOptions {
  ...
});
...
```
