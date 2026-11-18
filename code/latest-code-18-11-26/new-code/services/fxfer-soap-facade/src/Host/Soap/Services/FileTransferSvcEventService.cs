using CoreWCF;
using Fxfer.SoapFacade.CoreWcf.Contracts.Commands;
using Fxfer.SoapFacade.CoreWcf.Contracts.Domain;
using Fxfer.SoapFacade.CoreWcf.Contracts.Events;
using Fxfer.SoapFacade.CoreWcf.Contracts.Exceptions;
using Fxfer.SoapFacade.CoreWcf.Contracts.Services;
using Fxfer.SoapFacade.CoreWcf.Contracts.Utilities;
using Fxfer.SoapFacade.Host.Clients;
using Microsoft.Extensions.Logging;

namespace Fxfer.SoapFacade.Host.Soap.Services;

public sealed class FileTransferSvcEventService : IEventDistribution
{
    private const string DomainApiPrefix = "/api/fxfer-domain/v1";

    private readonly FxferDomainApiClient _domainApiClient;
    private readonly ILogger<FileTransferSvcEventService> _logger;

    public FileTransferSvcEventService(FxferDomainApiClient domainApiClient, ILogger<FileTransferSvcEventService> logger)
    {
        _domainApiClient = domainApiClient;
        _logger = logger;
    }

