1) `FTP_SERVER`

Stores reusable server connection details (host/IP, user, password, port) for FTP/SFTP/FTPS transfers.
Source and destination configurations can reference this server record instead of re-entering credentials.

Example flow (concrete):
Create one FTP server credential record named `TP01_SFTP` with IP `10.20.30.40` and user `vendor_user`.
When creating a File Transfer Configuration, choose `TP01_SFTP` in the Source “Server” dropdown.
When adding a Destination with protocol SFTP, choose the same `TP01_SFTP` record for that destination.
During batch execution, the job reads the server IP/user/password and connects to deliver files.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtpServer.cs`

Schema (fields and types):
- `FTP_SERVER_ID`: `Guid` (PK)
- `FTP_SERVER_NAME`: `string` (max 50)
- `FTP_SERVER_IP`: `string` (max 15)
- `FTP_TRANSFER_MODE`: `string`
- `FTP_USER`: `string` (max 50)
- `FTP_PASSWORD`: `string` (max 50)
- `FTP_DESCRIPTION`: `string` (max 100)
- `FTP_PORT`: `short`
- `FTP_PROXY_ADR`: `string?` (max 200)
- `FTP_PROXY_PORT`: `short?`
- `IS_VALID`: `bool`
- `SSH_AUTHENTICATION`: `bool`
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

2) `FTS_SOURCE_LOCATION`

Stores the “master configuration” for a transfer job: where to look for files, what to pick up, and when it should run.
Destinations, notifications, and tracking records link back to this job configuration.

Example flow (concrete):
Create a Source Location with protocol `SFTP`, source address `/incoming/outbound/`, and file mask `*.dat`.
Set the schedule to run Mon–Fri between `0800` and `1700`, and mark it Active.
When batch runs, it uses this source address + mask to list matching files and queue them for delivery.
Each discovered file becomes a tracking record linked back to this source location.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsSourceLocations.cs`

Schema (fields and types):
- `FTS_SOURCE_LOCATION_ID`: `Guid` (PK)
- `CONFIG_ID`: `int`
- `CONFIG_NAME`: `string` (max 50)
- `SOURCE_PROTOCOL_CD`: `string` (max 4)
- `SOURCE_ADDRESS`: `string` (max 248)
- `SOURCE_FILE_MASK`: `string` (max 50)
- `FTP_SERVER_ID`: `Guid?`
- `SEND_NOTIFICATIONS_IND`: `bool`
- `OWNER_NAME`: `string` (max 50)
- `MODULE_ID`: `Guid`
- `OWNER_PHONE`: `string` (max 10)
- `OWNER_EMAIL`: `string` (max 150)
- `CONFIG_DESCRIPTION`: `string?` (max 200)
- `RUN_ON_MONDAY_IND`: `bool`
- `RUN_ON_TUESDAY_IND`: `bool`
- `RUN_ON_WED_IND`: `bool`
- `RUN_ON_THURS_IND`: `bool`
- `RUN_ON_FRI_IND`: `bool`
- `RUN_ON_SAT_IND`: `bool`
- `RUN_ON_SUN_IND`: `bool`
- `RUN_ON_FRST_DAY_OF_MONTH_IND`: `bool`
- `RUN_ON_LAST_DAY_OF_MONTH_IND`: `bool`
- `RUN_ON_THIS_DAY_OF_MONTH`: `byte`
- `START_TIME_24HHMM`: `string` (len 4)
- `END_TIME_24HHMM`: `string` (len 4)
- `FILE_DIRECTION`: `string?` (max 50)
- `TRADING_PARTNER`: `string?` (max 10)
- `IS_VALID`: `bool`
- `IS_SUSPENDED`: `bool`
- `IS_SUSPENDED_CHANGED_TS`: `DateTime?`
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

3) `FTS_DESTINATION`

Stores one delivery target for a source job (a single source location can have many destinations).
Also stores delivery behavior rules like rename/compress/overwrite and optional integration metadata.

