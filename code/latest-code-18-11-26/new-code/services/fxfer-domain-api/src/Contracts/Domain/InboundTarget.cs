namespace Fxfer.DomainApi.Contracts.Domain;

/// <summary>
/// <para>Object containing properties of an Inbound File.</para>
/// </summary>
public sealed record InboundTarget
{
    /// <summary>
    /// <para>FtsInboundTargetId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>
    /// </summary>
    public required Guid FtsInboundTargetId { get; init; }

    /// <summary>
    /// <para>FTEMetadataId</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Guid</description></item>
    /// <item><term>MaxLength</term><description>Guid</description></item>
    /// <item><term>Valid Values</term><description>Guid</description></item>
    /// </list>
    /// </summary>
    public required Guid FteMetadataId { get; init; }

    /// <summary>
    /// <para>Import Job Name</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    public required string ImportJobName { get; init; }

    /// <summary>
    /// <para>Contract Name</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>Text</description></item>
    /// <item><term>MaxLength</term><description>200</description></item>
    /// </list>
    /// </summary>
    public required string ContractName { get; init; }

    /// <summary>
    /// <para>IsValid</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>bool</description></item>
    /// <item><term>MaxLength</term><description>1</description></item>
    /// <item><term>Valid Values</term><description>true or false</description></item>
    /// </list>
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// <para>Date and time that the Inbound Target record was created</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>dd-MM-YY HH:MI:SS</description></item>
    /// </list>
    /// </summary>
    public DateTimeOffset CreationDate { get; init; }

    /// <summary>
    /// <para>Date and time that the Inbound Target record was last modified</para>
    /// <list type="bullet">
    /// <item><term>Required</term><description>Yes</description></item>
    /// <item><term>Format</term><description>DateTime</description></item>
    /// <item><term>Valid Values</term><description>dd-MM-YY HH:MI:SS</description></item>
    /// </list>
    /// </summary>
    public DateTimeOffset LastModifiedDate { get; init; }
}

