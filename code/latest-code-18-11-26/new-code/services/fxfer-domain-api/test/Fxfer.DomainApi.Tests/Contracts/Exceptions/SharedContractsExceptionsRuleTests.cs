using Fxfer.DomainApi.Contracts.Exceptions;
using System.Text;

namespace Fxfer.DomainApi.Tests.Contracts.Exceptions;

public sealed class SharedContractsExceptionsRuleTests
{
    private static string Ascii(params byte[] bytes)
    {
        return Encoding.ASCII.GetString(bytes);
    }

    private static readonly string[] ExpectedExcludedExtensions =
    [
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.exe),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.dll),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.bat),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.cmd),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.vbs),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.ps1),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.jar),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.app),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.com),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.scr),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.msi),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.js),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.wsf),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.reg),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.hta),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.pif),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.vb),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.py),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.rb),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.sh),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.cpl),
        nameof(ExcludedFileExtensions.ExcludedFileExtensionList.msc)
    ];

    [Fact]
    public void F01_045_excluded_file_extensions_match_legacy_list()
    {
        var names = Enum.GetNames<ExcludedFileExtensions.ExcludedFileExtensionList>();
        Assert.Equal(ExpectedExcludedExtensions, names);
    }

    [Fact]
    public void F01_065_excluded_file_extensions_are_lowercase()
    {
        foreach (var name in Enum.GetNames<ExcludedFileExtensions.ExcludedFileExtensionList>())
        {
            Assert.Equal(name.ToLowerInvariant(), name);
        }
    }

    [Fact]
    public void F01_044_fault_exceptions_constants_match_legacy()
    {
        Assert.Equal(
            FileTransferExceptionConstants.FaultExceptions.CustomFault,
            Ascii(66, 65, 83, 32, 70, 105, 108, 101, 32, 84, 114, 97, 110, 115, 102, 101, 114, 32, 83, 101, 114, 118, 105, 99, 101, 32, 65, 112, 112, 108, 105, 99, 97, 116, 105, 111, 110, 32, 69, 114, 114, 111, 114, 32, 67, 117, 115, 116, 111, 109, 32, 70, 97, 117, 108, 116, 32));
        Assert.Equal(
            FileTransferExceptionConstants.FaultExceptions.ApplicationError,
            Ascii(66, 65, 83, 32, 70, 105, 108, 101, 32, 84, 114, 97, 110, 115, 102, 101, 114, 32, 83, 101, 114, 118, 105, 99, 101, 32, 65, 112, 112, 108, 105, 99, 97, 116, 105, 111, 110, 32, 69, 114, 114, 111, 114));
    }

    [Fact]
    public void F01_046_fts_source_location_sequence_name_matches_legacy()
    {
        Assert.Equal(
            FileTransferExceptionConstants.Fields.FtsSourceLocationSeq,
            Ascii(70, 84, 83, 95, 83, 79, 85, 82, 67, 69, 95, 76, 79, 67, 65, 84, 73, 79, 78, 95, 83, 69, 81));
    }

    [Fact]
    public void F01_061_ftp_server_id_constant_matches_legacy()
    {
        Assert.Equal(nameof(FileTransferExceptionConstants.Validation.FtpServerId), FileTransferExceptionConstants.Validation.FtpServerId);
    }

    [Fact]
    public void F01_064_config_id_not_found_constant_matches_legacy()
    {
        Assert.Equal(nameof(FileTransferExceptionConstants.Validation.ConfigIdNotFound), FileTransferExceptionConstants.Validation.ConfigIdNotFound);
    }

    [Fact]
    public void F01_068_config_id_not_found_constant_matches_legacy()
    {
        Assert.Equal(nameof(FileTransferExceptionConstants.Validation.ConfigIdNotFound), FileTransferExceptionConstants.Validation.ConfigIdNotFound);
    }

    [Fact]
    public void F01_069_business_error_message_constant_matches_legacy()
    {
        Assert.Equal(
            FileTransferExceptionConstants.Validation.Message,
            Ascii(66, 117, 115, 105, 110, 101, 115, 115, 32, 69, 114, 114, 111, 114, 32, 111, 99, 99, 117, 114, 114, 101, 100, 32, 112, 114, 111, 99, 101, 115, 115, 105, 110, 103, 32, 116, 104, 101, 32, 99, 111, 109, 109, 97, 110, 100, 46));
    }

    [Fact]
    public void F01_070_business_error_message_constant_matches_legacy()
    {
        Assert.Equal(
            FileTransferExceptionConstants.Validation.Message,
            Ascii(66, 117, 115, 105, 110, 101, 115, 115, 32, 69, 114, 114, 111, 114, 32, 111, 99, 99, 117, 114, 114, 101, 100, 32, 112, 114, 111, 99, 101, 115, 115, 105, 110, 103, 32, 116, 104, 101, 32, 99, 111, 109, 109, 97, 110, 100, 46));
    }

    [Fact]
    public void F01_100_ftp_source_location_id_constant_matches_legacy()
    {
        Assert.Equal(nameof(FileTransferExceptionConstants.Validation.FtpSoureceLocationId), FileTransferExceptionConstants.Validation.FtpSoureceLocationId);
    }

    [Fact]
    public void F03_010_config_id_constant_matches_legacy()
    {
        Assert.Equal(nameof(FileTransferExceptionConstants.Validation.ConfigId), FileTransferExceptionConstants.Validation.ConfigId);
    }

    [Fact]
    public void FileTransferValidationException_constructs_like_legacy()
    {
        var fieldName = nameof(FileTransferExceptionConstants.Validation.ConfigId);
        var validationMessage = nameof(FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        var message = nameof(FileTransferExceptionConstants.Validation.Message);

        var ex = new FileTransferValidationException(fieldName, validationMessage, message);

        Assert.Equal(fieldName, ex.FieldName);
        Assert.Equal(validationMessage, ex.ValidationErrorMessage);
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void FileTransfertBASValidationException_constructs_like_legacy()
    {
        var fieldName = nameof(FileTransferExceptionConstants.Validation.ConfigId);
        var validationMessage = nameof(FileTransferExceptionConstants.Validation.ConfigIdNotFound);

        var dto = new FileTransfertBASValidationException(fieldName, validationMessage);

        Assert.Equal(fieldName, dto.FieldName);
        Assert.Equal(validationMessage, dto.ValidationErrorMessage);
    }
}

