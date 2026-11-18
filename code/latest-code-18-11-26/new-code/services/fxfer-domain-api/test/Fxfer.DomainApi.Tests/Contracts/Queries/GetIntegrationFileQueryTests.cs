using Fxfer.DomainApi.Contracts.Queries;

namespace Fxfer.DomainApi.Tests.Contracts.Queries;

public sealed class GetIntegrationFileQueryTests
{
    [Fact]
    public void constructor_initializes_Where()
    {
        var query = new GetIntegrationFileQuery();

        Assert.NotNull(query.Where);
    }

    [Fact]
    public void Validate_does_not_throw()
    {
        var query = new GetIntegrationFileQuery();

        query.Validate();
    }
}
