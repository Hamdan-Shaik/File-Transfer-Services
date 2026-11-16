namespace FxferService.Domain.Utilities;

public interface IMessageCodeCatalog
{
    MessageCode? GetMessage(Guid tenantId, string contentId);
}

