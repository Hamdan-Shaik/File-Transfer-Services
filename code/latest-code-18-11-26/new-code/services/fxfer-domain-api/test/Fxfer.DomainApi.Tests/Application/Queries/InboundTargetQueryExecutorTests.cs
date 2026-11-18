using Microsoft.EntityFrameworkCore;

using Fxfer.DomainApi.Application.Queries;
using Fxfer.DomainApi.Contracts.Queries;
using Fxfer.DomainApi.Contracts.Queries.Parameters;
using Fxfer.DomainApi.Infrastructure.DataAccess;
using Fxfer.DomainApi.Infrastructure.DataAccess.Entities;

namespace Fxfer.DomainApi.Tests.Application.Queries;

public sealed class InboundTargetQueryExecutorTests
{
    [Fact]
    public async Task F05_032_filters_to_valid_records_and_supports_optional_metadata_and_contract_filters()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;

        var fteMetadataId = Guid.Parse("00000000-0000-0000-0000-000000000111");
        var otherMetadataId = Guid.Parse("00000000-0000-0000-0000-000000000222");

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsInboundTarget.AddRange(
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "JOB-1",
                    ContractName = "CONTRACT-A",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "JOB-2",
                    ContractName = "CONTRACT-B",
                    OperatorId = "op",
                    IsValid = false,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.Parse("00000000-0000-0000-0000-000000000030"),
                    FTEMetadataId = otherMetadataId,
                    ImportJobName = "JOB-3",
                    ContractName = "CONTRACT-A",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new InboundTargetQueryExecutor();
            var query = new InboundTargetQuery
            {
                Where = new InboundTargetQueryParms
                {
                    FTEMetadataId = fteMetadataId,
                    ContractName = "CONTRACT-A"
                }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(fteMetadataId, results[0].FTEMetadataId);
            Assert.Equal("CONTRACT-A", results[0].ContractName);
            Assert.Equal("JOB-1", results[0].ImportJobName);
        }
    }

    [Fact]
    public async Task F05_033_deduplicates_by_metadata_id_selecting_smallest_inbound_target_id()
    {
        var options = new DbContextOptionsBuilder<FxferDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var now = DateTimeOffset.UtcNow;
        var fteMetadataId = Guid.NewGuid();

        await using (var dbContext = new FxferDbContext(options))
        {
            dbContext.FtsInboundTarget.AddRange(
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.Parse("00000000-0000-0000-0000-000000000020"),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "JOB-20",
                    ContractName = "CONTRACT",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                },
                new FtsInboundTarget
                {
                    FtsInboundTargetId = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                    FTEMetadataId = fteMetadataId,
                    ImportJobName = "JOB-10",
                    ContractName = "CONTRACT",
                    OperatorId = "op",
                    IsValid = true,
                    CreationDate = now,
                    LastModifiedDate = now
                });

            await dbContext.SaveChangesAsync();
        }

        await using (var dbContext = new FxferDbContext(options))
        {
            var executor = new InboundTargetQueryExecutor();
            var query = new InboundTargetQuery
            {
                Where = new InboundTargetQueryParms { FTEMetadataId = fteMetadataId }
            };

            var results = await executor.ExecuteAsync(dbContext, query, CancellationToken.None);

            Assert.Single(results);
            Assert.Equal(Guid.Parse("00000000-0000-0000-0000-000000000010"), results[0].FtsInboundTargetId);
            Assert.Equal("JOB-10", results[0].ImportJobName);
        }
    }
}

