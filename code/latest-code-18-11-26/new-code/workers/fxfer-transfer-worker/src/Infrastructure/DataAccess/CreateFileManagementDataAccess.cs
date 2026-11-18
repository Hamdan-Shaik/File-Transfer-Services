using System.Net.Http.Json;

using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Application.DataAccess;
using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Infrastructure.DataAccess;

public sealed class CreateFileManagementDataAccess : DataAccessBase<FileManagementResponseModel>
{
    private readonly FileManagementRequestModel _requestModel;

    public CreateFileManagementDataAccess(
        IFxferDomainApiClient domainApiClient,
        IRequestContextAccessor requestContextAccessor,
        FileManagementRequestModel requestModel)
        : base(domainApiClient, requestContextAccessor)
    {
        _requestModel = requestModel;
    }

    protected override async Task<FileManagementResponseModel> ExecuteQueryAsync(CancellationToken cancellationToken)
    {
        var responseModel = new FileManagementResponseModel();

        var request = BuildCreateRequest();
        responseModel.FileId = request.FileID;

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/fxfer-domain/v1/file-tracking")
        {
            Content = JsonContent.Create(request)
        };

        using var response = await DomainApiClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<FileManagementResponseModel>(cancellationToken: cancellationToken);
        if (payload is not null)
        {
            responseModel.FileTrackId = payload.FileTrackId;
        }

        return responseModel;
    }

    private FileManagementRequestModel BuildCreateRequest()
    {
        var fileId = Guid.NewGuid();
        var request = new FileManagementRequestModel
        {
            ChecksumCode = _requestModel.ChecksumCode,
            FileID = fileId,
            FileName = _requestModel.FileName,
            FileSize = _requestModel.FileSize,
            SourceLocationId = _requestModel.SourceLocationId,
            FileStatus = FileStatusConstant.Transfering,
            OperatorId = _requestModel.OperatorId,
        };

        return request;
    }
}

