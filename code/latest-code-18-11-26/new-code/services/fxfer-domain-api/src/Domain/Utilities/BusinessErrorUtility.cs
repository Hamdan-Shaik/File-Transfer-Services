using System;
using HPP.Core.BAS.CQRS.Config;
using HPP.Core.BAS.CQRS.Domain;
using HPP.Core.BAS.CQRS.UserMeta;

namespace Fxfer.DomainApi.Contracts.Utilities;

public sealed class BusinessErrorUtility
{
    private readonly Lazy<IMessageCodesReadOnly> messageCodes;

    public BusinessErrorUtility()
    {
        messageCodes = new Lazy<IMessageCodesReadOnly>(() => new MessageCodesReadOnly());
    }

    internal BusinessErrorUtility(IMessageCodesReadOnly messageCodes)
    {
        this.messageCodes = new Lazy<IMessageCodesReadOnly>(() => messageCodes);
    }

    public string GetMessage(RequestorModel requestor, string contentId)
    {
        CodeListModel message = LoadConfigurationMessage(requestor, contentId);

        if (message != null && message.LanguageList != null && message.LanguageList.Count > 0)
        {
            Languages? lang = message.LanguageList.Find(f => f.LocaleID.ToLower() == requestor.LocaleCode.ToLower());
            return lang != null ? lang.Description : string.Empty;
        }

        return string.Empty;
    }

    private CodeListModel LoadConfigurationMessage(RequestorModel requestor, string contentId)
    {
        return messageCodes.Value.GetMessage(Guid.Parse(requestor.TenantId), contentId);
    }
}

