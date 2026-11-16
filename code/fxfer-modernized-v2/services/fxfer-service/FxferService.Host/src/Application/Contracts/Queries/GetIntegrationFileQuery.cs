using System.Runtime.Serialization;

using FxferService.Domain.Contracts.Queries.Parameters;
using FxferService.Domain.Constants;
using FxferService.Domain.Contracts.Domain;
using FxferService.Domain.Contracts.Exceptions;

namespace FxferService.Application.Contracts.Queries;

/// <summary>
/// <para>Used to search from Integration File table</para>
/// </summary>
[DataContract]
[KnownType(typeof(FtsIntegrationFileModel))]
[KnownType(typeof(IntegrationFileQueryParms))]
public class GetIntegrationFileQuery : Query<FtsIntegrationFileModel>
{
    #region Properties
    private List<BusinessExceptionMessage>? messages;
    #endregion

    #region Constructors
    public GetIntegrationFileQuery()
    {
        Where = new IntegrationFileQueryParms();
    }
    #endregion

    #region Validations
    /// <summary>
    /// <para>Params to get the Integration File Query</para>
    /// <list type=bullet>
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Object</description></item>
    /// <item><term>Valid Values</term><description><see cref=IntegrationFileQueryParms</description></item>
    /// </list>
    /// </summary>
    [DataMember]
    public IntegrationFileQueryParms Where { get; set; }

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

