# SaanSoft.CorrelationId.Web

Use with [SaanSoft.CorrelationId](https://github.com/saan800/saansoft-correlationid/blob/main/src/SaanSoft.CorrelationId/README.md) in web or api projects.

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` (or some systems call it `TraceId`) is metadata than can be used to bundle each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

Use `WebCorrelationIdMiddleware` to extract the `CorrelationId` from HTTP context and/or headers, and:
* set the `ICorrelationIdProvider` to use that value
* optionally set a response header with the `CorrelationId` (enabled by default)
* optionally add the `CorrelationId` to `ILogger.BeginScope` so all structured logs have the `CorrelationId` on them (enabled by default)

Use `WebCorrelationIdOptions.Evaluators` to configure how the `CorrelationId` is extracted from the HTTP request. 
The first valid match is used. If no match is found, `WebCorrelationIdMiddleware` will generate a random string instead.

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

### Evaluators

When registering `WebCorrelationIdMiddleware`, you can provide zero or more evaluators in the options.

The `Evaluators` are short functions which attempt to get the `CorrelationId` from `HttpContext`.
Such as from a request header, or dotnet's built in `Activity.Current`.

Common evaluators can be found in `SaanSoft.CorrelationId.Web.Evaluator`, but you can 
provide your own if desired.
