using System.Collections.Concurrent;
using Fxfer.TransferWorker.Application.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Tests.Specs;

public sealed class RunFileTransferBatchFlowTests
{
    private static readonly Guid ValidTenantId = Guid.Parse("00000000-0000-0000-0000-0000000000a1");

    [Fact]
    public void A_basic_batch_run_starts_the_file_transfer_work()
    {
        var logger = new TestLogger<FileTransferBatchBasicRunner>();
        var executor = new TestBatchExecutor(0);
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchBasicRunner(logger, runtimeFactory);

        var result = sut.Run(new[] { "any" });

        Assert.Equal(0, result);
        Assert.True(executor.WasCalled);
        Assert.Equal(IocConstants.FileTransferExecutor, runtime.LastResolveKey);
    }

    [Fact]
    public void A_basic_batch_run_reports_failure_as_minus1_when_it_cannot_start()
    {
        var logger = new TestLogger<FileTransferBatchBasicRunner>();
        var executor = new TestBatchExecutor(new InvalidOperationException("boom"));
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchBasicRunner(logger, runtimeFactory);

        var result = sut.Run(new[] { "any" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains(ExceptionConstants.BatchSource, StringComparison.Ordinal));
    }

    [Fact]
    public void The_managed_run_always_records_an_end_record()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(new InvalidOperationException("boom"));
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { ValidTenantId.ToString("D") });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("Ending FileTransfer Batch Process", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    public void The_managed_run_rejects_startup_values_that_do_not_match_an_accepted_pattern(int count)
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var settingsManager = new FakeTenantAppSettingsManager(null);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(0)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var args = Enumerable.Repeat("x", count).ToArray();
        var result = sut.Run(args);

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Message.Contains("Usage:", StringComparison.Ordinal));
        Assert.False(runtimeFactory.WasCreated);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-0000000000a1", true)]
    [InlineData("not-a-guid", false)]
    public void A_one_value_managed_run_processes_all_files_for_one_tenant(string tenant, bool accepted)
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenant });

        if (accepted)
        {
            Assert.Equal(0, result);
            Assert.True(executor.WasCalled);
        }
        else
        {
            Assert.Equal(-1, result);
            Assert.Contains(logger.Entries, e => e.Message.Contains("Usage:", StringComparison.Ordinal));
            Assert.False(executor.WasCalled);
        }
    }

    [Theory]
    [InlineData("any label", "10", "00000000-0000-0000-0000-0000000000a1", true)]
    [InlineData("any label", "00000000-0000-0000-0000-0000000000b1", "00000000-0000-0000-0000-0000000000a1", true)]
    [InlineData("any label", "not a number and not a long reference", "00000000-0000-0000-0000-0000000000a1", false)]
    [InlineData("any label", "10", "not-a-guid", false)]
    public void A_three_value_managed_run_checks_the_chosen_value_and_the_tenant_reference(string label, string chosen, string tenant, bool accepted)
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { label, chosen, tenant });

        if (accepted)
        {
            Assert.Equal(0, result);
            Assert.True(executor.WasCalled);
        }
        else
        {
            Assert.Equal(-1, result);
            Assert.Contains(logger.Entries, e => e.Message.Contains("Usage:", StringComparison.Ordinal));
            Assert.False(executor.WasCalled);
        }
    }

    [Fact]
    public void A_three_value_managed_run_does_not_reject_an_unrecognised_label()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "made-up-option", "10", ValidTenantId.ToString("D") });

        Assert.Equal(0, result);
        Assert.True(executor.WasCalled);
    }

    [Fact]
    public void The_managed_run_makes_tenant_settings_available_when_they_are_not_already_available()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var tenantId = ValidTenantId.ToString("D");
        var result = sut.Run(new[] { tenantId });

        Assert.Equal(0, result);
        Assert.True(executor.WasCalled);
        Assert.Equal(1, settingsManager.LoadCallCount);
        Assert.Equal(tenantId, settingsManager.LastTenantId);
    }

    [Theory]
    [InlineData("where files are picked up", FileTransferManagementAppSettingsConstants.ExternalFTECommonFolder)]
    [InlineData("where working files are placed", FileTransferManagementAppSettingsConstants.WorkingDirectory)]
    public void The_managed_run_stops_when_a_required_run_setting_is_missing(string missingArea, string missingKey)
    {
        _ = missingArea;

        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeysExcept(missingKey);
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { ValidTenantId.ToString("D") });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains($"{missingKey} is not specified in app.config", StringComparison.Ordinal));
        Assert.False(executor.WasCalled);
    }

    [Fact]
    public void The_managed_run_starts_the_file_transfer_work_after_checks_pass()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(0);
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { ValidTenantId.ToString("D") });

        Assert.Equal(0, result);
        Assert.True(executor.WasCalled);
        Assert.Equal(IocConstants.FileTransferExecutor, runtime.LastResolveKey);
    }

    private sealed class TestBatchExecutor : IBatchExecutor<int>
    {
        private readonly int? _result;
        private readonly Exception? _exception;

        public TestBatchExecutor(int result)
        {
            _result = result;
        }

        public TestBatchExecutor(Exception exception)
        {
            _exception = exception;
        }

        public bool WasCalled { get; private set; }

        public int Execute(string[] args)
        {
            WasCalled = true;

            if (_exception is not null)
            {
                throw _exception;
            }

            return _result.GetValueOrDefault();
        }
    }

    private sealed class FakeTenantAppSettingsManager : ITenantAppSettingsManager
    {
        public FakeTenantAppSettingsManager(ITenantAppSettings? initial)
        {
            AppSettings = initial;
        }

        public int LoadCallCount { get; private set; }

        public string? LastTenantId { get; private set; }

        public Func<string, ITenantAppSettings>? Loader { get; init; }

        public ITenantAppSettings? AppSettings { get; private set; }

        public void LoadApplicationConfiguration(string tenantId)
        {
            LoadCallCount++;
            LastTenantId = tenantId;

            AppSettings = Loader is null ? AppSettings : Loader(tenantId);
        }
    }

    private sealed class FakeTenantAppSettings : ITenantAppSettings
    {
        private readonly IReadOnlyDictionary<string, string?> _values;

        public FakeTenantAppSettings(IReadOnlyDictionary<string, string?> values)
        {
            _values = values;
        }

        public string? this[string key] => _values.TryGetValue(key, out var value) ? value : null;

        public static FakeTenantAppSettings WithAllRequiredKeys()
        {
            return new FakeTenantAppSettings(new Dictionary<string, string?>
            {
                [FileTransferManagementAppSettingsConstants.ExternalFTECommonFolder] = "x",
                [FileTransferManagementAppSettingsConstants.WorkingDirectory] = "x",
                [FileTransferManagementAppSettingsConstants.MaxConcurrentDeliveryProcesses] = "x",
                [FileTransferManagementAppSettingsConstants.MaxFilesPerCycle] = "x",
                [FileTransferManagementAppSettingsConstants.MinsToWaitBeforeRetryFailedFile] = "x",
                [FileTransferManagementAppSettingsConstants.MinsToWaitBeforeRetrySuspendedConfig] = "x",
                [FileTransferManagementAppSettingsConstants.DisableNotifications] = "x",
                [FileTransferManagementAppSettingsConstants.EnvironmentName] = "x",
                [FileTransferManagementAppSettingsConstants.DeleteArchiveIfExists] = "x",
                [FileTransferManagementAppSettingsConstants.DuplicateFileExtension] = "x",
                [FileTransferManagementAppSettingsConstants.ActivateDuplicateRenameLogic] = "x"
            });
        }

        public static FakeTenantAppSettings WithAllRequiredKeysExcept(string missingKey)
        {
            var values = new Dictionary<string, string?>(WithAllRequiredKeys()._values);
            values.Remove(missingKey);
            return new FakeTenantAppSettings(values);
        }
    }

    private sealed class TestWorkerBatchRuntimeFactory : IWorkerBatchRuntimeFactory
    {
        private readonly IWorkerBatchRuntime _runtime;

        public TestWorkerBatchRuntimeFactory(IWorkerBatchRuntime runtime)
        {
            _runtime = runtime;
        }

        public bool WasCreated { get; private set; }

        public IWorkerBatchRuntime Create()
        {
            WasCreated = true;
            return _runtime;
        }
    }

    private sealed class TestWorkerBatchRuntime : IWorkerBatchRuntime
    {
        private readonly IBatchExecutor<int> _executor;

        public TestWorkerBatchRuntime(IBatchExecutor<int> executor)
        {
            _executor = executor;

            Services = new ServiceCollection()
                .AddSingleton(new CorrelationIdContext())
                .AddSingleton(new TenantIdContext())
                .BuildServiceProvider();
        }

        public string? LastResolveKey { get; private set; }

        public IServiceProvider Services { get; }

        public void Initialize()
        {
        }

        public IBatchExecutor<int> ResolveBatchExecutor(string key)
        {
            LastResolveKey = key;
            return _executor;
        }

        public void Dispose()
        {
        }
    }

    private sealed class TestLogger<T> : ILogger<T>
    {
        private readonly ConcurrentQueue<LogEntry> _entries = new();

        public IReadOnlyCollection<LogEntry> Entries => _entries.ToArray();

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            _entries.Enqueue(new LogEntry(logLevel, formatter(state, exception), exception));
        }

        public sealed record LogEntry(LogLevel Level, string Message, Exception? Exception);

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
