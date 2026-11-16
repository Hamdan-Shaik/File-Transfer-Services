---
type: Behavior Specification
title: 'data-data-access-entities-2: EF entities for FXFER configuration + tracking'
description: 'What the data-data-access-entities-2 component does today: every class and method, and the business rules it implements.'
resource: urn:csharp-doc-agent:component:provider-file-transfer-v2/data-data-access-entities-2
tags:
- component
- data
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T21:22:58Z
sources:
- id: inventory
  resource: /technical/components/data-data-access-entities-2/INVENTORY.yaml
  title: Every legacy class and method (10 classes)
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (where it goes)
cef:
  pack: provider-file-transfer-v2
  item: component:data-data-access-entities-2
  generation:
    status: done
    attempts: 1
  tier: data
  classes: 10
  methods: 2
  owned_rules: 0
  flows: []
  depends_on: []
  deployable: fxfer-domain-api
  folder: src/Infrastructure/DataAccess
  body_sha256: b5fc0688f091de8f1e06e0c729c7213ce2914334eb62e5582ae417f724ec2a4b
---

# data-data-access-entities-2: EF entities for FXFER configuration + tracking

This component contains legacy Entity Framework 6 entity classes for the FXFER database schema and the EF6 `DbContext` used to map and relate those entities.

It is used by higher-level data access and business components to read and write:

- Source-location and destination configuration (FTP servers, source locations, destinations, notifications, suspension records).
- File tracking state (file management, events, deliveries, exceptions).

## FileTransferEntityDbContext
Entity Framework 6 `DbContext` for the legacy FXFER database.

### Create()
- Purpose: Create a new EF context using the configured default connection.
- Inputs: none.
- Logic:
  - Reads `ConfigurationManager.ConnectionStrings[DefaultConnection]`.
  - If present, constructs `DbSession(cs.ProviderName, cs.ConnectionString)`.
  - Else constructs `DbSession(AppSettings[DefaultConnectionProvider], AppSettings[DefaultConnectionString])`.
  - Returns `new FileTransferEntityDbContext(session, contextOwnsSession: true)`.
- Data: Opens a session to the legacy FXFER database.
- Errors: Missing/invalid configuration values and DB connection errors propagate when the context is used.
- Rules: none.

### OnModelCreating(DbModelBuilder)
- Purpose: Configure EF mappings and relationships for the FXFER entities.
- Inputs: `modelBuilder`.
- Logic:
  - Calls `base.OnModelCreating(modelBuilder)`.
  - Reads schema name from `AppSettings[FileTransferEntityDbContext.SchemaName]`.
  - Maps entities to tables (all within the configured schema):
    - `FtpServer` -> `FTP_SERVER`
    - `FtsSourceLocation` -> `FTS_SOURCE_LOCATION`
    - `FtsDestinationLocations` -> `FTS_DESTINATION`
    - `FtsFileDelivery` -> `FTS_FILE_DELIVERY`
    - `FtsFileEvent` -> `FTS_FILE_EVENT`
    - `FtsFileException` -> `FTS_FILE_EXCEPTION`
    - `FtsFileManagement` -> `FTS_FILE_MANAGEMENT`
    - `FtsSuspension` -> `FTS_SUSPENSION`
    - `FtsNotification` -> `FTS_NOTIFICATION`
  - Configures relationships (all with `WillCascadeOnDelete(false)`):
    - `FtpServer` -> `FtsSourceLocation` (FK `FtsSourceLocation.FtpServerId`).
    - `FtsDestinationLocations` -> `FtsFileDelivery` and `FtsFileEvent`.
    - `FtsFileManagement` -> `FtsFileDelivery`, `FtsFileEvent`, `FtsFileException`.
    - `FtsSourceLocation` -> `FtsFileManagement` (FK `FtsFileManagement.FileSourceLocationId`).
  - Configures `FtsFileManagement.FileStatus` as fixed-length.
- Data: ORM mapping only.
- Errors: EF model-building exceptions propagate.
- Rules: none.

## FtpServer
EF entity for `FTP_SERVER`.

- Purpose: Persist/query FTP server definitions and credentials used by FXFER source/destination configuration.
- Data: `FTP_SERVER` (read/write).
- Notes: Includes `FTP_USER` and `FTP_PASSWORD` columns in the legacy database.

## FtsSourceLocation
EF entity for `FTS_SOURCE_LOCATION`.

- Purpose: Persist/query source-location configuration (protocol, address, file mask, schedule flags/time window, suspension flags).
- Data: `FTS_SOURCE_LOCATION` (read/write).
- Notes: References `FtpServer` via `FTP_SERVER_ID`.

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

- Purpose: Persist/query file-level tracking state (`FILE_STATUS`, timestamps, checksum, size, group id) associated with a source location.
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

## Rules implemented

| Rule | In short | Where | Flow |
|---|---|---|---|

## Not carried over

None.

