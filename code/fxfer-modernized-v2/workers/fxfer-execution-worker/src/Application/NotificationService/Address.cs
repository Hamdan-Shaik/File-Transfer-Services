namespace Fxfer.ExecutionWorker.Application.NotificationService;

public class Address
{
    // shortcut: minimal DTO for `NotificationServiceHelper`, expand if/when the SOAP contract is ported.
    public string? EmailAddress { get; set; }

    public string? DisplayName { get; set; }
}
