using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Fxfer.DomainApi.Infrastructure.DataAccess;

public sealed class FileTransferReadOnlyRepository : IFileTransferReadOnlyRepository
{
    private readonly FxferDbContext _dbContext;
    private readonly ILogger<FileTransferReadOnlyRepository> _logger;

    public FileTransferReadOnlyRepository(FxferDbContext dbContext, ILogger<FileTransferReadOnlyRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<FtpServerModel>> GetFtpServerAsync(FtpServerQuery query, CancellationToken cancellationToken)
    {
        try
        {
            await OpenConnectionAsync(cancellationToken);
            return await ExecuteQueryAsync(new GetFtpServerQueryExecutor(), query, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(FormatErrorMsg(ex, query, CommonConstants.Validation.Messages.FTRepositoryGetResultsError));
            return Array.Empty<FtpServerModel>();
        }
        finally
        {
            await CloseConnectionAsync();
        }
    }

    public async Task<IReadOnlyList<FtsInboundTargetModel>> GetInboundTargetAsync(InboundTargetQuery query, CancellationToken cancellationToken)
    {
        try
        {
            await OpenConnectionAsync(cancellationToken);
            return await ExecuteQueryAsync(new InboundTargetQueryExecutor(), query, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(FormatErrorMsg(ex, query, CommonConstants.Validation.Messages.FTRepositoryGetResultsError));
            return Array.Empty<FtsInboundTargetModel>();
        }
        finally
        {
            await CloseConnectionAsync();
        }
    }

    private async Task OpenConnectionAsync(CancellationToken ct)
    {
        if (!_dbContext.Database.IsRelational())
        {
            return;
        }

        var connection = _dbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
        {
            await _dbContext.Database.OpenConnectionAsync(ct);
        }
    }

    private Task CloseConnectionAsync()
    {
        return _dbContext.Database.IsRelational()
            ? _dbContext.Database.CloseConnectionAsync()
            : Task.CompletedTask;
    }

    private async Task<IReadOnlyList<FtpServerModel>> ExecuteQueryAsync(
        GetFtpServerQueryExecutor executor,
        FtpServerQuery query,
        CancellationToken ct)
    {
        return await executor.ExecuteAsync(_dbContext, query, ct);
    }

    private async Task<IReadOnlyList<FtsInboundTargetModel>> ExecuteQueryAsync(
        InboundTargetQueryExecutor executor,
        InboundTargetQuery query,
        CancellationToken ct)
    {
        return await executor.ExecuteAsync(_dbContext, query, ct);
    }

    private static string FormatErrorMsg(Exception ex, object query, string msj)
    {
        return string.Format("{0}: {1}-{2}", msj, query.GetType(), ex.Message);
    }
}
