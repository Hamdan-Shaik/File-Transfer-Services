using System.Collections.Generic;

namespace Fxfer.ExecutionWorker.Domain.Clients;

public sealed class GetIntegrationFileQuery
{
    public RequestorModel? Requestor { get; set; }

    public IntegrationFileQueryParms? Where { get; set; }

    public List<FtsIntegrationFileModel>? Results { get; set; }
}