    [OperationContract]
    [FaultContract(typeof(ServiceException))]
    public Event TryRouteCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        return TryRoute(command);
    }

    private Event TryRoute(Command command)
    {
        // shortcut: legacy CQRS routing base (TryRoute) is not present in the target; route to fxfer-domain-api.
        // Keep this switch small and explicit: only supported commands are routed.
        try
        {
            return RouteToDomainApi(command);
        }
        catch (FaultException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while routing CQRS Command to fxfer-domain-api");
            throw CreateServiceFault("Unhandled error while routing command");
        }
    }

    private Event RouteToDomainApi(Command command)
    {
        return command switch
        {
            UpdateFtpServerCommand ftpUpdate => RouteUpdateFtpServer(ftpUpdate),
            InvalidateFtpServerCommand ftpInvalidate => RouteInvalidateFtpServer(ftpInvalidate),
            AddFtpServerCommand ftpAdd => RouteAddFtpServer(ftpAdd),
            FtsSourceLocationUpdateCommand locationUpdate => RouteUpdateTransferConfiguration(locationUpdate),
            FtsSourceLocationAddCommand locationAdd => RouteAddTransferConfiguration(locationAdd),
            FtsSourceLocationInvalidateCommand locationInvalidate => RouteInvalidateTransferConfiguration(locationInvalidate),
            UpdateIntegrationFileCommand integrationUpdate => RouteUpdateIntegrationFile(integrationUpdate),
            _ => throw CreateServiceFault($"Unsupported Command type: {command.GetType().FullName}")
        };
    }

    private FtpServerAdded RouteAddFtpServer(AddFtpServerCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var request = new FtpServerUpsertRequest
        {
            FtpServerName = command.FtpServerName,
            FtpDescription = command.FtpDescription,
            FtpServerIp = command.FtpServerIp,
            FtpTransferMode = command.FtpTransferMode,
            FtpUser = command.FtpUser,
            CredentialSecretRef = command.FtpPassword,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSshAuth = command.UseSSHAuth,
            IsValid = command.IsValid,
            OperatorId = command.OperatorId
        };

        var response = CallDomainApi(() => _domainApiClient.PostAsync<FtpServerUpsertRequest, FtpServerResponse>(
            $"{DomainApiPrefix}/ftp-servers",
            request,
            CancellationToken.None));

        return new FtpServerAdded
        {
            FtpServerId = response.FtpServerId,
            FtpServerName = response.FtpServerName,
            FtpDescription = response.FtpDescription,
            FtpServerIp = response.FtpServerIp,
            FtpTransferMode = response.FtpTransferMode,
            FtpUser = response.FtpUser,
            FtpPassword = response.CredentialSecretRef,
            FtpPort = (short)response.FtpPort,
            FtpProxyAddress = response.FtpProxyAddress,
            FtpProxyPort = response.FtpProxyPort is null ? null : (short?)response.FtpProxyPort,
            UseSSHAuth = response.UseSshAuth,
            IsValid = response.IsValid,
            OperatorId = response.OperatorId,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private FtpServerUpdated RouteUpdateFtpServer(UpdateFtpServerCommand command)
    {
        if (command.FtpServerId == Guid.Empty)
        {
            throw CreateServiceFault("Missing Parameters: FtpServerId");
        }

        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var request = new FtpServerUpsertRequest
        {
            FtpServerName = command.FtpServerName,
            FtpDescription = command.FtpDescription,
            FtpServerIp = command.FtpServerIp,
            FtpTransferMode = command.FtpTransferMode,
            FtpUser = command.FtpUser,
            CredentialSecretRef = command.FtpPassword,
            FtpPort = command.FtpPort,
            FtpProxyAddress = command.FtpProxyAddress,
            FtpProxyPort = command.FtpProxyPort,
            UseSshAuth = command.UseSSHAuth,
            IsValid = command.IsValid,
            OperatorId = command.OperatorId
        };

        var response = CallDomainApi(() => _domainApiClient.PutAsync<FtpServerUpsertRequest, FtpServerResponse>(
            $"{DomainApiPrefix}/ftp-servers/{command.FtpServerId}",
            request,
            CancellationToken.None));

        return new FtpServerUpdated
        {
            FtpServerId = response.FtpServerId,
            FtpServerName = response.FtpServerName,
            FtpDescription = response.FtpDescription,
            FtpServerIp = response.FtpServerIp,
            FtpTransferMode = response.FtpTransferMode,
            FtpUser = response.FtpUser,
            FtpPassword = response.CredentialSecretRef,
            FtpPort = (short)response.FtpPort,
            FtpProxyAddress = response.FtpProxyAddress,
            FtpProxyPort = response.FtpProxyPort is null ? null : (short?)response.FtpProxyPort,
            UseSSHAuth = response.UseSshAuth,
            IsValid = response.IsValid,
            OperatorId = response.OperatorId,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private FtpServerInvalidated RouteInvalidateFtpServer(InvalidateFtpServerCommand command)
    {
        if (command.FtpServerId == Guid.Empty)
        {
            throw CreateServiceFault("Missing Parameters: FtpServerId");
        }

        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var request = new FtpServerPatchRequest
        {
            IsValid = false,
            OperatorId = command.OperatorId
        };

        var response = CallDomainApi(() => _domainApiClient.PatchAsync<FtpServerPatchRequest, FtpServerResponse>(
            $"{DomainApiPrefix}/ftp-servers/{command.FtpServerId}",
            request,
            CancellationToken.None));

        return new FtpServerInvalidated
        {
            FtpServerId = response.FtpServerId,
            FtpServerName = response.FtpServerName,
            FtpDescription = response.FtpDescription,
            FtpServerIp = response.FtpServerIp,
            FtpTransferMode = response.FtpTransferMode,
            FtpUser = response.FtpUser,
            FtpPassword = response.CredentialSecretRef,
            FtpPort = (short)response.FtpPort,
            FtpProxyAddress = response.FtpProxyAddress,
            FtpProxyPort = response.FtpProxyPort is null ? null : (short?)response.FtpProxyPort,
            UseSSHAuth = response.UseSshAuth,
            IsValid = response.IsValid,
            OperatorId = response.OperatorId,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private FtsSourceLocationAdded RouteAddTransferConfiguration(FtsSourceLocationAddCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var request = new TransferConfigurationUpsertRequest
        {
            ConfigId = command.ConfigId,
            ConfigName = command.ConfigName ?? string.Empty,
            Description = command.Description,
            FtpServerId = TryParseGuid(command.FtpServerId),
            Address = command.Address ?? string.Empty,
            Mask = command.Mask,
            Protocol = command.Protocol ?? string.Empty,
            FileDirection = command.FileDirection,
            OwnerName = command.OwnerName ?? string.Empty,
            OwnerEmail = command.OwnerEmail ?? string.Empty,
            OwnerSubsystem = TryParseGuidOrEmpty(command.OwnerModule),
            OwnerPhoneNumber = command.OwnerPhone ?? string.Empty,
            TradingPartner = command.TradingPartner,
            Destinations = command.DestinationAddress.Select(MapDestination).ToArray(),
            OperatorId = command.OperatorId,
            RunOnMonday = command.RunOnMonday,
            RunOnTuesday = command.RunOnTuesday,
            RunOnWednesday = command.RunOnWed,
            RunOnThursday = command.RunOnThrus,
            RunOnFriday = command.RunOnFri,
            RunOnSaturday = command.RunOnSat,
            RunOnSunday = command.RunOnSun,
            RunOnFirstDay = command.RunOnFrstDayOfMonthInd,
            RunOnLastDay = command.RunOnLastDayOfMonthInd,
            RunOnThisDay = command.RunOnThisDayOfMonth,
            StartWindow = command.StartTime ?? string.Empty,
            EndWindow = command.EndTime ?? string.Empty,
            SendNotification = command.SendNotifications,
            IsActive = command.IsValid,
            Suspended = command.IsSuspended,
            SuspendedChangeDate = ToNullableUtcOffset(command.IsSuspendedChangedDate)
        };

        _ = CallDomainApi(() => _domainApiClient.PostAsync<TransferConfigurationUpsertRequest, TransferConfigurationResponse>(
            $"{DomainApiPrefix}/transfer-configurations",
            request,
            CancellationToken.None));

        return new FtsSourceLocationAdded
        {
            Payload = command,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private FtsSourceLocationUpdated RouteUpdateTransferConfiguration(FtsSourceLocationUpdateCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var sourceLocationId = RequireGuid(command.FtsSourceLocationId, "Missing Parameters: FtsSourceLocationId");

        var request = new TransferConfigurationUpsertRequest
        {
            ConfigId = command.ConfigId,
            ConfigName = command.ConfigName ?? string.Empty,
            Description = command.Description,
            FtpServerId = TryParseGuid(command.FtpServerId),
            Address = command.Address ?? string.Empty,
            Mask = command.Mask,
            Protocol = command.Protocol ?? string.Empty,
            FileDirection = command.FileDirection,
            OwnerName = command.OwnerName ?? string.Empty,
            OwnerEmail = command.OwnerEmail ?? string.Empty,
            OwnerSubsystem = TryParseGuidOrEmpty(command.OwnerModule),
            OwnerPhoneNumber = command.OwnerPhone ?? string.Empty,
            TradingPartner = command.TradingPartner,
            Destinations = command.DestinationAddress.Select(MapDestination).ToArray(),
            OperatorId = command.OperatorId,
            RunOnMonday = command.RunOnMonday,
            RunOnTuesday = command.RunOnTuesday,
            RunOnWednesday = command.RunOnWed,
            RunOnThursday = command.RunOnThrus,
            RunOnFriday = command.RunOnFri,
            RunOnSaturday = command.RunOnSat,
            RunOnSunday = command.RunOnSun,
            RunOnFirstDay = command.RunOnFrstDayOfMonthInd,
            RunOnLastDay = command.RunOnLastDayOfMonthInd,
            RunOnThisDay = command.RunOnThisDayOfMonth,
            StartWindow = command.StartTime ?? string.Empty,
            EndWindow = command.EndTime ?? string.Empty,
            SendNotification = command.SendNotifications,
            IsActive = command.IsValid,
            Suspended = command.IsSuspended,
            SuspendedChangeDate = ToNullableUtcOffset(command.IsSuspendedChangedDate)
        };

        _ = CallDomainApi(() => _domainApiClient.PutAsync<TransferConfigurationUpsertRequest, TransferConfigurationResponse>(
            $"{DomainApiPrefix}/transfer-configurations/{sourceLocationId}",
            request,
            CancellationToken.None));

        return new FtsSourceLocationUpdated
        {
            Payload = command,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private FtsSourceLocationInvalidated RouteInvalidateTransferConfiguration(FtsSourceLocationInvalidateCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.OperatorId))
        {
            throw CreateServiceFault("Missing Parameters: OperatorId");
        }

        var sourceLocationId = RequireGuid(command.FtsSourceLocationId, "Missing Parameters: FtsSourceLocationId");

        var request = new TransferConfigurationPatchRequest
        {
            ConfigId = command.ConfigId,
            IsActive = false,
            OperatorId = command.OperatorId
        };

        _ = CallDomainApi(() => _domainApiClient.PatchAsync<TransferConfigurationPatchRequest, TransferConfigurationResponse>(
            $"{DomainApiPrefix}/transfer-configurations/{sourceLocationId}",
            request,
            CancellationToken.None));

        return new FtsSourceLocationInvalidated
        {
            ConfigId = command.ConfigId,
            DestinationAddress = command.DestinationAddress,
            OperatorId = command.OperatorId,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private IntegrationFileUpdated RouteUpdateIntegrationFile(UpdateIntegrationFileCommand command)
    {
        // shortcut: SOAP UpdateIntegrationFileCommand is missing legacy fields (FileName/ETFOutboundMetadataDefnId).
        var request = new IntegrationFileProcessingRequest
        {
            ConfigId = command.ConfigId,
            IsValid = true
        };

        var response = CallDomainApi(() => _domainApiClient.PatchAsync<IntegrationFileProcessingRequest, IntegrationFileResponse>(
            $"{DomainApiPrefix}/integration-files/{command.FtsIntegrationFileId}",
            request,
            CancellationToken.None));

        return new IntegrationFileUpdated
        {
            ConfigId = response.ConfigId,
            EtfOutboundMetadataId = response.ETFOutboundMetadataDefnId,
            FileName = response.FileName,
            FtsIntegrationFileId = response.FtsIntegrationFileId,
            GroupInfo = [new GroupInfo { GroupId = response.GroupId, NumberOfRecords = 0 }],
            IsProcessed = response.IsProcessed,
            IsValid = response.IsValid,
            LastModifiedDate = response.LastModifiedDate.UtcDateTime,
            CorrelationId = command.Requestor?.CorrelationId,
            ApplicationName = command.Requestor?.ApplicationName,
            CycleDate = command.CycleDate
        };
    }

    private static DestinationUpsert MapDestination(FtsDestinationLocationAdded destination)
    {
        return new DestinationUpsert
        {
            Address = destination.DestinationAddress ?? string.Empty,
            Mask = null,
            Protocol = destination.DestinationProtocol ?? string.Empty,
            TradingPartner = null,
            DestinationType = destination.DestinationType,
            IsValid = destination.IsValid,
            OperatorId = destination.OperatorId ?? string.Empty
        };
    }

    private static Guid? TryParseGuid(string? value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return guid;
        }

        return null;
    }

    private static Guid TryParseGuidOrEmpty(string? value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return guid;
        }

        return Guid.Empty;
    }

    private static DateTimeOffset? ToNullableUtcOffset(DateTime? value)
    {
        if (value is null)
        {
            return null;
        }

        var utc = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
        return new DateTimeOffset(utc);
    }

    private static Guid RequireGuid(string? value, string faultMessage)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return guid;
        }

        throw CreateServiceFault(faultMessage);
    }

    private static T CallDomainApi<T>(Func<Task<T?>> call)
        where T : class
    {
        try
        {
            var response = call().GetAwaiter().GetResult();
            if (response is null)
            {
                throw CreateServiceFault("Domain API returned an empty response");
            }

            return response;
        }
        catch (HttpRequestException)
        {
            throw CreateServiceFault("Domain API call failed");
        }
        catch (TaskCanceledException)
        {
            throw CreateServiceFault("Domain API call timed out");
        }
    }

    private static FaultException<ServiceException> CreateServiceFault(string message)
    {
        return new FaultException<ServiceException>(
            new ServiceException
            {
                ErrorCode = StatusCodeType.Error,
                ErrorMessages = [message],
                Reason = message
            },
            new FaultReason(message));
    }

    private sealed class FtpServerUpsertRequest
    {
        public string? FtpServerName { get; init; }
        public string? FtpServerIp { get; init; }
        public string? FtpTransferMode { get; init; }
        public string? FtpUser { get; init; }
        public string? CredentialSecretRef { get; init; }
        public string? FtpDescription { get; init; }
        public short FtpPort { get; init; }
        public string? FtpProxyAddress { get; init; }
        public short? FtpProxyPort { get; init; }
        public bool UseSshAuth { get; init; }
        public bool IsValid { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed class FtpServerPatchRequest
    {
        public bool? IsValid { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed class FtpServerResponse
    {
        public Guid FtpServerId { get; init; }
        public string FtpServerName { get; init; } = string.Empty;
        public string FtpServerIp { get; init; } = string.Empty;
        public string FtpTransferMode { get; init; } = string.Empty;
        public string FtpUser { get; init; } = string.Empty;
        public string? CredentialSecretRef { get; init; }
        public string FtpDescription { get; init; } = string.Empty;
        public int FtpPort { get; init; }
        public string? FtpProxyAddress { get; init; }
        public int? FtpProxyPort { get; init; }
        public bool UseSshAuth { get; init; }
        public bool IsValid { get; init; }
        public string OperatorId { get; init; } = string.Empty;
    }

    private sealed class DestinationUpsert
    {
        public required string Address { get; init; }
        public string? Mask { get; init; }
        public required string Protocol { get; init; }
        public string? TradingPartner { get; init; }
        public string? DestinationType { get; init; }
        public bool IsValid { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed class TransferConfigurationUpsertRequest
    {
        public int ConfigId { get; init; }
        public required string ConfigName { get; init; }
        public string? Description { get; init; }
        public Guid? FtpServerId { get; init; }
        public required string Address { get; init; }
        public string? Mask { get; init; }
        public required string Protocol { get; init; }
        public string? FileDirection { get; init; }
        public required string OwnerName { get; init; }
        public required string OwnerEmail { get; init; }
        public Guid OwnerSubsystem { get; init; }
        public required string OwnerPhoneNumber { get; init; }
        public string? TradingPartner { get; init; }
        public required IReadOnlyList<DestinationUpsert> Destinations { get; init; }
        public required string OperatorId { get; init; }
        public bool? RunOnMonday { get; init; }
        public bool? RunOnTuesday { get; init; }
        public bool? RunOnWednesday { get; init; }
        public bool? RunOnThursday { get; init; }
        public bool? RunOnFriday { get; init; }
        public bool? RunOnSaturday { get; init; }
        public bool? RunOnSunday { get; init; }
        public bool? RunOnFirstDay { get; init; }
        public bool? RunOnLastDay { get; init; }
        public int? RunOnThisDay { get; init; }
        public required string StartWindow { get; init; }
        public required string EndWindow { get; init; }
        public bool? SendNotification { get; init; }
        public bool? IsActive { get; init; }
        public bool? Suspended { get; init; }
        public DateTimeOffset? SuspendedChangeDate { get; init; }
    }

    private sealed class TransferConfigurationPatchRequest
    {
        public int ConfigId { get; init; }
        public bool? IsActive { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed class TransferConfigurationResponse
    {
        public Guid FtsSourceLocationId { get; init; }
    }

    private sealed class IntegrationFileProcessingRequest
    {
        public int? ConfigId { get; init; }
        public bool IsValid { get; init; }
    }

    private sealed class IntegrationFileResponse
    {
        public Guid FtsIntegrationFileId { get; init; }
        public Guid? GroupId { get; init; }
        public int? ConfigId { get; init; }
        public string FileName { get; init; } = string.Empty;
        public Guid ETFOutboundMetadataDefnId { get; init; }
        public bool IsProcessed { get; init; }
        public bool IsValid { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
    }
}
