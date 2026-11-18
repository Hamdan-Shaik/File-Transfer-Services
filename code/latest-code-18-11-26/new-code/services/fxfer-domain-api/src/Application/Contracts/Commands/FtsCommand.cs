using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F01-038, F01-039, F01-040, F01-059
public class FtsCommand : Command
{
    public List<BusinessExceptionMessage> Validations { get; private set; } = new();

    // F01-038
    public void FtsCommandValidate()
    {
        Validations = new List<BusinessExceptionMessage>();
    }

    // F01-039
    public virtual void HandleValidations()
    {
        if (Validations.Any())
        {
            throw new BusinessException(
                Validations,
                CommonConstants.Validation.Messages.CommandError,
                CommonConstants.Validation.BusinessErrorCode);
        }
    }

    // F01-040
    public void AddValidation(string keyName, string message)
    {
        Validations.Add(new BusinessExceptionMessage(keyName, keyName, message));
    }
}

