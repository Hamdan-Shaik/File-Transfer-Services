using System.Reflection;
using Fxfer.DomainApi.Contracts.Domain;

namespace Fxfer.DomainApi.Tests.Contracts.Domain;

public sealed class SharedContractsDomain2ContractTests
{
    private const string SpecRoot = "C:/platform-code/specs/provider-file-transfer-v2";

    [Fact]
    public void OpenApi_has_no_http_surface()
    {
        var openApiPath = Path.Combine(SpecRoot, "technical/components/shared-contracts-domain-2/openapi.yaml");
        var yaml = File.ReadAllText(openApiPath);

        Assert.Contains("x-no-http-surface: true", yaml);
        Assert.Contains("paths: {}", yaml);
    }

    [Fact]
    public void Contract_lists_all_offered_types()
    {
        var contractPath = Path.Combine(SpecRoot, "technical/components/shared-contracts-domain-2/CONTRACT.md");
        var markdown = File.ReadAllText(contractPath);

        var offeredTypes = new[]
        {
            nameof(DestinationModel),
            nameof(ErrorLogDetailModel),
            nameof(FileManagementRequestModel),
            nameof(FileManagementResponseModel),
            nameof(FileToDeliverModel),
            nameof(FtpServerModel),
            nameof(FtsDeliveryModel),
            nameof(FtsNotificationModel),
            nameof(FtsNotificationQueryModel),
            nameof(FtsSuspensionModel),
            nameof(FtsSuspensionSearchCriteria),
            nameof(GetNotificationSearchCriteria),
            nameof(PendingFileModel),
            nameof(SourceLocationModel),
            nameof(SourceScheduleModel),
        };

        var contractsAssembly = typeof(SourceLocationModel).Assembly;

        foreach (var typeName in offeredTypes)
        {
            Assert.Contains($"`{typeName}`", markdown);

            var fullName = $"{typeof(SourceLocationModel).Namespace}.{typeName}";
            var type = contractsAssembly.GetType(fullName);
            Assert.NotNull(type);
        }
    }
}