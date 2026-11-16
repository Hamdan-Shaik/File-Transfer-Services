using System.Data;
using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Tests;

public sealed class F03_020_DataAccessTests
{
    [Fact]
    public void F03_020_records_delivery_and_archived_event_with_rename()
    {
        var fileId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase($"f03_020_{Guid.NewGuid():N}")
            .Options;

        var dbContext = new FxferDbContext(options);
        var dbSession = new TestDbSession(dbContext);

        dbContext.Set<FtsFileManagementEntity>().Add(new FtsFileManagementEntity
        {
            FileId = fileId,
            FtsSourceLocationId = Guid.NewGuid(),
            FileName = "source.txt",
            FileStatus = "0",
            FileStatusAt = DateTimeOffset.UtcNow,
            OperatorId = "operator",
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            FileSize = 0m,
        });
        dbContext.SaveChanges();

        var destinations = new List<DestinationModel>
        {
            new()
            {
                DestinationId = destinationId,
                IsRenameAtDestination = true,
                IsArchive = true,
            },
        };

        var dataAccess = new TestableCreateFtsFileDeliveryDataAccess(
            fileId,
            destinationId,
            destinationFileName: "destination.txt",
            operatorId: "operator",
            destinations,
            dbSession);

        var resultEventId = dataAccess.Execute();

        Assert.Single(dbContext.Set<FtsFileDeliveryEntity>().ToList());

        var events = dbContext.Set<FtsFileEventEntity>().ToList();
        Assert.Equal(2, events.Count);
        Assert.Contains(events, e => e.EventCode == FileDeliveryEventConstants.Renamed);

        var archivedEvent = Assert.Single(events, e => e.EventCode == FileDeliveryEventConstants.Archieved);
        Assert.Equal(resultEventId, archivedEvent.EventId);
        Assert.Equal("destination.txt", archivedEvent.TrackDescription);
    }

    [Fact]
    public void F03_020_records_delivery_and_transferred_event_without_rename()
    {
        var fileId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase($"f03_020_{Guid.NewGuid():N}")
            .Options;

        var dbContext = new FxferDbContext(options);
        var dbSession = new TestDbSession(dbContext);

        dbContext.Set<FtsFileManagementEntity>().Add(new FtsFileManagementEntity
        {
            FileId = fileId,
            FtsSourceLocationId = Guid.NewGuid(),
            FileName = "destination.txt",
            FileStatus = "0",
            FileStatusAt = DateTimeOffset.UtcNow,
            OperatorId = "operator",
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            FileSize = 0m,
        });
        dbContext.SaveChanges();

        var destinations = new List<DestinationModel>
        {
            new()
            {
                DestinationId = destinationId,
                IsRenameAtDestination = false,
                IsArchive = false,
            },
        };

        var dataAccess = new TestableCreateFtsFileDeliveryDataAccess(
            fileId,
            destinationId,
            destinationFileName: "destination.txt",
            operatorId: "operator",
            destinations,
            dbSession);

        var resultEventId = dataAccess.Execute();

        Assert.Single(dbContext.Set<FtsFileDeliveryEntity>().ToList());

        var transferredEvent = Assert.Single(dbContext.Set<FtsFileEventEntity>(), e => e.EventCode == FileDeliveryEventConstants.Transferred);
        Assert.Equal(fileId, transferredEvent.FileId);
        Assert.Equal(destinationId, transferredEvent.DestinationId);
        Assert.Equal(resultEventId, transferredEvent.EventId);
        Assert.Equal("destination.txt", transferredEvent.TrackDescription);
    }

    [Fact]
    public void F03_020_records_exception_event_and_error_log()
    {
        var fileId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase($"f03_020_{Guid.NewGuid():N}")
            .Options;

        var dbContext = new FxferDbContext(options);
        var dbSession = new TestDbSession(dbContext);

        var errorLog = new ErrorLogDetailModel
        {
            FileId = fileId,
            DestinationId = destinationId,
            ErrorType = "ErrorType",
            ErrorText = "ErrorText",
            MethodName = "MethodName",
            StackTrace = "StackTrace",
            OperatorId = "operator",
        };

        var dataAccess = new TestableCreateFtsFileExceptionEventDataAccess(
            fileId,
            destinationId,
            destinationFileName: "destination.txt",
            operatorId: "operator",
            errorLog,
            dbSession);

        var resultEventId = dataAccess.Execute();

        var exceptionEvent = Assert.Single(dbContext.Set<FtsFileEventEntity>().ToList());
        Assert.Equal(FileDeliveryEventConstants.Exception, exceptionEvent.EventCode);
        Assert.Equal(resultEventId, exceptionEvent.EventId);
        Assert.Equal("destination.txt", exceptionEvent.TrackDescription);

        var exceptionLog = Assert.Single(dbContext.Set<FtsFileExceptionEntity>().ToList());
        Assert.Equal(destinationId, exceptionLog.DestinationId);
        Assert.Equal(resultEventId, exceptionLog.EventId);
        Assert.Equal(fileId, exceptionLog.FileId);
        Assert.Equal("ErrorType", exceptionLog.ExceptionType);
        Assert.Equal("ErrorText", exceptionLog.Message);
    }

    private sealed class TestableCreateFtsFileDeliveryDataAccess : CreateFtsFileDeliveryDataAccess
    {
        private readonly IDbSession _dbSession;

        public TestableCreateFtsFileDeliveryDataAccess(
            Guid fileId,
            Guid destinationId,
            string destinationFileName,
            string operatorId,
            List<DestinationModel> destinations,
            IDbSession dbSession)
            : base(fileId, destinationId, destinationFileName, operatorId, destinations)
        {
            _dbSession = dbSession;
        }

        public override IDbSession CreateDbSession()
        {
            return _dbSession;
        }
    }

    private sealed class TestableCreateFtsFileExceptionEventDataAccess : CreateFtsFileExceptionEventDataAccess
    {
        private readonly IDbSession _dbSession;

        public TestableCreateFtsFileExceptionEventDataAccess(
            Guid fileId,
            Guid destinationId,
            string destinationFileName,
            string operatorId,
            ErrorLogDetailModel errorLog,
            IDbSession dbSession)
            : base(fileId, destinationId, destinationFileName, operatorId, errorLog)
        {
            _dbSession = dbSession;
        }

        public override IDbSession CreateDbSession()
        {
            return _dbSession;
        }
    }

    private sealed class TestDbSession : IDbSession
    {
        private readonly FxferDbContext _dbContext;

        public TestDbSession(FxferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FxferDbContext DbContext => _dbContext;

        public IDbTransaction BeginTransaction()
        {
            return new NoOpDbTransaction();
        }

        public void Dispose()
        {
            // Intentionally no-op; the test owns the DbContext lifetime.
        }

        private sealed class NoOpDbTransaction : IDbTransaction
        {
            public IDbConnection Connection => null!;

            public IsolationLevel IsolationLevel => IsolationLevel.ReadCommitted;

            public void Commit() { }

            public void Rollback() { }

            public void Dispose() { }
        }
    }
}