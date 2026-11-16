#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FxferService.Infrastructure.Persistence.Entities;

/// <summary>
/// Table containing the File Transfer FtsSuspension
/// <uol>
/// <li><b>Table:</b>FTS_SUSPENSION</li> 
/// <li><b>References:</b> None</li> 
/// <li><b>Referenced By:</b> None</li> 
/// </uol>
/// </summary>
[Table("FTS_SUSPENSION")]
[ExcludeFromCodeCoverage]
public partial class FtsSuspension
{
    /// <summary>
    /// System assigned GUID to uniquely identify a Suspension id
    /// <uol>
    /// <li><b>Column:</b> SUSPENSION_ID (PK)</li> 
    /// <li><b>Data Type:</b> Guid</li>        
    /// <li><b>Unique Index:</b> FTS_SUSPENSION_PK</li>
    /// </uol>
    /// </summary>
    [Key]
    [Column("SUSPENSION_ID")]
    public Guid SuspensionId { get; set; }

    /// <summary>
    /// System assigned GUID to uniquely identify a File Transfer source location id
    /// <uol>
    /// <li><b>Column:</b> FTS_SOURCE_LOCATION_ID (FK)</li> 
    /// <li><b>Data Type:</b> Guid</li>        
    /// <li><b>Unique Index:</b> FTS_SUSPENSION_FK0</li>
    /// </uol>
    /// </summary>
    [Column("FTS_SOURCE_LOCATION_ID")]
    public Guid FtsSourceLocationId { get; set; }

    /// <summary>
    /// The Error Text
    /// <uol>
    /// <li><b>Column:</b> ERROR_TEXT </li> 
    /// <li><b>Data Type:</b> string, StringLength(250)</li>
    /// <li><b>Required:</b>Yes</li>
    /// </uol>
    /// </summary> 
    [Required]
    [Column("ERROR_TEXT")]
    [StringLength(250)]
    public string ErrorText { get; set; }

    /// <summary>
    /// The Stack Trace
    /// <uol>
    /// <li><b>Column:</b> STATCK_TRACE </li> 
    /// <li><b>Data Type:</b> string, StringLength(1000)</li>
    /// <li><b>Required:</b>Yes</li>
    /// </uol>
    /// </summary> 
    [Required]
    [Column("STACK_TRACE")]
    [StringLength(1000)]
    public string StackTrace { get; set; }

    /// <summary>
    /// The Id of the operator of user who inserted or updated records
    /// <uol>
    /// <li><b>Column:</b> OPERATOR_ID</li> 
    /// <li><b>Data Type:</b> string, StringLength(200)</li>
    /// <li><b>Required:</b> Yes</li>
    /// </uol>
    /// </summary> 
    [Required]
    [Column("OPERATOR_ID")]
    [StringLength(200)]
    public string OperatorId { get; set; }

    /// <summary>
    /// The last modified timestamp of the event record, DB default to UTC systimestamp
    /// <uol>
    /// <li><b>Column:</b> LAST_MODIFIED_TS</li> 
    /// <li><b>Data Type:</b> DateTime</li>
    /// <li><b>Required:</b> Yes</li>
    /// </uol>
    /// </summary>
    [Column("LAST_MODIFIED_TS")]
    public DateTime LastModifiedTs { get; set; }

    /// <summary>
    /// The created date time stamp of the event record 
    /// <uol>
    /// <li><b>Column:</b> CREATED_TS</li> 
    /// <li><b>Data Type:</b> DateTime</li>
    /// <li><b>Required:</b> Yes</li>
    /// </uol>
    /// </summary> 
    [Column("CREATED_TS")]
    public DateTime CreatedTs { get; set; }

    public virtual FtsSourceLocation FtsSourceLocationFK { get; set; }
}