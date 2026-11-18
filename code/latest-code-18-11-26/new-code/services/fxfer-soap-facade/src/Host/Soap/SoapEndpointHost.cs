using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CoreWCF.Dispatcher;
using Fxfer.SoapFacade.CoreWcf.Contracts.Services;
using Fxfer.SoapFacade.Host.Soap.Bindings;
using Fxfer.SoapFacade.Host.Soap.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace Fxfer.SoapFacade.Host.Soap;

internal static class SoapEndpointHost
{
    internal static void MapSoapEndpoints(WebApplication app)
    {
        const string fileTransferSvcEventServiceEndpoint = "FileTransferSvcEventServiceEndpoint";
        const string fileTransferSvcEventServiceListener = "FileTransferSvcEventServiceListener";
        const string fileTransferSvcQueryServiceEndpoint = "FileTransferSvcQueryServiceEndpoint";
        const string fileTransferSvcQueryServiceListener = "FileTransferSvcQueryServiceListener";
        const string fileTransferSvcServiceEndpoint = "FileTransferSvcServiceEndpoint";
        const string fileTransferSvcServiceListener = "FileTransferSvcServiceListener";

        var server = app.Services.GetRequiredService<IServer>();
        var isTestServer = string.Equals(server.GetType().FullName, "Microsoft.AspNetCore.TestHost.TestServer", StringComparison.Ordinal);
        if (isTestServer)
        {
            return;
        }

        var serviceMetadata = app.Services.GetRequiredService<ServiceMetadataBehavior>();
        serviceMetadata.HttpGetEnabled = true;
        serviceMetadata.HttpsGetEnabled = true;

        var serverAddresses = server.Features.Get<IServerAddressesFeature>();
        var hasHttpsAddress = serverAddresses?.Addresses.Any(address => address.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) == true;
        var binding = hasHttpsAddress
            ? SoapBindingFactory.CreateBasicHttpsBinding()
            : SoapBindingFactory.CreateBasicHttpBinding();

        app.UseServiceModel(serviceBuilder =>
        {
            serviceBuilder.AddService<FileTransferSvcService>();
            serviceBuilder.AddServiceEndpoint<FileTransferSvcService, IFileTransferSvcService>(
                binding,
                "/soap/file-transfer/v1/FileTransferSvcService.svc",
                endpoint => AttachLegacyListenerNames(endpoint, fileTransferSvcServiceEndpoint, fileTransferSvcServiceListener));

            serviceBuilder.AddService<FileTransferSvcQueryService>();
            serviceBuilder.AddServiceEndpoint<FileTransferSvcQueryService, IFileTransferSvcQueryService>(
                binding,
                "/soap/file-transfer/v1/FileTransferSvcQueryService.svc",
                endpoint => AttachLegacyListenerNames(endpoint, fileTransferSvcQueryServiceEndpoint, fileTransferSvcQueryServiceListener));

            serviceBuilder.AddService<FileTransferSvcEventService>();
            serviceBuilder.AddServiceEndpoint<FileTransferSvcEventService, IEventDistribution>(
                binding,
                "/soap/file-transfer/v1/FileTransferSvcEventService.svc",
                endpoint => AttachLegacyListenerNames(endpoint, fileTransferSvcEventServiceEndpoint, fileTransferSvcEventServiceListener));
        });
    }

    private static void AttachLegacyListenerNames(ServiceEndpoint endpoint, string legacyEndpointName, string legacyListenerName)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(legacyEndpointName);
        ArgumentNullException.ThrowIfNull(legacyListenerName);

        endpoint.EndpointBehaviors.Add(new LegacyServiceFabricListenerNamesBehavior(legacyEndpointName, legacyListenerName));
    }

    private sealed class LegacyServiceFabricListenerNamesBehavior : IEndpointBehavior
    {
        internal LegacyServiceFabricListenerNamesBehavior(string legacyEndpointName, string legacyListenerName)
        {
            LegacyEndpointName = legacyEndpointName;
            LegacyListenerName = legacyListenerName;
        }

        internal string LegacyEndpointName { get; }

        internal string LegacyListenerName { get; }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}

