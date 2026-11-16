#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FxferService.Infrastructure.Persistence.Entities;
    #region Properties
    /// <summary>
    /// Table containing the File Transfer FTSSourceLocation
    /// <uol>
    /// <li><b>Table:</b>FTS_SOURCE_LOCATION</li>
    /// <li><b>References:</b> None</li>
    /// <li><b>Referenced By:</b> None</li>
    /// </uol>
    /// </summary>
    [Table("FTS_SOURCE_LOCATION")]
    [ExcludeFromCodeCoverage]
    public partial class FtsSourceLocation
    {
        /// <summary>
        /// System assigned GUID to uniquely identify a File Transfer location id
        /// <uol>
        /// <li><b>Column:</b> FTS_SOURCE_LOCATIONS_ID (PK)</li> 
        /// <li><b>Data Type:</b> Guid</li>
        /// <li><b>Unique Index:</b> FTS_LOCATIONS_PK</li>
        /// </uol>
        /// </summary>
        [Key]
        [Column("FTS_SOURCE_LOCATION_ID")]
        public Guid FtsSourceLocationId { get; set; }

        /// <summary>
        /// The Config id
        /// <uol>
        /// <li><b>Column:</b> CONFIG_ID </li>
        /// <li><b>Data Type:</b> Number</li>
        /// <li><b>Required:</b> No</li>
        /// </uol>
        /// </summary>

        [Column("CONFIG_ID")]
        public int Configid { get; set; }

        /// <summary>
        /// The Config Name
        /// <uol>
        /// <li><b>Column:</b> CONFIG_NAME </li>
        /// <li><b>Data Type:</b> string, StringLength(50)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("CONFIG_NAME")]
        [StringLength(50)]
        public string ConfigName { get; set; }

        /// <summary>
        /// The Source Protocal Code
        /// <uol>
        /// <li><b>Column:</b> SOURCE_PROTOCOL_CD </li>
        /// <li><b>Data Type:</b> string, StringLength(4)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("SOURCE_PROTOCOL_CD")]
        [StringLength(4)]
        public string SourceProtocalCode { get; set; }

        /// <summary>
        /// The Source Address
        /// <uol>
        /// <li><b>Column:</b> SOURCE_ADDRESS </li>
        /// <li><b>Data Type:</b> string, StringLength(100)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("SOURCE_ADDRESS")]
        [StringLength(248)]
        public string SourceAddress { get; set; }

        /// <summary>
        /// The Source File Mask
        /// <uol>
        /// <li><b>Column:</b> SOURCE_FILE_MASK </li>
        /// <li><b>Data Type:</b> string, StringLength(50)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("SOURCE_FILE_MASK")]
        [StringLength(50)]
        public string SourceFileMask { get; set; }

        /// <summary>
        /// The Ftp Server Indicator
        /// <uol>
        /// <li><b>Column:</b> FTP_SERVER_CD </li>
        /// <li><b>Data Type:</b> string, StringLength(50)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Column("FTP_SERVER_ID")]
        public Guid? FtpServerId { get; set; }

        /// <summary>
        /// The Send Notifications Indicator
        /// <uol>
        /// <li><b>Column:</b> SEND_NOTIFICATIONS_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("SEND_NOTIFICATIONS_IND")]
        public bool SendNotificationsIndicator { get; set; }

        /// <summary>
        /// The Owner Name
        /// <uol>
        /// <li><b>Column:</b> OWNER_NAME </li>
        /// <li><b>Data Type:</b> string, StringLength(50)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("OWNER_NAME")]
        [StringLength(50)]
        public string OwnerName { get; set; }

        /// <summary>
        /// The Owner Module
        /// <uol>
        /// <li><b>Column:</b> OWNER_MODULE </li>
        /// <li><b>Data Type:</b> string, StringLength(50)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("MODULE_ID")]
        public Guid OwnerModule { get; set; }

        /// <summary>
        /// The Owner Phone
        /// <uol>
        /// <li><b>Column:</b> OWNER_PHONE </li>
        /// <li><b>Data Type:</b> string, StringLength(10)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("OWNER_PHONE")]
        [StringLength(10)]
        public string OwnerPhone { get; set; }

        /// <summary>
        /// The Owner Email
        /// <uol>
        /// <li><b>Column:</b> OWNER_EMAIL </li>
        /// <li><b>Data Type:</b> string, StringLength(10)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("OWNER_EMAIL")]
        [StringLength(150)]
        public string OwnerEmail { get; set; }

        /// <summary>
        /// The Description
        /// <uol>
        /// <li><b>Column:</b> CONFIG_DESCRIPTION </li>
        /// <li><b>Data Type:</b> string, StringLength(200)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Column("CONFIG_DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }

        /// <summary>
        /// The RunOnMondayIndicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_MONDAY_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_MONDAY_IND")]
        public bool RunOnMondayIndicator { get; set; }

        /// <summary>
        /// The RunOnTuesday Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_TUESDAY_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_TUESDAY_IND")]
        public bool RunOnTuesdayIndicator { get; set; }

        /// <summary>
        /// The RunOnWed Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_WED_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_WED_IND")]
        public bool RunOnWedIndicator { get; set; }

        /// <summary>
        /// The RunOnThurs Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_THURS_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary> 
        [Column("RUN_ON_THURS_IND")]
        public bool RunOnThursIndicator { get; set; }

        /// <summary>
        /// The RunOnFri Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_FRI_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary> 
        [Column("RUN_ON_FRI_IND")]
        public bool RunOnFriIndicator { get; set; }

        /// <summary>
        /// The RunOnSat Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_SAT_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_SAT_IND")]
        public bool RunOnSatIndicator { get; set; }

        /// <summary>
        /// The RunOnSun Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_SUN_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_SUN_IND")]
        public bool RunOnSunIndicator { get; set; }

        /// <summary>
        /// The RunOnFrstDayOfMonth Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_FRST_DAY_OF_MONTH_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_FRST_DAY_OF_MONTH_IND")]
        public bool RunOnFrstDayOfMonthIndicator { get; set; }

        /// <summary>
        /// The RunOnLastDayOfMonth Indicator
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_LAST_DAY_OF_MONTH_IND </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_LAST_DAY_OF_MONTH_IND")]
        public bool RunOnLastDayOfMonthIndicator { get; set; }

        /// <summary>
        /// The RunOnThisDayOfMonth
        /// <uol>
        /// <li><b>Column:</b> RUN_ON_THIS_DAY_OF_MONTH </li>
        /// <li><b>Data Type:</b> string, StringLength(1)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Column("RUN_ON_THIS_DAY_OF_MONTH")]
        public byte RunOnThisDayOfMonth { get; set; }

        /// <summary>
        /// The StartTime24HHMM
        /// <uol>
        /// <li><b>Column:</b> START_TIME_24HHMM </li>
        /// <li><b>Data Type:</b> string, StringLength(4)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("START_TIME_24HHMM")]
        [StringLength(4)]
        public string StartTime24HHMM { get; set; }

        /// <summary>
        /// The EndTime24HHMM
        /// <uol>
        /// <li><b>Column:</b> END_TIME_24HHMM </li>
        /// <li><b>Data Type:</b> string, StringLength(4)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("END_TIME_24HHMM")]
        [StringLength(4)]
        public string EndTime24HHMM { get; set; }

        /// <summary>
        /// The FileDirection
        /// <uol>
        /// <li><b>Column:</b> FILE_DIRECTION </li>
        /// <li><b>Data Type:</b> string, StringLength(4)</li>
        /// <li><b>Required:</b>No</li>
        /// </uol>
        /// </summary>
        [Column("FILE_DIRECTION")]
        [StringLength(50)]
        public string FileDirection { get; set; }

        /// <summary>
        /// The TradingPartner
        /// <uol>
        /// <li><b>Column:</b> TP_ID </li>
        /// <li><b>Data Type:</b> string, StringLength(10)</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("TRADING_PARTNER")]
        [StringLength(10)]
        public string TradingPartner { get; set; }

        /// <summary>
        /// The IsValid
        /// <uol>
        /// <li><b>Column:</b> IS_VALID </li>
        /// <li><b>Data Type:</b> bool</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("IS_VALID")]
        public bool IsValid { get; set; }

        /// <summary>
        /// The IsSuspended
        /// <uol>
        /// <li><b>Column:</b> IS_SUSPENDED </li>
        /// <li><b>Data Type:</b> bool</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [Column("IS_SUSPENDED")]
        public bool IsSuspended { get; set; }

        /// <summary>
        /// The SuspendedIsChangedTs
        /// <uol>
        /// <li><b>Column:</b> SuspendedIsChangedTs </li>
        /// <li><b>Data Type:</b>DateTime</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Column("IS_SUSPENDED_CHANGED_TS")]
        public DateTime? IsSuspendedChangedDate { get; set; }

        /// <summary>
        /// The Operator ID
        /// <uol>
        /// <li><b>Column:</b> Operator ID </li>
        /// <li><b>Data Type:</b>String</li>
        /// <li><b>Required:</b>Yes</li>
        /// </uol>
        /// </summary>
        [Required]
        [StringLength(200)]
        [Column("OPERATOR_ID")]
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
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The creation date time stamp of the event record 
        /// <uol>
        /// <li><b>Column:</b> CREATION_TS</li>
        /// <li><b>Data Type:</b> DateTime</li>
        /// <li><b>Required:</b> Yes</li>
        /// </uol>
        /// </summary>
        [Column("CREATED_TS")]
        public DateTime CreationDate { get; set; }

        [ForeignKey("FtpServerId")]
        public virtual FtpServer FtpServer { get; set; }

        public virtual ICollection<FtsFileManagement> FtsFileManagementFK { get; set; }
        #endregion
    }
