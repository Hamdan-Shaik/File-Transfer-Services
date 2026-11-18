# Brief: port part 1 of 1 of component `shared-queries-parameters`

- Deployable: `fxfer-domain-api` at `services/fxfer-domain-api`; build: `dotnet build services/fxfer-domain-api`; tests: `dotnet test services/fxfer-domain-api`
- The component's folder (architecture.yaml): `services/fxfer-domain-api/src/Contracts/Queries`
- The component's spec: `technical/components/shared-queries-parameters/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 8 legacy methods or classes, 298 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `FileEventQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\FileEventQueryParms.cs`)

#### `FileEventQueryParms` (lines 15-19) → `FileEventQueryParms`

```csharp
    [DataContract]
    public class FileEventQueryParms : SerializableBase
    {  
        public string FileId { get; set; }
    }
```

### `FileExceptionQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\FileExceptionQueryParms.cs`)

#### `FileExceptionQueryParms` (lines 15-19) → `FileExceptionQueryParms`

```csharp
    [DataContract]
    public class FileExceptionQueryParms : SerializableBase
    {  
        public string FileId { get; set; }
    }
```

### `FileTransferTrackingQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\FileTransferTrackingQueryParms.cs`)

#### `FileTransferTrackingQueryParms` (lines 15-93) → `FileTransferTrackingQueryParms`

```csharp
    [DataContract]
    public class FileTransferTrackingQueryParms : SerializableBase
    {
        /// <summary>
        /// <para>Remarks: Unique Number of the particular File</para>
        /// </summary>
        [DataMember]
        public string FileId { get; set; }

        /// <summary>
        /// <para>File Name</para>
        /// <para> Remarks:The name of the file</para>
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// <para>File Status</para>
        /// <para> Remarks:The status of the file</para>
        /// </summary>
        [DataMember]
        public string FileStatus { get; set; }

        /// <summary>
        /// <para>File Direction</para>
        /// <para> Remarks:The direction of the File(s) e.g. viz. InBound/OutBound in which they are transferred</para>
        /// </summary>
        [DataMember]
        public string FileDirection { get; set; }

        /// <summary>
        /// <para>File Transport Type</para>
        /// <para> Remarks:The protocol used to transfer the file(s)</para>
        /// </summary>
        [DataMember]
        public string FileTransportType { get; set; }

        /// <summary>
        /// <para>File Source Location Id</para>
        /// <para> Remarks:The unique source location Id of the file(s)</para>
        /// </summary>
        [DataMember]
        public string FileSourceLocationId { get; set; }

        /// <summary>
        /// <para>Start Date</para>
        /// <para>Remarks: Timestamp of when the file transfer started</para>
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// <para>End Date</para>
        /// <para>Remarks: Timestamp of when the file transfer ended</para>
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// <para>File Size From</para>
        /// <para> Remarks:The size of the file(s) ranging from</para>
        /// </summary>
        [DataMember]
        public decimal? FileSizeFrom { get; set; }

        /// <summary>
        /// <para>File Size Through</para>
        /// <para> Remarks:The size of the file(s) ranging through</para>
        /// </summary>
        [DataMember]
        public decimal? FileSizeThrough { get; set; }

        /// <summary>
        /// <para>Trading Partner</para>
        /// <para> Remarks:The Trading Partner associated with the file(s)</para>
        /// </summary>
        [DataMember]
        public string TradingPartner { get; set; }
    }
```

### `FtpServerQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\FtpServerQueryParms.cs`)

#### `FtpServerQueryParms` (lines 17-80) → `FtpServerQueryParms`

```csharp
    [DataContract]
    public class FtpServerQueryParms : SerializableBase
    {
        #region Properties
        /// <summary>
        /// <para>Flag to identify if it is a valid FTP Server record.</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>Boolean</description></item>
        /// <item><term>Valid Values</term><description>true or false</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public bool? IsValid { get; set; }

        /// <summary>
        /// <para>The internal identifier for the FtpServer record.</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>Guid</description></item>  
        /// <item><term>MaxLength</term><description>36</description></item>
        /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public Guid FtpServerId { get; set; }

        /// <summary>
        /// <para>The unique Name assigned to FTP Server.</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>50/description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string FtpServerName { get; set; }

        /// <summary>
        /// <para>Brief description of the FTP Server.</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>100</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string FtpDescription { get; set; }

        /// <summary>
        /// <para>The FTP Server Username.</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>50</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string FtpUser { get; set; }

        [DataMember]
        public List<SearchModeOption> SearchOptions { get; set; }
        #endregion
    }
```

### `GetSourceLocationQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs`)

#### `GetSourceLocationQueryParms` (lines 15-37) → `GetSourceLocationQueryParms` · rules F01-101, F01-104

```csharp
    [DataContract]
   public class GetSourceLocationQueryParms : SerializableBase
    {
        /// <summary>
        /// <para>Gets or Sets the SourceLocation Id</para>
        /// </summary>
        [DataMember]
        public string SourceLocationId { get; set; }

        /// <summary>
        /// <para>Gets or Sets the IsActive</para>
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        public Guid SoureceLocationGuid
        {
            get
            {
                return Guid.Parse(this.SourceLocationId);
            }
        }
    }
```

### `InboundTargetQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\InboundTargetQueryParms.cs`)

#### `InboundTargetQueryParms` (lines 14-48) → `InboundTargetQueryParms` · rules F05-031

```csharp
    [DataContract]
    public class InboundTargetQueryParms
    {
        #region Constructor
        public InboundTargetQueryParms()
        {
            this.FTEMetadataId = Guid.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// <para>key identify an External File Definition record</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>Guid</description></item>  
        /// <item><term>MaxLength</term><description>36</description></item>
        /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public Guid FTEMetadataId { get; set; }

        /// <summary>
        /// <para>Contract Name</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>No</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>200</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string ContractName { get; set; }
        #endregion
    }
```

