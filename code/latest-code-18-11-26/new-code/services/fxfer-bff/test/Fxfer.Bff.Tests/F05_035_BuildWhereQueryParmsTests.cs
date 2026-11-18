using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Tests;

public sealed class F05_035_BuildWhereQueryParmsTests
{
    [Fact]
    public void F05_035_maps_only_supported_filters_and_parses_only_when_valid()
    {
        var searchFields = new List<FileTransferQueryProvider.TrackingSearchField>
        {
            new("FileId", "8a2f5b91-7a6a-4d01-8a84-0d9d4b7e36b6"),
            new("FileName", "test.txt"),
            new("FileStatus", "Processed"),
            new("Direction", "Inbound"),
            new("TransportType", "SFTP"),
            new("TradingPartnerId", "TP"),
            new("SourceLocationId", "5b0a7a0b-1bb8-4e73-9cda-6c8db00e81c1"),
            new("FromDate", "not-a-date"),
            new("ToDate", "2026-11-17"),
            new("FromSize", "not-a-decimal"),
            new("ToSize", "123.45"),
            new("Unsupported", "ignored")
        };

        var where = FileTransferQueryProvider.BuildWhereQueryParms(searchFields);

        Assert.Equal("8a2f5b91-7a6a-4d01-8a84-0d9d4b7e36b6", where.FileId);
        Assert.Equal("test.txt", where.FileName);
        Assert.Equal("Processed", where.FileStatus);
        Assert.Equal("Inbound", where.FileDirection);
        Assert.Equal("SFTP", where.FileTransportType);
        Assert.Equal("TP", where.TradingPartner);
        Assert.Equal("5b0a7a0b-1bb8-4e73-9cda-6c8db00e81c1", where.FileSourceLocationId);

        Assert.Null(where.StartDate);
        Assert.NotNull(where.EndDate);
        Assert.Equal(new DateTime(2026, 11, 17), where.EndDate!.Value.Date);

        Assert.Null(where.FileSizeFrom);
        Assert.Equal(123.45m, where.FileSizeThrough);
    }
}

