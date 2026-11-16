using System;
using System.Collections.Generic;
using System.Linq;

using FxferService.Infrastructure.Persistence;
using FxferService.Infrastructure.Persistence.Batch;
using FxferService.Infrastructure.Persistence.Batch.Contracts;
using FxferService.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FxferService.Host.UnitTests;

public sealed class F03_020_CreateFtsFileDeliveryDataAccessTests
{
    [Fact]
    public void F03_020_creates_delivery_and_events_for_renamed_and_archived()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase($"f03_020_{Guid.NewGuid():N}")
            .Options;

        using var db = new FxferDbContext(options);

        var fileId = Guid.NewGuid();
        var sourceLocationId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        db.FileManagement.Add(new FtsFileManagementEntity
        {
            FileId = fileId,
            FtsSourceLocationId = sourceLocationId,
            FileName = "original.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Transfering,
            FileStatusAt = DateTimeOffset.UtcNow,
            FileSize = 1,
            OperatorId = "operator",
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
        });

        db.SaveChanges();

        var dbcontext = new FileTransferEntityDbContext(new TestDbSession(db), true);
        var destinations = new List<DestinationModel>
        {
            new()
            {
                DestinationId = destinationId,
                IsRenameAtDestination = true,
                IsArchive = true,
            },
        };
        var dataAccess = new TestableCreateFtsFileDeliveryDataAccess(fileId, destinationId, "renamed.txt", "operator", destinations);

        var resultEventId = dataAccess.ExecuteQueryPublic(dbcontext);

        Assert.Single(db.FileDeliveries);

        var events = db.FileEvents.ToList();
        Assert.Equal(2, events.Count);
        Assert.Contains(events, e => e.EventCode == FileDeliveryEventConstants.Renamed);

        var archivedEvent = Assert.Single(events, e => e.EventCode == FileDeliveryEventConstants.Archieved);
        Assert.Equal(resultEventId, archivedEvent.EventId);
        Assert.Equal("renamed.txt", archivedEvent.TrackDescription);
    }

    [Fact]
    public void F03_020_creates_delivery_and_transferred_event_without_rename()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase($"f03_020_{Guid.NewGuid():N}")
            .Options;

        using var db = new FxferDbContext(options);

        var fileId = Guid.NewGuid();
        var sourceLocationId = Guid.NewGuid();
        var destinationId = Guid.NewGuid();

        db.FileManagement.Add(new FtsFileManagementEntity
        {
            FileId = fileId,
            FtsSourceLocationId = sourceLocationId,
            FileName = "same.txt",
            FileStatus = DataAccessConstants.FileStatusConstant.Transfering,
            FileStatusAt = DateTimeOffset.UtcNow,
            FileSize = 1,
            OperatorId = "operator",
            LastModifiedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
        });

        db.SaveChanges();

        var dbcontext = new FileTransferEntityDbContext(new TestDbSession(db), true);
        var destinations = new List<DestinationModel>
        {
            new()
            {
                DestinationId = destinationId,
                IsRenameAtDestination = false,
                IsArchive = false,
            },
        };
        var dataAccess = new TestableCreateFtsFileDeliveryDataAccess(fileId, destinationId, "same.txt", "operator", destinations);

        var resultEventId = dataAccess.ExecuteQueryPublic(dbcontext);

        Assert.Single(db.FileDeliveries);

        var transferredEvent = Assert.Single(db.FileEvents, e => e.EventCode == FileDeliveryEventConstants.Transferred);
        Assert.Equal(resultEventId, transferredEvent.EventId);
        Assert.Equal("same.txt", transferredEvent.TrackDescription);
    }

    private sealed class TestDbSession : IDbSession
    {
        public TestDbSession(FxferDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public FxferDbContext DbContext { get; }

        public System.Data.IDbTransaction BeginTransaction() => new NoopDbTransaction();

        public void Dispose()
        {
        }

        private sealed class NoopDbTransaction : System.Data.IDbTransaction
        {
            public void Dispose()
            {
            }

            public void Commit()
            {
            }

            public void Rollback()
            {
            }

            public System.Data.IDbConnection Connection => null!;

            public System.Data.IsolationLevel IsolationLevel => System.Data.IsolationLevel.Unspecified;
        }
    }

    private sealed class TestableCreateFtsFileDeliveryDataAccess : CreateFtsFileDeliveryDataAccess
    {
        public TestableCreateFtsFileDeliveryDataAccess(
            Guid fileId,
            Guid destinationId,
            string destinationFileName,
            string operatorId,
            List<DestinationModel> destinations)
            : base(fileId, destinationId, destinationFileName, operatorId, destinations)
        {
        }

        public Guid ExecuteQueryPublic(IDbContextBase dbcontext)
        {
            return ExecuteQuery(dbcontext);
        }
    }
}