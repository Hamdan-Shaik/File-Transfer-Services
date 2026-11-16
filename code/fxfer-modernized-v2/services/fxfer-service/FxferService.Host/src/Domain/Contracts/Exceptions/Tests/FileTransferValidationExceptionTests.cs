using FxferService.Application.LegacyContracts;
using FxferService.Domain.Contracts.Exceptions;
using Xunit;

namespace FxferService.Tests;

public sealed class FileTransferValidationExceptionTests
{
    [Fact]
    public void GenerateFault_returns_a_fault_exception_with_expected_reason_message()
    {
        var exception = new FileTransferValidationException("Field", "ValidationMessage");

        FaultException fault = exception.GenerateFault();

        Assert.Equal("ValidationMessage", fault.Message);
    }
}

