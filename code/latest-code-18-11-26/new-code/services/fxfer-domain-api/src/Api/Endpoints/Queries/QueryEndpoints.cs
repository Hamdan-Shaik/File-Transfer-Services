namespace Fxfer.DomainApi.Api.Endpoints.Queries;

public static class QueryEndpoints
{
    public static RouteGroupBuilder MapQueryEndpoints(this RouteGroupBuilder group)
    {
        group.MapFtpServersQueries();
        group.MapTransferConfigurationsQueries();
        group.MapSourceLocationsQueries();
        group.MapIntegrationFilesQueries();
        group.MapInboundTargetsQueries();
        group.MapFileTrackingQueries();
        return group;
    }
}

