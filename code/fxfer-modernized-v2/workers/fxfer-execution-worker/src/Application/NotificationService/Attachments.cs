namespace Fxfer.ExecutionWorker.Application.NotificationService;

public class Attachments
{
    // shortcut: minimal DTO for `NotificationServiceHelper`, expand if/when the SOAP contract is ported.
    public string? FileName { get; set; }
    public byte[]? Content { get; set; }
}
