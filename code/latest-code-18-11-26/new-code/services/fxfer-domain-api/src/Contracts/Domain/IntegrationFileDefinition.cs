namespace Fxfer.DomainApi.Contracts.Domain;

public sealed record IntegrationFileDefinition
{
    /// <summary>
    /// <para>FtsIntegrationFileId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>
    /// </summary>
    public required Guid FtsIntegrationFileId { get; init; }

    /// <summary>
    /// <para>GroupId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>
    /// </summary>
    public required Guid? GroupId { get; init; }

    /// <summary>
    /// <para>ConfigId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Numeric</description></item>
    /// <item><term>MaxLength</term><description>5</description></item>
    /// <item><term>Valid Values</term><description>-32,768 to 32,7672</description></item>
    /// </list>
    /// </summary>
    public required int? ConfigId { get; init; }

    /// <summary>
    /// <para>Name of the integration file </para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>
    /// <item><term>MaxLength</term><description>400</description></item>
    /// </list>
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// <para>ETFOutboundMetadataDefnId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>
    /// </summary>
    public required Guid ETFOutboundMetadataDefnId { get; init; }

    /// <summary>
    /// <para>Is Processed</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    public bool IsProcessed { get; init; }

    /// <summary>
    /// <para>Is Valid</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    public bool? IsValid { get; init; }

    /// <summary>
    /// <para>Date and time that the Integration File record was created</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>yyyy-MM-ddTHH.mm.ss.fffffffZ</description></item>
    /// </list>
    /// </summary>
    public DateTimeOffset? CreationDate { get; init; }

    /// <summary>
    /// <para>Date and time that the Integration File record was last modified</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>yyyy-MM-ddTHH.mm.ss.fffffffZ</description></item>
    /// </list>
    /// </summary>
    public DateTimeOffset? LastModifiedDate { get; init; }
}

