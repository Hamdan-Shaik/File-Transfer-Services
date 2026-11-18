namespace Fxfer.DomainApi.Contracts.Domain;

public sealed class FtsNotificationQueryModel
{
    public int ConfigId { get; set; }

    public bool IncludeError { get; set; }

    public bool IncludeSuccess { get; set; }
}