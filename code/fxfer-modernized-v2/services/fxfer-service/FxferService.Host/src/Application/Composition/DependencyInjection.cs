using FxferService.Application.CommandHandlers;
using FxferService.Application.ProjectionWriters;
using Microsoft.Extensions.DependencyInjection;

namespace FxferService.Application.Composition;

public static class DependencyInjection
{
    public static IServiceCollection AddBackendBasFileTransferSvc2(this IServiceCollection services)
    {
        services.AddTransient<FtsSourceLocationAddCommandHandler>();
        services.AddTransient<FtpServerCommandHandler>();
        services.AddTransient<FtsInboundTargetCommandHandler>();
        services.AddTransient<FtsIntegrationFileCommandHandler>();
        services.AddTransient<FtsSourceLocationProjectionWriter>();
        services.AddTransient<FtpServerProjectionWriter>();
        services.AddTransient<FtsInboundTargetProjectionWriter>();
        services.AddTransient<FtsIntegrationFileProjectionWriter>();

        return services;
    }
}


