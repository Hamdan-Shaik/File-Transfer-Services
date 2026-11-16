namespace FxferService.Domain.Utilities;

public sealed class BusinessErrorUtility
{
    private readonly IMessageCodeCatalog messageCodes;

    public BusinessErrorUtility(IMessageCodeCatalog messageCodes)
    {
        this.messageCodes = messageCodes;
    }

    public string GetMessage(Guid tenantId, string localeCode, string contentId)
    {
        MessageCode? message = LoadConfigurationMessage(tenantId, contentId);

        if (message != null && message.Languages != null && message.Languages.Count > 0)
        {
            MessageCodeLanguage? lang = null;
            foreach (MessageCodeLanguage language in message.Languages)
            {
                if (language.LocaleId.ToLower() == localeCode.ToLower())
                {
                    lang = language;
                    break;
                }
            }

            return lang != null ? lang.Description : string.Empty;
        }

        return string.Empty;
    }

    private MessageCode? LoadConfigurationMessage(Guid tenantId, string contentId)
    {
        return messageCodes.GetMessage(tenantId, contentId);
    }
}

