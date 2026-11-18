using System.Net;
using System.Net.Http.Json;
using Fxfer.DomainApi.Contracts.Commands;
using Fxfer.DomainApi.Contracts.Domain;
using Fxfer.DomainApi.Tests.Infrastructure;

namespace Fxfer.DomainApi.Tests.Specs;

public sealed class ConfigureSourceLocationsAndDestinationsFlowTests
{
    [Fact]
    public async Task Saving_a_pickup_location_normalizes_phone_numbers_and_assigns_a_new_configuration_number()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        var request = BuildCreateRequest(
            ownerPhoneNumber: "555-010-1234",
            destinationContactPhone: "555-010-2222");

        using var response = await client.PostAsJsonAsync("/api/fxfer-domain/v1/transfer-configurations", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var saved = await response.Content.ReadFromJsonAsync<TransferConfiguration>();
        Assert.NotNull(saved);
        Assert.True(saved!.ConfigId > 0);
        Assert.DoesNotContain("-", saved.OwnerPhoneNumber);

        var dest = Assert.Single(saved.Destinations);
        Assert.NotNull(dest.ContactPhone);
        Assert.DoesNotContain("-", dest.ContactPhone);
    }

    [Fact]
    public async Task A_staff_member_selects_a_pickup_location_from_the_active_list()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        var first = await CreateAsync(client, BuildCreateRequest(configName: "Daily Claims", address: "Claims Inbox"));
        var second = await CreateAsync(client, BuildCreateRequest(configName: "Monthly Reports", address: "Reports Inbox"));

        using var response = await client.GetAsync("/api/fxfer-domain/v1/source-locations?isActive=true");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var options = await response.Content.ReadFromJsonAsync<IReadOnlyList<SourceLocationOption>>();
        Assert.NotNull(options);

        var ids = options!.Select(x => x.FtsSourceLocationId).ToHashSet();
        Assert.Contains(first.FtsSourceLocationId, ids);
        Assert.Contains(second.FtsSourceLocationId, ids);

        var names = options.Select(x => x.ConfigName).ToArray();
        Assert.True(names.SequenceEqual(names.OrderBy(x => x, StringComparer.Ordinal)));
    }

    [Fact]
    public async Task A_staff_member_updates_an_existing_pickup_location()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        var created = await CreateAsync(client, BuildCreateRequest(configName: "Daily Claims", address: "Claims Inbox"));

        var update = BuildCreateRequest(configName: "Daily Claims Updated", address: "Claims Inbox") with
        {
            Destinations = new[]
            {
                new DestinationUpsert
                {
                    DestinationId = created.Destinations.Single().DestinationId,
                    DestinationAddress = "\\\\dest\\\\updated",
                    DestinationProtocol = "SFTP",
                    ContactPhone = "555-010-9999",
                    IsValid = true,
                    OperatorId = null
                }
            }
        };

        using var response = await client.PutAsJsonAsync($"/api/fxfer-domain/v1/transfer-configurations/{created.FtsSourceLocationId}", update);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content.ReadFromJsonAsync<TransferConfiguration>();
        Assert.NotNull(updated);
        Assert.Equal("Daily Claims Updated", updated!.ConfigName);

        var destination = Assert.Single(updated.Destinations);
        Assert.Equal("\\\\dest\\\\updated", destination.DestinationAddress);
        Assert.NotNull(destination.ContactPhone);
        Assert.DoesNotContain("-", destination.ContactPhone);
    }

    [Fact]
    public async Task A_staff_member_stops_using_a_pickup_location_and_its_destinations_become_inactive()
    {
        await using var factory = new FxferDomainApiFactory();
        using var client = factory.CreateClient();

        var created = await CreateAsync(client, BuildCreateRequest(destinations: 2));

        using var response = await client.PatchAsJsonAsync(
            $"/api/fxfer-domain/v1/transfer-configurations/{created.FtsSourceLocationId}",
            new TransferConfigurationPatchRequest { IsActive = false, OperatorId = "test-user" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var invalidated = await response.Content.ReadFromJsonAsync<TransferConfiguration>();
        Assert.NotNull(invalidated);
        Assert.False(invalidated!.IsActive);
        Assert.All(invalidated.Destinations, d => Assert.False(d.IsValid ?? false));
    }

    private static async Task<TransferConfiguration> CreateAsync(HttpClient client, TransferConfigurationUpsertRequest request)
    {
        using var response = await client.PostAsJsonAsync("/api/fxfer-domain/v1/transfer-configurations", request);
        response.EnsureSuccessStatusCode();
        var saved = await response.Content.ReadFromJsonAsync<TransferConfiguration>();
        return saved ?? throw new InvalidOperationException("Missing transfer configuration response.");
    }

    private static TransferConfigurationUpsertRequest BuildCreateRequest(
        string configName = "Daily Claims",
        string address = "Claims Inbox",
        string ownerPhoneNumber = "5550101234",
        string destinationContactPhone = "5550102222",
        int destinations = 1)
    {
        var destinationList = Enumerable.Range(0, destinations)
            .Select(i => new DestinationUpsert
            {
                DestinationId = null,
                DestinationAddress = $"\\\\dest\\\\{i}",
                DestinationProtocol = "SFTP",
                ContactPhone = destinationContactPhone,
                IsValid = true,
                OperatorId = null
            })
            .ToArray();

        return new TransferConfigurationUpsertRequest
        {
            ConfigName = configName,
            Description = "desc",
            FtpServerId = null,
            Address = address,
            Mask = "*.txt",
            Protocol = "2",
            FileDirection = "Outbound",
            OwnerName = "owner",
            OwnerEmail = "owner@example.com",
            OwnerPhoneNumber = ownerPhoneNumber,
            OwnerSubsystem = Guid.NewGuid(),
            TradingPartner = null,
            SendNotification = true,
            RunOnMonday = true,
            RunOnTuesday = false,
            RunOnWednesday = false,
            RunOnThursday = false,
            RunOnFriday = false,
            RunOnSaturday = false,
            RunOnSunday = false,
            RunOnFirstDay = false,
            RunOnLastDay = false,
            RunOnThisDay = null,
            StartWindow = "0000",
            EndWindow = "2359",
            IsActive = true,
            Suspended = false,
            Destinations = destinationList,
            OperatorId = "test-user"
        };
    }
}

