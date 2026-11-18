using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Fxfer.SoapFacade.Host.Configuration;
using Fxfer.SoapFacade.Host.Middleware;
using Microsoft.Extensions.Options;

namespace Fxfer.SoapFacade.Host.Clients;

public sealed class FxferDomainApiClient
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public FxferDomainApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken)
    {
        using var response = await _client.PostAsJsonAsync(path, request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken)
    {
        using var response = await _client.PutAsJsonAsync(path, request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }

    public async Task<TResponse?> PatchAsync<TRequest, TResponse>(string path, TRequest request, CancellationToken cancellationToken)
    {
        using var content = new StringContent(JsonSerializer.Serialize(request, SerializerOptions), Encoding.UTF8, "application/json");
        using var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Patch, path) { Content = content }, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
    }
}

public static class FxferDomainApiClientRegistration
{
    public static IServiceCollection AddFxferDomainApiClient(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<HttpClientCorrelationIdHandler>();

        services.AddHttpClient<FxferDomainApiClient>((sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<DomainApiOptions>>().Value;
            if (Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri))
            {
                client.BaseAddress = uri;
            }
        }).AddHttpMessageHandler<HttpClientCorrelationIdHandler>();

        return services;
    }
}
