# fxfer-transfer-worker

Tenant-scoped worker that consumes FXFER transfer work items, performs protocol-level file I/O, and records outcomes through `fxfer-domain-api`.

- Build: `dotnet build workers/fxfer-transfer-worker`
- Run (dev): `dotnet run --project workers/fxfer-transfer-worker/src/Worker/Fxfer.TransferWorker.Worker.csproj`

Health endpoints:
- `GET /api/health/live`
- `GET /api/health/ready`

