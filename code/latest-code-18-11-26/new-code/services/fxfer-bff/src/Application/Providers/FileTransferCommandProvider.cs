using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Runtime.ExceptionServices;
using Fxfer.Bff.Api.Configuration;
using Fxfer.Bff.Contracts.Common;
using Fxfer.Bff.Contracts.FtpServers;
using Fxfer.Bff.Contracts.TransferConfigurations;
using Microsoft.Extensions.Options;

namespace Fxfer.Bff.Application.Providers;

public sealed class FileTransferCommandProvider : IFileTransferCommandProvider
{
    private const string DownstreamBasePath = "/api/fxfer-domain/v1";
    private const string FileToEventDestinationType = "FileToEvent";

    private readonly FxferDomainApiClient _domainApiClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FileTransferManagementOptions _options;

    public FileTransferCommandProvider(
        FxferDomainApiClient domainApiClient,
        IHttpContextAccessor httpContextAccessor,
        IOptions<FileTransferManagementOptions> options)
    {
        _domainApiClient = domainApiClient;
        _httpContextAccessor = httpContextAccessor;
        _options = options.Value;
    }

    public async Task<TransferConfiguration> AddFileTransferBaseConfigurationAsync(TransferConfigurationUpsertRequest request, CancellationToken ct)
    {
        var operatorId = MapToServiceRequestor(request.OperatorId);
        var cmd = BuildTransferConfigurationUpsertCommand(configId: 0, request, operatorId);

        try
        {
            var result = await FileTransferAdd(cmd, ct);
            return MapToInternalProvider(result);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<TransferConfiguration> UpdateFileTransferBaseConfigurationAsync(Guid sourceLocationId, TransferConfigurationUpsertRequest request, CancellationToken ct)
    {
        try
        {
            var existing = await GetTransferConfigurationAsync(sourceLocationId, ct);
            var operatorId = MapToServiceRequestor(request.OperatorId);
            var cmd = BuildTransferConfigurationUpsertCommand(existing.ConfigId, request, operatorId);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"{DownstreamBasePath}/transfer-configurations/{sourceLocationId}")
            {
                Content = JsonContent.Create(cmd)
            };

            using var response = await _domainApiClient.SendAsync(httpRequest, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<DownstreamTransferConfiguration>(cancellationToken: ct);
            if (result is null)
            {
                throw new InvalidOperationException("Downstream transfer configuration response was empty.");
            }

            return MapToTransferConfiguration(result);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task InvalidateFileTransferBaseConfigurationAsync(Guid sourceLocationId, CancellationToken ct)
    {
        var existing = await GetTransferConfigurationAsync(sourceLocationId, ct);

        // F01-033: allow exceptions/HTTP failures to propagate to centralized Problem Details mapping.
        var operatorId = MapToServiceRequestor(existing.OperatorId);

        var cmd = new DownstreamTransferConfigurationPatchRequest
        {
            ConfigId = existing.ConfigId,
            OperatorId = operatorId,
            IsActive = false
        };

        try
        {
            await InvalidateFileTransferSourceLocation(sourceLocationId, cmd, ct);
        }
        catch (Exception ex)
        {
            HandleException(ex);
            throw;
        }
    }

    public async Task<FtpServer> AddFtpServerAsync(FtpServerUpsertRequest request, CancellationToken ct)
    {
        var operatorId = MapToServiceRequestor(request.OperatorId);

        // F02-051: legacy creates a new id client-side and uses requestor id as operator id.
        var cmd = BuildAddFtpServerCommand(request, operatorId);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{DownstreamBasePath}/ftp-servers")
        {
            Content = JsonContent.Create(cmd)
        };

        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        HandleFtpServerBusinessExceptions(response);

        var result = await response.Content.ReadFromJsonAsync<DownstreamFtpServer>(cancellationToken: ct);
        if (result is null)
        {
            throw new InvalidOperationException("Downstream FTP server response was empty.");
        }

        // F02-052: return the created id and preserve other returned values.
        return BuildFtpServerModel(result);
    }

    public async Task<FtpServer> UpdateFtpServerAsync(Guid ftpServerId, FtpServerUpsertRequest request, CancellationToken ct)
    {
        // F02-056/F02-063: if password is omitted, query existing credentials before any error handling.
        var secretRef = !string.IsNullOrEmpty(request.FtpPassword)
            ? request.FtpPassword
            : await GetExistingFtpServerPasswordAsync(ftpServerId, ct);

        var operatorId = MapToServiceRequestor(request.OperatorId);

        var cmd = new DownstreamFtpServerUpsertRequest
        {
            FtpServerId = ftpServerId,
            FtpServerName = request.FtpServerName,
            FtpDescription = request.FtpDescription,
            CredentialSecretRef = secretRef,
            FtpPort = request.FtpPort,
            FtpProxyAddress = request.FtpProxyAddress,
            FtpProxyPort = request.FtpProxyPort,
            FtpServerIp = request.FtpServerIp,
            FtpTransferMode = request.FtpTransferMode,
            FtpUser = request.FtpUser,
            UseSshAuth = request.UseSshAuth,
            IsValid = request.IsValid,
            OperatorId = operatorId
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Put, $"{DownstreamBasePath}/ftp-servers/{ftpServerId}")
        {
            Content = JsonContent.Create(cmd)
        };

        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        HandleFtpServerBusinessExceptions(response);

        var result = await response.Content.ReadFromJsonAsync<DownstreamFtpServer>(cancellationToken: ct);
        if (result is null)
        {
            throw new InvalidOperationException("Downstream FTP server response was empty.");
        }

        // F02-058: return id/isValid from downstream response.
        return BuildFtpServerModel(result);
    }

    public async Task<FtpServer> InvalidateFtpServerAsync(Guid ftpServerId, FtpServerPatchRequest request, CancellationToken ct)
    {
        var operatorId = MapToServiceRequestor(request.OperatorId);

        // F02-060: legacy invalidation command carries only id + requestor.
        // Target: send only the validity change + operator id; ignore other patch fields.
        var cmd = BuildInvalidateFtpServerCommand(request, operatorId);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"{DownstreamBasePath}/ftp-servers/{ftpServerId}")
        {
            Content = JsonContent.Create(cmd)
        };

        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        HandleFtpServerBusinessExceptions(response);

        var result = await response.Content.ReadFromJsonAsync<DownstreamFtpServer>(cancellationToken: ct);
        if (result is null)
        {
            throw new InvalidOperationException("Downstream FTP server response was empty.");
        }

        // F02-059: return id/isValid from downstream response.
        return BuildFtpServerModel(result);
    }

    private DownstreamFtpServerUpsertRequest BuildAddFtpServerCommand(FtpServerUpsertRequest request, string operatorId)
    {
        return new DownstreamFtpServerUpsertRequest
        {
            FtpServerId = Guid.NewGuid(),
            FtpServerName = request.FtpServerName,
            FtpDescription = request.FtpDescription,
            CredentialSecretRef = string.IsNullOrWhiteSpace(request.FtpPassword) ? null : request.FtpPassword,
            FtpPort = request.FtpPort,
            FtpProxyAddress = request.FtpProxyAddress,
            FtpProxyPort = request.FtpProxyPort,
            FtpServerIp = request.FtpServerIp,
            FtpTransferMode = request.FtpTransferMode,
            FtpUser = request.FtpUser,
            UseSshAuth = request.UseSshAuth,
            IsValid = request.IsValid,
            OperatorId = operatorId
        };
    }

    private static DownstreamFtpServerPatchRequest BuildInvalidateFtpServerCommand(FtpServerPatchRequest request, string operatorId)
    {
        return new DownstreamFtpServerPatchRequest
        {
            IsValid = request.IsValid,
            OperatorId = operatorId
        };
    }

    private static FtpServer BuildFtpServerModel(DownstreamFtpServer serviceResponse)
    {
        return MapToFtpServer(serviceResponse);
    }

    private static void HandleFtpServerBusinessExceptions(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
    }

    private DownstreamTransferConfigurationUpsertRequest BuildTransferConfigurationUpsertCommand(
        int configId,
        TransferConfigurationUpsertRequest request,
        string operatorId)
    {
        DateTimeOffset? suspendedChangeDate = request.Suspended == true
            ? DateTimeOffset.UtcNow
            : null;

        // F01-027..031: legacy mapping rules applied during destination/command mapping.
        var destinations = request.Destinations
            .Select(d => MapDestination(d, operatorId))
            .ToArray();

        return new DownstreamTransferConfigurationUpsertRequest
        {
            ConfigId = configId,
            ConfigName = request.ConfigName,
            Description = request.Description,
            FtpServerId = request.FtpServerId,
            Address = request.Address,
            Mask = request.Mask,
            Protocol = request.Protocol,
            FileDirection = request.FileDirection,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            OwnerSubsystem = request.OwnerSubsystem,
            // F01-024: remove hyphens from owner phone.
            OwnerPhoneNumber = request.OwnerPhoneNumber.Replace("-", string.Empty, StringComparison.Ordinal),
            TradingPartner = request.TradingPartner,
            Destinations = destinations,
            OperatorId = operatorId,
            SendNotification = request.SendNotification,
            RunOnMonday = request.RunOnMonday,
            RunOnTuesday = request.RunOnTuesday,
            RunOnWednesday = request.RunOnWednesday,
            RunOnThursday = request.RunOnThursday,
            RunOnFriday = request.RunOnFriday,
            RunOnSaturday = request.RunOnSaturday,
            RunOnSunday = request.RunOnSunday,
            RunOnFirstDay = request.RunOnFirstDay,
            RunOnLastDay = request.RunOnLastDay,
            // F01-030: empty run-on-this-day maps to 0.
            RunOnThisDay = request.RunOnThisDay ?? 0,
            StartWindow = request.StartWindow,
            EndWindow = request.EndWindow,
            IsActive = request.IsActive,
            Suspended = request.Suspended,
            // F01-031: set changed date only when suspended.
            SuspendedChangeDate = suspendedChangeDate
        };
    }

    private DownstreamDestinationUpsert MapDestination(DestinationUpsert destination, string operatorId)
    {
        var contactPhone = destination.ContactPhone;
        if (contactPhone is not null)
        {
            // F01-027: remove hyphens from destination contact phone.
            contactPhone = contactPhone.Replace("-", string.Empty, StringComparison.Ordinal);
        }

        var address = destination.DestinationAddress;
        if (string.Equals(destination.DestinationType, FileToEventDestinationType, StringComparison.Ordinal))
        {
            // F01-028: file-to-event override.
            address = _options.ExternalFTECommonFolder ?? string.Empty;
        }

        return new DownstreamDestinationUpsert
        {
            DestinationId = destination.DestinationId,
            DestinationAddress = address,
            DestinationProtocol = destination.DestinationProtocol,
            DestinationType = destination.DestinationType,
            // F01-029: map missing FTP server id to Guid.Empty.
            FtpServerId = destination.FtpServerId ?? Guid.Empty,
            RenamePattern = destination.RenamePattern,
            OverWriteAtDestination = destination.OverWriteAtDestination,
            CompressAtDestination = destination.CompressAtDestination,
            RenameAtDestination = destination.RenameAtDestination,
            DestinationArchive = destination.DestinationArchive,
            ContactName = destination.ContactName,
            ContactPhone = contactPhone,
            ContactEmail = destination.ContactEmail,
            IsValid = destination.IsValid,
            FteMetadataId = destination.FteMetadataId,
            ContractName = destination.ContractName,
            ImportJobName = destination.ImportJobName,
            // Legacy always stamps the current operator id for destination commands.
            OperatorId = operatorId
        };
    }

    private async Task<DownstreamTransferConfiguration> GetTransferConfigurationAsync(Guid sourceLocationId, CancellationToken ct)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"{DownstreamBasePath}/transfer-configurations/{sourceLocationId}");
        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DownstreamTransferConfiguration>(cancellationToken: ct);
        if (result is null)
        {
            throw new InvalidOperationException("Downstream transfer configuration response was empty.");
        }

        return result;
    }

    private async Task<DownstreamTransferConfiguration> FileTransferAdd(DownstreamTransferConfigurationUpsertRequest cmd, CancellationToken ct)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{DownstreamBasePath}/transfer-configurations")
        {
            Content = JsonContent.Create(cmd)
        };

        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<DownstreamTransferConfiguration>(cancellationToken: ct);
        if (result is null)
        {
            throw new InvalidOperationException("Downstream transfer configuration response was empty.");
        }

        return result;
    }

