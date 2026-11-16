using System;

namespace Fxfer.ExecutionWorker.Application.NotificationService;

public class PostEvent
{
    // shortcut: minimal DTO for `NotificationServiceHelper`, expand if/when the SOAP contract is ported.
    public Guid EventId { get; set; }
}
