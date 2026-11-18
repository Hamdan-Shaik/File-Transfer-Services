namespace Fxfer.DomainApi.Api.Endpoints.Commands;

public static class CommandEndpoints
{
    public static RouteGroupBuilder MapCommandEndpoints(this RouteGroupBuilder group)
    {
        group.MapFtpServersCommands();
        group.MapTransferConfigurationsCommands();
        group.MapIntegrationFilesCommands();
        return group;
    }
}

