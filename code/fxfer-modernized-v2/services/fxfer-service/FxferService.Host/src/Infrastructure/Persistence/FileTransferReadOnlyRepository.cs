using System;
using System.Collections.Generic;
using System.Data;

using FxferService.Application.Contracts.Queries;
using FxferService.Application.LegacyContracts;
using FxferService.Application.QueryHandlers;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Domain;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence;

public class FileTransferReadOnlyRepository : IFileTransferReadOnlyRepository
{
    private readonly FxferDbContext _dbContext;

    public FileTransferReadOnlyRepository(FxferDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual IReadOnlyList<FtpServerModel> GetFtpServer(FtpServerQuery query)
    {
        return GetResults<FtpServerQuery, GetFtpServerQueryExecutor, FtpServerModel>(query).Results!;
    }

    public virtual IReadOnlyList<FtsInboundTargetModel> GetInboundTarget(InboundTargetQuery query)
    {
        return GetResults<InboundTargetQuery, InboundTargetQueryExecutor, FtsInboundTargetModel>(query).Results!;
    }

    private TQuery GetResults<TQuery, TQueryExecutor, TModel>(TQuery query)
        where TQuery : Query<TModel>
        where TQueryExecutor : QueryExecutorBase<TModel, TQuery>, new()
    {
        query.Results ??= new List<TModel>();

        try
        {
            OpenConnection(_dbContext);
            return ExecuteQuery<TQuery, TQueryExecutor, TModel>(query, _dbContext);
        }
        catch (Exception ex)
        {
            LoggerManager.Logger.LogError(FormatErrorMsg(ex, query, CommonConstants.Validation.Messages.FTRepositoryGetResultsError));
            return query;
        }
        finally
        {
            _dbContext.Database.CloseConnection();
        }
    }

    private void OpenConnection(FxferDbContext dbContext)
    {
        if (dbContext.Database.GetDbConnection().State != ConnectionState.Open)
        {
            dbContext.Database.OpenConnection();
        }
    }

    private TQuery ExecuteQuery<TQuery, TQueryExecutor, TModel>(TQuery query, FxferDbContext dbContext)
        where TQuery : Query<TModel>
        where TQueryExecutor : QueryExecutorBase<TModel, TQuery>, new()
    {
        using (var fileTransferDbContext = new FileTransferEntityDbContext(dbContext))
        {
            try
            {
                return new TQueryExecutor().Execute(fileTransferDbContext, query);
            }
            finally
            {
                fileTransferDbContext.Dispose();
            }
        }
    }

    private string FormatErrorMsg(Exception ex, object query, string msj)
    {
        return string.Format("{0}: {1}-{2}", msj, query.GetType(), ex.Message);
    }
}