Example flow (concrete):
For Source Location `DailyOutbound`, add Destination #1 with protocol `UNC` and address `\\fileserver01\drop\outbound`.
Enable “Rename at destination” using the pattern `ORIG_<DATE>_<TIME>`.
Add Destination #2 with protocol `SFTP`, select server `TP01_SFTP`, and set destination folder `/outgoing/edi/`.
During batch, the same file is uploaded to both destinations and tracked separately per destination.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsDestinationLocations.cs`

Schema (fields and types):
- `DESTINATION_ID`: `Guid` (PK)
- `FTS_SOURCE_LOCATION_ID`: `Guid` (FK)
- `DESTINATION_ADDRESS`: `string` (max 248)
- `DEST_PROTOCOL_CD`: `string` (max 4)
- `OVERWRITE_AT_DEST_IND`: `bool`
- `COMPRESS_AT_DEST_IND`: `bool`
- `RENAME_AT_DEST_IND`: `bool`
- `DEST_IS_ARCHIVE_IND`: `bool`
- `IS_VALID`: `bool`
- `FTP_SERVER_ID`: `Guid`
- `RENAME_PATTERN`: `string?` (max 50)
- `CONTACT_NAME`: `string?` (max 50)
- `CONTACT_PHONE`: `string?` (max 15)
- `CONTACT_EMAIL`: `string?` (max 150)
- `OPERATOR_ID`: `string` (max 200)
- `CREATION_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`
- `CDE_ENCRYPTION`: `string?` (max 8)
- `ENCRYPTION_PUBLICKEY`: `string?`
- `ENCRYPTION_PRIVATEKEY`: `string?`
- `PRIVATEKEY_PASSWORD`: `string?` (max 25)
- `EXTERNAL_FILE_DEFN_ID`: `Guid?` (stored as `FTEMetadataId` in the entity)
- `DESTINATION_TYPE`: `string?` (max 200)
- `CONTRACT_NAME`: `string?` (max 200)

4) `FTS_FILE_MANAGEMENT`

Stores the primary tracking record for each discovered file (one row per file).
Captures file name, size, checksum, status, and ties the file back to the source location.

Example flow (concrete):
Batch scans Source Location `DailyOutbound` and finds `837_20261112.dat`.
The system inserts a new tracking row with `FILE_NAME=837_20261112.dat`, `FILE_SIZE=...`, `FILE_STATUS=P`.
As delivery succeeds or fails, the status is updated to reflect completion or error.
Tracking search screen filters and lists these records.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsFileManagement.cs`

Schema (fields and types):
- `FILE_ID`: `Guid` (PK)
- `FTS_SOURCE_LOCATION_ID`: `Guid` (FK)
- `FILE_NAME`: `string` (max 255)
- `FILE_STATUS`: `string` (len 1)
- `FILE_STATUS_TS`: `DateTime`
- `SAK_FILE_TRACK`: `int?`
- `FILE_SIZE`: `decimal`
- `CHECKSUM_CD`: `string?` (max 20)
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

5) `FTS_FILE_DELIVERY`

Stores a per-destination delivery record for a given file (one row per (file, destination) delivery).
Captures when delivery happened and what the file name was at the destination.

Example flow (concrete):
File `837_20261112.dat` is delivered to two destinations.
The system writes two delivery rows, each pointing to the same `FILE_ID` but different `DESTINATION_ID`s.
Each row stores the `DESTINATION_FILE_NAME` (including any rename pattern outcome).
Tracking detail screen can show which destination received what.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsFileDelivery.cs`

Schema (fields and types):
- `DELIVERY_ID`: `Guid` (PK)
- `FILE_ID`: `Guid` (FK)
- `DESTINATION_ID`: `Guid` (FK)
- `DELIVERY_TS`: `DateTime`
- `DESTINATION_FILE_NAME`: `string` (max 255)
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

6) `FTS_FILE_EVENT`

Stores milestone/timeline events for processing a file and its destinations.
Events are compact codes (plus optional description) used to understand what happened and in what order.

Example flow (concrete):
When delivery begins, the system records an event like “downloaded to working directory”.
After upload to a destination, it records another event containing the destination file name.
If the same file is delivered to multiple destinations, events are recorded per destination context.
Tracking detail screen shows these events in a grid.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsFileEvent.cs`

Schema (fields and types):
- `EVENT_ID`: `Guid` (PK)
- `FILE_ID`: `Guid` (FK)
- `DESTINATION_ID`: `Guid` (FK)
- `EVENT_CD`: `string` (max 20)
- `EVENT_TRACK_DESCRIPTION`: `string?` (max 500)
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

7) `FTS_FILE_EXCEPTION`

Stores captured exception snapshots that occurred while processing a file/destination.
This is persisted error detail (type/method/message/stack trace), not only log output.

