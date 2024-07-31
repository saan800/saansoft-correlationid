# SaanSoft.CorrelationId

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` is metadata than can be used to bundle each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

## Register in IoC

`ICorrelationIdProvider` should be registered at the request scope level, as it should be use the same 
object across all calls for a HTTP request or logical transaction.

### IServiceCollection

For api and web projects using the normal `AddScoped` function is perfect.

```csharp
serviceCollection.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
```

<!-- For services and background workers - todo.... -->

## Usage

Pass `ICorrelationIdProvider` into the class's constructor, then call `correlationIdProvider.Get()` 
to retrive the `CorrelationId` for the current request.

## Dependencies

`CorrelationIdProvider` can be used by itself, and it will provide a unique string for each request.

However, you can also setup `CorrelationIdProvider` to use values from other sources such as HTTP request header by using other dependencies and middleware.

