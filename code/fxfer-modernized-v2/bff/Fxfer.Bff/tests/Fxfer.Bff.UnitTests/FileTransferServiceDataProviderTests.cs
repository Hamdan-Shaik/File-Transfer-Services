using Fxfer.Bff.Downstream.FxferService;
using Fxfer.Bff.Downstream.FxferService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.ServiceModel;
using Xunit;
using QueryService = FileTransferSvcQueryService;

namespace Fxfer.Bff.UnitTests;

public sealed class FileTransferServiceDataProviderTests
{
    private const string FileToEvent = "FileToEvent";

    private const string DuplicatedFtpServerNameKey = "DuplicatedFtpServerName";
    private const string InvalidIpAddressFormatKey = "InvalidIpAddressFormat";
    private const string InvalidFtpProxyPortKey = "InvalidFtpProxyPort";

    private const string DuplicatedFtpServerNameMessageId = "FileTransfer.Msg.DuplicatedFTPServerName";
    private const string InvalidIpServerFormatMessageId = "FileTransfer.Msg.InvalidIpServerFormat";
    private const string InvalidFtpProxyPortMessageId = "FileTransfer.Msg.InvalidFTPProxyPort";

    [Fact]
    public void F02_023_strips_contact_phone_and_overrides_file_to_event_address()
    {
        var fakeService = new FakeFileTransferService
        {
            OnFileTransferAdd = _ => new FileTransferConfig.FtsSourceLocationAdded()
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService(), externalCommonFolder: "\\\\server\\fte");

        var config = new FileTransferConfigurationModel
        {
            ConfigName = "c",
            Description = "d",
            FTPServerId = "",
            ProtocolType = new ListItemModel { ID = "p" },
            SourceAddress = "src",
            SourceFileMask = "*.txt",
            FileDirection = new ListItemModel { ID = "IN" },
            RunOnThisDay = new ListItemModel { ID = string.Empty },
            SendNotification = true,
            OwnerName = "o",
            OwnerSubsystemId = "m",
            OwnerPhoneNumber = "1",
            OwnerEmail = "e",
            TimeWindowStart = new ListItemModel { ID = "00:00" },
            TimeWindowEnd = new ListItemModel { ID = "23:59" },
        };

        config.DestinationDetails.Add(new DestinationConfigurationModel
        {
            CompressAtDestination = true,
            ContactPhone = "555-123",
            DestinationType = new ListItemModel { ID = FileToEvent },
            DestinationAddress = "will-be-overwritten",
            DestinationProtocol = new ListItemModel { ID = "FTP" },
            ContractName = new ListItemModel { ID = "CN" },
            FTPServer = new ListItemModel { ID = string.Empty },
            Active = true,
            RenamePattern = new ListItemModel { ID = "RP" },
        });

        _ = provider.AddFileTransferBaseConfiguration(new RequestModel<FileTransferConfigurationModel> { Data = config });

        var cmd = fakeService.LastAddCommand;
        Assert.NotNull(cmd);
        Assert.Single(cmd!.DestinationAddress);
        Assert.Equal("555123", cmd.DestinationAddress[0].ContactPhone);
        Assert.Equal("\\\\server\\fte", cmd.DestinationAddress[0].DestinationAddress);
    }

