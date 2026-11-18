using System.ComponentModel.DataAnnotations;
using Fxfer.DomainApi.Contracts.Utilities;
using Fxfer.DomainApi.Domain.Constants;
using HPP.Core.BAS.CQRS.Base;

namespace Fxfer.DomainApi.Contracts.Commands;

// Rules: F02-026, F02-027
public abstract class FileTransferCommand : Command
{
    public FileTransferCommand()
    {
        ErrorUtility = new BusinessErrorUtility();
        Validations = new List<BusinessExceptionMessage>();
    }

    public List<BusinessExceptionMessage> Validations { get; private set; }

    public virtual BusinessErrorUtility ErrorUtility { get; set; }

    public void Validate()
    {
        ErrorUtility = ErrorUtility ?? new BusinessErrorUtility();
        Validations = new List<BusinessExceptionMessage>();
        ValidateRequiredFields();
        SetCommandValidations();
        HandleValidations();
    }

    public abstract void SetCommandValidations();

    public virtual void HandleValidations()
    {
        // F02-027: If any validations exist, throw BusinessException.
        if (Validations.Any())
        {
            throw new BusinessException(
                Validations,
                CommonConstants.Validation.Messages.CommandError,
                CommonConstants.Validation.BusinessErrorCode);
        }
    }

    public void AddValidation(string name, string keyName, string message, bool isMessageAContentId = false)
    {
        string errorMessage = isMessageAContentId ? ErrorUtility.GetMessage(Requestor, message) : message;

        Validations.Add(new BusinessExceptionMessage(name, keyName, errorMessage));
    }

    // F02-026: Required field failures add one validation per failing property.
    private void ValidateRequiredFields()
    {
        List<ValidationResult> validationResults = new();
        Validator.TryValidateObject(this, new ValidationContext(this), validationResults);

        validationResults.ForEach(
            validation => AddValidation(
                validation.MemberNames.First(),
                CommonConstants.Validation.ErrorType.RequiredFields,
                string.Format(
                    "{0}: {1}",
                    CommonConstants.Validation.Messages.MissingField,
                    validation.MemberNames.First())));
    }
}

