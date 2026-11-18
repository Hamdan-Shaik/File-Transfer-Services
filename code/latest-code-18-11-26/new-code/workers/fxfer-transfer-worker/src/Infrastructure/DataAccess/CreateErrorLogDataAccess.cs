using System.Net.Http.Json;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class CreateErrorLogDataAccess : DataAccessBase<int>
{
    private readonly ErrorLogDetailModel _errorLogDetail;
    private readonly Guid _exceptionId;

    public CreateErrorLogDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        ErrorLogDetailModel errorLog)
        : base(domainApiClient, requestContextAccessor)
    {
        _errorLogDetail = errorLog;
        _exceptionId = Guid.NewGuid();
    }

    protected override async Task<int> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        SetLimitedValue();

        var request = new FileExceptionCreateRequest
        {
            ExceptionId = _exceptionId,
            FileId = _errorLogDetail.FileId,
            ExceptionType = _errorLogDetail.ErrorType,
            Message = _errorLogDetail.ErrorText,
            MethodName = _errorLogDetail.MethodName,
            StackTrace = _errorLogDetail.StackTrace,
            OperatorId = _errorLogDetail.OperatorId,
            CreatedDate = DateTime.UtcNow,
            LastModifiedTs = DateTime.UtcNow
        };

        using var httpRequest = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/fxfer-domain/v1/file-tracking/" + _errorLogDetail.FileId + "/exceptions")
        {
            Content = JsonContent.Create(request)
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        return 1;
    }

    private void SetLimitedValue()
    {
        _errorLogDetail.ErrorType = Truncate(_errorLogDetail.ErrorType, GetMaxLength(ErrorLogConstants.ExceptionType));
        _errorLogDetail.ErrorText = Truncate(_errorLogDetail.ErrorText, GetMaxLength(ErrorLogConstants.Message));
        _errorLogDetail.MethodName = Truncate(_errorLogDetail.MethodName, GetMaxLength(ErrorLogConstants.MethodName));
        _errorLogDetail.StackTrace = Truncate(_errorLogDetail.StackTrace, GetMaxLength(ErrorLogConstants.StackTrace));
    }

    private string? Truncate(string? value, int maxLength)
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
        return name switch
        {
            ErrorLogConstants.ExceptionType => 2000,
            ErrorLogConstants.Message => 2000,
            ErrorLogConstants.MethodName => 2000,
            ErrorLogConstants.StackTrace => 4000,
            _ => 0
        };
    }

    private sealed class FileExceptionCreateRequest
    {
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