    [Fact]
    public void F02_024_maps_empty_ftp_server_to_guid_empty_and_uses_active_as_isvalid()
    {
        var fakeService = new FakeFileTransferService
        {
            OnFileTransferAdd = _ => new FileTransferConfig.FtsSourceLocationAdded()
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var config = new FileTransferConfigurationModel
        {
            ConfigName = "c",
            ProtocolType = new ListItemModel { ID = "p" },
            SourceAddress = "src",
            SourceFileMask = "*.txt",
            FileDirection = new ListItemModel { ID = "IN" },
            RunOnThisDay = new ListItemModel { ID = string.Empty },
            TimeWindowStart = new ListItemModel { ID = "00:00" },
            TimeWindowEnd = new ListItemModel { ID = "23:59" },
        };

        config.DestinationDetails.Add(new DestinationConfigurationModel
        {
            DestinationType = new ListItemModel { ID = "Other" },
            DestinationAddress = "a",
            DestinationProtocol = new ListItemModel { ID = "FTP" },
            ContractName = new ListItemModel { ID = "CN" },
            FTPServer = new ListItemModel { ID = string.Empty },
            Active = false,
            RenamePattern = new ListItemModel { ID = "RP" },
        });

        _ = provider.AddFileTransferBaseConfiguration(new RequestModel<FileTransferConfigurationModel> { Data = config });

        var cmd = fakeService.LastAddCommand;
        Assert.NotNull(cmd);
        Assert.Equal(Guid.Empty, cmd!.DestinationAddress[0].FtpServerId);
        Assert.False(cmd.DestinationAddress[0].IsValid);
    }

    [Fact]
    public void F02_025_defaults_run_on_this_day_to_0_and_only_sets_suspended_changed_when_suspended()
    {
        var fakeService = new FakeFileTransferService
        {
            OnFileTransferAdd = _ => new FileTransferConfig.FtsSourceLocationAdded()
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var baseConfig = new FileTransferConfigurationModel
        {
            ConfigName = "c",
            ProtocolType = new ListItemModel { ID = "p" },
            SourceAddress = "src",
            SourceFileMask = "*.txt",
            FileDirection = new ListItemModel { ID = "IN" },
            RunOnThisDay = new ListItemModel { ID = string.Empty },
            TimeWindowStart = new ListItemModel { ID = "00:00" },
            TimeWindowEnd = new ListItemModel { ID = "23:59" },
        };

        baseConfig.DestinationDetails.Add(new DestinationConfigurationModel
        {
            DestinationType = new ListItemModel { ID = "Other" },
            DestinationAddress = "a",
            DestinationProtocol = new ListItemModel { ID = "FTP" },
            ContractName = new ListItemModel { ID = "CN" },
            FTPServer = new ListItemModel { ID = string.Empty },
            Active = true,
            RenamePattern = new ListItemModel { ID = "RP" },
        });

        baseConfig.Suspended = false;
        _ = provider.AddFileTransferBaseConfiguration(new RequestModel<FileTransferConfigurationModel> { Data = baseConfig });
        var cmd = fakeService.LastAddCommand;
        Assert.NotNull(cmd);
        Assert.Equal((byte)0, cmd!.RunOnThisDayOfMonth);
        Assert.Null(cmd.IsSuspendedChangedDate);

        baseConfig.Suspended = true;
        _ = provider.AddFileTransferBaseConfiguration(new RequestModel<FileTransferConfigurationModel> { Data = baseConfig });
        cmd = fakeService.LastAddCommand;
        Assert.NotNull(cmd);
        Assert.NotNull(cmd!.IsSuspendedChangedDate);
    }

    [Fact]
    public void F02_026_fault_exception_converts_to_error_response()
    {
        var fakeService = new FakeFileTransferService
        {
            OnFileTransferAdd = _ => throw new FaultException<FileTransferConfig.ServiceException>(
                new FileTransferConfig.ServiceException { ErrorCode = "E" },
                "fault")
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var config = new FileTransferConfigurationModel
        {
            ConfigName = "c",
            ProtocolType = new ListItemModel { ID = "p" },
            SourceAddress = "src",
            SourceFileMask = "*.txt",
            FileDirection = new ListItemModel { ID = "IN" },
            RunOnThisDay = new ListItemModel { ID = string.Empty },
            TimeWindowStart = new ListItemModel { ID = "00:00" },
            TimeWindowEnd = new ListItemModel { ID = "23:59" },
        };

        config.DestinationDetails.Add(new DestinationConfigurationModel
        {
            DestinationType = new ListItemModel { ID = "Other" },
            DestinationAddress = "a",
            DestinationProtocol = new ListItemModel { ID = "FTP" },
            ContractName = new ListItemModel { ID = "CN" },
            FTPServer = new ListItemModel { ID = string.Empty },
            Active = true,
            RenamePattern = new ListItemModel { ID = "RP" },
        });

        var resp = provider.AddFileTransferBaseConfiguration(new RequestModel<FileTransferConfigurationModel> { Data = config });

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.Error, resp.Result.StatusCode);
        Assert.Contains(resp.Result.Messages, m => m.Message == "E");
    }

    [Fact]
    public void F01_052_add_ftp_server_generates_new_id_and_forwards_fields()
    {
        var fakeService = new FakeFileTransferService
        {
            OnAddFtpServer = cmd => new FileTransferConfig.FtpServerAdded { FtpServerId = cmd.FtpServerId }
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var request = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel
            {
                FtpServerName = "name",
                FtpDescription = "desc",
                FtpUser = "user",
                FtpPassword = "pass",
                FtpServerIp = "1.2.3.4",
                FtpPort = 21,
                FtpProxyAddress = "proxy",
                FtpProxyPort = 8080,
                FtpTransferMode = "mode",
            }
        };

        _ = provider.AddFtpServer(request);

        var cmd = fakeService.LastAddFtpServerCommand;
        Assert.NotNull(cmd);
        Assert.NotEqual(Guid.Empty, cmd!.FtpServerId);
        Assert.Equal("name", cmd.FtpServerName);
        Assert.Equal("desc", cmd.FtpDescription);
        Assert.Equal("user", cmd.FtpUser);
        Assert.Equal("pass", cmd.FtpPassword);
        Assert.Equal("1.2.3.4", cmd.FtpServerIp);
        Assert.Equal((short)21, cmd.FtpPort);
        Assert.Equal("proxy", cmd.FtpProxyAddress);
        Assert.Equal((short)8080, cmd.FtpProxyPort);
        Assert.Equal("mode", cmd.FtpTransferMode);
        Assert.Equal("operator", cmd.OperatorId);
    }

    [Fact]
    public void F01_053_add_ftp_server_success_returns_id_ok_and_copies_editor()
    {
        var returnedId = Guid.NewGuid();
        var fakeService = new FakeFileTransferService
        {
            OnAddFtpServer = _ => new FileTransferConfig.FtpServerAdded { FtpServerId = returnedId }
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());
        var editor = new object();

        var request = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel
            {
                FtpServerName = "name",
                FtpPassword = "pass",
                Editor = editor
            }
        };

        var response = provider.AddFtpServer(request);

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.OK, response.Result.StatusCode);
        Assert.Equal(returnedId.ToString(), response.Data!.FtpServerId);
        Assert.Same(editor, response.Data.Editor);
    }

