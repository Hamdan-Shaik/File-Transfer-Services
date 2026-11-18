using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class FileExceptionQueryExecutorTests
{
    [Fact]
    public async Task F05_029_filters_by_file_id_and_default_sorts_by_exception_id()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var fileId = Guid.Parse("00000000-0000-0000-0000-000000000010");
        var otherFileId = Guid.Parse("00000000-0000-0000-0000-000000000020");
        var now = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsFileManagement.AddRange(
                new FtsFileManagement { FileId = fileId, FileSourceLocationId = Guid.NewGuid(), FileName = "a", FileStatus = "1", OperatorId = "op", CreatedDate = now },
                new FtsFileManagement { FileId = otherFileId, FileSourceLocationId = Guid.NewGuid(), FileName = "b", FileStatus = "1", OperatorId = "op", CreatedDate = now });

            dbContext.FtsFileExceptions.AddRange(
                new FtsFileException { ExceptionId = Guid.Parse("00000000-0000-0000-0000-000000000002"), FileId = fileId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "B", MethodName = "M", CreatedDate = now.AddDays(-1), OperatorId = "op", LastModifiedTs = now },
                new FtsFileException { ExceptionId = Guid.Parse("00000000-0000-0000-0000-000000000001"), FileId = fileId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "A", MethodName = "M", CreatedDate = now.AddDays(-1), OperatorId = "op", LastModifiedTs = now },
                new FtsFileException { ExceptionId = Guid.Parse("00000000-0000-0000-0000-000000000003"), FileId = otherFileId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "C", MethodName = "M", CreatedDate = now.AddDays(-1), OperatorId = "op", LastModifiedTs = now });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileExceptionQueryExecutor();
            var query = new FileExceptionQuery { Where = new FileExceptionQueryParms { FileId = fileId.ToString() } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Equal(2, results.Count);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000001"), results[0].ExceptionId);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000002"), results[1].ExceptionId);
        }
    }

    [Fact]
    public async Task F05_030_projects_created_date_from_file_management_created_date()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var fileId = Guid.NewGuid();
        var fileCreated = new DateTime(2026, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        var exceptionCreated = new DateTime(2001, 1, 1, 1, 1, 1, DateTimeKind.Utc);

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsFileManagement.Add(new FtsFileManagement { FileId = fileId, FileSourceLocationId = Guid.NewGuid(), FileName = "a", FileStatus = "1", OperatorId = "op", CreatedDate = fileCreated });
            dbContext.FtsFileExceptions.Add(new FtsFileException { ExceptionId = Guid.NewGuid(), FileId = fileId, DestinationId = Guid.NewGuid(), EventId = Guid.NewGuid(), ExceptionType = "A", MethodName = "M", CreatedDate = exceptionCreated, OperatorId = "op", LastModifiedTs = exceptionCreated });
            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new FileExceptionQueryExecutor();
            var query = new FileExceptionQuery { Where = new FileExceptionQueryParms { FileId = fileId.ToString() } };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(fileCreated, results[0].CreatedDate);
        }
    }
}

