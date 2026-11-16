---
type: Behavior Specification
title: 'data-data-access-entities-3: EF Core entities and read-only repository'
description: 'What the data-data-access-entities-3 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-3
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:32:53Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-3/INVENTORY.yaml
  title: Every legacy class and method (14 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-3
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 14
  methods: 8
  owned_rules: 0
  flows:
  - manage-ftp-servers-and-credentials
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: f216ad483f9572e2275acc91fa55e0020f035de51e6c13172e3f802d174ca32d
---

# data-data-access-entities-3: EF Core entities and read-only repository

This component contains legacy Entity Framework Core entity classes for the FXFER database schema and a small read-only repository used by query-side CQRS code.

It participates in the `manage-ftp-servers-and-credentials` flow by providing:

- EF Core mappings for configuration/tracking tables (FTP server, source locations, destinations, file tracking).
- A read-only repository that executes CQRS query executors using `FileTransferEntityDbContext`.

## FileTransferEntityDbContext
Entity Framework Core `DbContext` (via `HPP.Core.BAS.CQRS.Base.DbContextBase`) for the legacy FXFER database.

### Create()
- Purpose: Create a new EF Core context using the configured default connection.
- Inputs: none.
- Logic:
  - Reads `ConfigurationManager.ConnectionStrings["DefaultConnection"]`.
  - If present, constructs `DbSession(cs.ProviderName, cs.ConnectionString)` from that entry.
  - Else constructs `DbSession(AppSettings["DefaultConnectionProvider"], AppSettings["DefaultConnectionString"])`.
  - Returns `new FileTransferEntityDbContext(session, contextOwnsSession: true)`.
- Data: Opens a session to the legacy FXFER database.
- Errors: Missing/invalid configuration values and DB connection errors propagate when the context is used.
- Rules: none.

### OnModelCreating(ModelBuilder)
- Purpose: Configure EF Core mappings and relationships for the FXFER entities.
- Inputs: `modelBuilder`.
- Logic:
  - Calls `base.OnModelCreating(modelBuilder)`.
  - Sets default schema to `GetSchemaName(string.Empty)`.
  - Maps entities to tables:
    - `FtpServer` -> `FTP_SERVER`
    - `FtsSourceLocation` -> `FTS_SOURCE_LOCATION`
    - `FtsDestinationLocations` -> `FTS_DESTINATION`
    - `FtsFileDelivery` -> `FTS_FILE_DELIVERY`
    - `FtsFileEvent` -> `FTS_FILE_EVENT`
    - `FtsFileException` -> `FTS_FILE_EXCEPTION`
    - `FtsFileManagement` -> `FTS_FILE_MANAGEMENT`
    - `FtsSuspension` -> `FTS_SUSPENSION`
    - `FtsNotification` -> `FTS_NOTIFICATION`
    - `FtsInboundTarget` -> `FTS_INBOUND_TARGET`
    - `FtsIntegrationFile` -> `FTS_INTEGRATION_FILE`
  - Configures relationships (all with `OnDelete(DeleteBehavior.ClientSetNull)`):
    - `FtpServer` -> `FtsSourceLocation` (FK `FtsSourceLocation.FtpServerId`).
    - `FtsDestinationLocations` -> `FtsFileDelivery`, `FtsFileEvent`, `FtsFileException`.
    - `FtsFileEvent` -> `FtsFileException`.
    - `FtsFileManagement` -> `FtsFileDelivery`, `FtsFileEvent`, `FtsFileException`.
    - `FtsSourceLocation` -> `FtsFileManagement` (FK `FtsFileManagement.FileSourceLocationId`).
  - Configures `FtsFileManagement.FileStatus` as fixed-length.
- Data: ORM mapping only.
- Errors: EF Core model-building exceptions propagate.
- Rules: none.

## FtpServer
EF entity for `FTP_SERVER`.

- Purpose: Persist/query FTP server definitions and credentials used by file transfer endpoints.
- Data: `FTP_SERVER` (read/write).
- Notes: Includes `FTP_USER` and `FTP_PASSWORD` columns in the legacy database.

## FtsSourceLocation
EF entity for `FTS_SOURCE_LOCATION`.

- Purpose: Persist/query source-location configuration (protocol, address, file mask, schedule flags/time window, suspension flags).
- Data: `FTS_SOURCE_LOCATION` (read/write).
- Notes: References `FtpServer` via `FTP_SERVER_ID` (nullable).

## FtsDestinationLocations
EF entity for `FTS_DESTINATION`.

- Purpose: Persist/query destination definitions for a source location (protocol, overwrite/compress/rename, archive indicator, contact fields).
- Data: `FTS_DESTINATION` (read/write).
- Notes: Includes encryption key material and passwords in legacy columns (`CDE_ENCRYPTION`, `ENCRYPTION_PUBLICKEY`, `ENCRYPTION_PRIVATEKEY`, `PRIVATEKEY_PASSWORD`).

## FtsNotification
EF entity for `FTS_NOTIFICATION`.

- Purpose: Persist/query notification recipients and flags per source location (`NOTIFY_ON_ERROR_IND`, `NOTIFY_ON_SUCCESS_IND`).
- Data: `FTS_NOTIFICATION` (read/write).

## FtsSuspension
EF entity for `FTS_SUSPENSION`.

- Purpose: Persist/query suspension diagnostic records associated with a source location (`ERROR_TEXT`, `STACK_TRACE`).
- Data: `FTS_SUSPENSION` (read/write).

## FtsFileManagement
EF entity for `FTS_FILE_MANAGEMENT`.

- Purpose: Persist/query file-level tracking state (`FILE_STATUS`, timestamps, checksum, size) associated with a source location.
- Data: `FTS_FILE_MANAGEMENT` (read/write).

## FtsFileDelivery
EF entity for `FTS_FILE_DELIVERY`.

- Purpose: Persist/query delivery outcomes for a file to a destination.
- Data: `FTS_FILE_DELIVERY` (read/write).

## FtsFileEvent
EF entity for `FTS_FILE_EVENT`.

- Purpose: Persist/query tracking events for file/destination (`EVENT_CD`, `EVENT_TRACK_DESCRIPTION`).
- Data: `FTS_FILE_EVENT` (read/write).

## FtsFileException
EF entity for `FTS_FILE_EXCEPTION`.

- Purpose: Persist/query exception records for file processing (exception type, method name, message, stack trace).
- Data: `FTS_FILE_EXCEPTION` (read/write).

## FtsInboundTarget
EF entity for `FTS_INBOUND_TARGET`.

- Purpose: Persist/query inbound-target configuration (import job name, contract name, metadata id).
- Data: `FTS_INBOUND_TARGET` (read/write).

## FtsIntegrationFile
EF entity for `FTS_INTEGRATION_FILE`.

- Purpose: Persist/query integration-file records for processing state (`GROUP_ID`, `ETF_OUTBOUND_METADATA_DEFN_ID`, `IS_PROCESSED`).
- Data: `FTS_INTEGRATION_FILE` (read/write).

## FileTransferReadOnlyRepository
Read-only repository that executes CQRS query executors using an EF Core context created over a short-lived `DbSession`.

### GetFtpServer(FtpServerQuery)
- Purpose: Execute a query for FTP servers and return `FtpServerModel` rows.
- Inputs: `query`.
- Logic: Returns `GetResults<FtpServerQuery, GetFtpServerQueryExecutor, FtpServerModel>(query).Results`.
- Data: `FTP_SERVER` (read).
- Errors: Exceptions are handled inside `GetResults`; this method returns whatever `.Results` contains on the returned query instance.
- Rules: none.

### GetInboundTarget(InboundTargetQuery)
- Purpose: Execute a query for inbound targets and return `FtsInboundTargetModel` rows.
- Inputs: `query`.
- Logic: Returns `GetResults<InboundTargetQuery, InboundTargetQueryExecutor, FtsInboundTargetModel>(query).Results`.
- Data: `FTS_INBOUND_TARGET` (read).
- Errors: Exceptions are handled inside `GetResults`; this method returns whatever `.Results` contains on the returned query instance.
- Rules: none.

### GetResults<Query, QueryExecutor, Model>(Query)
- Purpose: Open a DB session and execute a CQRS query executor.
- Inputs: `query`.
- Logic:
  - Creates `DbSession(providerName, connectionString)` from `ConnectionString` resolved in the repository constructor.
  - Calls `OpenConnection(session)`.
  - Calls `ExecuteQuery<Query, QueryExecutor, Model>(query, session)`.
  - On exception: logs `LoggerManager.Logger.LogError(FormatErrorMsg(...))` and returns the original `query`.
  - Always disposes the underlying ADO.NET connection.
- Data: Reads from the legacy FXFER database.
- Errors: Exceptions are caught/logged and the method returns `query`.
- Rules: none.

### OpenConnection(IDbSession)
- Purpose: Ensure the session connection is open.
- Inputs: `session`.
- Logic: If `session.Connection.State` is not `Open`, calls `session.Connection.Open()`.
- Data: none.
- Errors: Connection-open exceptions propagate to `GetResults` and are caught/logged there.
- Rules: none.

### ExecuteQuery<Query, QueryExecutor, Model>(Query, IDbSession)
- Purpose: Execute the query executor against an EF Core context.
- Inputs: `query`, `session`.
- Logic:
  - Creates `FileTransferEntityDbContext(session)`.
  - Calls `new QueryExecutor().Execute(dbContext, query)`.
  - Disposes the DbContext.
- Data: Reads from the legacy FXFER database.
- Errors: Exceptions propagate to `GetResults` and are caught/logged there.
- Rules: none.

### FormatErrorMsg(Exception, Query, string)
- Purpose: Format a repository error message for logging.
- Inputs: `ex`, `query`, `msj`.
- Logic: `string.Format("{0}: {1}-{2}", msj, query.GetType(), ex.Message)`.
- Data: none.
- Errors: none.
- Rules: none.

## IFileTransferReadOnlyRepository
Read-only repository interface.

- Offers:
  - `List<FtpServerModel> GetFtpServer(FtpServerQuery query)`
  - `List<FtsInboundTargetModel> GetInboundTarget(InboundTargetQuery query)`

## Rules implemented

| Rule | In short | Where | Flow |
|---|---|---|---|

## Not carried over

None.