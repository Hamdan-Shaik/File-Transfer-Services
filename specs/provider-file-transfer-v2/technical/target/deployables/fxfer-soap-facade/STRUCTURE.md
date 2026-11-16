---
type: Deployable Structure
title: 'Structure: fxfer-soap-facade'
description: 'Skeleton of the fxfer-soap-facade deployable: projects, folders and cross-cutting setup.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-soap-facade
tags:
- deployable
- dotnet-service
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:37:46Z
sources:
- id: component-backend-bas-file-transfer-svc
  resource: /technical/components/backend-bas-file-transfer-svc/SPEC.md
  title: Component placed in it
- id: component-backend-bas-file-transfer-svc-2
  resource: /technical/components/backend-bas-file-transfer-svc-2/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-svc-main
  resource: /technical/components/backend-file-transfer-svc-main/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (3 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-soap-facade
  generation:
    status: done
    attempts: 1
  path: services/fxfer-soap-facade
  kind: dotnet-service
  stack: .NET 10 (CoreWCF)
  components: 3
  depends_on:
  - fxfer-domain-api
  body_sha256: 44ec19e63e32b79ffa8ab8196ba4af6ad6a6203cd9a9b056832c13df69166416
---

# Structure: `fxfer-soap-facade`

Deployable path: `services/fxfer-soap-facade`

This deployable is an ASP.NET Core (.NET 10) service hosting CoreWCF SOAP endpoints that preserve the legacy FXFER SOAP contract surface. It performs contract translation and delegates all authoritative work (commands/queries) to `fxfer-domain-api`.

## Folder skeleton (new repository)

Create the following skeleton at `services/fxfer-soap-facade`:

```text
services/fxfer-soap-facade/
  Fxfer.SoapFacade.sln
  README.md
  src/
    Host/
      Fxfer.SoapFacade.Host.csproj
      Program.cs
      appsettings.json
      appsettings.Development.json
      Configuration/
        AuthenticationOptions.cs
        TenantResolutionOptions.cs
        DomainApiOptions.cs
      Middleware/
        CorrelationIdMiddleware.cs
        TenantContextMiddleware.cs
      Soap/
        Services/
          FileTransferSvcService.cs
          FileTransferSvcQueryService.cs
          FileTransferSvcEventService.cs
        Bindings/
          SoapBindingFactory.cs
      Health/
        HealthChecks.cs
      Clients/
        FxferDomainApiClient.cs
    CoreWcf/
      Fxfer.SoapFacade.CoreWcf.csproj
      Contracts/
        Commands/
        Queries/
        Domain/
        Events/
        Exceptions/
        Utilities/
    LegacyWcf/
      Fxfer.SoapFacade.LegacyWcf.csproj
      Contracts/
        (legacy WCF/IIS parity reference)
  test/
    Fxfer.SoapFacade.Tests/
      Fxfer.SoapFacade.Tests.csproj
  Dockerfile
  helm/
    fxfer-soap-facade/
      Chart.yaml
      values.yaml
      templates/
        deployment.yaml
        service.yaml
        ingress.yaml
        configmap.yaml
        hpa.yaml
```

Notes:

- The `src/` folder layout matches the component placement defined in `technical/target/architecture.yaml`.
- `src/CoreWcf` is the contract source of truth for the .NET 10 CoreWCF surface.
- `src/LegacyWcf` exists for parity reference and migration validation (no new contract development).

## Runtime entry point

- `src/Host/Program.cs` hosts the ASP.NET Core pipeline and configures CoreWCF endpoints.
- SOAP endpoints are hosted under a versioned base path:
  - `/soap/file-transfer/v1/FileTransferSvcService.svc`
  - `/soap/file-transfer/v1/FileTransferSvcQueryService.svc`
  - `/soap/file-transfer/v1/FileTransferSvcEventService.svc`
- Platform-standard health endpoints are exposed separately:
  - `GET /api/health/live`
  - `GET /api/health/ready`

## Authentication and tenant context

- SOAP operations are authenticated.
- Tenant context is derived from validated identity context (claims) for command/query operations.
- The legacy SOAP availability operation `IsServiceHealthy(tenantId)` accepts a `tenantId` string parameter and is limited to liveness/readiness checks (it is not an authorization source for business operations).

## Domain API delegation

- The facade does not own persistence.
- It calls `fxfer-domain-api` over HTTPS for all authoritative work:
  - Command operations map to `fxfer-domain-api` command endpoints.
  - Query operations map to `fxfer-domain-api` query endpoints.

## Observability (logs, metrics, traces)

- OpenTelemetry SDK is configured for traces/metrics/logs.
- W3C Trace Context is extracted/propagated (`traceparent`, `tracestate`, `baggage`).
- Correlation ID propagation uses `X-Correlation-ID`:
  - if missing on inbound requests, generate a UUID v4 and return it in the response header.
  - include `tenant_id`, `correlation_id`, and `trace_id` attributes on all spans/logs.

## Resilience and timeouts

- Outbound HTTP calls to `fxfer-domain-api` use bounded timeouts and retries appropriate for idempotent operations.
- The SOAP facade does not implement domain-level retries that could cause duplicate writes; command-side idempotency is owned by `fxfer-domain-api`.

## Container and deployment assets

- `Dockerfile` uses a multi-stage build (`dotnet publish`) and runs as a non-root user.
- `helm/fxfer-soap-facade/` follows the enterprise Helm chart structure and health check requirements.

## Enterprise-Architecture standards applied

- `standards/technology/eks/STD-TEC-EKS-003-helm.md` section `2.1 Standard Chart Directory Layout` (Helm chart folder structure)
- `standards/technology/eks/STD-TEC-EKS-003-helm.md` section `2.2 Chart.yaml Requirements` (required chart metadata)
- `standards/technology/eks/STD-TEC-EKS-003-helm.md` section `7.3 Health Checks` (liveness/readiness probe configuration)
- `strategy/guidance/security/Application/GUID-SEC-APPS-CONTAINER-SECURITY-2.7.05.md` section `1.1 Base Image Selection Strategy` (approved/minimal base images; reduce attack surface)
- `strategy/guidance/security/Application/GUID-SEC-APPS-CONTAINER-SECURITY-2.7.05.md` section `6. Secrets Management for Containers` (no secrets baked into images; runtime secret injection)

