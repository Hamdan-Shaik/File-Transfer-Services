namespace Fxfer.TransferWorker.Contracts.NotificationService;

public sealed class Attachments
{
    public string? FileName { get; set; }

    public byte[]? Content { get; set; }
}