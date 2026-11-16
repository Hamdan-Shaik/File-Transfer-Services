using System.Runtime.Serialization;

using FxferService.Application.LegacyContracts;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Domain.Commands;

[DataContract]
public class FtsCommand
{
    [DataMember]
    public RequestorModel? Requestor { get; set; }

    public List<BusinessExceptionMessage> Validations { get; private set; } = null!;

    // F02-029, F02-045: clears the validation list.
    public void FtsCommandValidate()
    {
        Validations = new List<BusinessExceptionMessage>();
    }

    // F02-029, F02-045: throws BusinessException("Command Execution Error", "E") when any validations exist.
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

    // F02-030, F02-031, F02-032, F02-034: adds a validation entry.
    public void AddValidation(string keyName, string message)
    {
        Validations.Add(new BusinessExceptionMessage(keyName, keyName, message));
    }
}
