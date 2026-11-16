using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using FxferService.Application.LegacyContracts;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Exceptions;
using FxferService.Domain.Utilities;

namespace FxferService.Domain.Commands;

[DataContract]
public abstract class FileTransferCommand
{
    // F01-024: validation state resets for every Validate() call.
    protected FileTransferCommand()
    {
        ErrorUtility = new BusinessErrorUtility(new NullMessageCodeCatalog());
        Validations = new List<BusinessExceptionMessage>();
    }

    // Used only when translating content-id messages via BusinessErrorUtility.
    public Guid TenantId { get; set; }

    // Used only when translating content-id messages via BusinessErrorUtility.
    public string LocaleCode { get; set; } = string.Empty;

    [DataMember]
    public RequestorModel? Requestor { get; set; }

    public List<BusinessExceptionMessage> Validations { get; private set; }

    public virtual BusinessErrorUtility? ErrorUtility { get; set; }

    // F01-024: runs attribute-based validation, then command validations, then throws if any exist.
    public void Validate()
    {
        ErrorUtility ??= new BusinessErrorUtility(new NullMessageCodeCatalog());
        Validations = new List<BusinessExceptionMessage>();
        ValidateRequiredFields();
        SetCommandValidations();
        HandleValidations();
    }

    public abstract void SetCommandValidations();

    // F01-023: throws BusinessException("Command Execution Error", "E") when any validations exist.
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

    public void AddValidation(string name, string keyName, string message, bool isMessageAContentId = false)
    {
        string errorMessage = isMessageAContentId
            ? ErrorUtility!.GetMessage(TenantId, LocaleCode, message)
            : message;
        Validations.Add(new BusinessExceptionMessage(name, keyName, errorMessage));
    }

    // F01-024: each attribute validation failure becomes "Missing field: <MemberName>".
    private void ValidateRequiredFields()
    {
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(this, new ValidationContext(this), validationResults);

        validationResults.ForEach(v => AddValidation(
            v.MemberNames.First(),
            CommonConstants.Validation.ErrorType.RequiredFields,
            string.Format("{0}: {1}", CommonConstants.Validation.Messages.MissingField, v.MemberNames.First())));
    }

    private sealed class NullMessageCodeCatalog : IMessageCodeCatalog
    {
        public MessageCode? GetMessage(Guid tenantId, string contentId) => null;
    }
}