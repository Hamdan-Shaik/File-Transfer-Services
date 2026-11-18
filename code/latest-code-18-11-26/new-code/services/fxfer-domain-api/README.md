# fxfer-domain-api

Authoritative FXFER Domain API for configuration and tracking.

- Build: `dotnet build services/fxfer-domain-api`
- Run (dev): `dotnet run --project services/fxfer-domain-api/src/Api/Fxfer.DomainApi.Api.csproj`

Health endpoints:
- `GET /api/health/live`
- `GET /api/health/ready`

OpenAPI contract source of truth:
- `services/fxfer-domain-api/openapi/fxfer-domain-api.v1.yaml`
