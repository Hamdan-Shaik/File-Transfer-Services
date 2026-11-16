---
type: Behavior Specification
title: 'backend-file-transfer-mgmt-data-access: file transfer management data access'
description: 'What the backend-file-transfer-mgmt-data-access component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-file-transfer-mgmt-data-access
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T19:58:48Z
sources:
- id: inventory
  resource: /technical/components/backend-file-transfer-mgmt-data-access/INVENTORY.yaml
  title: Every legacy class and method (18 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-file-transfer-mgmt-data-access
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 18
  methods: 54
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: ef040dcb3e06b79ee121db0e793c07b347724ff94c21e11c9206c01c08888a21
---

# backend-file-transfer-mgmt-data-access: file transfer management data access

This component is the legacy Entity Framework 6 data-access layer for file-transfer management/tracking.
It is called by batch/business code to read tracking/configuration state and to write tracking outcomes.

Implementation uses `FileTransferEntityDbContext` created from the `DefaultConnection` connection string.

## CreateErrorLogDataAccess
Creates a file exception/error-log record.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one `FTS_FILE_EXCEPTION` record for a file.
- Inputs: `errorLogDetail` (FileId, ErrorType, ErrorText, MethodName, StackTrace, OperatorId).
- Logic: truncates strings via `SetLimitedValue()`, creates `FtsFileException`, adds it, calls `SaveChanges()`.
- Data: `FTS_FILE_EXCEPTION` (write).
- Errors: EF/database exceptions propagate.

### SetLimitedValue()
- Purpose: Enforce DB max-lengths for exception log fields.
- Logic: Truncates ErrorType, ErrorText, MethodName, StackTrace via `Truncate(value, GetMaxLength(propertyName))`.

### Truncate(string value, int maxLength)
- Purpose: Truncate a string to a maximum length.
- Logic: returns value unchanged when null/empty; otherwise returns `value[..maxLength]` when longer.

### GetMaxLength(string name)
- Purpose: Read max length constraints from EF entity attributes.
- Logic: reads `StringLengthAttribute.MaximumLength` on `FtsFileException.<name>`; returns 0 when absent.

## CreateFileManagementDataAccess
Creates a file-management tracking record and returns the created file id.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one `FTS_FILE_MANAGEMENT` record and return its FileId.
- Inputs: `requestModel` (SourceLocationId, FileName, FileSize, ChecksumCode, OperatorId, GroupId).
- Logic: builds `FtsFileManagement` via `CreateFileManagementEntityData`, adds it, calls `SaveChanges()`, returns `FileManagementResponseModel.FileId`.
- Data: `FTS_FILE_MANAGEMENT` (write).
- Errors: EF/database exceptions propagate.

### CreateFileManagementEntityData(IDbContextBase dbcontext)
- Purpose: Build the EF entity for the new tracking record.
- Logic: creates `FileId = Guid.NewGuid()`, sets `FileStatus = FileStatusConstant.Transfering` (`3`), sets timestamps to `DateTime.UtcNow`.

## CreateFtsFileDeliveryDataAccess
Records one delivery for a file to a destination, and records related file events.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one `FTS_FILE_DELIVERY` row, then insert one or more `FTS_FILE_EVENT` rows.
- Inputs: fileId, destinationId, destinationFileName, operatorId, destinations.
- Logic:
  - Adds `FtsFileDelivery` (DeliveryId new Guid; DeliveryTs/CreatedDate/LastModifiedTs set to `DateTime.UtcNow`).
  - Calls `AddFileArchievedOrTransferredEvent(dbcontext)` and stores its return value in `deliveryresult`.
  - Calls `SaveChanges()`.
  - Returns `deliveryresult` (event id, or `Guid.Empty` when no destination match).
- Data: `FTS_FILE_DELIVERY` (write), `FTS_FILE_EVENT` (write).

### AddFileArchievedOrTransferredEvent(IDbContextBase dbcontext)
- Purpose: Record rename + archive/transfer events based on `DestinationModel` flags.
- Inputs: destinations.
- Logic:
  - Filters `destinations` to those with `DestinationId == destinationId`.
  - For the first match (method returns inside the loop):
    - If `IsRenameAtDestination` is true, inserts a `Renamed` event (return value ignored).
    - If `IsArchive` is true, inserts an `Archieved` event and returns its EventId.
    - Else inserts a `Transferred` event and returns its EventId.
  - If there is no match, returns `Guid.Empty`.

### UpdateFileEventData(ref IDbContextBase dbcontext, ref FtsFileEvent ftsEvent, string eventCode)
- Purpose: Insert one `FTS_FILE_EVENT` row.
- Logic: sets EventId = new Guid, EventCD = eventCode, EventTrackDescription = destinationFileName, timestamps = `DateTime.UtcNow`, adds to EF set, returns EventId.

## CreateFtsFileExceptionEventDataAccess
Records an exception event for a file/destination, then writes an exception log row.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one exception `FTS_FILE_EVENT` row, then one `FTS_FILE_EXCEPTION` row.
- Inputs: fileId, destinationId, destinationFileName, operatorId, errorLogDetail.
- Logic:
  - Calls `FtsFileExceptionEvent(dbcontext)` and stores returned EventId in `deliveryresult`.
  - Sets `errorLogDetail.EventId = deliveryresult`.
  - Calls `AddErrorLog(dbcontext)`.
  - Calls `SaveChanges()`.
  - Returns `deliveryresult`.
- Data: `FTS_FILE_EVENT` (write), `FTS_FILE_EXCEPTION` (write).

### FtsFileExceptionEvent(IDbContextBase dbcontext)
- Purpose: Insert an exception event row.
- Logic: inserts `FtsFileEvent` with `EventCD = FileDeliveryEventConstants.Exception`, timestamps = `DateTime.UtcNow`, returns EventId.

### AddErrorLog(IDbContextBase dbcontext)
- Purpose: Insert one exception log row tied to the exception event.
- Logic: truncates strings via `SetLimitedValue()`, inserts `FtsFileException` (ExceptionId new Guid created in constructor), returns `exceptionId`.

### SetLimitedValue()
- Purpose: Enforce DB max-lengths for exception log fields.
- Logic: Same truncation strategy as `CreateErrorLogDataAccess.SetLimitedValue()`.

### Truncate(string value, int maxLength)
- Purpose: Truncate a string to a maximum length.
- Logic: Same as `CreateErrorLogDataAccess.Truncate()`.

### GetMaxLength(string name)
- Purpose: Read max length constraints from EF entity attributes.
- Logic: Same as `CreateErrorLogDataAccess.GetMaxLength()`.

## DataAccessBase<TdacOutput>
Abstract base class that creates the EF database session/context and delegates to `ExecuteQuery`.

### Execute()
- Purpose: Execute the data-access operation inside a DB transaction.
- Logic:
  - Creates an `IDbSession` via `CreateDbSession()`.
  - Begins a transaction (`dbsession.BeginTransaction()`).
  - Creates a DB context via `CreateDbContext(dbsession)`.
  - Calls `ExecuteQuery(dbcontext)`.

### ExecuteWithNoTransaction()
- Purpose: Execute the data-access operation without starting a transaction.
- Logic: Creates `IDbSession` and `IDbContextBase`, then calls `ExecuteQuery(dbcontext)`.

### CreateDbSession()
- Purpose: Create a DB session using the `DefaultConnection` connection string.
- Logic: Uses `ConfigurationManager.ConnectionStrings[DefaultConnection]` when present; otherwise uses `ApplicationConfigurationManager.AppSettings` keys `DefaultConnectionProvider` and `DefaultConnectionString`.

### CreateDbContext(IDbSession dbsession)
- Purpose: Create the EF context for the file transfer schema.
- Logic: Returns `new FileTransferEntityDbContext(dbsession, true)`.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Abstract operation implemented by each concrete data-access class.
- Logic: Implemented by subclasses.

## DataAccessFactory
Concrete factory for creating data-access objects.

### CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
- Purpose: Create `FtsSuspensionDataAccess`.

### CreateGetPendingFileListDataAccess()
- Purpose: Create `GetPendingFileListDataAccess`.

### CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
- Purpose: Create `QueryFtsNotificationDataAccess`.

### CreateGetSourceLocationsDataAccess()
- Purpose: Create `GetSourceLocationsDataAccess`.

### CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
- Purpose: Create `CreateErrorLogDataAccess`.

### CreateGetConfigMetaDataDataAccess(Guid fileId)
- Purpose: Create `GetConfigMetaDataDataAccess`.

### CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
- Purpose: Create `UpdateFileManagementStatusDataAccess`.

### CreateUpdateSourceSuspendStatusDataAccess(FtsSourceLocation ftsFile)
- Purpose: Create `UpdateSourceSuspendStatusDataAccess`.

### GetSuspendedLocation(Guid sourceLocationId)
- Purpose: Create `GetSuspendedLocation`.

### CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations)
- Purpose: Create `CreateFtsFileDeliveryDataAccess`.

### CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
- Purpose: Create `CreateFileManagementDataAccess`.

### CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog)
- Purpose: Create `CreateFtsFileExceptionEventDataAccess`.

## FtsSuspensionDataAccess
Suspends a source location and records a suspension diagnostic row.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert `FTS_SUSPENSION` and set the source location suspension flag.
- Inputs: query (SourceLocationId, ErrorText, StackTrace, OperatorId).
- Logic:
  - Loads `FtsSourceLocation` where `FtsSoureceLocationId == query.SourceLocationId`.
  - If found: inserts `FtsSuspension` (via `CreateSourceLocationEntityData`), sets `IsSuspended = true`, sets `IsSuspendedChangedDate = DateTime.UtcNow`, saves, returns `suspensionModel.SuspensionId`.
  - If not found: returns `FtsSuspensionModel` with default (empty) id.
- Data: `FTS_SOURCE_LOCATION` (read+write), `FTS_SUSPENSION` (write).

### CreateSourceLocationEntityData(IDbContextBase dbcontext)
- Purpose: Build the EF entity for the new suspension record.
- Logic: creates `FtsSuspension` with `SuspensionId = Guid.NewGuid()` and timestamps = `DateTime.UtcNow`; `StackTrace` is truncated via `LimitLength(query.StackTrace, 1000)`.

### LimitLength(string source, int maxLength)
- Purpose: Truncate suspension stack trace to 1000 characters.
- Logic: returns `source` unchanged when `source.Length <= maxLength`; otherwise `source.Substring(0, maxLength)` (throws when `source` is null).

## GetConfigMetaDataDataAccess
Loads a source location and its destinations for a given file id, excluding already-delivered non-archive destinations.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return `SourceLocationModel` for the file's source location, with destinations filtered for pending delivery.
- Inputs: fileId.
- Logic:
  - Calls `SetSoureceLocationId(dbcontext)` to obtain the file's source location id from `FTS_FILE_MANAGEMENT`.
  - Loads `FtsSourceLocation` (no tracking).
  - Maps core fields to `SourceLocationModel` and `FtpServerModel` (from navigation `sourceLocation.FtpServer` when `FtpServerId` is non-empty).
  - Calls `GetDestinations(dbcontext)` and appends them to `source.Destinations`.
  - Returns null when the source location cannot be found.
- Data: `FTS_FILE_MANAGEMENT` (read), `FTS_SOURCE_LOCATION` (read), `FTS_FILE_DELIVERY` (read), `FTS_DESTINATION_LOCATIONS` (read), `FTP_SERVER` (read).

### SetSoureceLocationId(IDbContextBase dbcontext)
- Purpose: Set the internal `soureceLocationId` for the given file.
- Logic: loads `FtsFileManagement` by file id; when found sets `soureceLocationId = FileSourceLocationId`.

### GetDestinations(IDbContextBase dbcontext)
- Purpose: Load and filter destination configurations for the file's source location.
- Logic:
  - Loads `deliveredDestinationIds` from `FTS_FILE_DELIVERY` for the file.
  - Loads destination-location rows where `IsValid == true` and (destination not already delivered OR destination is archive).
  - Maps each row to `DestinationModel`; loads `FtpServer` details when `FtpServerId != Guid.Empty`.

## GetPendingFileListDataAccess
Lists pending (or very recently completed) files.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return `PendingFileModel` list from `FTS_FILE_MANAGEMENT` based on status and recent completion.
- Logic: includes rows where `FileStatus != Completed` OR (`FileStatus == Completed` AND `FileStatusTs > (UtcNow - 1 minute)`), projecting to `PendingFileModel`.
- Data: `FTS_FILE_MANAGEMENT` (read).

## GetSourceLocationsDataAccess
Loads all source locations and maps them to `SourceLocationModel` including schedule parsing.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return a list of all source locations.
- Logic: loads all `FtsSourceLocation` (no tracking) and maps each through `GetSourceLocationData`.
- Data: `FTS_SOURCE_LOCATION` (read), `FTP_SERVER` (read via navigation property).

### GetSourceLocationData(FtsSourceLocation sourceLocData, FtpServer ftpServer)
- Purpose: Build one `SourceLocationModel` including schedule + credentials.
- Logic: builds base model via `GetSourceLocationModel`, then applies weekly/monthly scheduling and start/end time parsing, then populates FTP fields via `UpdateFtpServerData`.

### GetSourceLocationModel(FtsSourceLocation entitySourceData)
- Purpose: Map source location fields to the legacy contract model.
- Logic: maps identifiers/owner fields/validity/suspension flags and initializes Schedule lists.

### UpdateWeeklyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Build the weekly run-days list.
- Logic: adds each `DayOfWeek` value when its indicator is true.

### UpdateMonthlyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Build the monthly run-days list.
- Logic: adds day 1 when RunOnFrstDayOfMonthIndicator, adds last day of current month when RunOnLastDayOfMonthIndicator, and adds `Convert.ToInt32(RunOnThisDayOfMonth)` when `Convert.ToBoolean(RunOnThisDayOfMonth)` is false.

### UpdateScheduleStartTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Parse StartTime24HHMM into a `TimeSpan`.
- Logic: parses HH/MM when string length > 3 and both parts parse; otherwise uses 00:00.

### UpdateScheduleEndTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Parse EndTime24HHMM into a `TimeSpan`.
- Logic: same parsing rules as `UpdateScheduleStartTime`.

### UpdateFtpServerData(ref SourceLocationModel sourceModel, FtpServer entityFtpServerData, string protocol)
- Purpose: Populate FTP credential fields when protocol is not UNC.
- Logic: when protocol != `ProtocolCodeConstant.UNC`, sets FtpServerIp/FtpUser/FtpPassword/FtpPort/SSHAuth.

## GetSuspendedLocation
Loads one source location entity by id.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Load one `FtsSourceLocation`.
- Logic: returns `SingleOrDefault(s => s.FtsSoureceLocationId == sourceLocationId)`.
- Data: `FTS_SOURCE_LOCATION` (read).

## QueryFtsNotificationDataAccess
Loads notification recipients for a configuration id based on the requested notify flags.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return the `FtsNotificationModel` list matching the notify flags.
- Logic: routes to `NotifyErrorSuccess`, `NotifyErrorInd`, or `NotifyOnSuccessInd` based on the two flags.
- Data: `FTS_NOTIFICATION` (read).

### NotifyErrorSuccess(IDbContextBase dbcontext)
- Purpose: Return recipients configured for both success and error notifications.

### NotifyOnSuccessInd(IDbContextBase dbcontext)
- Purpose: Return recipients configured for success notifications.

### NotifyErrorInd(IDbContextBase dbcontext)
- Purpose: Return recipients configured for error notifications.

## UpdateFileManagementStatusDataAccess
Updates a file status and timestamps.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Update `FTS_FILE_MANAGEMENT` status fields for a file id.
- Logic: when row exists, sets FileStatus + FileStatusTs + LastModifiedTs to `DateTime.UtcNow` and saves.
- Data: `FTS_FILE_MANAGEMENT` (read+write).

## UpdateSourceSuspendStatusDataAccess
Clears a source location suspended status.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Unsuspend a source location entity already attached to the context.
- Logic: when `ftsSourceLocation` is non-null, sets `IsSuspended = false`, sets change date, saves, returns `SaveChanges() > 0`.
- Data: `FTS_SOURCE_LOCATION` (write).

## IDataAccess<TdacOutput>
Generic interface implemented by each data-access class.

## IDataAccessFactory
Factory interface used by batch/business code to obtain data-access objects.

## FileStatusConstant
Defines status codes used in `FTS_FILE_MANAGEMENT.FileStatus`.

- Completed = `1`
- Error = `2`
- Transfering = `3`

Also defines `UNCProtocolCD = 3`.

## ProtocolCodeConstant
Defines protocol codes.

- FTP = `1`
- SFTP = `2`
- UNC = `3`
- FTPS = `4`

## Rules implemented

None.

## Not carried over

None.

<!-- END -->
