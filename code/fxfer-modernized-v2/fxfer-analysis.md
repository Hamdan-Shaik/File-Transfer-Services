# FXFER Analysis - Legacy vs Modernized vs Target State

Status: Draft (in progress)

Progress log:

- Progress (2026-11-10): Rebuilt Sections 1-4 with target-state citations (`tsd-work/target-state-fxfer-v2.md`).
- Progress (2026-11-10): Added Sections 5-10 (Business Behavior -> Eventing).
- Progress (2026-11-10): Added Sections 11-15 (Observability -> Executive Summary).

This document is a **3-way comparison** of:

- **Legacy** implementation behavior (source-of-truth: `fxfer-documentation/` + legacy code when needed)
- **Modernized** codebase behavior (source-of-truth: `bff/`, `services/`, `workers/`, `ux/`, `docs/`)
- **Target State v2** expectations (source-of-truth: `tsd-work/target-state-fxfer-v2.md`)

For every concrete claim, include at least one workspace-relative anchor (file path + `:line` where feasible).

---

## 1. Document Setup (Scope, Sources, Precedence)

Status: Partial

**What this section is about**

Define the scope of this analysis and how to treat sources when they disagree.

**What we looked at**

- Template structure: `analysis-template.md:15`
- Run guidance: `prompt.txt:1`
- Target state expectations: `tsd-work/target-state-fxfer-v2.md:172`
- Modernized facts ledger (observed): `modernized-facts.md:1`
- Legacy baseline docs: `fxfer-documentation/` (example inventory: `fxfer-documentation/shared-external-inventory.md:7`)

**What we found**

- The target-state document includes an explicit capability list (`CAP-FXFER-xxx`) and a target container/runtime view for UI/BFF/Domain APIs/Event Backbone/Workers. `tsd-work/target-state-fxfer-v2.md:190`, `tsd-work/target-state-fxfer-v2.md:862`
- The modernized repo implements a UI, BFF, domain service, projections, and two workers, but some eventing/worker wiring is still scaffolded. `modernized-facts.md:35`, `modernized-facts.md:160`
- The legacy baseline exposes similar domain concepts but via a broader set of runtime surfaces (WCF/SOAP + UX services + batch executables). `fxfer-documentation/shared-external-inventory.md:7`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Source of truth for expectations | Not applicable (legacy is "what is") | Not applicable (modernized is "what is") | `tsd-work/target-state-fxfer-v2.md` | Same | Target state is the expectation lens. `analysis-template.md:9` |
| Target-state availability in repo | N/A | N/A | Present under `tsd-work/` | Same | File exists and is now used for Sections 1-4. `tsd-work/target-state-fxfer-v2.md:2` |

**Disposition**

- Partial: Section 1 establishes sources and precedence; deeper parity calls happen in later sections.

**Evidence**

- `analysis-template.md:15`
- `prompt.txt:1`
- `tsd-work/target-state-fxfer-v2.md:190`
- `tsd-work/target-state-fxfer-v2.md:862`
- `modernized-facts.md:35`
- `fxfer-documentation/shared-external-inventory.md:7`

### 1.1 Purpose

This analysis answers:

- How much of **legacy behavior** is preserved in the modernized system.
- What changed intentionally to align to the **target state v2**.
- What changed unexpectedly (potential regressions/drift).
- What is missing vs legacy and/or vs target state.

### 1.2 In Scope

- Sections 1-4 from `analysis-template.md` (setup, repo/service graph, runtime entrypoints, capability mapping). `analysis-template.md:15`
- Legacy behavior as described in `fxfer-documentation/` plus referenced legacy code under `mms-cms-fxfer/` where needed. `fxfer-documentation/api.md:4`
- Modernized behavior as described in `modernized-facts.md` and evidenced by code/config under `bff/`, `services/`, `workers/`, `ux/`. `modernized-facts.md:35`
- Target-state expectations from `tsd-work/target-state-fxfer-v2.md`. `tsd-work/target-state-fxfer-v2.md:172`

### 1.3 Out of Scope

- Rule-by-rule business behavior parity (template Sections 5+). `analysis-template.md:115`
- Data model/persistence parity and migration (template Sections 7+). `analysis-template.md:169`

### 1.4 Compared Artifacts

| Artifact | What it represents | Reference |
|---|---|---|
| Legacy current-state docs | Legacy behavior inventory baseline | `fxfer-documentation/` |
| Legacy code snapshot | Legacy implementation evidence when docs are insufficient | `mms-cms-fxfer/` |
| Modernized codebase | Implemented behavior in modernized repo | `bff/`, `services/`, `workers/`, `ux/`, `docs/` |
| Target State v2 | Desired/approved future state | `tsd-work/target-state-fxfer-v2.md` |

### 1.5 Source Precedence (Conflict Handling)

When sources disagree:

1. **Target state** wins for "what should be true". `analysis-template.md:9`
2. **Running modernized code/config** wins for "what is implemented today" (facts ledger is a summary, not an authority). `modernized-facts.md:35`
3. For legacy, prefer `fxfer-documentation/` over legacy code scans (use code only when docs are insufficient). `fxfer-documentation/shared-external-inventory.md:5`

---
## 2. Repo / Service Graph (Is the Modernized Code Connected?)

Status: Partial

**What this section is about**

Identify the modernized deployables and how they connect (HTTP vs events vs DB), then compare that to the target-state container model.

Definitions:

- **BFF**: Backend for Frontend - an API tailored for the UI.
- **Event backbone**: The asynchronous messaging layer (target state references SNS/SQS). `tsd-work/target-state-fxfer-v2.md:898`

**What we looked at**

- Target-state container model (C2) and interaction matrix: `tsd-work/target-state-fxfer-v2.md:862`, `tsd-work/target-state-fxfer-v2.md:1042`
- Modernized component inventory and interactions: `modernized-facts.md:35`, `modernized-facts.md:129`
- Legacy runtime inventory baseline: `fxfer-documentation/shared-external-inventory.md:7`

**What we found**

- **Target state expects** UI -> BFF -> Domain APIs, with an event backbone and transfer workers, and tenant-routed PostgreSQL. `tsd-work/target-state-fxfer-v2.md:868`, `tsd-work/target-state-fxfer-v2.md:1042`
- **Modernized implements** UI (Next.js), BFF (.NET), domain service, projections process, and two workers (scheduler + execution). `modernized-facts.md:35`
- **Modernized event backbone wiring is partial**: contracts exist, but adapter/outbox and broker wiring are not fully evidenced end-to-end. `modernized-facts.md:153`, `modernized-facts.md:174`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| High-level component shape | Multiple legacy hosts (WCF/SOAP, UX services/presentation, batch executables). `fxfer-documentation/shared-external-inventory.md:7` | UI + BFF + domain service + projections + workers. `modernized-facts.md:35` | UI + BFF + Domain APIs + Event backbone + Worker(s) + PostgreSQL. `tsd-work/target-state-fxfer-v2.md:868` | Changed (intentional) | Modernized shape is closer to target state than legacy, but has extra/incomplete pieces (e.g., projections process vs target-state components). |
| Event backbone | Legacy has an event distribution surface, but contract is external. `fxfer-documentation/shared-external-inventory.md:57` | AsyncAPI contracts exist; adapter is scaffold/no-op in places. `modernized-facts.md:136` | SNS/SQS pattern is a first-class dependency between APIs and workers. `tsd-work/target-state-fxfer-v2.md:898` | Partial | Modernized has contract artifacts but not full runtime wiring evidenced. |
| Worker model | Batch executables run background transfer processing. `fxfer-documentation/shared-external-inventory.md:27` | Scheduler/execution workers exist but scheduling/publish path is scaffolded. `modernized-facts.md:160` | Transfer Worker(s) consume work items and report outcomes. `tsd-work/target-state-fxfer-v2.md:904` | Partial | Execution intent + durable work items are a target-state requirement (CAP-FXFER-005). `tsd-work/target-state-fxfer-v2.md:202` |

**Disposition**

