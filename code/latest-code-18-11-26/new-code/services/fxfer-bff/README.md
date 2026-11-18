# fxfer-bff

Browser-facing FXFER Backend-for-Frontend (BFF) API facade.

- Build: `dotnet build services/fxfer-bff`
- Run (dev): `dotnet run --project services/fxfer-bff/src/Api/Fxfer.Bff.Api.csproj`

Health endpoints:
- `GET /api/health/live`
- `GET /api/health/ready`

OpenAPI contract source of truth:
- `services/fxfer-bff/openapi/fxfer-bff.v1.yaml`

