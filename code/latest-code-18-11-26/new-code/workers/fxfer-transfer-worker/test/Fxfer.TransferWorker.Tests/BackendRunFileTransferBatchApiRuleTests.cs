using System.Collections.Concurrent;
using Fxfer.TransferWorker.Application.Batch;
using Fxfer.TransferWorker.Application.Constants;
using Fxfer.TransferWorker.Domain.Batch;
using Fxfer.TransferWorker.Domain.Constants;
using Fxfer.TransferWorker.Worker;
using Fxfer.TransferWorker.Worker.Configuration;
using Fxfer.TransferWorker.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Tests;

public sealed class BackendRunFileTransferBatchApiRuleTests
{
    [Fact]
    public void T_007_rerun_mode_executes_for_specific_tenant_and_tracked_file()
    {
        var logger = new TestLogger<FileTransferBatchRerunCommand>();
        var executor = new TestBatchExecutor(5);
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchRerunCommand(logger, runtimeFactory);

        var tenantId = Guid.NewGuid();
        var trackedFileId = Guid.NewGuid();

        var result = sut.Run(tenantId, trackedFileId);

        Assert.Equal(5, result);
        Assert.Equal(1, runtime.InitializeCallCount);
        Assert.Equal(IocConstants.FileTransferExecutor, runtime.LastResolveKey);

        Assert.NotNull(executor.LastArgs);
        Assert.Equal(BatchJobConstants.CommandSakTrack, executor.LastArgs![0]);
        Assert.Equal(trackedFileId.ToString(), executor.LastArgs![1]);
        Assert.Equal(tenantId.ToString(), executor.LastArgs![2]);
    }

