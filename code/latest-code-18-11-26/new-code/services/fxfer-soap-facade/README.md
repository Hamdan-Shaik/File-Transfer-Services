# fxfer-soap-facade

SOAP compatibility facade for legacy FXFER consumers.

- Build: `dotnet build services/fxfer-soap-facade`
- Run (dev): `dotnet run --project services/fxfer-soap-facade/src/Host/Fxfer.SoapFacade.Host.csproj`

Health endpoints:
- `GET /api/health/live`
- `GET /api/health/ready`

SOAP endpoints (CoreWCF):
- `/soap/file-transfer/v1/FileTransferSvcService.svc`
- `/soap/file-transfer/v1/FileTransferSvcQueryService.svc`
- `/soap/file-transfer/v1/FileTransferSvcEventService.svc`