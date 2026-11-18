using Fxfer.DomainApi.Contracts.Exceptions;

namespace Fxfer.DomainApi.Tests.Contracts.Exceptions;

public sealed class FileTransferExceptionConstantsRuleTests
{
    [Fact]
    public void F01_046_source_location_sequence_name_constant_matches_legacy()
    {
        Assert.Equal("FTS_SOURCE_LOCATION_SEQ", FileTransferExceptionConstants.Fields.FtsSourceLocationSeq);
    }

    [Fact]
    public void F01_061_ftp_server_id_constant_matches_legacy()
    {
        Assert.Equal("FtpServerId", FileTransferExceptionConstants.Validation.FtpServerId);
    }

    [Fact]
    public void F01_064_config_id_not_found_constant_matches_legacy()
    {
        Assert.Equal("ConfigIdNotFound", FileTransferExceptionConstants.Validation.ConfigIdNotFound);
    }

    [Fact]
    public void F01_068_config_id_not_found_constant_matches_legacy()
    {
        Assert.Equal("ConfigIdNotFound", FileTransferExceptionConstants.Validation.ConfigIdNotFound);
    }

    [Fact]
    public void F01_069_business_validation_constants_match_legacy()
    {
        Assert.Equal("ConfigId", FileTransferExceptionConstants.Validation.ConfigId);
        Assert.Equal("ConfigIdNotFound", FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        Assert.Equal("FileTransfer", FileTransferExceptionConstants.Validation.ProcessingSystem);
        Assert.Equal("Business Error occurred processing the command.", FileTransferExceptionConstants.Validation.Message);
    }

    [Fact]
    public void F01_070_business_validation_constants_match_legacy()
    {
        Assert.Equal("ConfigId", FileTransferExceptionConstants.Validation.ConfigId);
        Assert.Equal("ConfigIdNotFound", FileTransferExceptionConstants.Validation.ConfigIdNotFound);
        Assert.Equal("FileTransfer", FileTransferExceptionConstants.Validation.ProcessingSystem);
        Assert.Equal("Business Error occurred processing the command.", FileTransferExceptionConstants.Validation.Message);
    }
}