    [Fact]
    public void F01_054_build_update_ftp_server_command_prefers_request_password_and_falls_back_to_query_password()
    {
        var id = Guid.NewGuid();
        const string fromQuery = "fromQuery";
        const string provided = "provided";

        var queryService = new FakeQueryService
        {
            OnGetFtpServers = _ => new QueryService.FtpServerQuery
            {
                Results = new List<QueryService.FtpServerModel>
                {
                    new QueryService.FtpServerModel { FtpPassword = fromQuery }
                }
            }
        };

        var provider = CreateProvider(new FakeFileTransferService(), queryService);

        var method = typeof(FileTransferServiceDataProvider).GetMethod(
            "BuildUpdateFtpServerCommand",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(method);

        var requestUsesQuery = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel { FtpServerId = id.ToString(), FtpPassword = string.Empty }
        };

        _ = method!.Invoke(provider, new object[] { requestUsesQuery });
        var updateField = typeof(FileTransferServiceDataProvider).GetField("updateFtpServerCommand", BindingFlags.Instance | BindingFlags.NonPublic);
        var updateCmd = Assert.IsType<FileTransferConfig.UpdateFtpServerCommand>(updateField!.GetValue(provider));
        Assert.Equal(fromQuery, updateCmd.FtpPassword);

        var requestUsesProvided = new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel { FtpServerId = id.ToString(), FtpPassword = provided }
        };

