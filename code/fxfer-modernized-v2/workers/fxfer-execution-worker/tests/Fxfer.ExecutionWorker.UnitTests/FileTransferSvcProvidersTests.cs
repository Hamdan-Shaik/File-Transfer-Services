using System.Collections.Generic;
using Fxfer.ExecutionWorker.Application.Clients;
using Fxfer.ExecutionWorker.Domain.Clients;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class FileTransferSvcProvidersTests
{
    [Fact]
    public void GetIntegrationFile_returns_null_when_query_service_returns_null()
    {
        var provider = new FileTransferSvcQueryServiceProvider(new NullQueryService());

        var result = provider.GetIntegrationFile(new GetIntegrationFileQuery());

        Assert.Null(result);
    }

    [Fact]
    public void GetIntegrationFile_returns_null_when_results_is_null()
    {
        var provider = new FileTransferSvcQueryServiceProvider(new QueryServiceReturns(new GetIntegrationFileQuery { Results = null }));

        var result = provider.GetIntegrationFile(new GetIntegrationFileQuery());

        Assert.Null(result);
    }

    [Fact]
    public void GetIntegrationFile_returns_null_when_results_is_empty()
    {
        var provider = new FileTransferSvcQueryServiceProvider(new QueryServiceReturns(new GetIntegrationFileQuery { Results = new List<FtsIntegrationFileModel>() }));

        var result = provider.GetIntegrationFile(new GetIntegrationFileQuery());

        Assert.Null(result);
    }

    [Fact]
    public void GetIntegrationFile_returns_first_result_when_present()
    {
        var expected = new FtsIntegrationFileModel();
        var provider = new FileTransferSvcQueryServiceProvider(
            new QueryServiceReturns(new GetIntegrationFileQuery { Results = new List<FtsIntegrationFileModel> { expected, new FtsIntegrationFileModel() } }));

        var result = provider.GetIntegrationFile(new GetIntegrationFileQuery());

        Assert.Same(expected, result);
    }

    [Fact]
    public void UpdateIntegrationFile_delegates_to_service()
    {
        var expected = new IntegrationFileUpdated();
        var service = new CapturingIntegrationFileService(expected);
        var provider = new FileTransferSvcServiceProvider(service);

        var command = new UpdateIntegrationFileCommand();
        var result = provider.UpdateIntegrationFile(command);

        Assert.Same(command, service.CapturedCommand);
        Assert.Same(expected, result);
    }

    private sealed class NullQueryService : IFileTransferSvcQueryService
    {
        public GetIntegrationFileQuery? GetIntegrationFile(GetIntegrationFileQuery query) => null;
    }

    private sealed class QueryServiceReturns : IFileTransferSvcQueryService
    {
        private readonly GetIntegrationFileQuery _response;

        public QueryServiceReturns(GetIntegrationFileQuery response)
        {
            _response = response;
        }

        public GetIntegrationFileQuery? GetIntegrationFile(GetIntegrationFileQuery query) => _response;
    }

    private sealed class CapturingIntegrationFileService : IFileTransferSvcService
    {
        private readonly IntegrationFileUpdated _result;

        public CapturingIntegrationFileService(IntegrationFileUpdated result)
        {
            _result = result;
        }

        public UpdateIntegrationFileCommand? CapturedCommand { get; private set; }

        public IntegrationFileUpdated UpdateIntegrationFile(UpdateIntegrationFileCommand command)
        {
            CapturedCommand = command;
            return _result;
        }
    }
}

