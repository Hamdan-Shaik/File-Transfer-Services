using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Json;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class CreateFtsFileExceptionEventDataAccess : DataAccessBase<Guid>
{
    private readonly Guid _fileId;
    private readonly Guid _destinationId;
    private readonly string _destinationFileName;
    private readonly string _operatorId;
    private readonly ErrorLogDetailModel _errorLogDetail;
    private readonly Guid _exceptionId;

    public CreateFtsFileExceptionEventDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        Guid fileId,
        Guid destinationId,
        string destinationFileName,
        string operatorId,
        ErrorLogDetailModel errorLog)
        : base(domainApiClient, requestContextAccessor)
    {
        _fileId = fileId;
        _destinationId = destinationId;
        _destinationFileName = destinationFileName;
        _operatorId = operatorId;
        _errorLogDetail = errorLog;
        _exceptionId = Guid.NewGuid();
    }

    protected override async Task<Guid> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var eventId = await CreateExceptionEvent(DomainApiClient, cancellationToken);
        await AddErrorLog(DomainApiClient, eventId, cancellationToken);

        return eventId;
    }

    private async Task<Guid> CreateExceptionEvent(IFxferDomainApiClient domainApiClient, CancellationToken cancellationToken)
    {
        var eventId = Guid.NewGuid();

        var requestBody = new FileEventCreateRequest
        {
            EventId = eventId,
            FileId = _fileId,
            EventCD = FileDeliveryEventConstants.Exception,
            DestinationId = _destinationId,
            OperatorId = _operatorId,
            EventTrackDescription = _destinationFileName,
            CreatedDate = DateTime.UtcNow,
            LastModifiedTs = DateTime.UtcNow
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fxfer-domain/v1/file-tracking/" + _fileId + "/events")
        {
            Content = JsonContent.Create(requestBody)
        };

        using var response = await domainApiClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return eventId;
    }

    private async Task AddErrorLog(IFxferDomainApiClient domainApiClient, Guid eventId, CancellationToken cancellationToken)
    {
        SetLimitedValue();

        var requestBody = new FileExceptionCreateRequest
        {
            DestinationId = _destinationId,
            EventId = eventId,
            ExceptionId = _exceptionId,
            FileId = _fileId,
            ExceptionType = _errorLogDetail.ErrorType,
            Message = _errorLogDetail.ErrorText,
            MethodName = _errorLogDetail.MethodName,
            StackTrace = _errorLogDetail.StackTrace,
            OperatorId = _errorLogDetail.OperatorId,
            CreatedDate = DateTime.UtcNow,
            LastModifiedTs = DateTime.UtcNow
        };

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fxfer-domain/v1/file-tracking/" + _fileId + "/exceptions")
        {
            Content = JsonContent.Create(requestBody)
        };

        using var response = await domainApiClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private void SetLimitedValue()
    {
        _errorLogDetail.ErrorType = Truncate(_errorLogDetail.ErrorType, GetMaxLength(ErrorLogConstants.ExceptionType));
        _errorLogDetail.ErrorText = Truncate(_errorLogDetail.ErrorText, GetMaxLength(ErrorLogConstants.Message));
        _errorLogDetail.MethodName = Truncate(_errorLogDetail.MethodName, GetMaxLength(ErrorLogConstants.MethodName));
        _errorLogDetail.StackTrace = Truncate(_errorLogDetail.StackTrace, GetMaxLength(ErrorLogConstants.StackTrace));
    }

    private static string? Truncate(string? value, int maxLength)
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

    private static int GetMaxLength(string name)
    {
        var maxLength = 0;

        var strLenAttr = typeof(FtsFileException)
            .GetProperty(name)
            ?.GetCustomAttributes(typeof(StringLengthAttribute), inherit: false)
            .Cast<StringLengthAttribute>()
            .SingleOrDefault();

        if (strLenAttr is not null)
        {
            maxLength = strLenAttr.MaximumLength;
        }

        return maxLength;
    }

    private sealed class FtsFileException
    {
        [StringLength(2000)]
        public string? ExceptionType { get; init; }

        [StringLength(2000)]
        public string? MethodName { get; init; }

        [StringLength(2000)]
        public string? Message { get; init; }

        [StringLength(4000)]
        public string? StackTrace { get; init; }
    }

    private sealed class FileEventCreateRequest
    {
        public Guid EventId { get; init; }
        public Guid FileId { get; init; }
        public string? EventCD { get; init; }
        public Guid DestinationId { get; init; }
        public string? OperatorId { get; init; }
        public string? EventTrackDescription { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime LastModifiedTs { get; init; }
    }

    private sealed class FileExceptionCreateRequest
    {
        public Guid DestinationId { get; init; }
        public Guid EventId { get; init; }
        public Guid ExceptionId { get; init; }
        public Guid FileId { get; init; }
        public string? ExceptionType { get; init; }
        public string? Message { get; init; }
        public string? MethodName { get; init; }
        public string? StackTrace { get; init; }
        public string? OperatorId { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime LastModifiedTs { get; init; }
    }
}

