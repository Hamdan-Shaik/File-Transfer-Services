using System.ComponentModel.DataAnnotations;
using FxferService.Application.Persistence.Batch;
using FxferService.Domain.Constants;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;

namespace FxferService.Infrastructure.Persistence.Batch;

public class CreateFtsFileExceptionEventDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly Guid _destinationId;
    private readonly string _destinationFileName;
    private readonly string _operatorId;
    private Guid _deliveryResult;
    private readonly ErrorLogDetailModel _errorLogDetail;
    private readonly Guid _exceptionId;

    public CreateFtsFileExceptionEventDataAccess(
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog)
    {
        _fileId = fileId;
        _destinationId = destinationId;
        _destinationFileName = destinationFileName;
        _operatorId = operatorId;
        _errorLogDetail = errorLog;
        _exceptionId = Guid.NewGuid();
    }

    // F03-020: record an exception event + error details.
    protected override Guid ExecuteQuery(IDbContextBase dbcontext)
    {
        _deliveryResult = FtsFileExceptionEvent(dbcontext);
        _errorLogDetail.EventId = _deliveryResult;
        AddErrorLog(dbcontext);
        dbcontext.SaveChanges();
        return _deliveryResult;
    }

    private Guid FtsFileExceptionEvent(IDbContextBase dbcontext)
    {
        FtsFileEventEntity ftsEvent = new();

        ftsEvent.EventId = Guid.NewGuid();
        ftsEvent.FileId = _fileId;
        ftsEvent.EventCode = FileDeliveryEventConstants.Exception;
        ftsEvent.DestinationId = _destinationId;
        ftsEvent.OperatorId = _operatorId;
        ftsEvent.TrackDescription = _destinationFileName;
        ftsEvent.CreatedAt = DateTimeOffset.UtcNow;
        ftsEvent.LastModifiedAt = DateTimeOffset.UtcNow;
        dbcontext.Set<FtsFileEventEntity>().Add(ftsEvent);

        return ftsEvent.EventId;
    }

    private Guid AddErrorLog(IDbContextBase dbcontext)
    {
        SetLimitedValue();
        FtsFileExceptionEntity fileException = new()
        {
            DestinationId = _errorLogDetail.DestinationId,
            EventId = _errorLogDetail.EventId,
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

        return _exceptionId;
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
