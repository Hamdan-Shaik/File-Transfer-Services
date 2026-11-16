using System.Text;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fxfer.ExecutionWorker.Infrastructure.Secrets;

public interface ISecretsManagerClient
{
    Task<GetSecretValueResponse> GetSecretValueAsync(string secretId, CancellationToken cancellationToken);
}

public sealed class AwsSecretsManagerClient : ISecretsManagerClient
{
    private readonly IAmazonSecretsManager _secretsManager;

    public AwsSecretsManagerClient(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    public Task<GetSecretValueResponse> GetSecretValueAsync(string secretId, CancellationToken cancellationToken)
    {
        return _secretsManager.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretId,
        }, cancellationToken);
    }
}

public sealed class SecretsResolver
{
    private readonly ISecretsManagerClient _client;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SecretsResolver> _logger;

    public SecretsResolver(ISecretsManagerClient client, IConfiguration configuration, ILogger<SecretsResolver> logger)
    {
        _client = client;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> CanResolveAsync(CancellationToken cancellationToken)
    {
        var probeTenantId = _configuration["Secrets:ReadinessProbeTenantId"];
        var probeSecretReference = _configuration["Secrets:ReadinessProbeSecretReference"];

        if (string.IsNullOrWhiteSpace(probeTenantId) || string.IsNullOrWhiteSpace(probeSecretReference))
        {
            return true;
        }

        try
        {
            _ = await ResolveRequiredSecretAsync(probeTenantId, probeSecretReference, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Secrets readiness probe failed");
            return false;
        }
    }

    public async Task<string> ResolveRequiredSecretAsync(
        string tenantId,
        string secretReference,
        CancellationToken cancellationToken)
    {
        // T-004: Retrieve external-endpoint credentials and other secrets tenant-scoped from AWS Secrets Manager + KMS.
        var secretId = BuildTenantScopedSecretId(tenantId, secretReference);
        var response = await _client.GetSecretValueAsync(secretId, cancellationToken);

        if (!string.IsNullOrWhiteSpace(response.SecretString))
        {
            return response.SecretString;
        }

        if (response.SecretBinary is { Length: > 0 })
        {
            return Encoding.UTF8.GetString(response.SecretBinary.ToArray());
        }

        throw new InvalidOperationException($"Secrets Manager returned empty secret payload for '{secretId}'.");
    }

    public string ResolveRequiredSecret(string tenantId, string secretReference)
    {
        return ResolveRequiredSecretAsync(tenantId, secretReference, CancellationToken.None).GetAwaiter().GetResult();
    }

    internal string BuildTenantScopedSecretId(string tenantId, string secretReference)
    {
        if (string.IsNullOrWhiteSpace(tenantId))
        {
            throw new ArgumentException("Tenant id is required.", nameof(tenantId));
        }

        if (string.IsNullOrWhiteSpace(secretReference))
        {
            throw new ArgumentException("Secret reference is required.", nameof(secretReference));
        }

        var trimmed = secretReference.Trim();
        if (trimmed.StartsWith("arn:", StringComparison.OrdinalIgnoreCase))
        {
            return trimmed;
        }

        const string secretScheme = "secret:";
        const string smScheme = "sm://";

        string? secretKey = null;
        if (trimmed.StartsWith(secretScheme, StringComparison.OrdinalIgnoreCase))
        {
            secretKey = trimmed[secretScheme.Length..];
        }
        else if (trimmed.StartsWith(smScheme, StringComparison.OrdinalIgnoreCase))
        {
            secretKey = trimmed[smScheme.Length..];
        }

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException(
                "Secret references must use 'secret:<key>', 'sm://<key>', or a full Secrets Manager ARN.",
                nameof(secretReference));
        }

        var prefix = (_configuration["Secrets:NamePrefix"] ?? "provider-file-transfer/fxfer").Trim().TrimEnd('/');
        secretKey = secretKey.Trim().TrimStart('/');
        var tenantSegment = tenantId.Trim().Trim('/');

        return string.Join('/', new[] { prefix, tenantSegment, secretKey });
    }
}