        _ = method.Invoke(provider, new object[] { requestUsesProvided });
        updateCmd = Assert.IsType<FileTransferConfig.UpdateFtpServerCommand>(updateField.GetValue(provider));
        Assert.Equal(provided, updateCmd.FtpPassword);
    }

    [Fact]
    public void F01_055_invalidate_ftp_server_success_returns_id_isvalid_and_copies_editor()
    {
        var id = Guid.NewGuid();
        var editor = new object();

        var fakeService = new FakeFileTransferService
        {
            OnInvalidateFtpServer = _ => new FileTransferConfig.FtpServerInvalidated { FtpServerId = id, IsValid = false }
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var response = provider.InvalidateFtpServer(new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel { FtpServerId = id.ToString(), Editor = editor }
        });

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.OK, response.Result.StatusCode);
        Assert.Equal(id.ToString(), response.Data!.FtpServerId);
        Assert.False(response.Data.IsValid);
        Assert.Same(editor, response.Data.Editor);
    }

    [Fact]
    public void F01_055_update_ftp_server_success_returns_id_isvalid_and_copies_editor()
    {
        var id = Guid.NewGuid();
        var editor = new object();

        var fakeService = new FakeFileTransferService
        {
            OnUpdateFtpServer = _ => new FileTransferConfig.FtpServerUpdated { FtpServerId = id, IsValid = true }
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var response = provider.UpdateFtpServer(new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = new string(new[] { 'o', 'p', 'e', 'r', 'a', 't', 'o', 'r' }) },
            Data = new FtpServerModel { FtpServerId = id.ToString(), FtpPassword = new string(new[] { 'p', 'a', 's', 's' }), Editor = editor }
        });

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.OK, response.Result.StatusCode);
        Assert.Equal(id.ToString(), response.Data!.FtpServerId);
        Assert.True(response.Data.IsValid);
        Assert.Same(editor, response.Data.Editor);
    }

    [Fact]
    public void F01_056_maps_known_business_validation_keys_to_localization_message_ids()
    {
        var fakeService = new FakeFileTransferService
        {
            OnAddFtpServer = _ => throw new FaultException<FileTransferConfig.BusinessValidationException>(
                new FileTransferConfig.BusinessValidationException
                {
                    BusinessMessages =
                    [
                        new FileTransferConfig.BusinessExceptionMessage { MessageKey = DuplicatedFtpServerNameKey },
                        new FileTransferConfig.BusinessExceptionMessage { MessageKey = InvalidIpAddressFormatKey },
                        new FileTransferConfig.BusinessExceptionMessage { MessageKey = InvalidFtpProxyPortKey },
                    ]
                },
                "validation")
        };

        var provider = CreateProvider(fakeService, queryService: new FakeQueryService());

        var response = provider.AddFtpServer(new RequestModel<FtpServerModel>
        {
            Requestor = new RequestorModel { IdentifierId = "operator" },
            Data = new FtpServerModel { FtpServerName = "name", FtpPassword = "pass" }
        });

        Assert.Equal(CoreEnumerations.Messaging.StatusCodeType.Error, response.Result.StatusCode);
        Assert.Contains(response.Result.Messages, m => m.Message == DuplicatedFtpServerNameMessageId);
        Assert.Contains(response.Result.Messages, m => m.Message == InvalidIpServerFormatMessageId);
        Assert.Contains(response.Result.Messages, m => m.Message == InvalidFtpProxyPortMessageId);
    }

    private static FileTransferServiceDataProvider CreateProvider(
        FakeFileTransferService fakeService,
        FakeQueryService queryService,
        string? externalCommonFolder = null)
    {
        var configDict = new Dictionary<string, string?>();
        if (externalCommonFolder != null)
        {
            configDict["FileTransfer:ExternalFTECommonFolder"] = externalCommonFolder;
        }

        var config = new ConfigurationBuilder().AddInMemoryCollection(configDict).Build();

        var http = new DefaultHttpContext();
        http.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim("sub", "operator"),
        }, authenticationType: "test"));

        var accessor = new HttpContextAccessor { HttpContext = http };

        var queryProvider = new FileTransferQueryServiceDataProvider(
            queryService,
            NullLogger<FileTransferQueryServiceDataProvider>.Instance,
            accessor);

        return new FileTransferServiceDataProvider(
            fakeService,
            queryProvider,
            NullLogger<FileTransferServiceDataProvider>.Instance,
            accessor,
            config);
    }

    private sealed class FakeFileTransferService : FileTransferConfig.IFileTransferSvcService
    {
        public Func<FileTransferConfig.FtsSourceLocationAddCommand, FileTransferConfig.FtsSourceLocationAdded>? OnFileTransferAdd { get; set; }
        public Func<FileTransferConfig.FtsSourceLocationUpdateCommand, FileTransferConfig.FtsSourceLocationUpdated>? OnUpdateFileTransferSourceLocation { get; set; }
        public Func<FileTransferConfig.FtsSourceLocationInvalidateCommand, FileTransferConfig.FtsSourceLocationInvalidated>? OnInvalidateFileTransferSourceLocation { get; set; }
        public Func<FileTransferConfig.AddFtpServerCommand, FileTransferConfig.FtpServerAdded>? OnAddFtpServer { get; set; }
        public Func<FileTransferConfig.UpdateFtpServerCommand, FileTransferConfig.FtpServerUpdated>? OnUpdateFtpServer { get; set; }
        public Func<FileTransferConfig.InvalidateFtpServerCommand, FileTransferConfig.FtpServerInvalidated>? OnInvalidateFtpServer { get; set; }

        public FileTransferConfig.FtsSourceLocationAddCommand? LastAddCommand { get; private set; }
        public FileTransferConfig.AddFtpServerCommand? LastAddFtpServerCommand { get; private set; }
        public FileTransferConfig.FtsSourceLocationUpdateCommand? LastUpdateCommand { get; private set; }

        public FileTransferConfig.FtpServerAdded AddFtpServer(FileTransferConfig.AddFtpServerCommand command)
        {
            LastAddFtpServerCommand = command;
            return OnAddFtpServer?.Invoke(command) ?? new FileTransferConfig.FtpServerAdded();
        }

        public FileTransferConfig.FtsSourceLocationAdded FileTransferAdd(FileTransferConfig.FtsSourceLocationAddCommand command)
        {
            LastAddCommand = command;
            return OnFileTransferAdd?.Invoke(command) ?? new FileTransferConfig.FtsSourceLocationAdded();
        }

        public FileTransferConfig.FtsSourceLocationUpdated UpdateFileTransferSourceLocation(FileTransferConfig.FtsSourceLocationUpdateCommand command)
        {
            LastUpdateCommand = command;
            return OnUpdateFileTransferSourceLocation?.Invoke(command) ?? new FileTransferConfig.FtsSourceLocationUpdated();
        }

        public FileTransferConfig.FtpServerInvalidated InvalidateFtpServer(FileTransferConfig.InvalidateFtpServerCommand command)
        {
            return OnInvalidateFtpServer?.Invoke(command) ?? new FileTransferConfig.FtpServerInvalidated();
        }

        public FileTransferConfig.FtsSourceLocationInvalidated InvalidateFileTransferSourceLocation(FileTransferConfig.FtsSourceLocationInvalidateCommand command)
        {
            return OnInvalidateFileTransferSourceLocation?.Invoke(command) ?? new FileTransferConfig.FtsSourceLocationInvalidated();
        }

        public FileTransferConfig.FtpServerUpdated UpdateFtpServer(FileTransferConfig.UpdateFtpServerCommand command)
        {
            return OnUpdateFtpServer?.Invoke(command) ?? new FileTransferConfig.FtpServerUpdated();
        }
    }

    private sealed class FakeQueryService : QueryService.IFileTransferSvcQueryService
    {
        public Func<QueryService.FtpServerQuery, QueryService.FtpServerQuery>? OnGetFtpServers { get; set; }

        public QueryService.FtpServerQuery GetFtpServers(QueryService.FtpServerQuery query)
        {
            return OnGetFtpServers?.Invoke(query) ?? new QueryService.FtpServerQuery { Results = new List<QueryService.FtpServerModel>() };
        }

        public QueryService.GetSourceLocationQuery GetSourceLocations(QueryService.GetSourceLocationQuery query)
        {
            return new QueryService.GetSourceLocationQuery { Results = new List<QueryService.SourceLocationModel>() };
        }

        public QueryService.SourceLocationQuery SearchSourceLocations(QueryService.SourceLocationQuery query)
        {
            return new QueryService.SourceLocationQuery { Results = new List<QueryService.SourceLocationModel>() };
        }

        public QueryService.FileTransferTrackingQuery SearchFileTransferMovement(QueryService.FileTransferTrackingQuery query)
        {
            return new QueryService.FileTransferTrackingQuery { Results = new List<QueryService.FileTransferTrackingModel>(), RowCount = 0 };
        }

        public QueryService.InboundTargetQuery SearchInboundTarget(QueryService.InboundTargetQuery query)
        {
            return new QueryService.InboundTargetQuery { Results = new List<QueryService.FtsInboundTargetModel>() };
        }
    }
}
