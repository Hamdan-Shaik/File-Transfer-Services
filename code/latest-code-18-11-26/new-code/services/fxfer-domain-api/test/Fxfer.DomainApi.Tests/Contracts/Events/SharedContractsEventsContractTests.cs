using System.Reflection;
using Fxfer.DomainApi.Contracts.Events;

namespace Fxfer.DomainApi.Tests.Contracts.Events;

public sealed class SharedContractsEventsContractTests
{
    private const string SpecRoot = "C:/platform-code/specs/provider-file-transfer-v2";

    [Fact]
    public void T_006_contract_lists_all_offered_types()
    {
        var contractPath = Path.Combine(SpecRoot, "technical/components/shared-contracts-events/CONTRACT.md");
        var markdown = File.ReadAllText(contractPath);

        var offeredTypes = new[]
        {
            nameof(FxferEventEnvelope),
            "FxferDomainEvent<TPayload>",
            nameof(FxferConfigurationChangedV1),
            nameof(FxferSuspensionStateChangedV1),
            nameof(FxferDeliveryOutcomeRecordedV1)
        };

        var contractsAssembly = typeof(FxferEventEnvelope).Assembly;

        foreach (var typeName in offeredTypes)
        {
            Assert.Contains($"`{typeName}`", markdown);
        }

        Assert.NotNull(contractsAssembly.GetType(typeof(FxferEventEnvelope).FullName!));
        Assert.NotNull(contractsAssembly.GetType(typeof(FxferConfigurationChangedV1).FullName!));
        Assert.NotNull(contractsAssembly.GetType(typeof(FxferSuspensionStateChangedV1).FullName!));
        Assert.NotNull(contractsAssembly.GetType(typeof(FxferDeliveryOutcomeRecordedV1).FullName!));
    }

    [Fact]
    public void T_006_event_contract_does_not_expose_secret_material()
    {
        var payloadTypes = new[]
        {
            typeof(FxferConfigurationChangedV1),
            typeof(FxferSuspensionStateChangedV1),
            typeof(FxferDeliveryOutcomeRecordedV1)
        };

        foreach (var type in payloadTypes)
        {
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var name = property.Name;
                Assert.DoesNotContain("Password", name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Secret", name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("PrivateKey", name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Certificate", name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("FileContents", name, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("PayloadBytes", name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    [Fact]
    public void T_006_delivery_outcome_event_payload_is_minimized()
    {
        var expected = new[]
        {
            nameof(FxferDeliveryOutcomeRecordedV1.FileId),
            nameof(FxferDeliveryOutcomeRecordedV1.SourceLocationId),
            nameof(FxferDeliveryOutcomeRecordedV1.DestinationId),
            nameof(FxferDeliveryOutcomeRecordedV1.OutcomeStatus),
            nameof(FxferDeliveryOutcomeRecordedV1.OccurredAt)
        };

        var actual = typeof(FxferDeliveryOutcomeRecordedV1)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(p => p.Name)
            .OrderBy(n => n, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(expected.OrderBy(n => n, StringComparer.Ordinal), actual);
    }
}

