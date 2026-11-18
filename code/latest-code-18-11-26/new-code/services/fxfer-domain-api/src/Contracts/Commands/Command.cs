using Fxfer.DomainApi.Contracts.Utilities;

namespace Fxfer.DomainApi.Contracts.Commands;

public abstract class Command
{
    public HPP.Core.BAS.CQRS.UserMeta.RequestorModel? Requestor { get; set; }
}