Example flow (concrete):
Upload to destination fails due to authentication error.
The system writes an exception record linked to the `FILE_ID` and `DESTINATION_ID`.
The record stores exception type and stack trace so operators can view it later.
Tracking detail screen shows exceptions and the detailed stack trace.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsFileException.cs`

Schema (fields and types):
- `EXCEPTION_ID`: `Guid` (PK)
- `FILE_ID`: `Guid` (FK)
- `DESTINATION_ID`: `Guid` (FK)
- `EVENT_ID`: `Guid` (FK)
- `EXCEPTION_TYPE`: `string` (max 2000)
- `METHOD_NAME`: `string` (max 2000)
- `MESSAGE`: `string?` (max 2000)
- `STACK_TRACE`: `string?` (max 4000)
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

8) `FTS_NOTIFICATION`

Stores who should be notified for a given Source Location, and whether they want success and/or error notifications.
Batch uses this configuration to build a recipient list when it sends delivery notifications.

Example flow (concrete):
An operator adds a notification recipient `ops-team@example.com` for Source Location `DailyOutbound`.
They set “Notify on success” = true and “Notify on error” = true.
When batch completes delivery, it builds a subject/body and sends email to that recipient list.
Recipients are tied to the job via `FTS_SOURCE_LOCATION_ID`.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsNotification.cs`

Schema (fields and types):
- `NOTIFICATION_ID`: `Guid` (PK)
- `FTS_SOURCE_LOCATION_ID`: `Guid` (FK)
- `EMAIL_ADDRESS`: `string` (max 150)
- `CONTACT_NAME`: `string` (max 200)
- `CONTACT_PHONE`: `string` (max 15)
- `NOTIFY_ON_ERROR_IND`: `bool`
- `NOTIFY_ON_SUCCESS_IND`: `bool`
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

9) `FTS_SUSPENSION`

Stores suspension history details (error text + stack trace) for a source configuration.
This supports operational review of why a configuration was suspended.

Example flow (concrete):
A source location repeatedly fails due to bad credentials.
The system marks the source location as suspended and records the error and stack trace in suspension history.
An operator reviews suspension history, fixes the credentials, and reactivates the job.
Subsequent batch cycles may resume processing after the configured retry window.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsSuspension.cs`

Schema (fields and types):
- `SUSPENSION_ID`: `Guid` (PK)
- `FTS_SOURCE_LOCATION_ID`: `Guid` (FK)
- `ERROR_TEXT`: `string` (max 250)
- `STACK_TRACE`: `string` (max 1000)
- `OPERATOR_ID`: `string` (max 200)
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

10) `FTS_INBOUND_TARGET`

Stores inbound processing metadata for files that arrive and need to be handed off to an ingestion/import pipeline.
Links a contract + import job name to an external file definition/metadata identifier.

Example flow (concrete):
You configure a destination type that represents “files that should be picked up by ingestion”.
The destination stores a metadata id, and that metadata id is also represented in inbound target lookup.
When a file lands in the common folder, downstream ingestion can use that metadata id + import job name.
This supports consistent routing/processing for inbound files.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsInboundTarget.cs`

Schema (fields and types):
- `FTS_INBOUND_TARGET_ID`: `Guid` (PK)
- `EXTERNAL_FILE_DEFN_ID`: `Guid`
- `IMPORT_JOB_NAME`: `string` (max 200)
- `CONTRACT_NAME`: `string` (max 200)
- `OPERATOR_ID`: `string` (max 200)
- `IS_VALID`: `bool`
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`

11) `FTS_INTEGRATION_FILE`

Stores outbound integration file tracking, including the metadata definition id and whether the file has been processed.
This supports “outbound file generation + integration processing” workflows.

Example flow (concrete):
An outbound job generates a file `outbound_20261112.zip` that must be picked up by another system.
The system records the file name and associated outbound metadata definition id in the integration file table.
After the downstream system consumes/processes the file, the record is marked `IS_PROCESSED=true`.
Operations can query this table to see what outbound files are pending or completed.

Location (where this table is defined in this repo):
`mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess/Entities/FtsIntegrationFile.cs`

Schema (fields and types):
- `FTS_INTEGRATION_FILE_ID`: `Guid` (PK)
- `GROUP_ID`: `Guid?`
- `CONFIG_ID`: `int?`
- `FILE_NAME`: `string` (max 500)
- `ETF_OUTBOUND_METADATA_DEFN_ID`: `Guid`
- `IS_PROCESSED`: `bool`
- `OPERATOR_ID`: `string` (max 200)
- `IS_VALID`: `bool`
- `CREATED_TS`: `DateTime`
- `LAST_MODIFIED_TS`: `DateTime`
