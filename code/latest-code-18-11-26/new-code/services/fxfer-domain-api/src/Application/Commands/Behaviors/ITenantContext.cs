namespace Fxfer.DomainApi.Application.Commands.Behaviors;

public interface ITenantContext
{
    string? TenantId { get; }
}

