using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SecretsManager.Model;
using Fxfer.ExecutionWorker.Infrastructure.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class T004SecretsResolverTests
{
    [Fact]
    public async Task T_004_resolves_secret_references_as_tenant_scoped_secret_ids()
    {
        var client = new CapturingSecretsClient(new GetSecretValueResponse { SecretString = "value" });

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Secrets:NamePrefix"] = "fxfer",
            })
            .Build();

        var resolver = new SecretsResolver(client, configuration, NullLogger<SecretsResolver>.Instance);

        var value = await resolver.ResolveRequiredSecretAsync("tenant-a", "secret:external-endpoints/ftp/password", CancellationToken.None);

        Assert.Equal("value", value);
        Assert.Equal("fxfer/tenant-a/external-endpoints/ftp/password", client.LastSecretId);
    }

    [Fact]
    public void T_004_rejects_inline_secret_values_that_are_not_references()
    {
        var resolver = new SecretsResolver(
            new CapturingSecretsClient(new GetSecretValueResponse { SecretString = "value" }),
            new ConfigurationBuilder().Build(),
            NullLogger<SecretsResolver>.Instance);

        var ex = Assert.Throws<ArgumentException>(() => resolver.BuildTenantScopedSecretId("tenant", "pw"));
        Assert.Contains("Secret references must use", ex.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task T_004_allows_full_secrets_manager_arns_without_modification()
    {
        var arn = "arn:aws:secretsmanager:us-east-1:123456789012:secret:fxfer/tenant-a/external";
        var client = new CapturingSecretsClient(new GetSecretValueResponse { SecretString = "value" });
        var resolver = new SecretsResolver(client, new ConfigurationBuilder().Build(), NullLogger<SecretsResolver>.Instance);

        _ = await resolver.ResolveRequiredSecretAsync("tenant-a", arn, CancellationToken.None);

        Assert.Equal(arn, client.LastSecretId);
    }

    private sealed class CapturingSecretsClient : ISecretsManagerClient
    {
        private readonly GetSecretValueResponse _response;

        public CapturingSecretsClient(GetSecretValueResponse response)
        {
            _response = response;
        }

        public string? LastSecretId { get; private set; }

        public Task<GetSecretValueResponse> GetSecretValueAsync(string secretId, CancellationToken cancellationToken)
        {
            LastSecretId = secretId;
            return Task.FromResult(_response);
        }
    }
}
