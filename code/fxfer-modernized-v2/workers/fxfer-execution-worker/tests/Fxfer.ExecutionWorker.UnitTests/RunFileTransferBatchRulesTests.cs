using System;
using System.Collections.Generic;
using Fxfer.ExecutionWorker.BatchProcess;
using Fxfer.ExecutionWorker.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Fxfer.ExecutionWorker.UnitTests;

public sealed class RunFileTransferBatchRulesTests
{
    [Fact]
    public void F03_001_standard_scheduled_run_executes_and_returns_result()
    {
        var executor = new CapturingExecutor(result: 123);
        var entrypoint = CreateEntrypoint(executor, tenantSettings: new Dictionary<string, string?>());

        var runDetails = new[] { "a", "b" };
        var result = entrypoint.RunStandardScheduled(runDetails);

        Assert.Equal(123, result);
        Assert.Same(runDetails, executor.LastRunDetails);
    }

    [Fact]
    public void F03_002_standard_scheduled_run_returns_negative_one_on_error()
    {
        var executor = new ThrowingExecutor(new Exception("boom"));
        var logger = new ListLogger<RunFileTransferBatchEntrypoint>();
        var entrypoint = CreateEntrypoint(executor, logger: logger, tenantSettings: new Dictionary<string, string?>());

        var result = entrypoint.RunStandardScheduled(new[] { "x" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Messages, m => m.Contains("File Transfer Batch Process") && m.Contains("boom"));
    }

    [Fact]
    public void F03_003_management_scheduled_run_logs_start_and_always_logs_end()
    {
        var executor = new CapturingExecutor(result: 0);
        var logger = new ListLogger<RunFileTransferBatchEntrypoint>();
        var entrypoint = CreateEntrypoint(executor, logger: logger, tenantSettings: new Dictionary<string, string?>());

        _ = entrypoint.RunManagementScheduled(Array.Empty<string>());

        Assert.Contains(logger.Messages, m => m == "Starting FileTransfer Batch Process");
        Assert.Contains(logger.Messages, m => m == "Ending FileTransfer Batch Process");
    }

    [Fact]
    public void F03_004_management_scheduled_run_requires_1_or_3_run_details_and_logs_usage()
    {
        var executor = new CapturingExecutor(result: 0);
        var logger = new ListLogger<RunFileTransferBatchEntrypoint>();
        var entrypoint = CreateEntrypoint(executor, logger: logger, tenantSettings: new Dictionary<string, string?>());

        var result = entrypoint.RunManagementScheduled(Array.Empty<string>());

        Assert.Equal(-1, result);
        Assert.Contains(logger.Messages, m => m.StartsWith("Usage: FileTransferService <command> <command_value>"));
    }

    [Fact]
    public void F03_005_single_run_detail_is_tenant_id_and_must_parse_as_guid()
    {
        var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        Assert.True(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { tenantId.ToString("D") }, out var parsed, out var processingAllFiles));
        Assert.Equal(tenantId, parsed);
        Assert.True(processingAllFiles);

        Assert.False(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "not-a-guid" }, out _, out _));
    }

    [Fact]
    public void F03_006_three_run_details_require_int_or_guid_second_value_and_guid_tenant_third_value()
    {
        var tenantId = Guid.Parse("22222222-2222-2222-2222-222222222222");

        Assert.True(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "cmd", "12", tenantId.ToString("D") }, out var parsed, out var processingAllFiles));
        Assert.Equal(tenantId, parsed);
        Assert.False(processingAllFiles);

        var fileId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        Assert.True(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "cmd", fileId.ToString("D"), tenantId.ToString("D") }, out _, out _));

        Assert.False(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "cmd", "nope", tenantId.ToString("D") }, out _, out _));
        Assert.False(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "cmd", "12", "not-a-guid" }, out _, out _));
    }

    [Fact]
    public void F03_007_three_run_details_do_not_validate_first_value()
    {
        var tenantId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        Assert.True(RunFileTransferBatchValidation.TryValidateRunDetails(new[] { "not-validated", "1", tenantId.ToString("D") }, out _, out _));
    }

    [Fact]
    public void F03_008_management_scheduled_run_loads_tenant_settings_when_not_loaded()
    {
        var tenantId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var loadCount = 0;

        var executor = new CapturingExecutor(result: 0);
        var entrypoint = CreateEntrypoint(executor, tenantSettings: new Dictionary<string, string?>());

        entrypoint.OverrideTenantSettingsLoaderForTests(_ =>
        {
            loadCount++;
            return CreateAllMandatorySettings();
        });

        Assert.Equal(0, entrypoint.RunManagementScheduled(new[] { tenantId.ToString("D") }));
        Assert.Equal(1, loadCount);
    }

    [Fact]
    public void F03_009_missing_mandatory_setting_logs_key_and_throws_configuration_error()
    {
        var logger = new ListLogger<RunFileTransferBatchConfigurationValidation>();
        var validator = new RunFileTransferBatchConfigurationValidation(logger);

        var settings = CreateAllMandatorySettings();
        settings["WorkingDirectory"] = null;

        var ex = Assert.Throws<ConfigurationErrorsException>(() => validator.ValidateMandatorySettings(settings));
        Assert.Equal("WorkingDirectory is not specified in app.config", ex.Message);
        Assert.Contains(logger.Messages, m => m == "WorkingDirectory is not specified in app.config");
    }

    [Fact]
    public void F03_010_management_scheduled_run_executes_batch_with_run_details_unchanged()
    {
        var tenantId = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var executor = new CapturingExecutor(result: 77);
        var entrypoint = CreateEntrypoint(executor, tenantSettings: CreateAllMandatorySettings());

        var runDetails = new[] { tenantId.ToString("D") };
        var result = entrypoint.RunManagementScheduled(runDetails);

        Assert.Equal(77, result);
        Assert.Same(runDetails, executor.LastRunDetails);
    }

    [Fact]
    public void F03_011_management_scheduled_run_returns_negative_one_on_error()
    {
        var tenantId = Guid.Parse("77777777-7777-7777-7777-777777777777");
        var executor = new ThrowingExecutor(new Exception("kaboom"));
        var logger = new ListLogger<RunFileTransferBatchEntrypoint>();
        var entrypoint = CreateEntrypoint(executor, logger: logger, tenantSettings: CreateAllMandatorySettings());

        var result = entrypoint.RunManagementScheduled(new[] { tenantId.ToString("D") });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Messages, m => m.Contains("File Transfer Batch Process") && m.Contains("kaboom"));
    }

    private static RunFileTransferBatchEntrypoint CreateEntrypoint(
        IBatchExecutor<int> executor,
        ILogger<RunFileTransferBatchEntrypoint>? logger = null,
        IReadOnlyDictionary<string, string?>? tenantSettings = null)
    {
        logger ??= new ListLogger<RunFileTransferBatchEntrypoint>();

        var tenantContext = new TenantContext();

        var configurationValidation = new RunFileTransferBatchConfigurationValidation(
            new ListLogger<RunFileTransferBatchConfigurationValidation>());

        var entrypoint = new RunFileTransferBatchEntrypoint(
            (ILogger<RunFileTransferBatchEntrypoint>)logger,
            tenantContext,
            configurationValidation,
            executor,
            new ConfigurationBuilder().Build());

        entrypoint.OverrideTenantSettingsLoaderForTests(_ => tenantSettings ?? CreateAllMandatorySettings());

        return entrypoint;
    }

    private static Dictionary<string, string?> CreateAllMandatorySettings() => new(StringComparer.Ordinal)
    {
        ["ExternalFTECommonFolder"] = "x",
        ["WorkingDirectory"] = "x",
        ["MaxConcurrentDeliveryProcesses"] = "x",
        ["MaxFilesPerCycle"] = "x",
        ["MinsToWaitBeforeRetryFailedFile"] = "x",
        ["MinsToWaitBeforeRetrySuspendedConfig"] = "x",
        ["DisableNotifications"] = "x",
        ["EnvironmentName"] = "x",
        ["DeleteArchiveIfExists"] = "x",
        ["DuplicateFileExtension"] = "x",
        ["ActivateDuplicateRenameLogic"] = "x",
    };

    private sealed class CapturingExecutor : IBatchExecutor<int>
    {
        private readonly int _result;

        public CapturingExecutor(int result)
        {
            _result = result;
        }

        public string[]? LastRunDetails { get; private set; }

        public int Execute(string[] runDetails)
        {
            LastRunDetails = runDetails;
            return _result;
        }
    }

    private sealed class ThrowingExecutor : IBatchExecutor<int>
    {
        private readonly Exception _exception;

        public ThrowingExecutor(Exception exception)
        {
            _exception = exception;
        }

        public int Execute(string[] runDetails) => throw _exception;
    }

    private sealed class ListLogger<T> : ILogger<T>
    {
        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();
            public void Dispose() { }
        }

        public List<string> Messages { get; } = new();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Messages.Add(formatter(state, exception));
        }
    }
}
