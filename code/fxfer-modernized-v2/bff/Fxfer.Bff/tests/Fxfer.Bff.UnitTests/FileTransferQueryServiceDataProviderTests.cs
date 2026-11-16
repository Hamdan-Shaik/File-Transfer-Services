using Fxfer.Bff.Downstream.FxferService;
using Fxfer.Bff.Downstream.FxferService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using Xunit;
using Service = FileTransferSvcQueryService;

namespace Fxfer.Bff.UnitTests;

public sealed class FileTransferQueryServiceDataProviderTests
{
    [Fact]
    public void F01_047_and_F01_048_uses_last_sort_and_forwards_paging()
    {
        var fake = new FakeQueryService();
        fake.OnGetFtpServers = query =>
        {
            fake.LastFtpServerQuery = query;
            return new Service.FtpServerQuery { Results = new List<Service.FtpServerModel>(), RowCount = 0 };
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var request = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "op" },
            Data = new FtpServerModel { RemovePassword = null }
        };

        var criteria = new SearchCriteriaModel
        {
            PagingCriteria = new PagingCriteriaModel { Page = 3, RowsPerPage = 25 },
            SortCriteria = new List<SortCriteriaModel>
            {
                new SortCriteriaModel { SortField = "FtpServerName", SortDirection = CoreEnumerations.Messaging.SortDirectionType.Descending },
                new SortCriteriaModel { SortField = "FtpUser", SortDirection = CoreEnumerations.Messaging.SortDirectionType.Ascending },
            },
            SearchFields = new List<SearchFieldModel>()
        };

