using System.ComponentModel.DataAnnotations;
using FxferService.Application.Persistence.Batch;
using FxferService.Domain.Constants;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public sealed class CreateErrorLogDataAccess : DataAccessBase<int>
{
    private readonly ErrorLogDetailModel _errorLogDetail;
    private readonly Guid _exceptionId;

    public CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
    {
        _errorLogDetail = errorLog;
        _exceptionId = Guid.NewGuid();
    }

    // F03-020: record error details for a failed delivery.
    protected override int ExecuteQuery(IDbContextBase dbcontext)
    {
        SetLimitedValue();
        FtsFileExceptionEntity fileException = new()
        {
            ExceptionId = _exceptionId,
            FileId = _errorLogDetail.FileId,
            ExceptionType = _errorLogDetail.ErrorType,
            Message = _errorLogDetail.ErrorText,
            MethodName = _errorLogDetail.MethodName,
            StackTrace = _errorLogDetail.StackTrace,
            CreatedAt = DateTimeOffset.UtcNow,
            LastModifiedAt = DateTimeOffset.UtcNow,
            OperatorId = _errorLogDetail.OperatorId,
        };
        dbcontext.Set<FtsFileExceptionEntity>().Add(fileException);
        return dbcontext.SaveChanges();
    }

    private void SetLimitedValue()
    {
        _errorLogDetail.ErrorType = Truncate(_errorLogDetail.ErrorType, GetMaxLength(ErrorLogConstants.ExceptionType));
        _errorLogDetail.ErrorText = Truncate(_errorLogDetail.ErrorText, GetMaxLength(ErrorLogConstants.Message));
        _errorLogDetail.MethodName = Truncate(_errorLogDetail.MethodName, GetMaxLength(ErrorLogConstants.MethodName));
        _errorLogDetail.StackTrace = Truncate(_errorLogDetail.StackTrace, GetMaxLength(ErrorLogConstants.StackTrace));
    }

    private string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    private int GetMaxLength(string name)
    {
        int maxLength = 0;
        StringLengthAttribute? strLenAttr = typeof(FtsFileExceptionEntity)
            .GetProperty(name)!
            .GetCustomAttributes(typeof(StringLengthAttribute), false)
            .Cast<StringLengthAttribute>()
            .SingleOrDefault();
        if (strLenAttr != null)
        {
            maxLength = strLenAttr.MaximumLength;
        }

        return maxLength;
    }
}