    private async Task InvalidateFileTransferSourceLocation(Guid sourceLocationId, DownstreamTransferConfigurationPatchRequest cmd, CancellationToken ct)
    {
        using var httpRequest = new HttpRequestMessage(HttpMethod.Patch, $"{DownstreamBasePath}/transfer-configurations/{sourceLocationId}")
        {
            Content = JsonContent.Create(cmd)
        };

        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        response.EnsureSuccessStatusCode();
    }

    private async Task<string?> GetExistingFtpServerPasswordAsync(Guid ftpServerId, CancellationToken ct)
    {
        var criteria = BuildCriteria(ftpServerId);
        var result = await GetFTPServerCredentials(criteria, ct);
        if (result is null)
        {
            return null;
        }

        // F02-063: when OK but list empty, the legacy implementation throws (First() on an empty list).
        return result.Items.First().CredentialSecretRef;
    }

    private static IReadOnlyList<(string Key, string? Value)> BuildCriteria(Guid ftpServerId)
    {
        return new List<(string Key, string? Value)>
        {
            ("isValid", true.ToString()),
            ("ftpServerId", ftpServerId.ToString()),
            // Legacy BuildRequest uses RemovePassword=false.
            ("removePassword", false.ToString()),
            ("page", 1.ToString(CultureInfo.InvariantCulture)),
            ("pageSize", 50.ToString(CultureInfo.InvariantCulture))
        };
    }