- Partial: Modernized has the intended component breakdown, but some key connections (event backbone, work-item sourcing) are not fully evidenced.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:862`
- `tsd-work/target-state-fxfer-v2.md:1042`
- `modernized-facts.md:35`
- `modernized-facts.md:136`
- `fxfer-documentation/shared-external-inventory.md:7`

**Evidence gaps**

- **TBD (not evidenced):** Modernized real broker adapter wiring (SNS/SQS/etc) used by scheduler/execution workers. `modernized-facts.md:174`
- **TBD (not evidenced):** Modernized end-to-end mechanism that creates durable execution intent/work items (target-state CAP-FXFER-005). `tsd-work/target-state-fxfer-v2.md:202`

### 2.1 What to Capture

- Deployables, libraries, and entry points.
- Composition roots (DI/service registration), route registration, background job scheduling, message subscriptions.
- Call paths between components (sync vs async).

### 2.2 Container / Component Inventory

| Component | Type (UI/BFF/API/Worker/Lib) | Entry point | Calls into | Notes |
|---|---|---|---|---|
| `fxfer-remote` | UI | `ux/fxfer-remote/package.json:6` | BFF | Target state expects a UI micro-frontend. `tsd-work/target-state-fxfer-v2.md:870` |
| `fxfer-bff` | BFF | `bff/Fxfer.Bff/Program.cs:21` | `fxfer-service` | Target state expects a BFF that normalizes/aggregates browser-safe API. `tsd-work/target-state-fxfer-v2.md:877` |
| `fxfer-service` | API | `services/fxfer-service/FxferService.Host/Program.cs:22` | Postgres; event adapter | Target state expects command + query endpoints and tenant-routed DB. `tsd-work/target-state-fxfer-v2.md:886` |
| `fxfer-projections` | API/Worker | `services/fxfer-service/FxferService.Projections/Program.cs:1` | Inbox store (local DB) | Target state calls out projections/read models; final deployable boundaries are decision-driven. `tsd-work/target-state-fxfer-v2.md:864` |
| `fxfer-scheduler-worker` | Worker | `workers/fxfer-scheduler-worker/Program.cs:5` | Intended publish | Target state expects scheduling/trigger and redrive controls. `tsd-work/target-state-fxfer-v2.md:858` |
| `fxfer-execution-worker` | Worker | `workers/fxfer-execution-worker/Program.cs:4` | Intended consume + domain API | Target state expects protocol I/O worker(s) with controlled egress. `tsd-work/target-state-fxfer-v2.md:905` |

### 2.3 Interaction Matrix (High Value)

| Caller | Callee | Mode (HTTP/Event/DB) | Purpose | Evidence |
|---|---|---|---|---|
| UI | BFF | HTTP | Operator UI calls browser-safe endpoints | Target state: `tsd-work/target-state-fxfer-v2.md:874`; Modern: `modernized-facts.md:133` |
| BFF | Domain APIs | HTTP (private) | Forward commands/queries; normalize errors | Target state: `tsd-work/target-state-fxfer-v2.md:1043`; Modern: `modernized-facts.md:134` |
| Domain APIs | PostgreSQL | DB | Tenant-routed persistence | Target state: `tsd-work/target-state-fxfer-v2.md:1044`; Modern: `modernized-facts.md:135` |
| Domain APIs | Event backbone | Event | Publish config/outcome events | Target state: `tsd-work/target-state-fxfer-v2.md:1046`; Modern (scaffold): `modernized-facts.md:136` |
| Workers | External endpoints | Protocol I/O | Perform transfers with controlled egress | Target state: `tsd-work/target-state-fxfer-v2.md:1048`; Legacy equivalent: `fxfer-documentation/flow.md:97` |

### 2.4 Alignment to Target State

Disposition: Partial

---

## 3. Runtime Surfaces & Entry Points (What Actually Runs)

Status: Partial

**What this section is about**

List each runtime surface that receives work (HTTP, SOAP, batch executables, workers) so later parity checks can anchor to "what actually runs".

Definitions:

- **Runtime surface**: a process endpoint that receives input (HTTP request, SOAP call, message, or scheduled/CLI execution).

**What we looked at**

- Target-state runtime view: `tsd-work/target-state-fxfer-v2.md:1052`
- Target-state trust boundaries: `tsd-work/target-state-fxfer-v2.md:852`
- Modern runnable processes: `modernized-facts.md:80`
- Legacy runtime entry points: `fxfer-documentation/shared-external-inventory.md:7`

**What we found**

- Target state assumes browser -> BFF as the main interactive entrypoint and workers as protocol I/O runtimes. `tsd-work/target-state-fxfer-v2.md:852`, `tsd-work/target-state-fxfer-v2.md:905`
- Modernized provides container-friendly HTTP processes with `/health/live` and `/health/ready`. `modernized-facts.md:80`
- Legacy includes multiple SOAP/WCF and UX-specific endpoints, plus console batch executables. `fxfer-documentation/shared-external-inventory.md:9`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Interactive entrypoint | UX Presentation + UX Services + WCF are all part of call surface. `fxfer-documentation/shared-external-inventory.md:18` | UI micro-frontend calls BFF routes under `/api/v1/*`. `modernized-facts.md:109` | UI micro-frontend calls BFF; browser->BFF is the main boundary. `tsd-work/target-state-fxfer-v2.md:852` | Changed (intentional) | Target state explicitly frames browser->BFF as primary boundary. |
| SOAP/WCF | Present | Not observed (modernized HTTP-first) | Target state says no new SOAP/WCF dependencies. `tsd-work/target-state-fxfer-v2.md:859` | Changed (intentional) | Legacy migration posture is a target-state decision. |
| Background execution | Console batch executables | Worker processes exist but planner/publisher is scaffolded | Transfer worker(s) consume work items and execute protocol I/O | Partial | Modernized worker runtime exists but end-to-end scheduling/intent is incomplete. `modernized-facts.md:160` |

**Disposition**

- Partial: modernized runtime surfaces align with target shape, but worker feed/backbone wiring is not fully evidenced.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:852`
- `tsd-work/target-state-fxfer-v2.md:859`
- `tsd-work/target-state-fxfer-v2.md:1052`
- `modernized-facts.md:80`
- `fxfer-documentation/shared-external-inventory.md:7`

**Evidence gaps**

- **TBD (not evidenced):** Modernized runtime uses an actual event backbone as described in target state. `tsd-work/target-state-fxfer-v2.md:898`

### 3.1 Inventory

| Surface | Protocol | Auth context | Tenant context | Purpose | Evidence |
|---|---|---|---|---|---|
| Target UI micro-frontend | Browser | Platform identity broker issues JWT | JWT contains tenant context | Operator UI | `tsd-work/target-state-fxfer-v2.md:870`, `tsd-work/target-state-fxfer-v2.md:855` |
| Target BFF | HTTP | JWT validation | Tenant + correlation propagated | Browser-safe API surface | `tsd-work/target-state-fxfer-v2.md:877` |
| Target Domain APIs | HTTP | Service identity (platform) | Tenant-routed DB, tenant in requests | Commands/queries + persistence | `tsd-work/target-state-fxfer-v2.md:886` |
| Target Worker(s) | Event + protocol I/O | Service identity | Tenant from envelope | Execute transfers + report outcomes | `tsd-work/target-state-fxfer-v2.md:905`, `tsd-work/target-state-fxfer-v2.md:1049` |
| `fxfer-bff` | HTTP | JWT bearer configured | JWT claim `tenant_id` required (when authenticated) | External API surface for UI/operators | `modernized-facts.md:82`, `modernized-facts.md:236` |
| `fxfer-service` | HTTP | TBD (not evidenced) | Tenant DB routing present | Domain APIs + persistence | `modernized-facts.md:83`, `modernized-facts.md:118` |
| Legacy WCF SOAP hosts | SOAP/WCF | TBD (not evidenced) | TBD (not evidenced) | Command/query/event distribution service operations | `fxfer-documentation/shared-external-inventory.md:9` |
| Legacy batch executables | Console | N/A | Varies | Background file movement engine | `fxfer-documentation/shared-external-inventory.md:27` |

### 3.2 Comparison

- Legacy anchors: `fxfer-documentation/api.md:6`, `fxfer-documentation/shared-external-inventory.md:7`
- Target-state anchors: `tsd-work/target-state-fxfer-v2.md:1052` (runtime view)

Disposition: Partial

---
## 4. Capability Parity (Business Capabilities)

Status: Partial

**What this section is about**

Map business capabilities (CAP-FXFER-xxx) across legacy vs modernized vs target state.

Definitions:

- **Capability**: an operator-visible unit of value (e.g., manage configuration, execute transfers, track outcomes).

**What we looked at**

- Target-state capability table: `tsd-work/target-state-fxfer-v2.md:196`
- Legacy callable surface: `fxfer-documentation/shared-external-inventory.md:38`
- Modernized BFF/API surfaces and flows: `modernized-facts.md:219`, `modernized-facts.md:141`

**What we found**

- Target state defines 10 top-level capabilities and labels each as Preserve/Change. `tsd-work/target-state-fxfer-v2.md:196`
- Modernized clearly supports config management and tracking/search via BFF + service APIs.
- Modernized scheduling/execution and event publication capabilities exist as scaffolding/contracts, but are not fully evidenced end-to-end. `modernized-facts.md:160`, `modernized-facts.md:153`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Capability identifiers | Not standardized as CAP IDs in legacy docs | Not standardized as CAP IDs in code | CAP-FXFER-001..010 defined | Same | Target-state list is used as the mapping frame. `tsd-work/target-state-fxfer-v2.md:196` |

**Disposition**

- Partial: enough evidence exists to map config + tracking; execution intent/workers/event backbone are only partially evidenced.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:196`
- `fxfer-documentation/shared-external-inventory.md:38`
- `modernized-facts.md:221`
- `modernized-facts.md:160`

**Evidence gaps**

- **TBD (not evidenced):** target-state credential rotation/Secrets Manager posture implemented end-to-end in modernized. `tsd-work/target-state-fxfer-v2.md:201`

### 4.1 Capability Mapping Table

| Capability | Legacy behavior | Modernized behavior | Target-state expectation | Disposition | Evidence |
|---|---|---|---|---|---|
| CAP-FXFER-001 Manage source locations | Source location create/update/invalidate operations exist. | Transfer configuration endpoints exist in BFF and service (search/get/update patterns). | Master transfer configuration record supports enable/disable + suspension state. | Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:198`; Legacy ops: `fxfer-documentation/shared-external-inventory.md:40`; Modern route: `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:13` |
| CAP-FXFER-002 Manage destinations | Destination config screens/endpoints exist (legacy UX). | Destination-specific surfaces not yet mapped here. | One or more delivery targets per source location. | TBD | Target definition: `tsd-work/target-state-fxfer-v2.md:199` |
| CAP-FXFER-003 Manage endpoint connection profiles | FTP server creds and server details exist. | `api/v1/ftp-servers` exists (BFF forwards). | Maintain reusable endpoint definitions, including secret references. | Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:200`; Legacy: `fxfer-documentation/shared-external-inventory.md:43`; Modern: `modernized-facts.md:223` |
| CAP-FXFER-004 Manage credential references and rotation | Legacy stores credentials (details not validated here). | Modernized secret management/rotation not evidenced in Sections 1-4. | Safe rotation and least privilege via secret references. | TBD | Target definition: `tsd-work/target-state-fxfer-v2.md:201` |
| CAP-FXFER-005 Schedule and trigger execution intent | Batch execution exists; scheduling is outside the service boundary. | Scheduler worker exists but produces no work items (as evidenced). | Create durable execution intent/work items. | Missing/Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:202`; Modern gap: `modernized-facts.md:164` |
| CAP-FXFER-006 Execute transfers (worker runtime) | Batch executables perform protocol I/O (download/upload/delete) with retries. | Execution worker exists; end-to-end feed is TBD. | Worker(s) perform protocol operations with bounded retries and safe staging. | Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:203`; Legacy batch: `fxfer-documentation/flow.md:97`; Modern skeleton: `modernized-facts.md:170` |
| CAP-FXFER-007 Track outcomes, events, and exceptions | Tracking and movement search endpoints exist. | BFF exposes tracked file search. | Record per-file/per-destination outcomes and exceptions for operator search. | Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:204`; Legacy: `fxfer-documentation/shared-external-inventory.md:53`; Modern: `bff/Fxfer.Bff/src/Api/Controllers/FileTransfersController.cs:12` |
| CAP-FXFER-008 Suspend and (optionally) auto-unsuspend | Suspensions exist as a domain concept in legacy batch/service. | Modern suspension mechanics not evidenced in Sections 1-4. | Suspension as explicit operational state with history. | TBD | Target definition: `tsd-work/target-state-fxfer-v2.md:205` |
| CAP-FXFER-009 Notifications of operational conditions | Legacy has notification integration points (details not mapped here). | Modernized notification delivery not evidenced end-to-end. | Event-driven notification/integration posture. | TBD | Target definition: `tsd-work/target-state-fxfer-v2.md:206` |
| CAP-FXFER-010 Publish integration events | Legacy has event distribution surface. | AsyncAPI exists; publish adapter/outbox insertion unclear. | Publish tenant-aware schema-versioned events for config + outcomes. | Partial | Target definition: `tsd-work/target-state-fxfer-v2.md:207`; Modern contracts: `modernized-facts.md:323`; Legacy surface: `fxfer-documentation/shared-external-inventory.md:57` |

### 4.2 Notes

- Target state marks some capabilities as "Change" (credential rotation, execution intent scheduling, notifications, event publication). `tsd-work/target-state-fxfer-v2.md:201`

---

## 5. Business Behavior Parity (Rules, Idempotency, Duplicates, Retention)

Status: Partial

**What this section is about**

Compare the operational rules that govern FXFER behavior across legacy, modernized, and target state.

Definitions:

- **Idempotency**: repeating the “same” operation (same inputs / same intent) does not create additional side effects (for example: duplicate deliveries or duplicate tracking records).
- **Duplicate handling**: rules that decide when a file/config is considered “already processed” and what the system does next (skip, record duplicate, rename, or require operator redrive).
- **Retention / expiry**: how long tracking/audit/artifacts are kept before archival or deletion.

**What we looked at**

- Target-state rule catalog + lifecycle: `tsd-work/target-state-fxfer-v2.md:299`, `tsd-work/target-state-fxfer-v2.md:402`, `tsd-work/target-state-fxfer-v2.md:492`
- Legacy command/worker behaviors (validation, duplicates, retry): `fxfer-documentation/api.md:72`, `fxfer-documentation/lld/w6-batch-transfer-engine.md:76`
- Legacy persistence hints for audit/idempotency flags: `fxfer-documentation/data-models.md:102`, `fxfer-documentation/data-models.md:306`
- Modernized facts ledger (summary): `modernized-facts.md:305`, `modernized-facts.md:338`
- Modernized code evidence (rules + retry skeletons): `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:118`, `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`

**What we found**

- Target state defines explicit rule categories (configuration, execution/idempotency/duplicates, tracking/audit/redaction) and expects rules to be auditable. `tsd-work/target-state-fxfer-v2.md:299`, `tsd-work/target-state-fxfer-v2.md:529`
- Legacy has concrete duplicate-handling and retry gating behaviors in the batch transfer engine (for example: skipping already-renamed duplicates and retry gating by “minutes between retry”). `fxfer-documentation/lld/w6-batch-transfer-engine.md:88`, `fxfer-documentation/lld/w6-batch-transfer-engine.md:92`
- Modernized preserves some legacy business rules at the command/query level (example: duplicate FTP server name detection). `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:125`
- Modernized execution retry behavior is currently “at-least-once by re-enqueue on failure” with a configurable delay, but bounded retry / backoff policy is not evidenced. `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`, `workers/fxfer-execution-worker/src/Host/Hosting/WorkItemRetryOptions.cs:3`
- Target-state retention windows (tracking hot store, audit archive, staging/artifacts) are defined by decisions; no equivalent retention enforcement is evidenced in modernized or legacy docs available here. `tsd-work/target-state-fxfer-v2.md:106`, `tsd-work/target-state-fxfer-v2.md:602`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Duplicate FTP server name validation | Duplicate name check described for `AddFtpServer(...)` | Duplicate name check is enforced in command handler | Duplicate prevention expected as part of configuration rules | Same | Legacy behavior documented: `fxfer-documentation/api.md:84`; Modern rule: `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:125` |
| Worker retry posture | Batch producer/consumer applies retry gating and retry wait settings | Worker re-enqueues failed work items with a simple delay | Bounded retries + backoff + operator-visible history | Changed (unexpected) | Modern lacks bounded attempts evidence; target expects bounded retries. `fxfer-documentation/lld/w6-batch-transfer-engine.md:92`, `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`, `tsd-work/target-state-fxfer-v2.md:329` |
| Retention/expiry | TBD (not evidenced) | TBD (not evidenced) | Explicit retention windows (tracking/audit/artifacts) | Missing | Target defines expectations; code/docs do not evidence implementation. `tsd-work/target-state-fxfer-v2.md:106` |

**Disposition**

- Partial: configuration validation and some duplicate behaviors are evidenced; end-to-end idempotency/retention and bounded retry policy are not.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:299`
- `tsd-work/target-state-fxfer-v2.md:329`
- `tsd-work/target-state-fxfer-v2.md:492`
- `fxfer-documentation/api.md:84`
- `fxfer-documentation/lld/w6-batch-transfer-engine.md:88`
- `fxfer-documentation/lld/w6-batch-transfer-engine.md:92`
- `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:125`
- `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`

**Evidence gaps**

- **TBD (not evidenced):** where/if modernized enforces command idempotency using `Idempotency-Key` (beyond passing the header). `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:103`
- **TBD (not evidenced):** modernized bounded retry policy (max attempts, exponential backoff with jitter, per-error classification). Target expectation: `tsd-work/target-state-fxfer-v2.md:504`
- **TBD (not evidenced):** any automated retention/expiry enforcement for tracking/outbox/inbox/diagnostics artifacts. Target expectations: `tsd-work/target-state-fxfer-v2.md:106`, `tsd-work/target-state-fxfer-v2.md:602`
- **TBD (not evidenced):** redaction/classification logic for operator-visible tracking (target-state `Decision (DD-FXFER-016)`). `tsd-work/target-state-fxfer-v2.md:320`

### 5.1 Rules Catalogue Mapping

| Rule area | Legacy | Modernized | Target-state | Disposition | Evidence |
|---|---|---|---|---|---|
| Configuration validation | Command handlers validate required fields; duplicate name checks exist (example: FTP server) | `command.Validate()` is executed in command handlers; validation errors are returned as structured `ProblemDetails` | Explicit configuration rule catalog (`BR-xxx`) expected and auditable | Partial | Legacy: `fxfer-documentation/api.md:72`; Modern: `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:118`; Target: `tsd-work/target-state-fxfer-v2.md:299` |
| Idempotency & duplicates | Batch includes duplicate rename filtering and per-file retry gating; outbound integration file table has `IS_PROCESSED` for reprocessing decisions | Duplicate FTP server name validation exists; projections consumer has an inbox/idempotency store; API exposes `Idempotency-Key` header but enforcement is TBD | Duplicate discovery must not cause re-delivery; idempotency enforced at command boundary | Partial | Legacy: `fxfer-documentation/lld/w6-batch-transfer-engine.md:88`, `fxfer-documentation/data-models.md:310`; Modern: `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`, `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:103`; Target: `tsd-work/target-state-fxfer-v2.md:329`, `tsd-work/target-state-fxfer-v2.md:332` |
| Retry/backoff/timeouts | Retry gating includes configurable wait time (“minutes between retry”) | Worker retry is re-enqueue + optional fixed delay | Exponential backoff + bounded retries; attempts recorded as operator-visible history | Changed (unexpected) | Legacy: `fxfer-documentation/lld/w6-batch-transfer-engine.md:92`; Modern: `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`; Target: `tsd-work/target-state-fxfer-v2.md:509`, `tsd-work/target-state-fxfer-v2.md:563` |
| Tracking/audit/redaction | Tracking tables include audit fields (operator + timestamps) and a file event/exception timeline model | Tracking/audit columns are mapped in modernized EF model; redaction logic is not evidenced | Tracking/audit narrative is required; redaction must be explicit and conservative by default | Partial | Legacy: `fxfer-documentation/data-models.md:173`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:178`; Target: `tsd-work/target-state-fxfer-v2.md:340`, `tsd-work/target-state-fxfer-v2.md:320` |
| Retention/expiry | TBD (not evidenced) | TBD (not evidenced) | Explicit retention windows (tracking/audit/artifacts) | Missing | Target: `tsd-work/target-state-fxfer-v2.md:106` |

### 5.2 Lifecycle / Workflow Parity

What this means (in practice): operators need predictable “what happens next” behavior when they create/update configuration, trigger execution, and investigate outcomes.

Observed lifecycle anchors:

- Legacy end-to-end flow narrative: `fxfer-documentation/flow.md:1`
- Target-state lifecycle overview: `tsd-work/target-state-fxfer-v2.md:404`

High-level lifecycle comparison (externally observable):

1. **Configuration change**
   - Legacy: UI/UX Services → service command handler persists config. `fxfer-documentation/api.md:10`
   - Modernized: UI → BFF `api/v1/*` → FxferService command route persists config. `modernized-facts.md:145`, `services/fxfer-service/FxferService.Host/src/Api/Commands/Controllers/FtpServersCommandsController.cs:14`
   - Target state: same conceptual flow, but emphasizes auditability + event backbone emission. `tsd-work/target-state-fxfer-v2.md:440`, `tsd-work/target-state-fxfer-v2.md:1679`

2. **Execution scheduling / intent creation**
   - Legacy: batch exe is invoked on a schedule (outside the service boundary). `fxfer-documentation/api.md:66`
   - Modernized: scheduler + execution worker exist, but scheduler→execution linkage is not evidenced end-to-end. `modernized-facts.md:355`, `modernized-facts.md:176`
   - Target state: durable `transfer_run` intent is created and is explicitly auditable. `tsd-work/target-state-fxfer-v2.md:102`

3. **Transfer execution and tracking**
   - Legacy: batch producer/consumer processes work and records outcomes into tracking tables. `fxfer-documentation/lld/w6-batch-transfer-engine.md:97`
   - Modernized: execution worker processes `TransferRunRequested` envelopes with retry-by-reenqueue; per-attempt history is not evidenced. `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`
   - Target state: attempt history must be durable and operator-visible; retries bounded. `tsd-work/target-state-fxfer-v2.md:340`, `tsd-work/target-state-fxfer-v2.md:329`

Disposition: Partial

---

## 6. API / Endpoint Parity (Do Seams Work the Same Way?)

Status: Partial

**What this section is about**

Compare the callable surfaces (HTTP / SOAP / messages) between legacy and modernized, and check whether modernized meets target-state â€œseamâ€ expectations.

Definitions:

- **Surface / seam**: a network-callable boundary (for example: an HTTP route or a WCF/SOAP operation) that another component or client depends on.
- **Contract compatibility**: whether callers can make the same kinds of requests and get equivalent results and error shapes.

**What we looked at**

- Legacy API inventory and runtime surfaces: `fxfer-documentation/api.md:40`, `fxfer-documentation/wcfs.md:1`
- Target-state BFF route taxonomy and contract governance posture: `tsd-work/target-state-fxfer-v2.md:1528`, `tsd-work/target-state-fxfer-v2.md:1655`
- Modernized BFF controllers (operator-facing seam): `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:9`, `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:9`
- Modernized error envelope normalization: `modernized-facts.md:241`

**What we found**

- Legacy exposes multiple seams (SOAP/WCF command/query/event distribution; UX Services HTTP; batch executable). `fxfer-documentation/api.md:40`, `fxfer-documentation/api.md:66`
- Modernized primarily exposes an operator-authenticated HTTP surface via the BFF at `api/v1/*`, which forwards to FxferService. `modernized-facts.md:112`, `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:9`
- Target-state route naming is explicitly â€œplaceholder taxonomyâ€ and a decision item; therefore, modernized `/api/v1/*` naming is not provably wrong, but should be treated as provisional until `Decision (DD-FXFER-033)` is resolved. `tsd-work/target-state-fxfer-v2.md:1528`, `tsd-work/target-state-fxfer-v2.md:1536`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Primary operator API seam | UX Services HTTP endpoints + WCF behind | BFF HTTP routes (`api/v1/*`) | BFF treated as explicit contract; prefix/name not finalized | Changed (intentional) | Modern BFF is the new â€œfront doorâ€; target state expects BFF. `fxfer-documentation/api.md:60`, `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:9`, `tsd-work/target-state-fxfer-v2.md:1528` |
| Command vs query separation | WCF has separate command vs query services | FxferService has separate commands and queries controllers (same URI group, different verbs) | Target expects logical CQRS (command/write vs query/read separation) | Same | Target CQRS: `tsd-work/target-state-fxfer-v2.md:101`; Modern: `services/fxfer-service/FxferService.Host/src/Api/Commands/Controllers/FtpServersCommandsController.cs:14`, `services/fxfer-service/FxferService.Host/src/Api/Queries/Controllers/FtpServersQueriesController.cs:17` |
| Error shape | Legacy depends on WCF fault/contract patterns (details not fully evidenced here) | Modern returns `ProblemDetails` + `ErrorEnvelope` | Target expects structured, actionable errors with safe redaction | Partial | Modern error normalization: `modernized-facts.md:241`; Target error posture: `tsd-work/target-state-fxfer-v2.md:547` |

**Disposition**

- Partial: modernized endpoints exist and are callable, but full legacyâ†’modern mapping (including tracking/execution seams) and strict contract compatibility (error taxonomy, pagination semantics) is not completely evidenced.

**Evidence**

- `fxfer-documentation/api.md:40`
- `fxfer-documentation/api.md:60`
- `fxfer-documentation/api.md:66`
- `tsd-work/target-state-fxfer-v2.md:1528`
- `modernized-facts.md:112`
- `modernized-facts.md:241`
- `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:9`
- `services/fxfer-service/FxferService.Host/src/Api/Commands/Controllers/FtpServersCommandsController.cs:14`

**Evidence gaps**

- **TBD (not evidenced):** a complete authoritative list of legacy â€œevent distributionâ€ operations (defined in external base class). `fxfer-documentation/api.md:52`
- **TBD (not evidenced):** modernized public seam for â€œexecution intentâ€ creation and run outcome retrieval (target-state `transfer_run` intent model). Target: `tsd-work/target-state-fxfer-v2.md:102`

### 6.1 Endpoint Inventory (Modernized)

| Endpoint / Message | Type (HTTP/Event/etc.) | Purpose | Consumers | Evidence |
|---|---|---|---|---|
| `GET api/v1/ftp-servers` | HTTP | Search/list FTP server profiles (optionally include password) | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:21` |
| `GET api/v1/ftp-servers/selection` | HTTP | Selection list for dropdowns | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:66` |
| `POST api/v1/ftp-servers` | HTTP | Create FTP server profile | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:99` |
| `PUT api/v1/ftp-servers/{ftpServerId}` | HTTP | Update FTP server profile | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:121` |
| `DELETE api/v1/ftp-servers/{ftpServerId}` | HTTP | Soft-invalidate FTP server profile | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:143` |
| `GET api/v1/transfer-configurations` | HTTP | Search/list transfer configurations (source locations) | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:20` |
| `GET api/v1/transfer-configurations/{id}` | HTTP | Load config detail | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:103` |
| `POST api/v1/transfer-configurations` | HTTP | Create/update config | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:126` |
| `DELETE api/v1/transfer-configurations/{id}` | HTTP | Soft-invalidate config | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/TransferConfigurationsController.cs:149` |
| `POST api/v1/file-transfers/search` | HTTP | Search tracked file transfers | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/FileTransfersController.cs:19` |
| `GET api/v1/integration-files` | HTTP | Search outbound integration file rows | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/IntegrationFilesController.cs:20` |
| `PUT api/v1/integration-files/{id}` | HTTP | Update integration file record | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/IntegrationFilesController.cs:53` |
| `GET api/v1/inbound-targets/search` | HTTP | Resolve inbound target metadata | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/InboundTargetsController.cs:20` |
| `GET api/v1/modules/selection` | HTTP | List module reference data | UX operator screens | `bff/Fxfer.Bff/src/Api/Controllers/ModulesController.cs:20` |

### 6.2 Legacy â†’ Modernized Mapping

| Legacy surface | Modernized surface | Compatibility notes | Disposition | Evidence |
|---|---|---|---|---|
| WCF SOAP command ops (example: `AddFtpServer(...)`) | `POST api/v1/ftp-servers` (BFF â†’ FxferService) | Transport changes (SOAPâ†’HTTP/JSON). â€œDuplicate nameâ€ validation still exists. | Changed (intentional) | Legacy: `fxfer-documentation/api.md:72`, `fxfer-documentation/api.md:84`; Modern: `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:99`, `services/fxfer-service/FxferService.Host/src/Application/CommandHandlers/FtpServerCommandHandler.cs:125` |
| WCF SOAP query ops (FTP server search/list) | `GET api/v1/ftp-servers` | Paging/sort parameters exist; contract shape differs from SOAP DTOs | Changed (intentional) | Legacy query surface: `fxfer-documentation/api.md:48`; Modern: `services/fxfer-service/FxferService.Host/src/Api/Queries/Controllers/FtpServersQueriesController.cs:23` |
| UX Services tracking search endpoints | `POST api/v1/file-transfers/search` | Modern uses POST body filters; legacy UX services shape not mapped field-for-field here | Partial | Modern: `bff/Fxfer.Bff/src/Api/Controllers/FileTransfersController.cs:19`; Legacy UX services seam: `fxfer-documentation/api.md:122` |
| Batch executable invocation | Scheduler + execution workers | Worker linkage is not evidenced; legacy had explicit scheduling outside service | Missing | Legacy: `fxfer-documentation/api.md:66`; Modern gaps: `modernized-facts.md:176` |

### 6.3 Contract Compatibility Notes

Observed differences / compat considerations:

- **Transport and shape**: legacy uses SOAP/WCF and UX Services; modernized uses HTTP `ProblemDetails` + JSON models. `fxfer-documentation/api.md:42`, `modernized-facts.md:241`
- **Idempotency key support**: modernized BFF accepts `Idempotency-Key` header, but enforcement is not evidenced. `bff/Fxfer.Bff/src/Api/Controllers/FtpServersController.cs:103`
- **Route naming**: target state route taxonomy is placeholder; modernized uses `/api/v1/*`. Treat as provisional until `Decision (DD-FXFER-033)`. `tsd-work/target-state-fxfer-v2.md:1528`, `tsd-work/target-state-fxfer-v2.md:1536`

Anchors:

- Legacy anchor: `fxfer-documentation/api.md:70`
- Legacy WCF anchor (if applicable): `fxfer-documentation/wcfs.md:1`
- Target-state anchors: `tsd-work/target-state-fxfer-v2.md:1528`

Disposition: Partial

---

## 7. Data Model & Persistence Parity

Status: Partial

**What this section is about**

Compare how FXFER concepts are persisted (tables/entities) across legacy, modernized, and target state.

Definitions:

- **Persistence model**: the database tables/entities that store configuration, tracking, and audit information.
- **Projection / read model**: a query-optimized shape derived from events (target-state CQRS posture).

**What we looked at**

- Legacy data model inventory: `fxfer-documentation/data-models.md:102`
- Target-state data architecture (Aurora PostgreSQL per tenant; CQRS posture): `tsd-work/target-state-fxfer-v2.md:128`, `tsd-work/target-state-fxfer-v2.md:101`
- Modernized EF mappings and tables: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:13`
- Modernized tenancy DB routing (per tenant): `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33`

**What we found**

- Legacy uses a table set that includes `FTP_SERVER`, `FTS_SOURCE_LOCATION`, `FTS_DESTINATION`, and tracking tables like `FTS_FILE_MANAGEMENT`. `fxfer-documentation/data-models.md:102`, `fxfer-documentation/data-models.md:120`, `fxfer-documentation/data-models.md:173`
- Modernized FxferService maps to the same table names (suggesting a â€œportâ€ of the legacy persistence model) and adds an `FXFER_OUTBOX` table for event publishing. `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:39`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:304`
- Target state expects Aurora PostgreSQL database-per-tenant and CQRS-friendly read models; the concrete table names in the target state are expressed conceptually rather than as a finalized physical schema in this repo. `tsd-work/target-state-fxfer-v2.md:128`, `tsd-work/target-state-fxfer-v2.md:1733`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Primary DB technology posture | Not explicitly stated in this repoâ€™s docs (tables described at logical level) | EF Core model mapped to legacy table names; connection string per tenant is resolved at runtime | Aurora PostgreSQL database-per-tenant is explicit | Partial | Target: `tsd-work/target-state-fxfer-v2.md:128`; Modern per-tenant routing: `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33` |
| Table naming / entity mapping | `FTP_SERVER`, `FTS_SOURCE_LOCATION`, `FTS_DESTINATION`, tracking tables documented | Same tables mapped in `FxferDbContext` | Physical schema details TBD; conceptual entities are defined | Same (legacyâ†”modern), TBD (vs target) | Modern table mappings: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:39`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:65` |
| Outbox/inbox pattern | Not documented as a first-class pattern in legacy docs here | `FXFER_OUTBOX` exists; projections has an inbox store | Target expects event backbone integration with durable outbox/inbox semantics | Partial | Modern outbox: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:304`; Modern inbox: `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`; Target event posture: `tsd-work/target-state-fxfer-v2.md:101` |

**Disposition**

- Partial: core entities are mapped and comparable; target-state physical persistence details (including read-models and retention strategy) are not fully evidenced in the modernized implementation.

**Evidence**

- `fxfer-documentation/data-models.md:102`
- `fxfer-documentation/data-models.md:120`
- `fxfer-documentation/data-models.md:173`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:13`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:304`
- `tsd-work/target-state-fxfer-v2.md:128`
- `tsd-work/target-state-fxfer-v2.md:1733`

**Evidence gaps**

- **TBD (not evidenced):** a modernized read-model/projection store design that matches target-state CQRS expectations (beyond the in-memory consumer skeleton). Target: `tsd-work/target-state-fxfer-v2.md:1733`
- **TBD (not evidenced):** how modernized handles target-state retention windows in storage design (hot store vs archive). Target: `tsd-work/target-state-fxfer-v2.md:106`

### 7.1 Core Entities / Tables

| Domain concept | Legacy persistence | Modernized persistence | Target-state expectation | Disposition | Evidence |
|---|---|---|---|---|---|
| FTP server profile | `FTP_SERVER` | `FTP_SERVER` (EF mapped) | Endpoint definition exists but credentials should be secret references (not raw) | Partial | Legacy: `fxfer-documentation/data-models.md:102`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:39`; Target secret posture: `tsd-work/target-state-fxfer-v2.md:318` |
| Source location / transfer configuration | `FTS_SOURCE_LOCATION` | `FTS_SOURCE_LOCATION` | Source location is the parent config record | Same | Legacy: `fxfer-documentation/data-models.md:120`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:65`; Target: `tsd-work/target-state-fxfer-v2.md:275` |
| Destination | `FTS_DESTINATION` | `FTS_DESTINATION` | One or more delivery targets per source location | Same | Legacy: `fxfer-documentation/data-models.md:147`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:105`; Target: `tsd-work/target-state-fxfer-v2.md:275` |
| File tracking record | `FTS_FILE_MANAGEMENT` | `FTS_FILE_MANAGEMENT` | Per-file tracking record is authoritative and searchable | Same | Legacy: `fxfer-documentation/data-models.md:173`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:178`; Target: `tsd-work/target-state-fxfer-v2.md:280` |
| Integration file (outbound tracking) | `FTS_INTEGRATION_FILE` (`IS_PROCESSED`) | `FTS_INTEGRATION_FILE` | Target state expects tracking narrative + attempt history; integration-file concept not explicitly called out as-is | TBD | Legacy: `fxfer-documentation/data-models.md:306`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:149` |
| Suspension history | `FTS_SUSPENSION` | `FTS_SUSPENSION` | Suspension state + history is an explicit operational state | Same | Legacy: `fxfer-documentation/data-models.md:267`; Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:165`; Target: `tsd-work/target-state-fxfer-v2.md:486` |
| Outbox messages | TBD (not evidenced) | `FXFER_OUTBOX` | Durable publishing to event backbone via outbox is expected | Partial | Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:304`; Target: `tsd-work/target-state-fxfer-v2.md:101` |

### 7.2 Migration / Mapping Notes

- Modernized uses legacy table names; this suggests migration strategy may be â€œlift-and-shift schema + adapt runtimeâ€ rather than â€œnew schemaâ€. `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:39`
- Target state introduces additional concepts like explicit `transfer_run` intent and retention-aware read models; these are not fully evidenced as persisted entities in modernized. `tsd-work/target-state-fxfer-v2.md:102`, `modernized-facts.md:176`

Anchors:

- Legacy anchor: `fxfer-documentation/data-models.md:102`
- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:223`

Disposition: Partial

---

## 8. Tenancy & Isolation Parity

Status: Partial

**What this section is about**

Compare how tenant identity is derived, propagated, and enforced across layers.

Definitions:

- **Tenant context**: the tenant identifier (for example, `tenant_id`) that determines which tenantâ€™s configuration/tracking data is accessed.
- **Isolation**: ensuring one tenant cannot read/write another tenantâ€™s data.

**What we looked at**

- Legacy tenancy mental model: `fxfer-documentation/tentant-isolation.md:17`
- Target-state tenancy expectations (tenant derived from JWT; DB-per-tenant): `tsd-work/target-state-fxfer-v2.md:1359`, `tsd-work/target-state-fxfer-v2.md:1497`
- Modernized tenant derivation in BFF: `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:32`
- Modernized per-tenant DB routing: `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33`
- Modernized worker tenant propagation: `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:44`

**What we found**

- Legacy introduces tenant identity in multiple ways (batch args; service request messages; UX request context). `fxfer-documentation/tentant-isolation.md:33`, `fxfer-documentation/tentant-isolation.md:41`
- Target state expects tenant identity derived from JWT claim and enforced consistently, with Aurora PostgreSQL database-per-tenant routing as a primary isolation mechanism. `tsd-work/target-state-fxfer-v2.md:1359`, `tsd-work/target-state-fxfer-v2.md:1497`
- Modernized BFF rejects authenticated requests that lack `tenant_id` claim and tags logs/spans with tenant and correlation identifiers. `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:32`, `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:64`
- Modernized FxferService resolves a per-tenant connection string using `Tenancy:Tenants:{tenantId}:ConnectionString`; if tenant is missing, it cannot resolve a connection string (enforcement behavior is elsewhere). `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:20`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Where tenant identity comes from | Batch args / request message context / UI context | JWT claim `tenant_id` in BFF; message envelope in workers | JWT claim derived and propagated end-to-end | Changed (intentional) | Modern aligns to target-state â€œtenant from JWT claimâ€. `fxfer-documentation/tentant-isolation.md:33`, `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:32`, `tsd-work/target-state-fxfer-v2.md:1602` |
| DB isolation mechanism | Not fully evidenced here (docs describe tenant config dependency) | Per-tenant connection string resolution | Aurora PostgreSQL database-per-tenant | Partial | Modern: `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33`; Target: `tsd-work/target-state-fxfer-v2.md:128` |

**Disposition**

- Partial: BFF tenant derivation/enforcement is evidenced; full enforcement inside FxferService and for all background workers is not fully evidenced.

**Evidence**

- `fxfer-documentation/tentant-isolation.md:33`
- `tsd-work/target-state-fxfer-v2.md:1497`
- `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:32`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33`
- `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:44`

**Evidence gaps**

- **TBD (not evidenced):** where FxferService rejects requests with missing tenant context (analogous to BFFâ€™s `TENANT_MISSING` response). Target posture: `tsd-work/target-state-fxfer-v2.md:1602`
- **TBD (not evidenced):** how tenant isolation is enforced for outbox publisher polling across tenants (it loops all tenants from config). `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/Outbox/OutboxPublisherService.cs:47`

### 8.1 Tenant Context Propagation

| Layer | How tenant is determined | Enforcement points | Evidence |
|---|---|---|---|
| UI | Derived from operator JWT session context (tenant selection behavior not evidenced here) | N/A (UI calls BFF with JWT) | Target posture: `tsd-work/target-state-fxfer-v2.md:1602` |
| BFF | Reads `tenant_id` claim from JWT when authenticated | Rejects requests when `tenant_id` is missing; tags telemetry scope | `bff/Fxfer.Bff/src/Host/Middleware/CorrelationMiddleware.cs:32` |
| Domain API (FxferService) | Uses `RequestContext.TenantId` for tenant routing | Connection string resolution is tenant-based; rejection behavior TBD | `services/fxfer-service/FxferService.Host/src/Infrastructure/Tenancy/TenantDbResolver.cs:33` |
| Workers | Tenant id carried in message envelope | Worker sets tenant context on scope before consuming | `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:44` |
| Events | Tenant id is included in event envelope schema (AsyncAPI) | Envelope schema requires `tenant_id`; broker adapter is TBD/no-op | `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:42`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8` |

Anchors:

- Legacy anchor: `fxfer-documentation/tentant-isolation.md:17`
- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:1497`

Disposition: Partial

---

## 9. External Integrations & Protocol Adapters

Status: Partial

**What this section is about**

Compare what external protocols and integrations FXFER supports, and how credentials/network assumptions are handled.

Definitions:

- **Protocol adapter**: the code that talks to external endpoints (for example FTP/SFTP/UNC) to list/download/upload/rename/delete files.
- **Integration**: any dependency outside FXFER (event backbone, tenant policy, notification services, trading partner endpoints).

**What we looked at**

- Legacy integration inventory: `fxfer-documentation/shared-external-inventory.md:1`
- Legacy protocol support and batch engine notes: `fxfer-documentation/flow.md:97`, `fxfer-documentation/lld/w6-batch-transfer-engine.md:119`
- Target-state external interaction posture: `tsd-work/target-state-fxfer-v2.md:101`, `tsd-work/target-state-fxfer-v2.md:203`
- Modernized protocol implementations in execution worker: `workers/fxfer-execution-worker/src/Infrastructure/Protocols/FTPConnection.cs:179`, `workers/fxfer-execution-worker/src/Application/Batch/Business/FileTransferOperations.cs:717`
- Modernized credential storage evidence: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:52`

**What we found**

- Legacy supports multiple transfer methods, including UNC and (S)FTP variants, implemented in the batch transfer engine. `fxfer-documentation/lld/w6-batch-transfer-engine.md:119`
- Modernized execution worker includes protocol code for FTP via `FtpWebRequest`, SFTP via SSH.NET (`SftpClient`), and UNC file operations. `workers/fxfer-execution-worker/src/Infrastructure/Protocols/FTPConnection.cs:179`, `workers/fxfer-execution-worker/src/Application/Batch/Business/FileTransferOperations.cs:717`, `workers/fxfer-execution-worker/src/Domain/Constants/ProtocolCodeConstant.cs:7`
- Target state expects protocol workers as separate deployables with bounded retry behavior and safe staging; it also expects credentials to be stored as secret references rather than raw credentials. `tsd-work/target-state-fxfer-v2.md:101`, `tsd-work/target-state-fxfer-v2.md:318`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Protocol support | Batch implements protocol operations incl. UNC staging behavior | Execution worker contains FTP/SFTP/UNC protocol code | Protocol workers exist; safe staging + bounded retries required | Partial | Modern has protocol code; schedulerâ†’execution and bounded retries not evidenced. `fxfer-documentation/lld/w6-batch-transfer-engine.md:119`, `workers/fxfer-execution-worker/src/Infrastructure/Protocols/FTPConnection.cs:179`, `tsd-work/target-state-fxfer-v2.md:203` |
| Credential handling | Credentials stored in config tables (FTP server password) | Credentials stored in `FTP_SERVER.FTP_PASSWORD` (raw) and can be returned when `includePassword=true` | Must not store raw secrets in config; store secret references and audit privileged access | Missing | Modern still matches legacy, not target. Legacy table: `fxfer-documentation/data-models.md:102`; Modern mapping: `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:52`; Target rule: `tsd-work/target-state-fxfer-v2.md:318` |

**Disposition**

- Partial: protocol adapters exist; target-state credential posture and network assumptions (rotation, egress posture) are not evidenced.

**Evidence**

- `fxfer-documentation/shared-external-inventory.md:1`
- `fxfer-documentation/lld/w6-batch-transfer-engine.md:119`
- `workers/fxfer-execution-worker/src/Infrastructure/Protocols/FTPConnection.cs:179`
- `workers/fxfer-execution-worker/src/Application/Batch/Business/FileTransferOperations.cs:717`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:52`
- `tsd-work/target-state-fxfer-v2.md:318`

**Evidence gaps**

- **TBD (not evidenced):** where modernized stores secret references / integrates with a secrets manager (target requires rejecting raw secrets). `tsd-work/target-state-fxfer-v2.md:318`
- **TBD (not evidenced):** egress posture / IP allowlisting / network zone assumptions for partner endpoints. Target anchor: `tsd-work/target-state-fxfer-v2.md:101`

### 9.1 Supported Protocols / Endpoints

| Integration | Legacy support | Modernized support | Target-state expectation | Disposition | Evidence |
|---|---|---|---|---|---|
| FTP/SFTP/FTPS/UNC | Supported by batch engine and documented as protocol codes + flows | Supported by execution worker protocol code (FTP + SFTP + UNC paths evidenced) | Supported by protocol workers with safe staging and bounded retries | Partial | Legacy: `fxfer-documentation/shared-external-inventory.md:9`; Modern: `workers/fxfer-execution-worker/src/Infrastructure/Protocols/FTPConnection.cs:179`, `workers/fxfer-execution-worker/src/Application/Batch/Business/FileTransferProtocols.cs:156`; Target: `tsd-work/target-state-fxfer-v2.md:203` |
| Tenant policy service | N/A (legacy tenant config store is internal) | Scheduler has a liveness-only client | Tenant policy/registry is expected as platform dependency | Partial | Modern: `modernized-facts.md:355`; Target: `tsd-work/target-state-fxfer-v2.md:1359` |
| Event backbone | Event distribution WCF host exists; concrete ops are external | AsyncAPI contract declares SNS protocol; adapter is no-op | SNS/SQS pattern used for cross-domain events | Partial | Legacy: `fxfer-documentation/api.md:52`; Modern: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:11`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`; Target: `tsd-work/target-state-fxfer-v2.md:101` |

### 9.2 Credentials & Network Assumptions

- Where secrets live
  - Legacy: credentials are persisted in `FTP_SERVER` and used by batch/service layers. `fxfer-documentation/data-models.md:102`
  - Modernized: credentials are still persisted in `FTP_SERVER.FTP_PASSWORD` and are accepted via command API body. `services/fxfer-service/FxferService.Host/src/Api/Commands/Controllers/FtpServersCommandsController.cs:24`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Persistence/FxferDbContext.cs:52`
  - Target state: raw secrets must be rejected; config stores only secret references. `tsd-work/target-state-fxfer-v2.md:318`

- Rotation expectations
  - Legacy: TBD (not evidenced)
  - Modernized: TBD (not evidenced)
  - Target state: credential rotation is a privileged, audited workflow. `tsd-work/target-state-fxfer-v2.md:256`

- Egress posture
  - Legacy: TBD (not evidenced)
  - Modernized: TBD (not evidenced)
  - Target state: expects platform-standard network posture (details referenced via ARB baseline). `tsd-work/target-state-fxfer-v2.md:295`

Anchors:

- Legacy anchor: `fxfer-documentation/shared-external-inventory.md:1`
- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:223`

Disposition: Partial

---

## 10. Eventing & Messaging Parity

Status: Partial

**What this section is about**

Compare what events are published/consumed and whether delivery guarantees and idempotency match expectations.

Definitions:

- **Event backbone**: the platform messaging infrastructure (target state references SNS/SQS pattern) used for cross-domain integrations.
- **Outbox**: a database table used to durably record events to publish so that â€œDB write + event publishâ€ can be made reliable.
- **Inbox**: a record of consumed event IDs used to enforce idempotent consumption.

**What we looked at**

- Target-state event backbone posture: `tsd-work/target-state-fxfer-v2.md:101`
- Modernized AsyncAPI artifacts and event types: `modernized-facts.md:323`, `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:1`
- Modernized outbox publisher and event adapter implementation: `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/Outbox/OutboxPublisherService.cs:1`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`
- Legacy event distribution seam description: `fxfer-documentation/api.md:52`

**What we found**

- Target state expects FXFER domain services to publish tenant-aware, schema-versioned events to the event backbone for cross-domain consumers. `tsd-work/target-state-fxfer-v2.md:101`, `tsd-work/target-state-fxfer-v2.md:1730`
- Modernized contains AsyncAPI contracts with channels like `fxfer.configuration-changed` and declares an SNS protocol server, but the runtime publish adapter is currently a no-op. `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:11`, `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`
- Modernized includes an outbox publisher loop that polls per-tenant DBs for undispatched messages and calls the adapter; evidence for where outbox rows are created during command handling is still TBD. `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/Outbox/OutboxPublisherService.cs:47`, `modernized-facts.md:158`
- Projections includes an inbox/idempotency store to skip already-processed events, but the â€œbroker consumptionâ€ is currently represented as an in-memory queue. `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`, `modernized-facts.md:344`
- Legacy has an â€œevent distributionâ€ WCF surface, but the operation inventory is external to this repo (evidence gap). `fxfer-documentation/api.md:52`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Event contract artifacts | Not evidenced as versioned contract files in this repo | AsyncAPI YAMLs exist for service/workers | Contract artifacts are required and governed | Same (modernâ†”target), TBD (legacy) | Modern contracts: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:1`; Target governance: `tsd-work/target-state-fxfer-v2.md:1655` |
| Publish mechanism | Event distribution WCF host exists, but ops are external | Outbox + publisher loop exists; publish adapter is `NoopEventAdapter` | Durable outbox publishing to event backbone | Partial | Modern outbox: `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/Outbox/OutboxPublisherService.cs:47`; No-op adapter: `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8` |
| Consume mechanism | TBD (not evidenced) | Projections consumer uses inbox store and in-memory queue | Consumers must be idempotent and tenant-aware | Partial | Inbox: `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`; Target: `tsd-work/target-state-fxfer-v2.md:332` |

**Disposition**

- Partial: contracts exist and basic outbox/inbox scaffolding exists; external broker wiring and outbox write points are not evidenced.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:101`
- `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:11`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/Outbox/OutboxPublisherService.cs:47`
- `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`
- `fxfer-documentation/api.md:52`

**Evidence gaps**

- **TBD (not evidenced):** where FxferService writes `FXFER_OUTBOX` records during command processing (no proven `OutboxMessages.Add(...)` call site). `modernized-facts.md:158`
- **TBD (not evidenced):** concrete broker integration (SNS/SQS clients, subscriptions/queues, DLQ/redrive handling) for publish/consume. Target anchor: `tsd-work/target-state-fxfer-v2.md:101`

### 10.1 Published / Consumed Events

| Event family | Legacy | Modernized | Target-state | Disposition | Evidence |
|---|---|---|---|---|---|
| Configuration changed | TBD (not evidenced) | AsyncAPI channel `fxfer.configuration-changed` exists | Config changes should emit events for projections/cross-domain consumers | Partial | Modern: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:15`; Target: `tsd-work/target-state-fxfer-v2.md:1679` |
| Run state changed | TBD (not evidenced) | AsyncAPI channel `fxfer.run-state-changed` exists | Execution/run lifecycle should be evented | Partial | Modern: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:25`; Target: `tsd-work/target-state-fxfer-v2.md:102` |
| Delivery outcome recorded | TBD (not evidenced) | AsyncAPI channel `fxfer.delivery-outcome-recorded` exists | Per-file/per-destination outcomes should be recorded and searchable | Partial | Modern: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:35`; Target: `tsd-work/target-state-fxfer-v2.md:135` |
| Suspension state changed | TBD (not evidenced) | AsyncAPI channel `fxfer.suspension-state-changed` exists | Suspension is an explicit operational state with history | Partial | Modern: `services/fxfer-service/FxferService.Host/deploy/contracts/asyncapi/fxfer-service.asyncapi.yaml:45`; Target: `tsd-work/target-state-fxfer-v2.md:486` |

### 10.2 Delivery Guarantees & Idempotency

- At-least-once assumptions
  - Modernized execution worker explicitly re-enqueues on failure (at-least-once delivery posture). `workers/fxfer-execution-worker/src/Host/Hosting/WorkerHostedService.cs:59`
  - Target state expects at-least-once assumptions but requires bounded retry and operator-visible attempt history. `tsd-work/target-state-fxfer-v2.md:329`

- Deduplication strategy
  - Modernized projections consumer uses an inbox store to skip already-processed events by `event_id`. `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`
  - Target state requires idempotency at the command boundary and deterministic duplicate handling. `tsd-work/target-state-fxfer-v2.md:332`, `tsd-work/target-state-fxfer-v2.md:459`

- Ordering assumptions
  - Legacy: TBD (not evidenced)
  - Modernized: TBD (not evidenced)
  - Target state: TBD (not evidenced in extracted sources for strict ordering guarantees)

Anchor:

- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:101`

Disposition: Partial




---

## 11. Observability & Operability Parity

Status: Partial

**What this section is about**

Compare operational supportability: correlation (request -> work -> outcomes) plus day-2 operability artifacts (runbooks, dashboards, alerts).

Definitions:

- **Observability**: emitted logs/metrics/traces used to understand system behavior.
- **Runbook**: a step-by-step operator procedure (for example redrive).

**What we looked at**

- Target-state observability posture + required tag set. `tsd-work/target-state-fxfer-v2.md:758`, `tsd-work/target-state-fxfer-v2.md:3402`
- Target-state readiness requirement for runbooks. `tsd-work/target-state-fxfer-v2.md:3403`
- Modernized OpenTelemetry wiring + correlation tagging. `services/fxfer-service/FxferService.Host/src/Host/Composition/DependencyInjection.cs:201`, `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:19`
- Modernized facts ledger for ops artifacts gap. `modernized-facts.md:431`
- Legacy log format and telemetry key wiring. `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`, `mms-cms-fxfer/components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvc.Main/Program.cs:54`

**What we found**

- Target state requires OpenTelemetry signals with `tenant_id`, `correlation_id`, `trace_id` tags end-to-end. `tsd-work/target-state-fxfer-v2.md:3402`
- Modernized sets `tenant_id`/`correlation_id` tags and baggage and captures `trace_id`/`span_id` in a logging scope. `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:45`
- Modernized configures OpenTelemetry tracing + metrics in service/worker/projections components. `services/fxfer-service/FxferService.Host/src/Host/Composition/DependencyInjection.cs:201`, `workers/fxfer-execution-worker/src/Host/Composition/DependencyInjection.cs:51`, `services/fxfer-service/FxferService.Projections/src/Host/Composition/DependencyInjection.cs:54`
- Legacy log4net patterns include `TenantId` and `CorrelationId` properties in both EventLog and rolling file outputs; legacy host also wires an Application Insights instrumentation key via config. `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`, `mms-cms-fxfer/components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvc.Main/Program.cs:54`
- Runbooks/dashboards/alerts are required in the target state but are not evidenced for modernized in-repo. `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Telemetry standard | log4net + Application Insights wiring | OpenTelemetry tracing + metrics configured | OpenTelemetry traces/metrics/logs | Changed (intentional) | Modern aligns to target posture. `services/fxfer-service/FxferService.Host/src/Host/Composition/DependencyInjection.cs:201`, `tsd-work/target-state-fxfer-v2.md:758` |
| Required correlation tags | TenantId + CorrelationId appear in log format | `tenant_id`, `correlation_id`, `trace_id` tags are set | `tenant_id`, `correlation_id`, `trace_id` end-to-end | Partial | Legacy source of correlation properties is TBD. `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`, `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53` |
| Runbooks | TBD (not evidenced) | Not evidenced | Required | Missing | `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431` |

**Disposition**

- Partial: OTel instrumentation + tagging is evidenced; operability artifacts (runbooks/dashboards/alerts) are missing/TBD.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:3402`
- `tsd-work/target-state-fxfer-v2.md:3403`
- `services/fxfer-service/FxferService.Host/src/Host/Composition/DependencyInjection.cs:201`
- `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:45`
- `modernized-facts.md:431`
- `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`
- `mms-cms-fxfer/components/bas/FileTransferSvc/HPP.FileTransfer.BAS.FileTransferSvc.Main/Program.cs:54`

**Evidence gaps**

- **TBD (not evidenced):** modernized runbooks/dashboards/alerts configuration. `modernized-facts.md:431`
- **TBD (not evidenced):** where legacy sets `TenantId`/`CorrelationId` log properties (format is evidenced, producer is not). `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`

### 11.1 Correlation & Traceability

| Concern | Legacy | Modernized | Target-state expectation | Disposition | Evidence |
|---|---|---|---|---|---|
| Correlation IDs | `TenantId` + `CorrelationId` present in log format | Request middleware reads correlation header; scopes logs and tags activity with `tenant_id`, `correlation_id`, `trace_id` | End-to-end tag set present and provable | Changed (intentional) | Legacy: `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`; Modern: `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:19`; Target: `tsd-work/target-state-fxfer-v2.md:3402` |
| Audit trails | Legacy schema docs include operator/audit columns (example: `OPERATOR_ID`, timestamps) | TBD (not evidenced) | Privileged ops must emit audit evidence | TBD | Legacy: `fxfer-documentation/data-models.md:113`; Target: `tsd-work/target-state-fxfer-v2.md:3594` |
| Operator workflows | TBD (not evidenced) | Runbooks not evidenced | Runbooks required; UI includes an overview/dashboard concept | Missing | Target runbooks: `tsd-work/target-state-fxfer-v2.md:3403`; Target dashboard: `tsd-work/target-state-fxfer-v2.md:3607`; Modern gap: `modernized-facts.md:431` |

Anchor:

- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:3402`

Disposition: Partial

---

## 12. Testing & Coverage (Behavior Coverage, Not Just Line Coverage)

Status: Partial

**What this section is about**

Compare how well tests prove behavior parity and target-state invariants (contracts, tenant isolation, failure modes), not just code coverage.

Definitions:

- **Contract test**: verifies a seam’s OpenAPI/AsyncAPI artifact matches the implemented surface and is compatibility gated.
- **Golden dataset**: a versioned, repeatable test dataset used to validate parity and failure modes.

**What we looked at**

- Target-state testing and dataset expectations. `tsd-work/target-state-fxfer-v2.md:2917`, `tsd-work/target-state-fxfer-v2.md:2964`
- Modernized test inventory. `modernized-facts.md:442`, `ux/fxfer-remote/package.json:10`
- Modernized contract readiness tests (OpenAPI + AsyncAPI). `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- Legacy test inventory described in legacy docs / code. `fxfer-documentation/lld/w6-batch-transfer-engine.md:62`, `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Batch.Main.Tests/BatchProcessTests.cs:13`

**What we found**

- Target state expects contract-first seams with merge-blocking verification and multi-tenant negative tests across levels; it also expects repeatable golden datasets. `tsd-work/target-state-fxfer-v2.md:2925`, `tsd-work/target-state-fxfer-v2.md:2951`, `tsd-work/target-state-fxfer-v2.md:2964`
- Modernized contains test projects across BFF/service/projections/workers; UX uses Vitest (`test`) and Playwright (`test:e2e`). `modernized-facts.md:442`, `ux/fxfer-remote/package.json:10`
- Modernized includes explicit OpenAPI and AsyncAPI readiness tests, including a compatibility gate when a baseline exists. `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- Legacy includes multiple unit-test projects (data access, batch engine, UX), but contract artifact diff-gating is not evidenced in the sources used here. `fxfer-documentation/lld/w5-data-access.md:115`, `fxfer-documentation/lld/w6-batch-transfer-engine.md:62`

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Test levels exist | Yes (unit tests across components) | Yes (unit/integration/contract; UX unit + e2e) | Yes (explicit taxonomy) | Same (broad shape) | Target is more explicit about datasets and negative tests. `fxfer-documentation/lld/w6-batch-transfer-engine.md:62`, `modernized-facts.md:442`, `tsd-work/target-state-fxfer-v2.md:2937` |
| Contract gating | TBD (not evidenced) | Implemented in tests for OpenAPI + AsyncAPI | Required (merge-blocking) | Partial | CI wiring proof is TBD. `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`, `tsd-work/target-state-fxfer-v2.md:2925` |
| Golden datasets | TBD (not evidenced) | TBD (not evidenced) | Required | TBD | `tsd-work/target-state-fxfer-v2.md:2964` |

**Disposition**

- Partial: modernized has strong contract-level tests; dataset-based parity harness is not evidenced.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:2925`
- `tsd-work/target-state-fxfer-v2.md:2951`
- `tsd-work/target-state-fxfer-v2.md:2964`
- `modernized-facts.md:442`
- `ux/fxfer-remote/package.json:10`
- `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- `fxfer-documentation/lld/w6-batch-transfer-engine.md:62`

**Evidence gaps**

- **TBD (not evidenced):** modernized golden dataset manifests / endpoint simulators / repeatable dataset run evidence. `tsd-work/target-state-fxfer-v2.md:2964`
- **TBD (not evidenced):** CI pipeline proof that contract tests are merge-blocking gates in this repo. `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:129`

### 12.1 Test Inventory

| Test level | Exists? | What it proves | Gaps | Evidence |
|---|---:|---|---|---|
| Unit | Legacy: Yes; Modernized: Yes | Pure logic correctness and key invariants | Tenant-isolation negative tests are required across levels; specific coverage is TBD | Legacy: `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess.Tests/QueryHandlers/GetFtpServerQueryExecutorTests.cs:1`; Target: `tsd-work/target-state-fxfer-v2.md:2953` |
| Integration | Legacy: TBD; Modernized: TBD (not evidenced as DB/broker-backed suites) | Persistence/eventing semantics with real dependencies | Dataset + simulator-backed integration suites are required by target state | Target: `tsd-work/target-state-fxfer-v2.md:2943`, `tsd-work/target-state-fxfer-v2.md:2987` |
| Contract | Legacy: TBD; Modernized: Yes | Seam artifact correctness and removal detection | Broker integration behind event contracts is still partial | Modern: `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85` |
| E2E | Legacy: Partial; Modernized: Yes | Operator journeys through the UI | Dataset-driven, deterministic parity evidence is TBD | Modern: `ux/fxfer-remote/package.json:12`; Target: `tsd-work/target-state-fxfer-v2.md:2946` |

### 12.2 Parity Regression Suite (Recommended Minimal Set)

- Tenant isolation negative test (cross-tenant attempts fail). `tsd-work/target-state-fxfer-v2.md:2953`
- Contract readiness: OpenAPI and AsyncAPI artifacts match runtime surface and are compatibility gated. `tsd-work/target-state-fxfer-v2.md:2925`, `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- Failure-mode dataset: duplicates/retries are safe (idempotency) and DLQ/redrive behavior is bounded (once broker wiring exists). `tsd-work/target-state-fxfer-v2.md:2944`, `tsd-work/target-state-fxfer-v2.md:2964`

Anchor:

- Target-state anchor: `tsd-work/target-state-fxfer-v2.md:2917`

Disposition: Partial

---

## 13. Known Differences Register (Single Authoritative List)

Status: Partial

**What this section is about**

List each evidenced difference once, so parity and readiness discussions have a single reference.

**What we looked at**

- Observability requirements vs implementation evidence. `tsd-work/target-state-fxfer-v2.md:3402`, `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`
- Runbook readiness requirement vs modern evidence gap. `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431`
- Modern contract artifact tests (OpenAPI + AsyncAPI). `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- Modern messaging scaffolding (no-op adapter / in-memory queue). `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`, `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`

**What we found**

- Only items with direct repo evidence are recorded here; anything unproven is treated as a gap in Section 14.

**Legacy vs Modernized vs Target State**

| ID | Difference | Type (Intentional/Regression/Fix) | Impact | Decision needed? | Evidence |
|---|---|---|---|---|---|
| DIFF-001 | Telemetry posture shifts to OpenTelemetry tagging (`tenant_id`, `correlation_id`, `trace_id`) from legacy log4net/AppInsights wiring | Intentional | Aligns to platform standard and improves correlation | No | Modern: `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`; Legacy: `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`; Target: `tsd-work/target-state-fxfer-v2.md:3402` |
| DIFF-002 | Target-state runbooks are required but not evidenced for modernized | Regression | Reduces operational readiness | Yes | Target: `tsd-work/target-state-fxfer-v2.md:3403`; Modern gap: `modernized-facts.md:431` |
| DIFF-003 | Modernized has explicit contract readiness tests (OpenAPI + AsyncAPI) with compatibility gating logic | Fix | Detects breaking seam changes pre-deploy | No | `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85` |
| DIFF-004 | Modernized event backbone wiring is scaffolding (no-op event adapter / in-memory queue) rather than a concrete broker integration | Regression | Limits cross-domain integration; blocks DLQ/redrive proof | Yes | `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`, `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`, `tsd-work/target-state-fxfer-v2.md:750` |

**Disposition**

- Partial: this register is complete only for differences that are evidenced in the sources used.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:3402`
- `tsd-work/target-state-fxfer-v2.md:3403`
- `modernized-facts.md:431`
- `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`
- `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`
- `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`

---

## 14. Gaps (Keep Two Lists Separate)

Status: Partial

**What this section is about**

Separate gaps into:

1) parity gaps vs legacy (something legacy had that modernized lacks), and
2) requirement gaps vs target state (something target state requires that modernized lacks).

**What we looked at**

- Target-state readiness requirements for observability tags and runbooks. `tsd-work/target-state-fxfer-v2.md:3402`, `tsd-work/target-state-fxfer-v2.md:3403`
- Modernized facts ledger gap statement about runbooks/dashboards/alerting. `modernized-facts.md:431`
- Modernized messaging scaffolding evidence. `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`

**What we found**

- The clearest evidenced gaps are target-state readiness artifacts (runbooks/evidence) and broker wiring (event backbone).

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Runbooks | TBD (not evidenced) | Not evidenced | Required | Missing | `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431` |
| Event backbone integration | WCF seam exists (ops inventory external) | No-op adapter / in-memory queue scaffolding | Concrete backbone integration expected | Missing | `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`, `tsd-work/target-state-fxfer-v2.md:750` |

**Disposition**

- Partial: parity gaps vs legacy are limited by available evidence; requirement gaps vs target state are clearer.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:3402`
- `tsd-work/target-state-fxfer-v2.md:3403`
- `modernized-facts.md:431`
- `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`

### 14.1 Missing vs Legacy (Parity Gaps)

| Gap | Legacy evidence | Modernized status | Impact | Next action |
|---|---|---|---|---|
| “Last-resort” Windows Event Log sink | Legacy config includes `EventLogAppender` with `ERROR` threshold | TBD (not evidenced) | Medium: fatal/startup failures may be hard to see if exporters fail | Confirm and document modernized fatal-error visibility strategy | 

### 14.2 Missing vs Target State (Requirement Gaps)

| Gap | Target-state requirement | Modernized status | Impact | Next action |
|---|---|---|---|---|
| Operational runbooks + drills | Runbooks exist and are exercised (redrive, onboarding, rotation, incident response, retention enforcement) | Not evidenced | High readiness risk | Produce runbooks and drill evidence artifacts | 
| Concrete event backbone wiring | Cross-domain propagation is async over shared event backbone | Scaffolding (no-op adapter / in-memory queue) | High: cross-domain integration cannot be proven | Implement broker integration and prove via integration tests | 
| Dataset-based parity harness | Golden datasets exist and are replayable for parity + failure modes | TBD (not evidenced) | Medium: parity validation becomes manual/slow | Create versioned dataset manifests and repeatable run evidence | 

---

## 15. Summary (Executive View)

Status: Partial

**What this section is about**

Provide an executive, operator-facing summary of parity and readiness between legacy, modernized, and target state.

**What we looked at**

- Target-state readiness expectations for observability tags and runbooks. `tsd-work/target-state-fxfer-v2.md:3402`, `tsd-work/target-state-fxfer-v2.md:3403`
- Modernized observability tagging implementation. `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`
- Modernized contract readiness tests. `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- Modernized ops artifacts evidence gap statement. `modernized-facts.md:431`

**What we found**

- Modernized is closer to target-state standards on telemetry and contract artifacts, but readiness-critical operability evidence (runbooks/drills) and concrete backbone wiring are still missing/TBD.

**Legacy vs Modernized vs Target State**

| Topic | Legacy | Modernized | Target State | Disposition | Notes |
|---|---|---|---|---|---|
| Observability tags | TenantId + CorrelationId in log format | OTel tags/baggage for `tenant_id`, `correlation_id`, `trace_id` | Required end-to-end | Partial | Automated proof (dashboards/CI conformance) is not evidenced here. `tsd-work/target-state-fxfer-v2.md:3402` |
| Operability artifacts | TBD | Not evidenced | Required runbooks + drills | Missing | `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431` |

**Disposition**

- Partial: code-level posture (telemetry tagging, contracts) is evidenced; operability/runbook readiness is not.

**Evidence**

- `tsd-work/target-state-fxfer-v2.md:3402`
- `tsd-work/target-state-fxfer-v2.md:3403`
- `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`
- `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`
- `modernized-facts.md:431`

### 15.1 What Stayed the Same

- Tenant and correlation context are operator-relevant signals in both systems (legacy formats them; modernized tags them). `mms-cms-fxfer/components/bas/FileTransferSvc/HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc/Configs/log4net.config:20`, `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:45`

### 15.2 What Changed / Improved

- Modernized aligns to target-state OpenTelemetry posture and tags requests with `tenant_id`, `correlation_id`, `trace_id`. `services/fxfer-service/FxferService.Host/src/Host/Composition/RequestContextMiddleware.cs:53`, `tsd-work/target-state-fxfer-v2.md:3402`
- Modernized adds explicit contract readiness tests for OpenAPI + AsyncAPI artifacts. `services/fxfer-service/FxferService.Host/src/Host/Contracts/Tests/T_007_ContractReadinessTests.cs:85`

### 15.3 What’s Risky / Needs Decision

- Runbooks/drills required for readiness are not evidenced for modernized. `tsd-work/target-state-fxfer-v2.md:3403`, `modernized-facts.md:431`
- Event backbone wiring is scaffolded (no-op adapter / in-memory queue), blocking proof of cross-domain eventing and redrive workflows. `services/fxfer-service/FxferService.Host/src/Infrastructure/Messaging/EventAdapter/EventAdapter.cs:8`, `services/fxfer-service/FxferService.Projections/src/Infrastructure/Messaging/Consumer/FxferEventConsumer.cs:43`

### 15.4 Next Steps

- Produce operational runbooks and drill evidence required by the readiness checklist. `tsd-work/target-state-fxfer-v2.md:3403`
- Implement concrete broker wiring for publish/consume and validate via integration/failure-mode tests. `tsd-work/target-state-fxfer-v2.md:2944`, `tsd-work/target-state-fxfer-v2.md:750`
- Establish golden dataset manifests and repeatable parity run evidence (target-state “golden datasets”). `tsd-work/target-state-fxfer-v2.md:2964`
