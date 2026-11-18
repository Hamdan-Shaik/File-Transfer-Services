# fxfer-projection-worker

Tenant-scoped projector worker that consumes FXFER domain events and maintains query/read-model projections.

- Build: `dotnet build workers/fxfer-projection-worker`
- Run (dev): `dotnet run --project workers/fxfer-projection-worker/src/Worker/Fxfer.ProjectionWorker.Worker.csproj`

Health endpoints:
- `GET /api/health/live`
- `GET /api/health/ready`