    private async Task<PagedResult<DownstreamFtpServerWithPassword>?> GetFTPServerCredentials(
        IReadOnlyList<(string Key, string? Value)> criteria,
        CancellationToken ct)
    {
        var requestUri = BuildUri($"{DownstreamBasePath}/ftp-servers", criteria);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
        using var response = await _domainApiClient.SendAsync(httpRequest, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PagedResult<DownstreamFtpServerWithPassword>>(cancellationToken: ct);
    }

    private string? GetCurrentOperatorId()
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal is null)
        {
            return null;
        }

        return principal.FindFirstValue("sub")
               ?? principal.FindFirstValue(ClaimTypes.NameIdentifier)
               ?? principal.Identity?.Name;
    }

    private string MapToServiceRequestor(string? fallbackOperatorId)
    {
        // Target: request context is propagated via headers; fall back to request-supplied operator id.
        var operatorId = GetCurrentOperatorId() ?? fallbackOperatorId;
        if (string.IsNullOrWhiteSpace(operatorId))
        {
            throw new InvalidOperationException("OperatorId was not present in the current request context.");
        }

        return operatorId;
    }

    private static TransferConfiguration MapToInternalProvider(DownstreamTransferConfiguration serviceModel)
    {
        return MapToTransferConfiguration(serviceModel);
    }

    private static void HandleException(Exception ex)
    {
        ExceptionDispatchInfo.Capture(ex).Throw();
    }

    // MaintainabilityDetails: ApprovedIndex = 38, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez"
    private static TransferConfiguration MapToTransferConfiguration(DownstreamTransferConfiguration serviceModel)
    {
        var destinations = serviceModel.Destinations.Select(MapToDestination).ToArray();

        return new TransferConfiguration
        {
            FtsSourceLocationId = serviceModel.FtsSourceLocationId,
            ConfigId = serviceModel.ConfigId,
            ConfigName = serviceModel.ConfigName,
            Description = serviceModel.Description ?? string.Empty,
            FtpServerId = serviceModel.FtpServerId ?? Guid.Empty,
            FtpServerName = serviceModel.FtpServerName ?? string.Empty,
            Address = serviceModel.Address,
            Mask = serviceModel.Mask ?? string.Empty,
            Protocol = serviceModel.Protocol,
            FileDirection = serviceModel.FileDirection ?? string.Empty,
            OwnerName = serviceModel.OwnerName,
            OwnerEmail = serviceModel.OwnerEmail,
            OwnerSubsystem = serviceModel.OwnerSubsystem,
            OwnerPhoneNumber = serviceModel.OwnerPhoneNumber,
            TradingPartner = serviceModel.TradingPartner,
            OperatorId = serviceModel.OperatorId,
            RunOnMonday = serviceModel.RunOnMonday,
            RunOnTuesday = serviceModel.RunOnTuesday,
            RunOnWednesday = serviceModel.RunOnWednesday,
            RunOnThursday = serviceModel.RunOnThursday,
            RunOnFriday = serviceModel.RunOnFriday,
            RunOnSaturday = serviceModel.RunOnSaturday,
            RunOnSunday = serviceModel.RunOnSunday,
            RunOnFirstDay = serviceModel.RunOnFirstDay,
            RunOnLastDay = serviceModel.RunOnLastDay,
            RunOnThisDay = serviceModel.RunOnThisDay,
            StartWindow = serviceModel.StartWindow,
            EndWindow = serviceModel.EndWindow,
            SendNotification = serviceModel.SendNotification,
            IsActive = serviceModel.IsActive,
            ValidChangeDate = serviceModel.ValidChangeDate,
            Suspended = serviceModel.Suspended,
            SuspendedChangeDate = serviceModel.SuspendedChangeDate,
            LastModifiedDate = serviceModel.LastModifiedDate,
            CreationDate = serviceModel.CreationDate,
            Destinations = destinations
        };
    }

    private static Destination MapToDestination(DownstreamDestination destination)
    {
        return new Destination
        {
            DestinationId = destination.DestinationId,
            DestinationAddress = destination.DestinationAddress,
            DestinationProtocol = destination.DestinationProtocol,
            FtpServerId = destination.FtpServerId,
            RenamePattern = destination.RenamePattern,
            OverWriteAtDestination = destination.OverWriteAtDestination,
            CompressAtDestination = destination.CompressAtDestination,
            RenameAtDestination = destination.RenameAtDestination,
            DestinationArchive = destination.DestinationArchive,
            ContactName = destination.ContactName,
            ContactPhone = destination.ContactPhone,
            ContactEmail = destination.ContactEmail,
            IsValid = destination.IsValid,
            FteMetadataId = destination.FteMetadataId,
            DestinationType = destination.DestinationType,
            ContractName = destination.ContractName,
            ImportJobName = destination.ImportJobName,
            OperatorId = destination.OperatorId,
            CreationDate = destination.CreationDate,
            LastModifiedDate = destination.LastModifiedDate
        };
    }

    private static FtpServer MapToFtpServer(DownstreamFtpServer serviceModel)
    {
        return new FtpServer
        {
            FtpServerId = serviceModel.FtpServerId,
            FtpServerName = serviceModel.FtpServerName,
            FtpDescription = serviceModel.FtpDescription,
            FtpServerIp = serviceModel.FtpServerIp,
            FtpTransferMode = serviceModel.FtpTransferMode,
            FtpUser = serviceModel.FtpUser,
            FtpPort = serviceModel.FtpPort,
            FtpProxyAddress = serviceModel.FtpProxyAddress,
            FtpProxyPort = serviceModel.FtpProxyPort,
            UseSshAuth = serviceModel.UseSshAuth,
            IsValid = serviceModel.IsValid,
            OperatorId = serviceModel.OperatorId,
            CreationDate = serviceModel.CreationDate,
            LastModifiedDate = serviceModel.LastModifiedDate
        };
    }

    private static Uri BuildUri(string basePath, IReadOnlyList<(string Key, string? Value)> queryParams)
    {
        var filtered = queryParams
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value!)}");

        var queryString = string.Join("&", filtered);
        var uriText = string.IsNullOrEmpty(queryString) ? basePath : $"{basePath}?{queryString}";
        return new Uri(uriText, UriKind.Relative);
    }

    private sealed record DownstreamTransferConfigurationUpsertRequest
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
        public required Guid OwnerSubsystem { get; init; }
        public required string OwnerPhoneNumber { get; init; }
        public string? TradingPartner { get; init; }
        public required IReadOnlyList<DownstreamDestinationUpsert>? Destinations { get; init; }
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
        public int RunOnThisDay { get; init; }
        public required string StartWindow { get; init; }
        public required string EndWindow { get; init; }
        public bool? SendNotification { get; init; }
        public bool? IsActive { get; init; }
        public bool? Suspended { get; init; }
        public DateTimeOffset? SuspendedChangeDate { get; init; }
    }

    private sealed record DownstreamDestinationUpsert
    {
        public Guid? DestinationId { get; init; }
        public required string DestinationAddress { get; init; }
        public required string DestinationProtocol { get; init; }
        public Guid FtpServerId { get; init; }
        public string? RenamePattern { get; init; }
        public bool? OverWriteAtDestination { get; init; }
        public bool? CompressAtDestination { get; init; }
        public bool? RenameAtDestination { get; init; }
        public bool? DestinationArchive { get; init; }
        public string? ContactName { get; init; }
        public string? ContactPhone { get; init; }
        public string? ContactEmail { get; init; }
        public bool? IsValid { get; init; }
        public Guid? FteMetadataId { get; init; }
        public string? DestinationType { get; init; }
        public string? ContractName { get; init; }
        public string? ImportJobName { get; init; }
        public string? OperatorId { get; init; }
    }

    private sealed record DownstreamTransferConfigurationPatchRequest
    {
        public int ConfigId { get; init; }
        public bool? IsActive { get; init; }
        public bool? Suspended { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed record DownstreamTransferConfiguration
    {
        public Guid FtsSourceLocationId { get; init; }
        public int ConfigId { get; init; }
        public required string ConfigName { get; init; }
        public string? Description { get; init; }
        public Guid? FtpServerId { get; init; }
        public string? FtpServerName { get; init; }
        public required string Address { get; init; }
        public string? Mask { get; init; }
        public required string Protocol { get; init; }
        public string? FileDirection { get; init; }
        public required string OwnerName { get; init; }
        public required string OwnerEmail { get; init; }
        public Guid OwnerSubsystem { get; init; }
        public required string OwnerPhoneNumber { get; init; }
        public string? TradingPartner { get; init; }
        public required string OperatorId { get; init; }
        public bool RunOnMonday { get; init; }
        public bool RunOnTuesday { get; init; }
        public bool RunOnWednesday { get; init; }
        public bool RunOnThursday { get; init; }
        public bool RunOnFriday { get; init; }
        public bool RunOnSaturday { get; init; }
        public bool RunOnSunday { get; init; }
        public bool RunOnFirstDay { get; init; }
        public bool RunOnLastDay { get; init; }
        public int RunOnThisDay { get; init; }
        public required string StartWindow { get; init; }
        public required string EndWindow { get; init; }
        public bool SendNotification { get; init; }
        public bool IsActive { get; init; }
        public DateTimeOffset ValidChangeDate { get; init; }
        public bool Suspended { get; init; }
        public DateTimeOffset SuspendedChangeDate { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public IReadOnlyList<DownstreamDestination> Destinations { get; init; } = Array.Empty<DownstreamDestination>();
    }

    private sealed record DownstreamDestination
    {
        public Guid? DestinationId { get; init; }
        public required string DestinationAddress { get; init; }
        public required string DestinationProtocol { get; init; }
        public Guid? FtpServerId { get; init; }
        public string? RenamePattern { get; init; }
        public bool? OverWriteAtDestination { get; init; }
        public bool? CompressAtDestination { get; init; }
        public bool? RenameAtDestination { get; init; }
        public bool? DestinationArchive { get; init; }
        public string? ContactName { get; init; }
        public string? ContactPhone { get; init; }
        public string? ContactEmail { get; init; }
        public bool? IsValid { get; init; }
        public Guid? FteMetadataId { get; init; }
        public string? DestinationType { get; init; }
        public string? ContractName { get; init; }
        public string? ImportJobName { get; init; }
        public string? OperatorId { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
    }

    private sealed record DownstreamFtpServerUpsertRequest
    {
        public Guid FtpServerId { get; init; }
        public required string FtpServerName { get; init; }
        public required string FtpDescription { get; init; }
        public string? CredentialSecretRef { get; init; }
        public int FtpPort { get; init; }
        public string? FtpProxyAddress { get; init; }
        public int? FtpProxyPort { get; init; }
        public required string FtpServerIp { get; init; }
        public required string FtpTransferMode { get; init; }
        public required string FtpUser { get; init; }
        public bool UseSshAuth { get; init; }
        public bool IsValid { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed record DownstreamFtpServerPatchRequest
    {
        public string? FtpServerName { get; init; }
        public string? FtpDescription { get; init; }
        public string? FtpServerIp { get; init; }
        public string? FtpTransferMode { get; init; }
        public string? FtpUser { get; init; }
        public string? CredentialSecretRef { get; init; }
        public int? FtpPort { get; init; }
        public string? FtpProxyAddress { get; init; }
        public int? FtpProxyPort { get; init; }
        public bool? UseSshAuth { get; init; }
        public bool? IsValid { get; init; }
        public required string OperatorId { get; init; }
    }

    private sealed record DownstreamFtpServerWithPassword
    {
        public Guid FtpServerId { get; init; }
        public string? CredentialSecretRef { get; init; }
    }

    private sealed record DownstreamFtpServer
    {
        public Guid FtpServerId { get; init; }
        public required string FtpServerName { get; init; }
        public required string FtpDescription { get; init; }
        public required string FtpServerIp { get; init; }
        public required string FtpTransferMode { get; init; }
        public required string FtpUser { get; init; }
        public string? CredentialSecretRef { get; init; }
        public int FtpPort { get; init; }
        public string? FtpProxyAddress { get; init; }
        public int? FtpProxyPort { get; init; }
        public bool UseSshAuth { get; init; }
        public bool IsValid { get; init; }
        public required string OperatorId { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public DateTimeOffset LastModifiedDate { get; init; }
    }
}

