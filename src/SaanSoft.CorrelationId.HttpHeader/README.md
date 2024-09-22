# SaanSoft.CorrelationId.HttpHeader

Intended for use with [SaanSoft.CorrelationId](https://github.com/saan800/saansoft-correlationid/blob/main/src/SaanSoft.CorrelationId/README.md).

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` is metadata than can be used to bundle each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

Use `SaanSoft.CorrelationId.HttpHeader` package to extract the `CorrelationId` from supported http headers (in this order if multiple headers exist):
* `traceparent`: From the [W3C Tract Context](https://www.w3.org/TR/trace-context-2/#traceparent-header) spec. Extract the `trace-id` value for use as the `CorrelationId`
* `x-correlation-id` or `correlation-id` or `correlationid`: Commonly used correlation id headers

## Use in your Api or Website

### Prerequisite

Configure [SaanSoft.CorrelationId](https://github.com/saan800/saansoft-correlationid/blob/main/src/SaanSoft.CorrelationId/README.md).

### Register middleware

Then in your `Program.cs` or `Startup.cs` add:

```csharp
...
var app = builder.Build();
app.UseCorrelationIdHeaderMiddleware();
...
```