        var response = provider.GetFTPServerCredentials(request, criteria);

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.OK, response.Result.StatusCode);
        Assert.NotNull(fake.LastFtpServerQuery);
        Assert.Equal(25, fake.LastFtpServerQuery!.PageSize);
        Assert.Equal(3, fake.LastFtpServerQuery.CurrentPage);
        Assert.NotNull(fake.LastFtpServerQuery.SortCriteria);
        Assert.Single(fake.LastFtpServerQuery.SortCriteria!);
        Assert.Equal("FtpUser", fake.LastFtpServerQuery.SortCriteria![0].FieldName);
        Assert.Equal(Service.SortOrder.Asc, fake.LastFtpServerQuery.SortCriteria[0].Order);
    }

    [Fact]
    public void F01_045_removes_password_by_default_and_keeps_when_false()
    {
        var fake = new FakeQueryService
        {
            OnGetFtpServers = _ =>
                new Service.FtpServerQuery
                {
                    Results = new List<Service.FtpServerModel>
                    {
                        new Service.FtpServerModel
                        {
                            FtpServerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            FtpPassword = "secret",
                        }
                    },
                    RowCount = 1,
                }
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var criteria = new SearchCriteriaModel { PagingCriteria = new PagingCriteriaModel { Page = 1, RowsPerPage = 1 }, SortCriteria = new List<SortCriteriaModel>() };

        var removed = provider.GetFTPServerCredentials(
            new RequestModel<FtpServerModel> { Data = new FtpServerModel { RemovePassword = null } },
            criteria);

        Assert.Single(removed.Data!);
        Assert.Equal(string.Empty, removed.Data![0].FtpPassword);

        var kept = provider.GetFTPServerCredentials(
            new RequestModel<FtpServerModel> { Data = new FtpServerModel { RemovePassword = false } },
            criteria);

        Assert.Single(kept.Data!);
        Assert.Equal("secret", kept.Data![0].FtpPassword);
    }

    [Fact]
    public void F01_044_fault_exception_sets_error_and_collects_messages()
    {
        var fake = new FakeQueryService
        {
            OnGetFtpServers = _ => throw new FaultException<Service.ServiceException>(
                new Service.ServiceException { ErrorMessages = new List<string> { "m1", "m2" } },
                "fault-msg")
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var response = provider.GetFTPServerCredentials(
            new RequestModel<FtpServerModel> { Data = new FtpServerModel() },
            new SearchCriteriaModel { PagingCriteria = new PagingCriteriaModel { Page = 1, RowsPerPage = 1 }, SortCriteria = new List<SortCriteriaModel>() });

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.Error, response.Result.StatusCode);
        Assert.Contains(response.Result.Messages, m => m.Message == "m1");
        Assert.Contains(response.Result.Messages, m => m.Message == "m2");
        Assert.Contains(response.Result.Messages, m => m.Message == "fault-msg");
    }

    [Fact]
    public void F04_031_ignores_invalid_dates_and_sizes()
    {
        var provider = new FileTransferQueryServiceDataProvider(new FakeQueryService(), NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var method = typeof(FileTransferQueryServiceDataProvider).GetMethod(
            "BuildWhereQueryParms",
            BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(method);

        var fields = new List<SearchFieldModel>
        {
            new SearchFieldModel { FieldName = "FromDate", FieldValue = "not-a-date" },
            new SearchFieldModel { FieldName = "ToDate", FieldValue = "2024-01-02" },
            new SearchFieldModel { FieldName = "FromSize", FieldValue = "NaN" },
            new SearchFieldModel { FieldName = "ToSize", FieldValue = "123.45" },
        };

        var where = (Service.FileTransferTrackingQueryParms)method!.Invoke(provider, new object?[] { fields })!;

        Assert.Null(where.StartDate);
        Assert.Equal(new DateTime(2024, 1, 2), where.EndDate);
        Assert.Null(where.FileSizeFrom);
        Assert.Equal(123.45m, where.FileSizeThrough);
    }

    [Fact]
    public void F01_046_builds_where_clause_and_search_mode_options()
    {
        var op = 0;
        var name = 0;
        var desc = 0;
        var user = 0;
        var ftpServerGuid = new Guid(0x11111111, 0x1111, 0x1111, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11, 0x11);

        var fake = new FakeQueryService();
        fake.OnGetFtpServers = query =>
        {
            fake.LastFtpServerQuery = query;
            return new Service.FtpServerQuery { Results = new List<Service.FtpServerModel>(), RowCount = 0 };
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var request = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = nameof(op) },
            Data = new FtpServerModel
            {
                FtpServerId = ftpServerGuid.ToString(),
                FtpServerName = nameof(name),
                FtpDescription = nameof(desc),
                FtpUser = nameof(user),
                IsValid = true,
            }
        };

        var criteria = new SearchCriteriaModel
        {
            PagingCriteria = new PagingCriteriaModel { Page = 1, RowsPerPage = 5 },
            SortCriteria = new List<SortCriteriaModel>(),
            SearchFields = new List<SearchFieldModel>
            {
                new SearchFieldModel { FieldName = nameof(FtpServerModel.FtpServerName), SearchMode = CoreEnumerations.Messaging.SearchModeType.Contains },
                new SearchFieldModel { FieldName = nameof(FtpServerModel.FtpUser), SearchMode = CoreEnumerations.Messaging.SearchModeType.EqualTo },
            }
        };

        _ = provider.GetFTPServerCredentials(request, criteria);

        Assert.NotNull(fake.LastFtpServerQuery);
        Assert.NotNull(fake.LastFtpServerQuery!.Where);
        Assert.Equal(ftpServerGuid, fake.LastFtpServerQuery.Where.FtpServerId);
        Assert.Equal(nameof(name), fake.LastFtpServerQuery.Where.FtpServerName);
        Assert.Equal(nameof(desc), fake.LastFtpServerQuery.Where.FtpDescription);
        Assert.Equal(nameof(user), fake.LastFtpServerQuery.Where.FtpUser);
        Assert.True(fake.LastFtpServerQuery.Where.IsValid);
        Assert.NotNull(fake.LastFtpServerQuery.Where.SearchOptions);
        Assert.Collection(
            fake.LastFtpServerQuery.Where.SearchOptions!,
            first =>
            {
                Assert.Equal(nameof(FtpServerModel.FtpServerName), first.FieldName);
                Assert.Equal(Service.SearchModeOption.SearchMode.Contains, first.FilterType);
            },
            second =>
            {
                Assert.Equal(nameof(FtpServerModel.FtpUser), second.FieldName);
                Assert.Equal(Service.SearchModeOption.SearchMode.EqualTo, second.FilterType);
            });
    }

    [Fact]
    public void SearchFileTransferTracking_when_no_results_keeps_not_found_and_null_payload()
    {
        var fake = new FakeQueryService
        {
            OnSearchFileTransferMovement = _ => new Service.FileTransferTrackingQuery { Results = new List<Service.FileTransferTrackingModel>(), RowCount = 0 }
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var request = new RequestModel<SearchCriteriaModel>
        {
            Data = new SearchCriteriaModel()
        };

        var response = provider.SearchFileTransferTracking(request);

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.NotFound, response.Result.StatusCode);
        Assert.Null(response.Data);
        Assert.Null(response.PagingResults);
        Assert.NotNull(fake.LastTrackingQuery);
        Assert.Equal(1, fake.LastTrackingQuery!.CurrentPage);
        Assert.Equal(5, fake.LastTrackingQuery.PageSize);
    }

    [Fact]
    public void SearchFileTransferTracking_when_results_maps_rows_and_sets_ok()
    {
        var fake = new FakeQueryService
        {
            OnSearchFileTransferMovement = _ => new Service.FileTransferTrackingQuery
            {
                Results = new List<Service.FileTransferTrackingModel>
                {
                    new Service.FileTransferTrackingModel
                    {
                        FileId = 1.ToString(),
                        FileName = string.Concat('f', '1'),
                        FileDirection = string.Concat('I', 'N'),
                        FileTransportType = new string(new[] { 'F', 'T', 'P' }),
                        FileStatus = new string(new[] { 'O', 'K' }),
                        FileSize = 10,
                        TradingPartner = new string(new[] { 'T', 'P' })
                    },
                    new Service.FileTransferTrackingModel
                    {
                        FileId = 2.ToString(),
                        FileName = string.Concat('f', '2'),
                        FileDirection = new string(new[] { 'O', 'U', 'T' }),
                        FileTransportType = new string(new[] { 'S', 'F', 'T', 'P' }),
                        FileStatus = new string(new[] { 'E', 'R', 'R' }),
                        FileSize = 20,
                        TradingPartner = new string(new[] { 'T', 'P', '2' })
                    },
                },
                RowCount = 123,
            }
        };

        var provider = new FileTransferQueryServiceDataProvider(fake, NullLogger<FileTransferQueryServiceDataProvider>.Instance, new HttpContextAccessor());

        var request = new RequestModel<SearchCriteriaModel>
        {
            Data = new SearchCriteriaModel { PagingCriteria = new PagingCriteriaModel { Page = 2, RowsPerPage = 25 } }
        };

        var response = provider.SearchFileTransferTracking(request);

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.OK, response.Result.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data!.Count);
        Assert.NotNull(response.PagingResults);
        Assert.Equal(2, response.PagingResults!.RowCount);
        Assert.Equal(123, response.PagingResults.TotalRowsAvailable);
        Assert.Equal(1.ToString(), response.Data[0].FileId);
        Assert.Equal(string.Concat('f', '1'), response.Data[0].FileName);
    }

    private sealed class FakeQueryService : Service.IFileTransferSvcQueryService
    {
        public Func<Service.FtpServerQuery, Service.FtpServerQuery>? OnGetFtpServers { get; set; }

        public Func<Service.FileTransferTrackingQuery, Service.FileTransferTrackingQuery>? OnSearchFileTransferMovement { get; set; }

        public Service.FtpServerQuery? LastFtpServerQuery { get; set; }

        public Service.FileTransferTrackingQuery? LastTrackingQuery { get; set; }

        public Service.FtpServerQuery GetFtpServers(Service.FtpServerQuery query)
        {
            return OnGetFtpServers?.Invoke(query) ?? new Service.FtpServerQuery { Results = new List<Service.FtpServerModel>(), RowCount = 0 };
        }

        public Service.FileTransferTrackingQuery SearchFileTransferMovement(Service.FileTransferTrackingQuery query)
        {
            LastTrackingQuery = query;
            return OnSearchFileTransferMovement?.Invoke(query) ?? new Service.FileTransferTrackingQuery { Results = new List<Service.FileTransferTrackingModel>(), RowCount = 0 };
        }

        public Service.GetSourceLocationQuery GetSourceLocations(Service.GetSourceLocationQuery query)
        {
            return new Service.GetSourceLocationQuery { Results = new List<Service.SourceLocationModel>() };
        }

        public Service.SourceLocationQuery SearchSourceLocations(Service.SourceLocationQuery query)
        {
            return new Service.SourceLocationQuery { Results = new List<Service.SourceLocationModel>() };
        }

        public Service.InboundTargetQuery SearchInboundTarget(Service.InboundTargetQuery query)
        {
            return new Service.InboundTargetQuery { Results = new List<Service.FtsInboundTargetModel>() };
        }
    }
}

