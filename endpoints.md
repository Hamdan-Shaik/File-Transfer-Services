1) File Transfer Configuration (Search/Create/Edit “jobs”)

- GET SearchFileTransfer — Returns the list of configured transfer jobs for the search grid (so admins can find existing configs).
- PUT AddFileTransferBaseConfiguration — Creates or updates a transfer job (source/job header + destination rows) when the admin clicks Save.
- GET GetDataListData — Supplies dropdown list values used by the screen (e.g., modules/lookup lists) so the UI can render choices.
- GET GetFtpServerDetails — Supplies the selectable “FTP/SFTP server credential profiles” list for the config screen.
- GET GetFTEMetadatas — Supplies integration metadata IDs (FTE metadata) used when configuring metadata-driven destinations.

2) FTP Server Credentials (Manage server login profiles)

- GET GetFTPServerCredentials — Searches/list existing saved server credential profiles (the FTP_SERVER records) for the credentials grid.
- POST AddFTPServerCredentials — Creates a new server credential profile (host/user/password/port/etc) to be reused by jobs.
- POST UpdateFTPServerCredentials — Updates or invalidates an existing server credential profile.

3) File Transfer Tracking (Monitor actual file runs)

- GET GetSourceLocations — Returns the list of configured jobs (source locations) for the tracking filter dropdown.
- GET SearchFileTransferTracking — Returns file-level tracking records (what files ran, status, size, date, etc.) for the tracking results grid.




### Contract 1: Command / Write contract (IFileTransferSvcService) — 7 operations

These are used when an admin changes something (create/update/disable config, manage server credentials, mark integration files processed).

1. FileTransferAdd

- Creates a new file-transfer job (“source location” header + its destinations).
- Writes FTS_SOURCE_LOCATION and inserts one-or-more FTS_DESTINATION rows linked to it.
- The job can reference FTP_SERVER via FTP_SERVER_ID (source and/or destination side).

2. UpdateFileTransferSourceLocation

- Updates an existing job’s header fields (protocol/address/mask/owner/schedule/etc).
- Updates FTS_SOURCE_LOCATION, and also adds/updates destination rows in FTS_DESTINATION.
- Used when editing a job after it already exists.

3. InvalidateFileTransferSourceLocation

- “Soft disables” a job so it’s no longer considered active/valid.
- Sets FTS_SOURCE_LOCATION.IS_VALID = false and also marks related FTS_DESTINATION rows invalid.
- Used when retiring a transfer config without deleting history.

4. AddFtpServer

- Creates a new saved server credential profile.
- Inserts a row into FTP_SERVER (host/IP, port, user/password, proxy, SSH auth flag, etc.).
- Jobs later point to this row via FTP_SERVER_ID.

5. UpdateFtpServer

- Updates an existing saved server credential profile.
- Updates the FTP_SERVER row (name/ip/user/pass/port/proxy/etc).
- Used when a partner changes credentials or endpoints.

6. InvalidateFtpServer

- Soft-disables a saved server credential profile (instead of deleting it).
- Updates FTP_SERVER.IS_VALID (and audit fields).
- Prevents new/active use while keeping history.

7. UpdateIntegrationFile

- Marks an “integration file” tracking record as processed / updates its linkage.
- Updates FTS_INTEGRATION_FILE (sets CONFIG_ID, sets IS_PROCESSED = true, updates validity/audit).

———

### Contract 2: Query / Read contract (IFileTransferSvcQueryService) — 6 operations

These are used when the UI needs to search or display data (configs, servers, tracking/history, integration metadata).

1. SearchSourceLocations

- Searches and returns configured jobs (source-location configs) for the admin UI.
- Reads mainly from FTS_SOURCE_LOCATION (and may include related config context like address/protocol).
- Used by the “Search Source Locations / Job search” screens.

2. GetFtpServers

- Returns the list of saved server credential profiles for dropdowns/grids.
- Reads from FTP_SERVER.
- Used to let admins pick “which server credentials to use” in configs.

3. GetSourceLocations

- Returns source locations (jobs) typically for dropdown selection lists.
- Reads from FTS_SOURCE_LOCATION.
- Used in places like tracking filters where you pick a job/source.

4. SearchFileTransferMovement

- Searches file-level tracking history (one row per file that was discovered/processed).
- Reads FTS_FILE_MANAGEMENT joined to FTS_SOURCE_LOCATION; for FTP/SFTP/FTPS it also joins FTP_SERVER.
- Also loads per-file timelines/errors from FTS_FILE_EVENT and FTS_FILE_EXCEPTION.

5. SearchInboundTarget

- Returns metadata mappings used by inbound processing integrations.
- Reads FTS_INBOUND_TARGET (external metadata id → import job name + contract name).
- Used when configuring/looking up FTE metadata-driven routing.

6. GetIntegrationFile

- Returns “integration file” tracking records (outbound/integration file ledger).
- Reads from FTS_INTEGRATION_FILE.
- Used to query which integration files exist / were processed.

