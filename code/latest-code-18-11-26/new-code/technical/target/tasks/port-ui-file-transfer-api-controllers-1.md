# Brief: port part 1 of 1 of component `ui-file-transfer-api-controllers`

- Deployable: `fxfer-bff` at `services/fxfer-bff`; build: `dotnet build services/fxfer-bff`; tests: `dotnet test services/fxfer-bff`
- The component's folder (architecture.yaml): `services/fxfer-bff/src/Api`
- The component's spec: `technical/components/ui-file-transfer-api-controllers/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 1 legacy methods or classes, 9 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `FileTransferSvcEventServiceController`: role **application**, target file(s) none of its own (`drop: legacy CQRS event-distribution controller wrapper`; see `technical/components/ui-file-transfer-api-controllers/TARGET.md`) (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferApi\Controllers\FileTransferSvcEventServiceController.cs`)

#### `FileTransferSvcEventServiceController` (lines 12-20)

```csharp
    public class FileTransferSvcEventServiceController : EventDistributionBaseApiController
    {
        private readonly IEventDistribution eventDistribution;

        public FileTransferSvcEventServiceController(IEventDistribution eventDistribution) : base(eventDistribution)
        {
            this.eventDistribution = eventDistribution;
        }
    }
```

