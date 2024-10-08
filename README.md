# SaanSoft.CorrelationId

[![ci](https://github.com/saan800/saansoft-correlationid/actions/workflows/ci.yml/badge.svg)](https://github.com/saan800/saansoft-correlationid/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/saan800/saansoft-correlationid/branch/main/graph/badge.svg?token=K88KBHI5K1)](https://codecov.io/gh/saan800/saansoft-correlationid)

In a distributed system it can be a challenge to trace HTTP requests and messages through multiple microservices.

A `CorrelationId` bundles each logical transaction as it moves through multiple processors.

With this system, your client's requests are collected under one value for easier tracking and troubleshooting.

Depending on the `ICorrelationIdProvider` configured you can extract the value from HTTP request headers or OpenTelemetry tracing for greater ease of tracking the transaction, or simply default to a generated GUID string.
