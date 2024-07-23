# SaanSoft.CorrelationId

[![build-and-test](https://github.com/saan800/saansoft-correlationid/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/saan800/saansoft-correlationid/actions/workflows/build-and-test.yml)

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` bundles each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

Depending on the `ICorrelationIdProvider` configured you can extract the value from HTTP request headers or OpenTelemetry tracing for greater ease of tracking the transaction, or simply default to a generated GUID string.
