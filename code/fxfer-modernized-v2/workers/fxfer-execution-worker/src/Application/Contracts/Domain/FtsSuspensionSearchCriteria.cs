using System;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class FtsSuspensionSearchCriteria
{
    public Guid SourceLocationId { get; set; }

    public string? ErrorText { get; set; }

    public string? StackTrace { get; set; }
}

