# External Dependencies (FXFER)

This document summarizes the major external dependencies FXFER relies on during runtime and operations.

## 1) Notifications

### What this is
FXFER sends notifications so operators/support teams know what happened during file transfer processing. Typical scenarios include a transfer **success**, **failure**, **retry/suspension**, or a **bad configuration** (for example, credentials or connectivity issues) that needs investigation.

### External module called
FXFER does not implement its own notification system. It calls a shared **Notification Service module** (WCF/SOAP) to send emails and to record operational event messages.

### Endpoint shape (how it looks)
The Notification Service is reached via a configured WCF endpoint address (varies by environment). It typically looks like:
- `http://<host>:<port>/NotificationService.svc`
- Example (local/dev): `http://localhost:10172/NotificationService.svc`

### Operations FXFER actually uses
The Notification Service contract contains many operations, but FXFER primarily uses **two**:

#### A) `SendEmail` (email notification)
**What it does:** sends a human-facing email.

**Typical inputs FXFER provides:**
- `tenantId` (tenant/customer context; used in the multi-tenant batch variant)
- `subject` (short summary)
- `emailBody` (details; treated as text content)
- `addressList` (recipients)
- `fromAddress` (optional)
- `priority` (for example Normal/High)
- `attachmentList` (optional)

**SOAPAction style example:** `.../INotificationService/SendEmail`

#### B) `PostEvent` (operational event record)
**What it does:** records an operational event in the shared notification/event system (often used for success/error tracking, dashboards, and operational history).

**Typical inputs FXFER provides:**
- `tenantId`
- `eventName` (string label such as `File Delivery Success <configId>` or `File Delivery Error <configId>`)
- `subSystem` (category such as `EDI`)
- `messageBody` (details)
- `isArchived`
- `processRunTime` (usually “now”)
- `previousEventId` (optional; to chain related events)
- `operatorId` (who/what produced the event; often a batch job identity)

**SOAPAction style example:** `.../INotificationService/PostEvent`

### Where recipients come from
Recipients are configuration-driven. FXFER stores notification recipients in its database and uses them to build the `addressList` for `SendEmail`.

| Table | Meaning | How FXFER uses it |
|---|---|---|
| `FTS_NOTIFICATION` | Who should be notified for a configuration/tenant | Read during batch processing to build the email recipient list |


## 2) External libraries

### What this is
FXFER relies on libraries to avoid re-implementing platform capabilities (hosting, routing, logging, configuration) and technical plumbing (database access, FTP/SFTP protocols, UI widgets). Some libraries are shared across the organization (platform/framework), and others are third-party/vendor.

### Organization / platform libraries (shared)
- **CQRS framework (commands/queries/events):** shared pattern and plumbing for routing write commands and read queries through standard handlers.
- **UX framework (UX Data / Providers / Services / Presentation):** shared UI conventions (controllers/providers/models) and tenant-aware resolution so File Transfer screens behave consistently with other modules.
- **Hosting helpers (Service Fabric + bootstrap):** shared patterns for starting services, exposing endpoints, and integrating with the platform runtime.
- **Shared config/runtime context:** shared approach for tenant-aware configuration loading and tenant-scoped dependency resolution.
- **Shared logging/telemetry wrappers:** consistent logging and standard operational context (tenant, correlation) across services and batch.

### Third-party libraries
- **File transfer protocol libraries:**
  - `edtFTPnetPRO` (EnterpriseDT): used for FTP/FTPS/SFTP behavior in batch processing; requires licensing config.
  - `Renci.SshNet` (SSH.NET): used for SFTP/SSH behavior (notably in the FileTransferManagement batch variant).
- **Database access:** Entity Framework (EF) maps relational tables to entities and performs DB read/write.
- **Dependency injection:** Unity constructs/wires batch components (protocol handlers, data access factories, executors).
- **Logging & telemetry:** `log4net` for logging; Application Insights packages for telemetry in hosted environments.
- **UI components:** Telerik/Kendo UI widgets used by the legacy MVC UI.


## 3) Reference data list (shared reference data / data lists)

### What this is
Reference data lists are shared “lookups” that populate dropdowns and standard labels in the UI (for example: protocol types, statuses, yes/no lists, schedules). This data is not the core file-transfer configuration itself; it supports consistent UX and validation.

### External shared component
FXFER depends on a shared **Reference Data / Data List provider** module/component rather than hardcoding lists inside File Transfer. This helps keep lists consistent across modules and allows them to vary per tenant/environment when needed.

### What FXFER uses it for
- Populating UI dropdowns (protocol, status, schedule, rename patterns, module/owner-module lists)
- Validating that user inputs match allowed codes/values
- Retrieving label/value pairs in a tenant-aware way


## 4) Integration event distribution (shared eventing platform)

### What this is
Integration Event Distribution is a shared eventing capability for system-to-system messaging. Modules publish “events” (something happened) and other modules consume them without tight coupling.

### How FXFER uses it
- **FXFER as a consumer:** FXFER receives external integration events (for example, generated file events or updated metadata events) and maps them into FXFER internal actions/state updates.
- **FXFER as a producer:** FXFER publishes events when transfers happen (for example, a “FileTransferred” event) so downstream modules can react.

### How to visualize it
- Events are broadcasts/messages, not direct request/response API calls.
- FXFER relies on shared event contracts (schemas/types) owned outside FXFER.
- FXFER can write event messages into a shared distributed event storage mechanism; FXFER does not implement the backing store/transport.


## 5) Tenant configuration and runtime context

### What this is
Tenant configuration is how the platform runs the same FXFER code for multiple tenants/customers without mixing them. Tenant-specific settings are loaded at runtime and applied consistently across services, batch jobs, notifications, and events.

### What tenant/runtime components provide
- Tenant-scoped configuration values (DB connection/provider, working directories, notification toggles, service endpoints)
- Tenant-aware dependency resolution (resolve the correct provider/service for the tenant)
- Propagation of tenant identity into logs, notifications, and integration events


## 6) External databases

### What this is
FXFER uses an external relational database as both:
- a **configuration store** (what to transfer, where, how)
- an **operational ledger** (what happened, what failed, what was retried)

The database server and credentials live outside this repo; FXFER connects via configured providers and connection strings.

### Core FXFER tables (11)
- `FTP_SERVER` — server definitions used for remote connections.
- `FTS_SOURCE_LOCATION` — source definitions (where to look for files, protocol, masks, schedules).
- `FTS_DESTINATION` — destination definitions (where to deliver files, overwrite/archive/rename rules).
- `FTS_FILE_MANAGEMENT` — master record tracking a file through processing.
- `FTS_FILE_DELIVERY` — per-destination delivery tracking for a file.
- `FTS_FILE_EVENT` — timeline of state changes/milestones for a file.
- `FTS_FILE_EXCEPTION` — captured error/exception details.
- `FTS_SUSPENSION` — suspension/hold state for configs/files.
- `FTS_NOTIFICATION` — notification recipient/config records.
- `FTS_INBOUND_TARGET` — inbound/integration targets tied to metadata.
- `FTS_INTEGRATION_FILE` — integration-file records (often driven by external events).
