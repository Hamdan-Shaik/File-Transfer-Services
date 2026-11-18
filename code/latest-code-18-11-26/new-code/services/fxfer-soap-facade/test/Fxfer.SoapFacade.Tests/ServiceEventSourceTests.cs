using Fxfer.SoapFacade.Host.Logging;
using Microsoft.Extensions.Logging;

namespace Fxfer.SoapFacade.Tests;

public sealed class ServiceEventSourceTests
{
    [Fact]
    public void Message_WhenLoggingDisabled_DoesNotFormat()
    {
        var logger = new ListLogger<ServiceEventSource>(isEnabled: false);
        var serviceEventSource = new ServiceEventSource(logger);

        serviceEventSource.Message("{0", 123);

        Assert.Empty(logger.Messages);
    }

    [Fact]
    public void Message_WhenLoggingEnabled_FormatsAndWrites()
    {
        var logger = new ListLogger<ServiceEventSource>(isEnabled: true);
        var serviceEventSource = new ServiceEventSource(logger);

        serviceEventSource.Message("Hello {0}", "World");

        Assert.Single(logger.Messages);
        Assert.Equal("Hello World", logger.Messages.Single());
    }

    [Fact]
    public void ServiceRequestStart_WhenLoggingEnabled_UsesLegacyMessageText()
    {
        var logger = new ListLogger<ServiceEventSource>(isEnabled: true);
        var serviceEventSource = new ServiceEventSource(logger);

        serviceEventSource.ServiceRequestStart("MyRequest");

        Assert.Single(logger.Messages);
        Assert.Equal("Service request 'MyRequest' started", logger.Messages.Single());
    }

    private sealed class ListLogger<T> : ILogger<T>
    {
        private readonly bool _isEnabled;
        public List<string> Messages { get; } = new();

        public ListLogger(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel) => _isEnabled;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Messages.Add(formatter(state, exception));
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}

