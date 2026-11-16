using FxferService.Domain.Exceptions;
using Xunit;

namespace FxferService.Tests;

public sealed class FileTransferExceptionConstantsDomainRuleTests
{
    [Fact]
    public void F02_012_uses_expected_missing_source_location_validation_key()
    {
        Assert.Equal("FtpSoureceLocationId", Validation.FtpSoureceLocationId);
    }

    [Fact]
    public void F02_033_uses_expected_ftp_server_id_validation_key()
    {
        Assert.Equal("FtpServerId", Validation.FtpServerId);
    }

    [Fact]
    public void F02_035_uses_expected_config_id_sequence_name()
    {
        Assert.Equal("FTS_SOURCE_LOCATION_SEQ", Fields.FtsSourceLocationSeq);
        Assert.Equal("OperatorId", Validation.OperatorId);
    }

    [Fact]
    public void F02_048_uses_expected_ftp_server_id_validation_key()
    {
        Assert.Equal("FtpServerId", Validation.FtpServerId);
    }

    [Fact]
    public void F02_049_uses_expected_missing_config_and_source_location_validation_keys()
    {
        Assert.Equal("FtpSoureceLocationId", Validation.FtpSoureceLocationId);
        Assert.Equal("ConfigIdNotFound", Validation.ConfigIdNotFound);
    }

    [Fact]
    public void F02_050_uses_expected_is_suspended_changed_date_validation_key()
    {
        Assert.Equal("IsSuspendedChangedDate", Validation.IsSuspendedChangedDate);
    }

    [Fact]
    public void F02_052_uses_expected_invalidate_required_field_validation_keys()
    {
        Assert.Equal("OperatorId", Validation.OperatorId);
        Assert.Equal("FtpSoureceLocationId", Validation.FtpSoureceLocationId);
        Assert.Equal("ConfigIdNotFound", Validation.ConfigIdNotFound);
    }
}