### `IntegrationFileQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\IntegrationFileQueryParms.cs`)

#### `IntegrationFileQueryParms` (lines 14-48) → `IntegrationFileQueryParms` · rules F03-006

```csharp
    [DataContract]
    public class IntegrationFileQueryParms
    {
        #region Constructor
        public IntegrationFileQueryParms()
        {
            this.FTSIntegrationFileId = Guid.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// <para>key that identify an Integration File record</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>Guid</description></item>  
        /// <item><term>MaxLength</term><description>36</description></item>
        /// <item><term>Valid Values</term><description>XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public Guid FTSIntegrationFileId { get; set; }

        /// <summary>
        /// <para>File Name</para>
        /// <list type="bullet">
        /// <item><term>Required</term><description>Yes</description></item>
        /// <item><term>Format</term><description>AlphaNumeric</description></item>  
        /// <item><term>MaxLength</term><description>200</description></item>
        /// </list>
        /// </summary>
        [DataMember]
        public string FileName { get; set; }
        #endregion
    }
```

### `SourceLocationQueryParms`: role **domain**, target file(s) none of its own (port it into the data layer that calls it, as TARGET.md says) (legacy `bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\SourceLocationQueryParms.cs`)

#### `SourceLocationQueryParms` (lines 15-66) → `SourceLocationQueryParms` · rules F01-095

```csharp
    [DataContract]
    public class SourceLocationQueryParms : SerializableBase
    {
        /// <summary>
        /// <para>Gets or Sets the SoureceLocation Id</para>
        /// </summary>
        public string SoureceLocationId { get; set; }

        /// <summary>
        /// <para>Gets or Sets the Config Id</para>
        /// </summary>
        [DataMember]
        public int? ConfigId { get; set; }

        /// <summary>
        /// <para>Gets or Sets the Config Name/para>
        /// </summary>
        [DataMember]
        public string ConfigName { get; set; }

        /// <summary>
        /// <para>Gets or Sets the Owner Name</para>
        /// </summary>
        [DataMember]
        public string OwnerName { get; set; }

        /// <summary>
        /// <para>Gets or Sets the Owner Subsystem</para>
        /// </summary>
        [DataMember]
        public string OwnerSubsystem { get; set; }

        /// <summary>
        /// <para>Gets the type of Protocol</para>
        /// </summary>
        [DataMember]
        public string Protocol { get; set; }

        /// <summary>
        /// <para>Gets or Sets the IsActive</para>
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; }

        public Guid OwnerModuleGuid
        {
            get
            {
                return Guid.Parse(this.OwnerSubsystem);
            }
        }
    }
```

## The rules these methods implement

**F01-095** When `OwnerSubsystem` is provided, searching source locations parses it as a GUID and filters to source locations whose owner module GUID equals it.
- Doors: file-transfer-svc-query-service-search-source-locations, file-transfer-svc-query-service-search-source-locations-2, file-transfer-svc-query-service-search-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\SourceLocationQueryExecutor.cs:49-51, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\SourceLocationQueryParms.cs:59-65
- Legacy: Uses `p.OwnerSubsystem == query.Where.OwnerModuleGuid`, where `OwnerModuleGuid` is computed as `Guid.Parse(OwnerSubsystem)`.
- Decision: keep
- ⚑ Flag: `OwnerSubsystem` must be a GUID string; if it is not parseable as a GUID, `Guid.Parse` throws during query construction.

**F01-101** The requested `SourceLocationId` is parsed as a GUID and used to look up `FtsSourceLocation` by id; the lookup returns a source location only when exactly one row matches.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:30-35, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:130-135
- Legacy: Parses the id via `Guid.Parse(SourceLocationId)` and queries `FtsSourceLocationId == query.Where.SoureceLocationGuid`, then returns `locations.First()` only when `locations.Count == 1`.
- Decision: keep
- ⚑ Flag: If `SourceLocationId` is not a GUID string, `Guid.Parse` throws.

**F01-104** The details query does not filter by requested `IsActive` and always returns the source location when the id matches, including the stored active flag.
- Doors: file-transfer-svc-query-service-get-source-locations, file-transfer-svc-query-service-get-source-locations-2, file-transfer-svc-query-service-get-source-locations-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\GetSourceLocationQueryParms.cs:24-28, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:31-34, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\QueryHandlers\GetSourceLocationQueryExecutor.cs:52-88
- Legacy: `ApplyWhereClause` returns the base query unchanged; the `IsActive` request parameter is not referenced, and the response model sets `IsActive = sourceLocation.IsValid`.
- Decision: keep

**F03-006** When a specific integration file id is supplied, only that exact definition is returned; when no id is supplied, the lookup does not filter by id.
- Doors: file-transfer-svc-query-service-get-integration-file, file-transfer-svc-query-service-get-integration-file-2
- Source: QEX-UA3:31-33, QPARMS-UA3:18-21
- Legacy: The default id is `Guid.Empty`, and the lookup applies an equality filter only when the supplied id is not empty.
- Decision: keep

**F05-031** `InboundTargetQuery` initializes its `Where` object and defaults `FTEMetadataId` to `Guid.Empty`.
- Doors: file-transfer-svc-query-service-search-inbound-target, file-transfer-svc-query-service-search-inbound-target-2, file-transfer-svc-query-service-search-inbound-target-3
- Source: bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\InboundTargetQuery.cs:27-30, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts\Queries\Parameters\InboundTargetQueryParms.cs:18-21
- Legacy: A new `InboundTargetQuery` always has a non-null `Where`, and `Where.FTEMetadataId` starts as `Guid.Empty`.
- Decision: keep