    [Fact]
    public void T_007_rerun_mode_rejects_empty_ids_returns_minus1_and_emits_usage_guidance()
    {
        var logger = new TestLogger<FileTransferBatchRerunCommand>();
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(1)));

        var sut = new FileTransferBatchRerunCommand(logger, runtimeFactory);

        using var error = new StringWriter();
        var original = Console.Error;
        Console.SetError(error);

        try
        {
            var result = sut.Run(Guid.Empty, Guid.NewGuid());

            Assert.Equal(-1, result);
            Assert.Contains("Usage:", error.ToString());
        }
        finally
        {
            Console.SetError(original);
        }
    }

    [Fact]
    public void T_007_rerun_mode_returns_minus1_and_records_failure_on_unexpected_exception()
    {
        var logger = new TestLogger<FileTransferBatchRerunCommand>();
        var executor = new TestBatchExecutor(new InvalidOperationException("boom"));
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchRerunCommand(logger, runtimeFactory);

        var result = sut.Run(Guid.NewGuid(), Guid.NewGuid());

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains(ExceptionConstants.BatchSource) && e.Message.Contains("boom"));
    }

    [Fact]
    public void F04_001_basic_runner_initializes_container_resolves_executor_and_executes_args()
    {
        var logger = new TestLogger<FileTransferBatchBasicRunner>();
        var executor = new TestBatchExecutor(42);
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchBasicRunner(logger, runtimeFactory);

        var args = new[] { "a", "b" };
        var result = sut.Run(args);

        Assert.Equal(42, result);
        Assert.Equal(1, runtime.InitializeCallCount);
        Assert.Same(args, executor.LastArgs);
    }

    [Fact]
    public void F04_002_basic_runner_on_unexpected_failure_records_message_under_source_and_returns_minus1()
    {
        var logger = new TestLogger<FileTransferBatchBasicRunner>();
        var executor = new TestBatchExecutor(new InvalidOperationException("boom"));
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchBasicRunner(logger, runtimeFactory);

        var result = sut.Run(new[] { "x" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains(ExceptionConstants.BatchSource) && e.Message.Contains("boom"));
    }

    [Fact]
    public void F04_003_managed_runner_logs_start_and_always_logs_end()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var appSettings = FakeTenantAppSettings.WithAllRequiredKeys();
        var settingsManager = new FakeTenantAppSettingsManager(appSettings);

        var executor = new TestBatchExecutor(new Exception("oops"));
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message == "Starting FileTransfer Batch Process");
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message == "Ending FileTransfer Batch Process");
    }

    [Fact]
    public void F04_004_managed_runner_returns_minus1_when_validation_fails_and_prints_usage()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var settingsManager = new FakeTenantAppSettingsManager(null);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(1)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "a", "b" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("Usage: FileTransferService <command> <command_value>"));
        Assert.Equal(0, runtimeFactory.CreateCallCount);
    }

    [Fact]
    public void F04_005_one_value_form_treats_value_as_tenant_and_processes_all_files()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(7);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(7, result);
        Assert.Equal(1, settingsManager.LoadCallCount);
        Assert.Equal(tenantId, settingsManager.LastTenantId);
    }

    [Fact]
    public void F04_006_one_value_form_requires_tenant_id_be_a_guid()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var settingsManager = new FakeTenantAppSettingsManager(null);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(1)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "not-a-guid" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("Usage: FileTransferService"));
        Assert.Equal(0, settingsManager.LoadCallCount);
    }

    [Fact]
    public void F04_007_three_value_form_treats_third_value_as_tenant_identifier()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(0);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "sakTrack", "1", tenantId });

        Assert.Equal(0, result);
        Assert.Equal(tenantId, settingsManager.LastTenantId);
    }

    [Fact]
    public void F04_008_three_value_form_requires_second_value_be_int_or_guid()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(1)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "sakTrack", "nope", tenantId });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("Usage: FileTransferService"));
        Assert.Equal(0, settingsManager.LoadCallCount);
    }

    [Fact]
    public void F04_009_three_value_form_requires_third_value_be_guid()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();

        var settingsManager = new FakeTenantAppSettingsManager(null);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(1)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "sakTrack", "1", "not-a-guid" });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Information && e.Message.Contains("Usage: FileTransferService"));
    }

    [Fact]
    public void F04_010_three_value_form_does_not_validate_first_value()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(5);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { "any-first-arg", Guid.NewGuid().ToString(), tenantId });

        Assert.Equal(5, result);
        Assert.Equal(tenantId, settingsManager.LastTenantId);
    }

    [Fact]
    public void F04_011_loads_settings_only_when_not_already_loaded()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(FakeTenantAppSettings.WithAllRequiredKeys());
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(0)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(0, result);
        Assert.Equal(0, settingsManager.LoadCallCount);
    }

    [Fact]
    public void F04_012_missing_required_setting_logs_error_and_run_ends_as_failure()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var incomplete = new FakeTenantAppSettings(new Dictionary<string, string?>
        {
            [FileTransferManagementAppSettingsConstants.ExternalFTECommonFolder] = "x"
        });

        var settingsManager = new FakeTenantAppSettingsManager(incomplete);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(new TestBatchExecutor(0)));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message == "WorkingDirectory is not specified in app.config");
    }

    [Fact]
    public void F04_013_managed_runner_initializes_container_resolves_FileTransferExecutor_and_executes()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(99);
        var runtime = new TestWorkerBatchRuntime(executor);
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(runtime);

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(99, result);
        Assert.Equal(1, runtime.InitializeCallCount);
        Assert.Equal(IocConstants.FileTransferExecutor, runtime.LastResolveKey);
    }

    [Fact]
    public void F04_014_on_unexpected_failure_managed_runner_logs_fatal_records_source_message_and_returns_minus1()
    {
        var logger = new TestLogger<FileTransferBatchManagedRunner>();
        var tenantId = Guid.NewGuid().ToString();

        var settingsManager = new FakeTenantAppSettingsManager(null)
        {
            Loader = _ => FakeTenantAppSettings.WithAllRequiredKeys()
        };

        var executor = new TestBatchExecutor(new Exception("bad"));
        var runtimeFactory = new TestWorkerBatchRuntimeFactory(new TestWorkerBatchRuntime(executor));

        var sut = new FileTransferBatchManagedRunner(logger, settingsManager, runtimeFactory);

        var result = sut.Run(new[] { tenantId });

        Assert.Equal(-1, result);
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Critical && e.Message == "Exception occurred: ");
        Assert.Contains(logger.Entries, e => e.Level == LogLevel.Error && e.Message.Contains(ExceptionConstants.BatchSource) && e.Message.Contains("bad"));
    }

    private sealed class TestLogger<T> : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = new();

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
            Entries.Add(new LogEntry(logLevel, formatter(state, exception), exception));
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

        public string[]? LastArgs { get; private set; }

        public int Execute(string[] args)
        {
            LastArgs = args;

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
    }

    private sealed class TestWorkerBatchRuntimeFactory : IWorkerBatchRuntimeFactory
    {
        private readonly IWorkerBatchRuntime _runtime;

        public TestWorkerBatchRuntimeFactory(IWorkerBatchRuntime runtime)
        {
            _runtime = runtime;
        }

        public int CreateCallCount { get; private set; }

        public IWorkerBatchRuntime Create()
        {
            CreateCallCount++;
            return _runtime;
        }
    }

    private sealed class TestWorkerBatchRuntime : IWorkerBatchRuntime
    {
        private readonly IBatchExecutor<int> _executor;

        public TestWorkerBatchRuntime(IBatchExecutor<int> executor)
        {
            _executor = executor;
            Services = new ServiceCollection().AddSingleton(new CorrelationIdContext()).BuildServiceProvider();
        }

        public int InitializeCallCount { get; private set; }

        public string? LastResolveKey { get; private set; }

        public void Initialize()
        {
            InitializeCallCount++;
        }

        public IServiceProvider Services { get; }

        public IBatchExecutor<int> ResolveBatchExecutor(string key)
        {
            LastResolveKey = key;
            return _executor;
        }

        public void Dispose()
        {
        }
    }
}
