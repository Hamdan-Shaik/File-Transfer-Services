---
type: Behavior Specification
title: technical/components/backend-data-access-batch-data-access-2/SPEC
description: 'What the backend-data-access-batch-data-access-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/backend-data-access-batch-data-access-2
tags:
- backend
- component
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T18:55:48Z
sources:
- id: inventory
  resource: /technical/components/backend-data-access-batch-data-access-2/INVENTORY.yaml
  title: Every legacy class and method (16 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:backend-data-access-batch-data-access-2
  generation:
    status: done
    attempts: 1
  tier: backend
  classes: 16
  methods: 47
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-transfer-worker
  folder: src/Infrastructure/DataAccess
  body_sha256: e2934fa485d6fe693d1ece52409fa6bac08c2455e70479d9563b06ee116d9825
---

﻿# backend-data-access-batch-data-access-2: batch data-access for FXFER tracking/config

This component is the legacy batch-oriented data-access layer for file-transfer processing. It is called by batch/business code to read configuration + pending work, and to write transfer tracking outcomes.

Implementation is Entity Framework Core against the legacy file-transfer database via FileTransferEntityDbContext created from the TenantConfig connection string.

## CreateErrorLogDataAccess
Creates a file exception/error-log record.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one FTS_FILE_EXCEPTION record for a file.
- Inputs: errorLogDetail (FileId, ErrorType, ErrorText, MethodName, StackTrace, OperatorId).
- Logic:
  - Calls SetLimitedValue() to truncate strings to DB column max lengths.
  - Creates FtsFileException with:
    - ExceptionId = Guid.NewGuid() (created in constructor)
    - FileId = errorLogDetail.FileId
    - ExceptionType = errorLogDetail.ErrorType
    - Message = errorLogDetail.ErrorText
    - MethodName = errorLogDetail.MethodName
    - StackTrace = errorLogDetail.StackTrace
    - CreatedDate = DateTime.UtcNow
    - LastModifiedTs = DateTime.UtcNow
    - OperatorId = errorLogDetail.OperatorId
  - Adds to EF set and calls SaveChanges().
- Data: FTS_FILE_EXCEPTION (write).
- Errors: Any EF/database exception propagates to the caller.
- Rules: none.

### SetLimitedValue()
- Purpose: Enforce DB max-lengths for exception log fields.
- Inputs: errorLogDetail fields.
- Logic:
  - Truncates ErrorType, ErrorText, MethodName, StackTrace via Truncate(value, GetMaxLength(propertyName)).
  - Max lengths come from StringLength attributes on FtsFileException properties.
- Data: none.
- Errors: Reflection failures propagate.
- Rules: none.

### Truncate(string value, int maxLength)
- Purpose: Truncate a string to a maximum length.
- Inputs: value, maxLength.
- Logic:
  - If value is null/empty, returns it unchanged.
  - Else returns value if value.Length <= maxLength, otherwise value.Substring(0, maxLength).
- Data: none.
- Errors: If maxLength is negative, Substring throws.
- Rules: none.

### GetMaxLength(string name)
- Purpose: Read max length constraints from the EF entity type.
- Inputs: name (property name on FtsFileException).
- Logic:
  - Uses reflection to read StringLengthAttribute.MaximumLength for the named property.
  - Returns 0 if attribute not found.
- Data: none.
- Errors: Reflection exceptions propagate.
- Rules: none.

## CreateFileManagementDataAccess
Creates a file-management tracking record and returns the created file id.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert one FTS_FILE_MANAGEMENT record and return its FileId.
- Inputs: requestModel (ChecksumCode, FileName, FileSize, SourceLocationId, OperatorId).
- Logic:
  - Calls CreateFileManagementEntityData(dbcontext) to build a new FtsFileManagement.
  - Adds the entity and calls SaveChanges().
  - Returns FileManagementResponseModel with FileId set to the created entity FileId.
- Data: FTS_FILE_MANAGEMENT (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

### CreateFileManagementEntityData(IDbContextBase dbcontext)
- Purpose: Build the EF entity for the new tracking record.
- Inputs: requestModel.
- Logic:
  - Creates fileId = Guid.NewGuid().
  - Populates ChecksumCD, CreatedDate, FileId, FileName, FileSize, FileSourceLocationId, FileStatus, LastModifiedTs, OperatorId.
  - FileStatus is set to DataAccessConstants.FileStatusConstant.Transfering (value 3).
- Data: none.
- Errors: none explicitly.
- Rules: none.

## CreateFtsFileDeliveryDataAccess
Records one delivery for a file to a destination, and records related file events.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Insert a delivery record and related file-event records.
- Inputs: fileId, destinationId, destinationFileName, operatorId.
- Logic:
  - Creates FtsFileDelivery with DeliveryId = Guid.NewGuid(), FileId, DestinationId, DestinationFileName, DeliveryTs = DateTime.UtcNow, OperatorId, CreatedDate = DateTime.UtcNow.
  - Adds and saves the delivery.
  - Calls AddDestinationFileRenamedEvent(dbcontext).
  - Calls AddFileArchievedOrTransferredEvent(dbcontext).
  - Returns the DeliveryId.
- Data: FTS_FILE_DELIVERY (write), FTS_FILE_MANAGEMENT (read), FTS_DESTINATION (read), FTS_FILE_EVENT (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

### AddDestinationFileRenamedEvent(IDbContextBase dbcontext)
- Purpose: Record a Renamed event when destination filename differs from the source filename.
- Inputs: fileId, destinationFileName.
- Logic:
  - Queries FtsFileManagement rows with FileId == fileId.
  - For each returned row, if destinationFileName != fileManagement.FileName, calls UpdateFileEventData(..., FileDeliveryEventConstants.Renamed).
- Data: FTS_FILE_MANAGEMENT (read), FTS_FILE_EVENT (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

### AddFileArchievedOrTransferredEvent(IDbContextBase dbcontext)
- Purpose: Record an Archieved or Transferred event based on the destination configuration.
- Inputs: destinationId.
- Logic:
  - Queries FtsDestinationLocations rows where DestinationId == destinationId.
  - For each returned row:
    - If DestIsArchive is true, records FileDeliveryEventConstants.Archieved.
    - Else records FileDeliveryEventConstants.Transferred.
  - Event recording is done by calling UpdateFileEventData.
- Data: FTS_DESTINATION (read), FTS_FILE_EVENT (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

### UpdateFileEventData(ref IDbContextBase dbcontext, ref FtsFileEvent ftsEvent, string eventCode)
- Purpose: Populate and persist a file-event record.
- Inputs: eventCode, plus the current fileId, destinationId, operatorId.
- Logic:
  - Sets EventId = Guid.NewGuid(), FileId, EventCD = eventCode, DestinationId, OperatorId, CreatedDate = DateTime.UtcNow.
  - Adds ftsEvent to the EF set and calls SaveChanges().
- Data: FTS_FILE_EVENT (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

## DataAccessBase<TdacOutput>
Abstracts data-access execution by creating a DB session + context and then executing the query.

### Execute()
- Purpose: Run a data-access operation inside a DB session/transaction.
- Inputs: none.
- Logic:
  - Creates an IDbSession via CreateDbSession() in a using scope.
  - Begins a transaction via dbsession.BeginTransaction() in a using scope.
  - Creates a context via CreateDbContext(dbsession).
  - Calls ExecuteQuery(dbcontext) and returns its value.
  - Transaction commit/rollback behavior is not explicit in this method; it relies on IDbTransaction disposal semantics.
- Data: none directly (delegated to ExecuteQuery).
- Errors: Any exceptions propagate.
- Rules: none.

### CreateDbSession()
- Purpose: Open a DB session using the configured connection string.
- Inputs: none.
- Logic:
  - Reads connection string settings from ApplicationConfigurationManager.Configuration.ConnectionStrings() under key TenantConfig.
  - Copies the connection string and provider name into a System.Configuration.ConnectionStringSettings.
  - Calls DbSession.Create(settings).
- Data: none.
- Errors: Missing configuration / connection errors propagate (including null dereference if TenantConfig is not present).
- Rules: none.

### CreateDbContext(IDbSession dbsession)
- Purpose: Create the EF context wrapper for file transfer entities.
- Inputs: dbsession.
- Logic: Returns new FileTransferEntityDbContext(dbsession, true).
- Data: none.
- Errors: Context construction exceptions propagate.
- Rules: none.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Template method implemented by derived classes.
- Inputs: dbcontext.
- Logic: abstract.
- Data: n/a.
- Errors: n/a.
- Rules: none.

## DataAccessFactory
Provides factory methods for creating the specific data-access objects used by batch processing.

### CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
- Purpose: Construct a FtsSuspensionDataAccess.
- Inputs: searchQuery.
- Logic: Returns new FtsSuspensionDataAccess(searchQuery).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateGetPendingFileListDataAccess()
- Purpose: Construct a GetPendingFileListDataAccess.
- Inputs: none.
- Logic: Returns new GetPendingFileListDataAccess().
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
- Purpose: Construct a QueryFtsNotificationDataAccess.
- Inputs: searchQuery.
- Logic: Returns new QueryFtsNotificationDataAccess(searchQuery).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateGetSourceLocationsDataAccess()
- Purpose: Construct a GetSourceLocationsDataAccess.
- Inputs: none.
- Logic: Returns new GetSourceLocationsDataAccess().
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
- Purpose: Construct a CreateErrorLogDataAccess.
- Inputs: errorLog.
- Logic: Returns new CreateErrorLogDataAccess(errorLog).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateGetConfigMetaDataDataAccess(Guid fileId)
- Purpose: Construct a GetConfigMetaDataDataAccess.
- Inputs: fileId.
- Logic: Returns new GetConfigMetaDataDataAccess(fileId).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
- Purpose: Construct an UpdateFileManagementStatusDataAccess.
- Inputs: fileId, fileStatus.
- Logic: Returns new UpdateFileManagementStatusDataAccess(fileId, fileStatus).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateUpdateSourceSuspendStatusDataAccess(Guid sourceLocationId)
- Purpose: Construct an UpdateSourceSuspendStatusDataAccess.
- Inputs: sourceLocationId.
- Logic: Returns new UpdateSourceSuspendStatusDataAccess(sourceLocationId).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId)
- Purpose: Construct a CreateFtsFileDeliveryDataAccess.
- Inputs: fileId, destinationId, destinationFileName, operatorId.
- Logic: Returns new CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, operatorId).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
- Purpose: Construct a CreateFileManagementDataAccess.
- Inputs: requestModel.
- Logic: Returns new CreateFileManagementDataAccess(requestModel).
- Data: none.
- Errors: none explicitly.
- Rules: none.

## FtsSuspensionDataAccess
Marks a source location as suspended and records a suspension entry.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Suspend a source location and persist a suspension record.
- Inputs: query (SourceLocationId, ErrorText, StackTrace).
- Logic:
  - Loads FtsSourceLocation by FtsSourceLocationId == query.SourceLocationId.
  - If found, calls UpdateSourceLocation(...) to set suspended flags and saves changes.
  - Creates a new FtsSuspension via CreateSourceLocationEntityData().
  - Persists it via AddEntitySourceLocation(...).
  - Returns FtsSuspensionModel with SuspensionId set to the created entity SuspensionId.
- Data: FTS_SOURCE_LOCATION (read+write), FTS_SUSPENSION (write).
- Errors: Any EF/database exception propagates.
- Rules: none.

### GetSourceLocation(IDbContextBase dbcontext)
- Purpose: Load the source location entity.
- Inputs: query.SourceLocationId.
- Logic: FirstOrDefault on FtsSourceLocation.
- Data: FTS_SOURCE_LOCATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

### UpdateSourceLocation(IDbContextBase dbcontext, FtsSourceLocation sourceLocations)
- Purpose: Mark a source location as suspended.
- Inputs: sourceLocations entity.
- Logic:
  - Sets IsSuspended = true.
  - Sets IsSuspendedChangedDate = DateTime.UtcNow.
  - Calls SaveChanges().
- Data: FTS_SOURCE_LOCATION (write).
- Errors: EF/database exceptions propagate.
- Rules: none.

### CreateSourceLocationEntityData()
- Purpose: Build a new suspension entity.
- Inputs: query.
- Logic: Creates FtsSuspension with SuspensionId = Guid.NewGuid(), FtsSourceLocationId, ErrorText, StackTrace.
- Data: none.
- Errors: none explicitly.
- Rules: none.

### AddEntitySourceLocation(IDbContextBase dbcontext, FtsSuspension entitySourceLocation)
- Purpose: Persist the suspension entity.
- Inputs: entitySourceLocation.
- Logic: Adds to set and calls SaveChanges().
- Data: FTS_SUSPENSION (write).
- Errors: EF/database exceptions propagate.
- Rules: none.

## GetConfigMetaDataDataAccess
Loads the source location configuration (including destinations) for a given file id.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return SourceLocationModel containing source + destination configuration for a file.
- Inputs: fileId.
- Logic:
  - Calls SetSoureceLocationId(dbcontext) to resolve soureceLocationId from file management.
  - Loads FtsSourceLocation by FtsSourceLocationId == soureceLocationId using AsNoTracking().
  - If not found, returns null.
  - Determines ftpServer:
    - If sourceLocation.FtpServerId is not Guid.Empty and not null, uses sourceLocation.FtpServer navigation.
    - Else uses a new FtpServer instance.
  - Builds SourceLocationModel with basic fields + FtpServerDetail (includes FtpPassword).
  - Calls GetDestinations(dbcontext) and appends to source.Destinations.
- Data: FTS_FILE_MANAGEMENT (read), FTS_SOURCE_LOCATION (read), FTP_SERVER (read), FTS_FILE_DELIVERY (read), FTS_DESTINATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

### SetSoureceLocationId(IDbContextBase dbcontext)
- Purpose: Resolve source location id for the file.
- Inputs: fileId.
- Logic:
  - Loads FtsFileManagement via SingleOrDefault(f => f.FileId == fileId).
  - If found, sets soureceLocationId = fileManagment.FileSourceLocationId.
- Data: FTS_FILE_MANAGEMENT (read).
- Errors: If multiple rows exist for FileId, SingleOrDefault throws.
- Rules: none.

### GetDestinations(IDbContextBase dbcontext)
- Purpose: Load destination configurations for the resolved source location, excluding already-delivered destinations except archive destinations.
- Inputs: fileId, soureceLocationId.
- Logic:
  - Reads already delivered destination ids for the file from FtsFileDelivery.
  - Queries FtsDestinationLocations for rows where:
    - FtsSourceLocationId == soureceLocationId
    - IsValid is true
    - (DestinationId not in deliveredDestinationIds OR DestIsArchive is true)
  - Maps each result row into a DestinationModel including encryption fields.
  - If item.FtpServerId is not Guid.Empty, loads corresponding FtpServer and maps to destination.FtpServerDetail (includes FtpPassword).
- Data: FTS_FILE_DELIVERY (read), FTS_DESTINATION (read), FTP_SERVER (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

## GetPendingFileListDataAccess
Reads the list of pending (or very recently completed) files.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return a list of PendingFileModel to be processed.
- Inputs: none.
- Logic:
  - Sets currentDate = DateTime.UtcNow.AddMinutes(-1).
  - Selects rows from FtsFileManagement with:
    - FileStatus != Completed (case-insensitive string compare), OR
    - FileStatus == Completed AND FileStatusTs > currentDate.
  - Maps each selected row to PendingFileModel with FileId, FileName, FileSourceLocationId, FileStatus, FileTrackId = SakFileTrack, StatusDate = FileStatusTs.
- Data: FTS_FILE_MANAGEMENT (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

## GetSourceLocationsDataAccess
Loads all defined source locations and maps scheduling configuration.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return all SourceLocationModel entries.
- Inputs: none.
- Logic:
  - Loads all FtsSourceLocation rows via AsNoTracking().ToList().
  - For each row:
    - Picks ftpServer = data.FtpServer when data.FtpServerId is not null, otherwise a new FtpServer.
    - Calls GetSourceLocationData(data, ftpServer) to map the record.
- Data: FTS_SOURCE_LOCATION (read), FTP_SERVER (read via navigation).
- Errors: EF/database exceptions propagate.
- Rules: none.

### GetSourceLocationData(FtsSourceLocation sourceLocData, FtpServer ftpServer)
- Purpose: Build a fully populated SourceLocationModel.
- Inputs: sourceLocData, ftpServer.
- Logic:
  - Calls GetSourceLocationModel(sourceLocData) to map base fields.
  - Calls:
    - UpdateWeeklyScheduleData(ref model, sourceLocData)
    - UpdateMonthlyScheduleData(ref model, sourceLocData)
    - UpdateScheduleStartTime(ref model, sourceLocData)
    - UpdateScheduleEndTime(ref model, sourceLocData)
    - UpdateFtpServerData(ref model, ftpServer, sourceLocData.SourceProtocalCode)
- Data: none.
- Errors: Any exceptions propagate.
- Rules: none.

### GetSourceLocationModel(FtsSourceLocation entitySourceData)
- Purpose: Map a source location entity to a model with initialized child objects.
- Inputs: entitySourceData.
- Logic:
  - Initializes FtpServerDetail and Schedule (DaysToRun list; MonthlyProcessDayToRun list).
  - Maps SourceLocationId, ConfigurationId, SourceProtocolCode, SourceAddress, FileMask, SendNotificationsIndicator, IsActive, IsSuspended.
  - Sets SuspendedIndChanged to IsSuspendedChangedDate when present, otherwise DateTime.MinValue.
- Data: none.
- Errors: none explicitly.
- Rules: none.

### UpdateWeeklyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Build the weekly run-days list from boolean flags.
- Inputs: RunOnMondayIndicator, RunOnTuesdayIndicator, RunOnWedIndicator, RunOnThursIndicator, RunOnFriIndicator, RunOnSatIndicator, RunOnSunIndicator.
- Logic: Adds the corresponding DayOfWeek value for each true indicator.
- Data: none.
- Errors: none explicitly.
- Rules: none.

### UpdateMonthlyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Build the monthly run-days list.
- Inputs: RunOnFrstDayOfMonthIndicator, RunOnLastDayOfMonthIndicator, RunOnThisDayOfMonth.
- Logic:
  - If RunOnFrstDayOfMonthIndicator is true, adds 1.
  - If RunOnLastDayOfMonthIndicator is true, adds DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month).
  - If Convert.ToBoolean(RunOnThisDayOfMonth) is false (byte is 0), adds Convert.ToInt32(RunOnThisDayOfMonth) (adds 0).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### UpdateScheduleStartTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Parse the configured start time.
- Inputs: StartTime24HHMM.
- Logic:
  - If non-empty, length > 3, and HH/MM parse as ints, sets Schedule.StartTime = new TimeSpan(hours, minutes, 0).
  - Otherwise sets Schedule.StartTime = new TimeSpan(0, 0, 0).
- Data: none.
- Errors: none explicitly.
- Rules: none.

### UpdateScheduleEndTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
- Purpose: Parse the configured end time.
- Inputs: EndTime24HHMM.
- Logic: Same parsing rules as UpdateScheduleStartTime, setting Schedule.EndTime.
- Data: none.
- Errors: none explicitly.
- Rules: none.

### UpdateFtpServerData(ref SourceLocationModel sourceModel, FtpServer entityFtpServerData, string protocol)
- Purpose: Populate FTP server credentials onto the model when the protocol is not UNC.
- Inputs: protocol.
- Logic:
  - If protocol is not ProtocolCodeConstant.UNC (case-insensitive compare), sets FtpServerIp, FtpUser, FtpPassword.
- Data: none.
- Errors: none explicitly.
- Rules: none.

## QueryFtsNotificationDataAccess
Loads notification recipients for a configuration id based on the requested notify flags.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Return the FtsNotificationModel list matching the notify flags.
- Inputs: query (ConfigId, NotifyOnErrorInd, NotifyOnSuccessInd).
- Logic:
  - If both flags are true, calls NotifyErrorSuccess.
  - Else if error flag true, calls NotifyErrorInd.
  - Else calls NotifyOnSuccessInd.
- Data: FTS_NOTIFICATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

### NotifyErrorSuccess(IDbContextBase dbcontext)
- Purpose: Return recipients configured for both success and error notifications.
- Inputs: query.ConfigId.
- Logic: Queries FtsNotification where NotifyOnErrorInd and NotifyOnSuccessInd are true and FtsSourceLocationId == query.ConfigId.
- Data: FTS_NOTIFICATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

### NotifyOnSuccessInd(IDbContextBase dbcontext)
- Purpose: Return recipients configured for success notifications.
- Inputs: query.ConfigId.
- Logic: Queries FtsNotification where NotifyOnSuccessInd is true and FtsSourceLocationId == query.ConfigId.
- Data: FTS_NOTIFICATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

### NotifyErrorInd(IDbContextBase dbcontext)
- Purpose: Return recipients configured for error notifications.
- Inputs: query.ConfigId.
- Logic: Queries FtsNotification where NotifyOnErrorInd is true and FtsSourceLocationId == query.ConfigId.
- Data: FTS_NOTIFICATION (read).
- Errors: EF/database exceptions propagate.
- Rules: none.

## UpdateFileManagementStatusDataAccess
Updates a file status and timestamps.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Update FTS_FILE_MANAGEMENT status fields for a file id.
- Inputs: fileId, fileStatus.
- Logic:
  - Loads the first FtsFileManagement where FileId == fileId.
  - If found:
    - FileStatus = fileStatus
    - FileStatusTs = DateTime.UtcNow
    - LastModifiedTs = DateTime.UtcNow
    - SaveChanges()
  - Returns fileId.
- Data: FTS_FILE_MANAGEMENT (read+write).
- Errors: EF/database exceptions propagate.
- Rules: none.

## UpdateSourceSuspendStatusDataAccess
Clears a source location suspended status.

### ExecuteQuery(IDbContextBase dbcontext)
- Purpose: Unsuspend a source location.
- Inputs: sourceLocationId.
- Logic:
  - Loads FtsSourceLocation via SingleOrDefault(s => s.FtsSourceLocationId == sourceLocationId).
  - If found:
    - IsSuspended = false
    - IsSuspendedChangedDate = DateTime.UtcNow
    - result = SaveChanges() > 0
  - Returns result.
- Data: FTS_SOURCE_LOCATION (read+write).
- Errors: EF/database exceptions propagate.
- Rules: none.

## IDataAccess<TdacOutput>
Generic interface implemented by each data-access class. Execute() is the only method.

## IDataAccessFactory
Factory interface used by batch/business code to obtain the data-access objects.

## DataAccessConstants.FileStatusConstant
Defines status codes used in FTS_FILE_MANAGEMENT.FileStatus.

- Completed = 1
- Error = 2
- Transfering = 3

## ProtocolCodeConstant
Defines protocol codes.

- FTP = 1
- SFTP = 2
- UNC = 3
- FTPS = 4

## Rules implemented

None (this component owns no business rules).

| Rule | In short | Where | Flow |
|---|---|---|---|

## Not carried over

None.

<!-- END -->



