using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>Used to search from Inbound Target table</para>
/// </summary>
[DataContract]
[KnownType(typeof(FtsInboundTargetModel))]
[KnownType(typeof(InboundTargetQueryParms))]
public class InboundTargetQuery : Query<FtsInboundTargetModel>
{
    #region Properties
    private List<BusinessExceptionMessage>? messages;
    #endregion

    #region Constructors
    public InboundTargetQuery()
    {
        Where = new InboundTargetQueryParms();
    }
    #endregion

    #region Validations
    /// <summary>
    /// <para>Params to get the Inbound Target Query</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Object</description></item>
    /// <item><term>Valid Values</term><description><see cref=InboundTargetQueryParms</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public InboundTargetQueryParms Where { get; set; }

    public void Validate()
    {
        messages = new List<BusinessExceptionMessage>();
        HandleErrors();
    }
    #endregion

    #region Private Methods
    private void HandleErrors()
    {
        if (messages!.Count > 0)
        {
            throw new BusinessException(
                messages!,
                CommonConstants.Validation.ErrorType.InvalidParameters,
                CommonConstants.Validation.ProcessingSystem);
        }
    }
    #endregion
}

