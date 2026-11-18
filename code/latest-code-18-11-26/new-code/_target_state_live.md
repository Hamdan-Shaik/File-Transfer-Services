---
type: Target State
title: Target State â€” Provider File Transfer (MMS-CMS-FXFER)
description: '**Program:** Provider Plus Modernization **Template version:** 2.1 (2026-11-01)'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 63f54a03062d932997bd7bd29b070a22dd7c3552ad92120e24da661d11ea07e0
---

# Target State â€” Provider File Transfer (MMS-CMS-FXFER)

**Program:** Provider Plus Modernization
**Template version:** 2.1 (2026-11-01)

---

## Read First: Authoring Rules

- Every section includes a `Status:` line using one of: **Complete**, **Partial**, **Not started**, **N/A**, **Inherited**.
- Facts are written as plain text. Target-state constraints are labeled inline as one of:
 - `...`
 - `...`
 - `...`

---

# 0. Document Control

## 0.1 Document Information

Status: Complete â€” metadata set for target-state v2.

| Field | Value |
|---|---|
| Module name and code | Provider File Transfer (MMS-CMS-FXFER) |
| Module type | Supporting module |
| Parent document | N/A |
| Version | 0.1 |
| Status | Proposed |
| Date | 2026-11-02 |
| Owner | Provider Plus Modernization â€” Enterprise Architecture Practice |
| Reviewers | Provider Plus ARB; Security; Platform Engineering; FXFER domain SMEs |
| Distribution | Provider Plus modernization stakeholders |
| Confidentiality | Confidential |

## 0.2 Revision History

Status: Complete â€” initial entry recorded.

| Version | Date | Author | Change summary |
|---|---|---|---|
| 0.1 | 2026-11-02 | Provider Plus Modernization â€” Enterprise Architecture Practice | Initial FXFER target-state v2.1 draft with module-specific decisions resolved. |

## 0.3 Governing Inputs

Status: Partial â€” primary governing inputs listed; enterprise standards and ADR inventory to be expanded.

This target state is governed by the Provider Plus modernization platform baseline. When two sources disagree, the ARB decision set is authoritative.

Primary governing inputs:

- Provider Plus Modernization Architecture Review Board (ARB) packet: **ARB-Provider-Plus-Modernization (v5, June 2026)**.
- Enterprise ADRs and standards explicitly referenced by the ARB (identity/OIDC, API gateway posture, multi-tenant SaaS patterns, event backbone patterns, observability requirements, and AWS runtime standards).
- The legacy **MMS-CMS-FXFER (Provider File Transfer)** system, including its existing runtime behaviors, current operational procedures, and current-state documentation. Legacy sources are used for parity and migration planning, not as a reason to keep legacy architecture.

This document intentionally focuses on what the future state must do and how it is built, rather than being a restatement of legacy design.

## 0.4 How to Use This Document

Status: Partial â€” precedence is defined; remaining sections still being authored.

**Intended audience.**

This document is written for:

- Solution architects and reviewers validating that MMS-CMS-FXFER aligns to the Provider Plus platform target state.
- Engineering teams implementing MMS-CMS-FXFER modernization (UI, BFF, APIs, workers).
- Security, operations, and compliance stakeholders validating trust boundaries, tenancy isolation, and auditability.

**Source precedence.**

When multiple sources disagree, the following precedence applies:

| Precedence | Source |
|---|---|
| 1 | Provider Plus Modernization ARB decisions (v5, June 2026) |
| 2 | Enterprise ADRs/standards explicitly referenced by the ARB |
| 3 | This document (MMS-CMS-FXFER target state) once approved |
| 4 | Legacy implementation behavior (used only for parity and migration planning) |

**How to read disposition.**

This document uses the following disposition terms consistently:

- **Preserve:** the capability remains, though implementation may modernize.
- **Change:** the capability remains, but behavior or interface changes.
- **Retire:** the capability is removed and replaced.
- **New:** the capability does not exist today and will be introduced.

## 0.5 Decision Status

Status: Complete â€” platform and FXFER-local decisions are closed for implementation.

The following items are treated as binding decisions for MMS-CMS-FXFER:

- **Service boundaries / deployables.** FXFER ships as one bounded context with: (1) File Transfer micro-frontend (Next.js 15, Pages Router), (2) File Transfer BFF (.NET 10), (3) File Transfer Domain API (.NET 10; separate command/query surfaces), (4) File Transfer Worker (.NET 10; scheduled execution + privileged CLI rerun mode), and (5) a SOAP compatibility facade for legacy consumers (no new SOAP functionality).
- **Trigger / scheduling + rerun.** Scheduled execution remains the default. Privileged reruns are performed via a CLI rerun mode targeting a specific tenant and tracked file identifier. FXFER does not introduce a first-class transfer-run write model or a required UI rerun/run button.
- **UI authentication + CSRF posture.** Browser uses OIDC Authorization Code + PKCE and calls the BFF with bearer JWT on every request; the BFF is stateless and does not create server-side sessions or use cookies for authentication. For state-changing requests, the BFF enforces strict CORS and validates Origin/Referer for same-site requests.
- **Legacy SOAP/WCF posture.** Provide a SOAP compatibility facade in the modern stack with the same operations as legacy (no new SOAP functionality), translating calls into the modern Domain API.
- **Networking (worker egress).** Workers run in private subnets (no public IPs) and reach client/partner endpoints using private connectivity. Outbound traffic follows the platform's standard outbound inspection/egress controls, is restricted to approved destinations, and uses stable, platform-managed egress ranges for allow-listing.
- **Notifications.** The worker preserves legacy notification behavior: when enabled, it sends success and error notifications with operational/diagnostic metadata (never payload bytes); when disabled, it sends nothing. For any tenant, only the authoritative FXFER system sends notifications.
- **Rules engine.** FXFER does not introduce a new rules-engine dependency; behavior remains configuration-driven (stored fields) plus deterministic code in the worker/domain service.
- **Payload handling.** File payload bytes are treated as opaque: never persisted in the database and never included in API responses, events, logs, traces, or metrics. FXFER persists only payload-adjacent tracking metadata and cleans up worker staging files after processing.
- **Retention.** FXFER does not introduce an FXFER-specific purge policy for tracking; tracking remains available as long as the tenant database retains it. Worker working-directory files remain ephemeral.
- **Sensitive data access.** FXFER does not introduce a separate break-glass feature. Secrets are never revealed back to operators; authorized users may reset/update them.
- **Versioning.** APIs use URI major versioning and evolve additively within a major; breaking changes require a new major adopted tenant-by-tenant. Events are schema-versioned and evolve additively within a version.
- **Shell UX conventions.** Breadcrumbs and tab conventions are owned by the shared shell. Return-to-search behavior is URL-driven using querystring state. Operator views use polling/manual refresh only (no push/WebSockets/SSE).

## 0.6 Target-State Summary

Status: Complete â€” summary reflects closed module decisions.

MMS-CMS-FXFER (Provider File Transfer) is the platform capability responsible for configuring, executing, and tracking provider file transfers (inbound and outbound) across multiple tenants.

In the legacy implementation, these responsibilities are delivered through a mix of operator web screens, multiple backend host styles (including SOAP/WCF-style service contracts and HTTP endpoints), and batch executables that perform the protocol-level file movement (for example, SFTP operations). The legacy shape is functionally complete but operationally costly: behavior is spread across multiple surfaces, security and error-handling behaviors vary by host, and runtime assumptions are often environment-specific.

In the target state, MMS-CMS-FXFER is modernized into a platform-aligned, cloud-native architecture consistent with Provider Plus:

- A React/Next.js micro-frontend provides operational screens for configuration and tracking.
- A dedicated BFF implemented in .NET 10 Native AOT provides browser-safe response shaping, tenant-aware request normalization, and consistent error handling. The browser calls the BFF only.
- Domain APIs own authoritative business rules, persistence, and domain event emission.
- Transfer workers perform protocol-level file movement (FTP/SFTP/FTPS/UNC where required) under controlled network and credential constraints.
- Aurora PostgreSQL database-per-tenant persists configuration and tracking state with strong tenant isolation.
- Event backbone integration enables decoupled downstream processing of outcomes and configuration change events.
- OpenTelemetry signals provide consistent observability for supportability, auditability, and operational readiness.

At a high level, the target-state architecture is designed so that:

- Operators can safely create, change, suspend, and troubleshoot transfer configurations without needing direct access to endpoints or databases.
- Every file movement outcome is durably recorded and searchable (including failures), with correlation identifiers that make support workflows efficient.
- Downstream domains integrate through events rather than tight synchronous coupling.
- Tenant isolation holds end-to-end across UI, BFF, APIs, data stores, and events.

## 0.7 Section Status Index

Status: Partial â€” top-level index aligned to current section statuses; subsection index will be expanded during authoring.

| Section | Status |
|---|---|
| 0. Document Control | Partial |
| 1. Business Scope and Capabilities | Partial |
| 2. Business Logic | Partial |
| 3. Architecture Overview | Partial |
| 4. Frontend | Partial |
| 5. BFF | Partial |
| 6. Domain Services | Partial |
| 7. Eventing | Partial |
| 8. External Interactions and Shared Services | Partial |
| 9. Data Architecture and Persistence | Partial |
| 10. Security, Identity and Tenancy | Partial |
| 11. Batch and File Processing | Partial |
| 12. Infrastructure, Delivery and Operations | Partial |
| 13. Non-Functional Requirements | Partial |
| 14. Testing and Quality Strategy | Partial |
| 15. Legacy Transition and Retirement | Partial |
| 16. Architecture Decisions | Complete |
| 17. Anti-Pattern Analysis | Complete |
| 18. Risks and Assumptions | Complete |
| 19. Repository Structure and Build Plan | Partial |
| 20. Readiness | Partial |
| Appendices | Partial |

---

# PART I â€” BUSINESS

# 1. Business Scope and Capabilities

## 1.1 Purpose

Status: Partial â€” purpose is defined.

MMS-CMS-FXFER exists to provide a tenant-aware, operationally supportable capability for moving provider-related files between internal systems and external endpoints.

The module is designed to make file transfer a managed, observable, and auditable platform capability rather than an ad-hoc collection of scripts and host-specific services. It does this by combining configuration management (what should happen) with execution (what happens) and tracking (what did happen).

From an operator point of view, MMS-CMS-FXFER must reliably answer three fundamental questions:

1. What should happen? Which source locations, destinations, schedules, and credential references are configured for a tenant.
2. What did happen? What the system discovered and attempted to move, what succeeded, what failed, and when.
3. What needs attention now? Which configurations are suspended, which endpoints are failing, which retries are pending, and what action is safe.

MMS-CMS-FXFER is explicitly not a generic file-sharing platform. It is a controlled transfer mechanism designed for predictable delivery, tenant isolation, and traceability.

## 1.2 Capabilities

Status: Partial â€” capability set is stable; dispositions and acceptance criteria will be finalized with stakeholders.

The table below lists the primary business capabilities MMS-CMS-FXFER must provide in the target state. The intent is to preserve the established file-transfer domain vocabulary (source locations, destinations, server profiles, tracking), while changing the architecture and interfaces to match the Provider Plus platform baseline.

| Capability | Description | Disposition |
|---|---|---|
| Manage source locations | Create/update the master transfer configuration record that anchors a job definition. Includes enable/disable, ownership/contact metadata, and suspension state. | Preserve |
| Manage destinations | Define one or more delivery targets per source location, including per-destination routing and protocol settings. | Preserve |
| Manage endpoint connection profiles | Maintain reusable endpoint definitions (FTP/SFTP/FTPS/UNC) including non-secret connection parameters and secret references. | Preserve |
| Manage credential references and rotation | Maintain secret references (passwords/keys/certificates) and support safe rotation and least-privilege access patterns. | Change |
| Schedule and trigger execution intent | Determine when a source location is eligible to run and create durable execution intent records/work items. | Change |
| Execute transfers (worker runtime) | Perform protocol-level operations (list/download/upload/rename/delete where applicable), using safe staging and bounded retry behavior. | Preserve |
| Track outcomes, events, and exceptions | Record one-file and per-destination outcomes, timeline events, and exception snapshots so operators can search and troubleshoot. | Preserve |
| Suspend and (optionally) auto-unsuspend | Support suspension as an explicit operational state with history; optionally resume automatically after a configured wait window. | Preserve |
| Notifications of operational conditions | Notify on key operational events (suspension, repeated failures, completion) through an event-driven integration model. | Change |
| Publish integration events | Publish tenant-aware, schema-versioned events for configuration changes and transfer outcomes so downstream consumers can react asynchronously. | Change |

Notes:

- Credential administration is a privileged operational capability and is treated as a separation-of-duties concern.

## 1.3 Ownership Boundary

Status: Partial â€” ownership boundaries are defined.

MMS-CMS-FXFER owns the file transfer mechanics and the file transfer operational record, not the business meaning of the file payload.

MMS-CMS-FXFER owns:

- The configuration and tracking data model for file transfer operations.
- The domain APIs that enforce validation, authorization, and authoritative state transitions.
- The worker execution model for protocol-level file movement.
- The domain events that represent file-transfer outcomes and configuration changes.

MMS-CMS-FXFER does not own:

- Enterprise identity (OIDC broker), authentication policy, or enterprise user lifecycle.
- Platform tenant registry and feature flag services.
- Event backbone infrastructure.
- The business semantics of the provider files themselves (file payload meaning is owned by upstream/downstream domains).

## 1.4 Out of Scope

Status: Partial â€” scope boundaries described.

MMS-CMS-FXFER target state does not include:

- Building bespoke, partner-facing portals or UIs for external trading partners.
- Implementing domain-specific processing of file content (parsing, adjudication, business workflows). Those responsibilities belong to the consuming domain(s).
- Becoming a long-term file payload repository or system of record for provider files. Long-lived storage, if required, is an enterprise/platform storage concern.
- Introducing tenant-specific builds or deployments for the UI. Tenant differences are resolved at runtime.
- Supporting hidden operational workflows that require direct database access by operators.

## 1.5 Actors and Roles

Status: Partial â€” actor set is defined.

MMS-CMS-FXFER is an operational module. The key actors are primarily internal platform users and service identities.

| Actor / role | Description | Typical interactions |
|---|---|---|
| Operator | Uses UI workflows to search, create, and maintain configurations and to investigate outcomes. | Create/update source locations and destinations; view tracking; initiate safe reruns/redrives where permitted. |
| Credential administrator (privileged) | Manages endpoint credential references and performs rotation under strict audit requirements. | Create/update credential references; rotate keys/certificates; validate access controls. |
| Platform support / audit user (privileged) | Investigates incidents and supports compliance/audit workflows. | Read-only access to tracking and audit trails; break-glass actions where explicitly approved. |
| Worker service identity | Executes transfers and records outcomes through APIs; does not require human privileges. | Consume work items; perform protocol I/O; write outcomes via domain APIs; emit telemetry. |
| Downstream consumer service identity | Subscribes to events to trigger domain-specific processing after a transfer completes/fails. | Consume events; maintain idempotency; publish downstream events if needed. |

Separation of duties expectation:

- Credential administration is not a default operator capability.
- Sensitive operational actions (credential rotation, redrive of repeated failures, break-glass access) require explicit authorization and durable audit records.

## 1.6 Business Objects

Status: Partial â€” object model is stable at a conceptual level; detailed schemas will be defined in appendices.

The following business objects define the stable vocabulary of MMS-CMS-FXFER across UI, APIs, worker behavior, persistence, and events.

| Business object | Definition | Notes |
|---|---|---|
| Server profile / endpoint profile | A reusable definition of how to reach an endpoint using a protocol family (FTP/SFTP/FTPS/UNC). | Holds non-secret parameters (host/port/path); links to secret references for auth. |
| Source location | The master configuration record for a transfer job definition. | Acts as the parent record for destinations, notifications, suspension history, and tracking records. |
| Destination | A delivery target for a source location. | One source location can have multiple destinations, driving fan-out of delivery attempts. |
| Notification recipient / rule | Configuration defining who is notified and under what conditions. | Target state is event-driven; migration posture may vary . |
| Suspension record | A durable record of a source location being suspended, including diagnostics captured at the time. | Suspension can be used to isolate repeated failures while allowing other source locations to continue processing. |
| Transfer run | A single execution instance for a source location at a point in time. | how runs are triggered and represented as first-class objects. |
| File tracking record | The authoritative record for one discovered file that is being processed or has completed processing. | Includes identifying metadata (name/size/checksum), status, timestamps, and operator/audit fields. |
| Delivery attempt | A per-file, per-destination outcome record. | One file can produce multiple delivery attempts when a source has multiple destinations. |
| Timeline event | A time-ordered record of milestones for a fileâ€™s processing lifecycle. | Used to provide an operator-friendly narrative across retries and destinations. |
| Exception snapshot | A captured diagnostic record of a failure (message, stack trace, error classification). | Must be safely redacted before exposure to non-privileged users. |

---

# 2. Business Logic

## 2.1 Business Rules Catalogue

Status: Partial â€” initial catalog structure and representative rules documented.

This section defines how MMS-CMS-FXFER business rules are catalogued so they can be implemented consistently across UI, BFF, domain APIs, workers, persistence, and events.

The governing posture for platform standards (multi-tenancy, event backbone integration, CQRS, observability, security) is the Provider Plus ARB baseline (Provider Plus Modernization ARB packet (v5, June 2026)).

Rule catalog approach:

- **Rule statements are implementation-neutral**: they describe observable behavior and invariants, not code details.
- **Enforcement points are explicit**: the catalog records where a rule is enforced (UI/BFF/API/worker) and which layer is authoritative.
- **Acceptance criteria**: each rule has at least one acceptance criterion in Section 2.10, expressed in Given/When/Then terms.

The rule catalog is split into three categories to separate concerns:

1. **Configuration validation rules** (authoritative at command write time)
2. **Execution / idempotency / duplicate handling rules** (authoritative at execution time)
3. **Tracking / audit / redaction rules** (authoritative when persisting tracking and exposing it to humans or emitting events)

### Configuration validation rules

These rules prevent â€œbad configurationsâ€ from entering the system, because invalid configuration is an operational risk. Validation must be deterministic and must occur at the authoritative write boundary (domain command API).

| Rule statement (behavior) | Authoritative enforcement | Operator-visible outcome |
|---|---|---|
| A configuration change is rejected if the resolved tenant context does not match the configuration objectâ€™s tenant ownership. | Domain API (command) | The UI shows a validation failure; no partial write occurs. |
| A source location must not be enabled unless it has at least one enabled destination. | Domain API (command) | The UI shows a validation failure; the source location remains disabled. |
| Endpoint credentials must never be stored in configuration records; configuration stores only references to tenant-scoped secrets. | Domain API (command) | Attempt to submit raw secrets is rejected and audited. |
| A source location must declare its suspension posture (whether it can be auto-suspended, and whether auto-unsuspension is allowed). | Domain API (command) | The UI requires the suspension posture fields before enabling. |
| Any file payload classification and redaction policy must be explicitly declared or derived before tracking is exposed to non-privileged users. | Domain API (command) | payload classification by file type; until resolved, exposure defaults to conservative redaction. |

### Execution / idempotency / duplicate handling rules

These rules govern how work is created and performed. They must prevent duplicate delivery, ensure retry behavior is safe, and ensure that operational controls (suspension, redrive) are respected.

| Rule statement (behavior) | Authoritative enforcement | Operator-visible outcome |
|---|---|---|
| A suspended source location must not generate new execution work items until it is unsuspended. | Worker + Domain API (command) | Operators see â€œSuspendedâ€ state and the reason/time bounds. |
| Discovery of a file that is already tracked as completed must not cause re-delivery unless a permitted operator redrive/rerun explicitly authorizes it. | Worker + Domain API (command) | Duplicate is recorded as a tracking outcome; delivery attempts are not created. rerun/redrive controls and policy. |
| A failed delivery attempt must be recorded with classification and diagnostic summary; retries must be bounded and must not silently loop forever. | Worker + Domain API (command) | Operators can see attempt history and whether additional retries remain. scheduling/trigger model and retry parameters. |
| Fan-out delivery to multiple destinations is independent: one destination failing must not prevent attempts to other destinations for the same file. | Worker | Operators see per-destination outcomes and an aggregate per-file status. |
| Idempotency must be enforced at the command boundary so duplicate work item submissions do not create duplicate tracking records. | Domain API (command) | Operators see a single tracking record per file identity; duplicate commands return a stable result. |

### Tracking / audit / redaction rules

These rules ensure the system remains operator-actionable while staying compliant: full traceability, safe diagnostics, and durable history.

| Rule statement (behavior) | Authoritative enforcement | Operator-visible outcome |
|---|---|---|
| Every execution and delivery attempt produces a durable, time-ordered tracking narrative (timestamps, statuses, and correlation identifiers). | Domain API (command) + projector(s) | Operators can reconstruct â€œwhat happenedâ€ without reading logs. |
| Diagnostic details are redacted before exposure to non-privileged users (PII suppression, secret suppression, safe error summaries). | Domain API (query) / read model | Operators see sanitized summaries; privileged auditors can access deeper diagnostics under strict authorization. |
| Configuration change history is immutable and queryable (who/when/what changed) and linked to downstream execution outcomes. | Domain API (command) + audit store | Operators can link an execution outcome back to the config revision in effect. |
| Event emission must not leak tenant data across tenants; every emitted event includes tenant context and correlation identifiers. | Domain event publisher | Events are tenant-scoped and versioned. (Platform posture: Provider Plus Modernization ARB packet (v5, June 2026).) |
| Tracking and artifact retention must follow an explicit retention policy and support lawful holds where required. | Platform + Domain | retention windows for tracking records and artifacts (separate from platform audit retention). |

## 2.2 Calculations and Decision Logic

Status: Partial â€” decision points documented; rule ownership (code vs rules engine) is defined by the decision register (Section 16).

MMS-CMS-FXFER is primarily a workflow/orchestration domain, but it still contains explicit decision logic that must be deterministic and auditable. Most â€œcalculationsâ€ are in the form of classification, selection, and aggregation decisions rather than numeric computation.

Key decision points:

1. **Which source locations are eligible to run**
 - Must consider: enabled/disabled status, suspension status, and trigger intent.
 - the scheduling/trigger model and how operators can rerun/redrive safely.

2. **Which files are eligible to be processed**
 - Must consider: file identity, duplicate detection, and any tenant-specific inclusion/exclusion rules.
 - file payload classification by file type (impacts redaction/exposure).

3. **How outcomes roll up for operator display and event emission**
 - Per-file status aggregates per-destination delivery attempts.
 - Source-location status aggregates across files/runs (including suspension posture).
 - CQRS read-model granularity/indexing/retention for operator queries.

4. **When to suspend operations**
 - Suspension can be operator-driven or automatically triggered by repeated failure patterns.
 - rerun/redrive controls and whether auto-unsuspension exists.

#### Decision logic sketch (implementation-neutral)

The following illustrates the intended decision flow without prescribing the final triggering mechanism:

```
Input: ExecutionIntent(sourceLocationId, triggerType)

1) Load SourceLocation + Destinations (tenant-scoped)
2) If SourceLocation is disabled -> record outcome NotEligible; stop
3) If SourceLocation is suspended -> record outcome Suspended; stop
4) Discover candidate files at source
5) For each discovered file:
 a) Compute file identity key (idempotency key)
 b) If file already completed and no explicit redrive intent -> record DuplicateOrAlreadyProcessed; continue
 c) Create/ensure TrackingRecord exists (idempotent)
 d) For each enabled Destination:
 i) Create DeliveryAttempt (idempotent)
 ii) Execute transfer attempt
 iii) Record attempt outcome (success/failure with classification)
 e) Compute aggregate file outcome
6) Emit outcome events (tenant-scoped, versioned)
7) Evaluate suspension triggers and apply if required
```

Rules engine usage is intentionally not assumed here.
 rules engine scope/governance (which of the above decisions are implemented as managed rules vs code).

## 2.3 Workflow and Lifecycle

Status: Partial â€” lifecycle and operator controls described; detailed state machine and trigger model are defined by the decision register (Section 16).

This section describes the end-to-end lifecycle from configuration through execution and tracking. It focuses on externally observable behavior and operator workflow rather than internal implementation.

### Lifecycle overview: source location â†’ destinations â†’ execution â†’ tracking

MMS-CMS-FXFER revolves around **source locations** that define where files are discovered, and **destinations** that define where those files are delivered. An execution produces durable **tracking** records that represent both the per-file lifecycle and the per-destination attempt outcomes.

```
Configuration time (operator) Runtime (worker + APIs)

 [Source Location] 1----* [Destination] [Execution Intent]
 | | |
 | | v
 | | [File Discovery]
 | | |
 v v v
 (Endpoint Profiles + Secret Refs) [Tracking Records]
 | |
 | +--> [Delivery Attempt]* (per destination)
 |
 +--> [Outcome Events] (tenant-scoped)
```

### Operator-driven configuration lifecycle

1. **Create endpoint / server profile**
 - Captures connection parameters and references to tenant-scoped credentials.
 - partner endpoint connectivity pattern (private networking vs public endpoints, allow-lists, certificate onboarding).

2. **Create a source location**
 - Establishes a logical job definition for discovering files.
 - The source location owns: destinations, suspension posture/history, and tracking history.

3. **Create one or more destinations**
 - Each destination is an independent delivery target for each discovered file.
 - Fan-out is first-class: one file may produce multiple delivery attempts.

4. **Enable the source location**
 - A source location is eligible for execution only when enabled and not suspended.
 - Configuration changes are audited and linked to subsequent execution outcomes.

### Runtime execution lifecycle

The runtime path is triggered by an execution intent.
 scheduling/trigger model and rerun/redrive controls (what triggers execution, who may trigger it, and how intent is represented).

The intended observable lifecycle is:

1. **Intent creation**
 - A scheduler and/or an operator action creates an execution intent for a specific source location.

2. **Eligibility check**
 - The worker (and/or domain API) confirms the source location is enabled and not suspended.
 - Ineligible intents result in a recorded outcome (operator-visible) and do not proceed.

3. **File discovery**
 - The worker enumerates candidate files at the source endpoint.
 - For each discovered file, the system determines whether it is new, in-progress, or already completed.
 - Duplicate detection must be deterministic and idempotent.

4. **Tracking record creation (idempotent)**
 - For each eligible file, the system creates (or reuses) a tracking record.
 - A tracking record is the authoritative operator-facing representation of the file lifecycle.

5. **Fan-out delivery attempts**
 - For each enabled destination, the system creates a delivery attempt and executes it.
 - Each attempt produces a structured outcome (success/failure classification + safe diagnostic summary).

6. **Outcome finalization and event emission**
 - The fileâ€™s aggregate outcome is computed from destination outcomes.
 - The system emits tenant-scoped outcome events to the event backbone (SNS/SQS-based pattern per Provider Plus Modernization ARB packet (v5, June 2026)).
 - The system records correlation identifiers linking UI actions, API calls, worker actions, and events.

### Operational suspension and unsuspension (business behavior)

Suspension is a business control used to protect operations and reduce incident blast radius.

- **Manual suspension**: an operator explicitly suspends a source location with a reason.
- **Automatic suspension**: the system may auto-suspend a source location when failure patterns indicate a persistent issue.
 - auto-suspension triggers (thresholds, time windows, failure classifications).

Unsuspension behavior:

- **Manual unsuspension**: an operator removes suspension and optionally triggers a controlled rerun/redrive.
- **Automatic unsuspension** (if supported): a suspension may expire and the source location may return to eligible state.
 - whether auto-unsuspension exists and what parameters govern it (expiry, retries, operator notification).

## 2.4 Time-Based Behaviour

Status: Partial â€” time-based behavior described at a policy level; scheduling, retry, and retention parameters are defined by the decision register (Section 16).

Time-based behavior exists in three areas: (1) triggering and rerun/redrive control, (2) retry/backoff and timeouts during execution, and (3) retention/expiry windows for tracking and artifacts.

### Triggering and rerun/redrive

 scheduling/trigger model and rerun/redrive controls.

At minimum, the target state must support:

- A non-interactive trigger suitable for consistent operations (for example, a scheduler).
- A controlled operator action to rerun/redrive when a business need exists.
- Guardrails to prevent uncontrolled duplication and to preserve auditability.

### Retry, backoff, and timeouts

Retries are required for transient failures, but must be bounded to avoid silent infinite loops.

- retries are applied at the **delivery attempt** level (per destination) rather than rerunning the entire source location for a transient failure.
- backoff uses an exponential strategy with jitter.
- partner endpoint connectivity constraints (some partners may require conservative retry patterns or specific maintenance windows).

### Retention and expiry

- Platform audit posture includes immutable audit logging and retention requirements (see Provider Plus Modernization ARB packet (v5, June 2026)).
- retention windows for tracking records and artifacts (transfer payload staging, extracted metadata, exception snapshots).
- CQRS read-model retention vs write-model retention (what is projected, indexed, and for how long).

## 2.5 Rules Engine Integration

Status: Partial â€” platform capability acknowledged; module scope and governance are defined by the decision register (Section 16).

The Provider Plus platform baseline includes a business rules capability (Provider Plus Modernization ARB packet (v5, June 2026) references InRule as a shared service). MMS-CMS-FXFER may use a rules engine to externalize certain tenant-variable or policy-driven decisions, but it is not assumed by default for all logic.

 rules engine scope/governance:

- Which decisions are implemented as managed rules vs code.
- Who authors/approves rules (tenant administrators vs platform admins vs engineering).
- How rule versions are tested, promoted, and rolled back.
- How rule evaluation is audited and correlated to operator-visible outcomes.

Conservative target-state posture:

- Rules engine integration is used only when a decision must be tenant-variable and must be changeable without redeploying MMS-CMS-FXFER.
- When rules are used, rule evaluation is treated as an auditable dependency: inputs and outputs are recorded (with redaction) and correlated to tracking outcomes.
- Rules engine unavailability must not produce silent behavior changes; failures must be explicit and operator-actionable.

## 2.6 Validation and Error Handling

Status: Partial â€” target error-handling posture documented.

Validation occurs at multiple layers:

- UI performs client-side validation for usability.
- BFF normalizes requests and can enforce coarse-grained policy.
- Domain APIs perform authoritative validation and enforce invariants.

Error handling must be deterministic and operator-actionable. Endpoint failures (invalid credentials, unreachable host, remote permission denied, partial transfer, duplicate file) must be recorded as structured outcomes in tracking, and exposed to operators in a safe, redacted format.

 CSRF/session strategy for UI/BFF interactions.

### Failure semantics: tracking outcomes vs fatal failures

The system distinguishes between:

- **Operational outcomes** (expected failure modes): recorded into tracking as structured outcomes.
- **Fatal/system failures** (platform invariants broken): fail fast, alert, and prevent unsafe continuation.

| Failure class | Examples | Becomes tracking outcome? | Fatal to execution? | Operator visibility |
|---|---|---:|---:|---|
| Configuration invalid | Missing destination, invalid references, tenant mismatch | N/A (blocked at write time) | Yes (command rejected) | UI shows validation failure; audit records the attempt. |
| Endpoint / partner failure | Auth failed, unreachable host, permission denied | Yes | Not necessarily | Recorded per delivery attempt; may contribute to suspension. |
| File-level data issue | Duplicate/already processed, unexpected filename pattern | Yes | No (per file) | Recorded per file tracking with safe summary. |
| Worker transient | Timeout, throttling, intermittent network | Yes | No (bounded retry) | Attempt history shows retries; bounded by policy. |
| Trust boundary violation | Cross-tenant access attempt, invalid token claims | Yes (audit) | Yes | Audited; user sees safe error; security monitoring alerted. |
| Tracking persistence failure | Cannot persist outcomes or cannot emit required audit | No (must not pretend success) | Yes | Operator sees a run-level failure; platform alerts fire. |

### Operator experience (high-level)

Operators must be able to answer:

- What configuration revision was active for a given outcome?
- For a file: what destinations were attempted, with what results, and when?
- If failures are repeating: is the source location suspended, and why?

The target state uses CQRS for operator query views.
 read-model granularity/indexing/retention required to keep queries fast without leaking diagnostic detail.

## 2.7 Audit, History and Versioning

Status: Partial â€” audit/event posture documented; retention and version window decisions are defined by the decision register (Section 16).

The target state requires:

- Audit of configuration changes (who/when/what changed).
- Audit of privileged actions (credential changes, redrive, break-glass access).
- Traceable correlation identifiers between UI actions, API calls, worker activity, and emitted events.

Audit posture (platform): the ARB baseline requires immutable audit events and long-retention audit storage (S3 Object Lock / WORM) (Provider Plus Modernization ARB packet (v5, June 2026)). MMS-CMS-FXFER audit-relevant actions include configuration writes, privilege-elevated actions (credential rotation, break-glass), suspension/unsuspension, and explicit redrive actions.

History requirements:

- Configuration history is queryable (who/when/what changed) and linked to execution outcomes.
- Execution history preserves the per-file and per-destination narrative, including redacted diagnostics.
- Operator actions are correlated to outcomes via correlation identifiers and durable timestamps.

Versioning requirements:

- Versioning must support staged rollout per tenant across UI, APIs, and event schemas.
- Event schema versions must be explicit and backwards compatible within the platformâ€™s supported window.
 - version support window policy (how long consumers can remain on old schemas/clients).

Retention requirements:

- retention windows for tracking records/artifacts (separate from platform audit retention).
- retention strategy for CQRS read models (what is retained for operator search vs archived).

## 2.8 Tenant-Variable Behaviour

Status: Partial â€” variability points enumerated; governance/source-of-truth for each point is defined by the decision register (Section 16).

Tenant-variable behavior is resolved at runtime via tenant metadata and feature flags. Tenant variability must not require per-tenant builds or separate per-tenant deployments.

Tenant isolation is enforced through database-per-tenant for authoritative operational data (Provider Plus Modernization ARB packet (v5, June 2026)). As a result, tenant variability must be expressed through:

- Tenant metadata (tenant registry) and feature flags (read at runtime)
- Tenant-scoped configuration records (source locations, destinations, credential references)
- Tenant-scoped policy logic remains configuration-driven and implemented deterministically in code.

Representative variability points (non-exhaustive):

| Variability point | Description | Notes |
|---|---|---|
| Enabled protocols | Which endpoint protocols are permitted for a tenant | partner connectivity pattern influences which protocols are feasible. |
| Payload classification / redaction | How file metadata and error diagnostics are exposed (and what is redacted) | payload classification by file type; impacts what operators can see. |
| Notification routing | Which outcomes trigger notifications and where they go | Event-driven; see Section 2.9 and . |
| Scheduling posture | When/how source locations run (time windows, blackout windows) | scheduling/trigger model and rerun/redrive controls. |
| Retention posture | How long tracking/artifacts are retained in operational stores | retention windows; must align with audit/compliance. |
| Read-model tuning | Indexing and retention decisions for operator query views | CQRS read-model granularity/indexing/retention. |

## 2.9 Notifications

Status: Partial â€” event-driven posture documented.

Notification behavior is treated as an event-driven capability. MMS-CMS-FXFER emits outcomes and exception events; a notification service (or other consumers) subscribes and delivers tenant-appropriate notifications.

Notifications are therefore downstream of authoritative tracking outcomes:

- MMS-CMS-FXFER records tracking outcomes first.
- MMS-CMS-FXFER emits tenant-scoped outcome events.
- Notification services (or other modules) decide whether to notify humans based on tenant policy.

 notification triggers are defined only in terms of stable tracking outcomes (for example, source location suspended, delivery failed after retries, delivery succeeded) to keep coupling low.

 migration strategy for any direct legacy notification dependency.

## 2.10 Acceptance Criteria

Status: Partial â€” acceptance criteria pattern established with representative examples.

Acceptance criteria are expressed per business rule (rule catalog in Section 2.1). Each acceptance criterion is written as a small, testable scenario describing:

- Preconditions and configuration
- Trigger (intent creation)
- Expected tracking outcomes (operator-visible)
- Expected events (for downstream consumers)
- Audit/observability expectations

 rerun/redrive controls are foundational to many criteria and must be resolved to finalize acceptance tests.

# AC-01 (BR-xxx)

**Rule(s) covered:** BR-xxx 

**Preconditions / setup:**

- Given tenant T exists and operator is authorized for tenant T
- And a source location exists with one or more destinations
- And the source location suspension posture is defined
- partner endpoint connectivity pattern may impose additional setup (network allow-lists, certificates)

**Scenario examples (representative; details finalize with contracts):**

1) **Suspension blocks execution**

- Given a source location is in a suspended state
- When an execution intent is created for that source location
- Then no new delivery attempts are executed
- And an operator-visible outcome indicates the source location is suspended
- And the outcome includes a suspension reason and timestamps (subject to redaction)
- whether auto-unsuspension exists and how suspension expiry is represented

2) **Duplicate file discovery does not re-deliver by default**

- Given a file has already completed delivery for a source location
- When the same file is discovered again during a later execution intent
- Then the system records a duplicate/already-processed outcome in tracking
- And the system does not create new delivery attempts for the file
- And any downstream event emitted reflects a duplicate/ignored outcome (if emitted)
- policy for operator-authorized redrive and how it overrides duplicate detection

3) **Per-destination failures are visible and do not block other destinations**

- Given a source location has multiple destinations
- When delivery fails for one destination but succeeds for another
- Then tracking shows one failed delivery attempt and one successful delivery attempt
- And the aggregate per-file outcome is derived from per-destination attempts: **SUCCESS** (all destinations succeed), **PARTIAL_SUCCESS** (mix of success/failure), **FAILED** (all destinations fail), **SKIPPED** (no attempt due to suspension/not-eligible/duplicate).
- And downstream consumers can determine which destination(s) succeeded from emitted events
- CQRS read-model design needed to support this view efficiently

4) **Safe diagnostics / redaction**

- Given a delivery attempt fails with a diagnostic message containing sensitive details
- When an operator without privileged audit scope views the failure
- Then the operator sees a safe diagnostic summary without PII/secrets
- And privileged audit workflows can access deeper diagnostics only under explicit authorization
- payload classification by file type and the redaction rules derived from it

## 2.11 Business Behaviour Parity

Status: Partial â€” parity scope clarified; parity plan still depends on trigger model, connectivity posture, and read-model decisions.

The modernization target is parity of observable behavior: the same configuration options produce the same external endpoint actions and the same operator-visible outcomes, except where differences are explicitly recorded in the Known-Difference Register.

Parity expectations by area:

- **Configuration semantics parity**: the same operator inputs produce the same eligibility behavior (enabled/disabled, validation failures).
- **Execution semantics parity**: the same file discovery and delivery actions occur at the endpoints, subject to explicit idempotency/duplicate handling rules.
- **Operator parity**: operators can find what happened for a file without reading logs.
- **Integration parity**: downstream systems receive materially equivalent outcomes via the event backbone.

Known parity dependencies:

- scheduling/trigger model and rerun/redrive controls (many parity comparisons require consistent triggering).
- partner endpoint connectivity pattern (networking/onboarding constraints can change operational behavior).
- legacy SOAP/WCF migration posture (if any legacy integrations depend on SOAP/WCF, parity must define how they are replaced or bridged).
- CQRS read-model granularity/indexing/retention (operator parity requires queryable projections).

 parity is evaluated primarily through tracking outcomes and endpoint-side observable behavior (file delivered/not delivered), rather than requiring byte-for-byte equivalence of every internal log.

---

# PART II â€” TECHNICAL

# 3. Architecture Overview

## 3.1 Architecture Classification and Patterns

Status: Complete â€” architecture baseline and mandatory patterns captured; compute and protocol constraints are defined by the decision register (Section 16).

MMS-CMS-FXFER v2 is an **operator-facing** module that inherits the enterprise platform target posture from Provider Plus Modernization ARB packet (v5, June 2026) (authoritative, June 2026). The architecture is intentionally split into browser-facing UX concerns, domain-authoritative command/query concerns, and protocol I/O execution concerns.

**Architecture classification (C4-style):**

- **Micro-frontend UI â†’ BFF â†’ domain APIs**: the browser calls a UI-scoped backend (BFF) only; the BFF calls domain APIs only.
- **Event-driven CQRS**: commands and queries are separated; read models are derived from domain events.
- **Single event backbone for cross-domain integration**: cross-domain propagation is async over the shared Event Backbone (SNS/SQS pattern).
- **Worker execution model for protocol I/O**: any file transfer protocol interaction is performed by workers (not the browser, and not the BFF).

**Mandatory platform standards (inherited from Provider Plus Modernization ARB packet (v5, June 2026)):**

- **Zero-trust, brokered identity**: inbound requests must carry a validated broker-issued JWT; tenant context is derived from `tenant_id` claims.
- **Tenant isolation**: Aurora PostgreSQL **database-per-tenant** with per-tenant Secrets Manager credentials and per-tenant KMS encryption posture.
- **Tenant Registry**: tenant metadata and policy/feature enablement is centralized (Tenant Registry is the source of truth for tenant onboarding and capability flags).
- **Observability**: OpenTelemetry traces/metrics/logs; tenant context and correlation identifiers are carried end-to-end.

**FXFER-specific patterns (module intent):**

- **Authoritative tracking over â€œlog archaeologyâ€**: execution intent, progress, and outcomes are recorded through domain commands and persisted; worker logs are diagnostic only.
- **Tenant-scoped event envelopes**: every published message is tenant-scoped and correlatable to the originating command/request.
- **Controlled egress to external endpoints**: partner/on-prem endpoints are outside the trust boundary and are reached only by worker workloads.
- **Idempotent outcome recording**: write-side commands that record outcomes must be idempotent because workers consume work items with at-least-once delivery semantics.

 CQRS read-model granularity/indexing/retention.
 file payload classification by file type (impacts storage, encryption, scanning, and observability).
 retention windows for tracking/artifacts.
 version support window policy.

## 3.2 System Context (C1)

Status: Partial â€” context diagram and trust boundaries documented; partner connectivity, CSRF/session, and scheduling triggers are defined by the decision register (Section 16).

```

 +--------------------------------------------+
 | Platform Identity Broker (OIDC) |
 | - AuthN for internal operators |
 | - Issues JWT containing tenant_id claims |
 +--------------------------+-----------------+
 ^
 | OIDC browser flows
 |
 +----------------------------+----------------------------+
 | Internal Operators (Support / Ops / Admin) |
 +----------------------------+----------------------------+
 |
 | HTTPS (browser)
 v
 +----------------------------+----------------------------+
 | MMS-CMS-FXFER UI Micro-Frontend (React/Next.js) |
 | - Served as static assets (CloudFront/S3 baseline) |
 +----------------------------+----------------------------+
 |
 | HTTPS (JWT, correlation headers)
 v
 +----------------------------+----------------------------+
 | MMS-CMS-FXFER BFF (.NET 10 Native AOT) |
 | - Single browser-callable API surface for FXFER |
 | - Tenant-aware response shaping; no DB access |
 +----------------------------+----------------------------+
 |
 | HTTPS (private)
 v
 +----------------------------+----------------------------+
 | MMS-CMS-FXFER Domain APIs (.NET 10) |
 | - Commands (write model) + Queries (read model) |
 +---------------+---------------------------+--------------+
 | |
 | SQL (tenant-routed) | async publish/consume
 v v
 +----------------------------+ +-------------------------------+
 | Aurora PostgreSQL | | Event Backbone (SNS/SQS) |
 | - Tenant Registry (db_shared)| | - Tenant-scoped envelopes |
 | - Per-tenant FXFER DBs | +---------------+---------------+
 +----------------------------+ |
 ^ |
 | tenant metadata / routing |
 | |
 +----------+----------+ |
 | Secrets Manager + | |
 | KMS (per tenant) | |
 +---------------------+ |
 |
 v
 +---------------------------+
 | Transfer Worker(s) |
 | - Protocol I/O execution |
 | - Outcome reporting |
 +-------------+-------------+
 |
 | outbound connections
 v
 +---------------------------+
 | External Endpoints |
 | (partners / on-prem) |
 +---------------------------+

 +---------------------------+
 | Other Provider Plus Domains|
 | (consume FXFER events) |
 +---------------------------+
```

Context notes:

- **Primary actors**: the UI is intended for internal operators (Support/Ops/Admin). The target-state operator experience is not a public integration surface for partners.
- **Trust boundaries**:
 - **Browser â†’ BFF** is the main interactive boundary: everything from the operator browser must be authenticated/authorized and treated as untrusted input.
 - **BFF â†’ Domain APIs** stays within the platform trust boundary: the BFF is still required to pass tenant context and correlation context explicitly.
 - **Worker â†’ External Endpoints** crosses out of the platform trust boundary: partner/on-prem endpoints are treated as untrusted and must be reached through controlled egress.
- **Authentication (high level)**: internal operators authenticate via the platform identity broker, which issues JWTs containing tenant context. Exact broker/product configuration is inherited from the platform baseline in Provider Plus Modernization ARB packet (v5, June 2026).
- **Authorization (high level)**: enforcement is expected at the API entry layer (request-level validation) and within the domain APIs (operation-level authorization on commands/queries). Fine-grained authorization policy is domain-owned and must be auditable.
- **Session and CSRF posture**: CSRF/session strategy (cookie-based session vs token-only, anti-forgery tokens, and same-site behavior).
- **Scheduling / trigger posture**: scheduling/trigger model and rerun/redrive controls.
- **Legacy contracts**: legacy SOAP/WCF migration posture. The target-state end surface should not introduce new SOAP/WCF dependencies.
- **Partner connectivity**: partner endpoint connectivity pattern (networking, onboarding, allowlisting, and protocol support constraints).

## 3.3 Containers and Components (C2)

Status: Partial â€” decomposition defined; final deployable boundaries (service splits, ownership, and packaging) are defined by the decision register (Section 16).

The following C2 view is intentionally conservative: it names the minimum set of runtime containers required to support the target-state patterns while leaving service boundary decisions explicit.

```
 +---------------------+ +--------------------------+
 | UI Micro-Frontend | | Platform Identity Broker |
 | (React/Next.js) |<----->| (OIDC / JWT issuance) |
 +----------+----------+ +--------------------------+
 |
 | HTTPS
 v
 +----------------------------+
 | BFF (.NET 10 Native AOT) |
 | - Browser-safe surface |
 | - Tenant/correlation |
 | - Aggregation/normalization|
 +-------------+--------------+
 |
 | HTTPS (private)
 v
 +----------------------------+ +-----------------------------+
 | FXFER Domain APIs (.NET 10)| | Tenant Registry + Flags |
 | - Command endpoints (write)|<------------>| (db_shared / platform API) |
 | - Query endpoints (read) | +-----------------------------+
 +------+---------------------+
 |
 | SQL (tenant-routed)
 v
 +----------------------------+
 | Aurora PostgreSQL |
 | - per-tenant FXFER DB |
 +----------------------------+

 +----------------------------+ +----------------------------+
 | Event Backbone (SNS/SQS) |<------------>| Other Domain Consumers |
 +-------------+--------------+ +----------------------------+
 ^
 | publish/consume (tenant-scoped envelopes)
 |
 +-------------+--------------+
 | Transfer Worker(s) |
 | - protocol I/O execution |
 | - consumes work items |
 | - reports outcomes |
 +-------------+--------------+
 |
 | outbound connections (controlled egress)
 v
 +----------------------------+
 | External Endpoints |
 | (partners / on-prem) |
 +----------------------------+

 +----------------------------+
 | Secrets Manager + KMS |
 | (per-tenant) |
 +----------------------------+
```

Container responsibilities (target-state intent):

| Container | Target technology / posture | Primary responsibilities |
|---|---|---|
| UI micro-frontend | React 19 + Next.js 15 + Module Federation (platform baseline) | Operator screens for configuration and operational tracking; calls BFF only; never calls domain APIs directly. |
| BFF | .NET 10 Native AOT | Browser-safe, tenant-aware API surface; request shaping/aggregation; error normalization; does not access databases and does not perform protocol I/O. |
| Domain APIs (commands + queries) | .NET 10 | Authoritative domain validation and persistence orchestration; publish domain events/work items; serve query/read-model endpoints suitable for operator UX. |
| Transfer Worker(s) | .NET 10 worker runtime | Consume work items; perform protocol interactions with external endpoints; call back into domain command endpoints to record authoritative outcomes and tracking state. |
| Event backbone | SNS/SQS pattern (platform baseline) | Tenant-scoped event distribution and durable work item delivery across modules and within FXFER. |
| Aurora PostgreSQL (per tenant) | Aurora PostgreSQL database-per-tenant (platform baseline) | System of record for FXFER configuration and tracking write model; hosts tenant-scoped read models/projections as applicable. |
| Tenant registry + feature flags | Platform capability (Tenant Registry) | Resolve tenant policies (enabled features, protocol allowances, retention policies, data handling policy). |
| Secrets Manager + KMS | Platform baseline | Store per-tenant DB credentials and endpoint secrets; encrypt at rest and control key access policies by tenant. |

 final service boundaries and deployables (single â€œFXFER APIâ€ vs separate config/execution/tracking services; query surface split; Lambda vs container/EKS runtime per workload).
 rules engine scope/governance (what is expressed as InRule vs code, and how rules are versioned per tenant).

## 3.4 Technology Stack

Status: Partial â€” mandated platform technologies captured; FXFER-specific compute placement and protocol connectivity details are defined by the decision register (Section 16).

The table below captures **technology by concern**, explicitly calling out what is mandated by the platform baseline vs what remains an FXFER decision.

| Concern | Technology / Pattern | Notes / Status |
|---|---|---|
| Authoritative platform posture | Provider Plus Modernization ARB packet (v5, June 2026) | Inherited baseline for compute, identity, tenant isolation, eventing, and observability. **Status: Inherited** |
| UI delivery | React 19 + Next.js (micro-frontend) served from S3 + CloudFront | Platform baseline posture for micro-frontends. **Status: Inherited** |
| Browser â†’ backend API shape | BFF pattern | Browser calls BFF only; BFF is the UX contract boundary. **Status: Complete** |
| BFF runtime | .NET 10 Native AOT | Explicitly settled for FXFER. **Status: Complete** |
| Domain API runtime | .NET 10 (ASP.NET Core) | Platform baseline favors serverless APIs; FXFER specific packaging (AOT vs container fallback) is not fully settled. FXFER domain APIs target Native AOT unless protocol or library constraints require container packaging fallback, consistent with ARB guidance. **Status: Partial** |
| Command vs query separation | CQRS (commands vs queries; async projections/read models) | CQRS is mandatory at the platform level; FXFER read-model granularity and indexing is open. CQRS read-model granularity/indexing/retention. **Status: Partial** |
| Cross-domain integration | Single Event Backbone (SNS/SQS pattern) | All cross-domain state propagation occurs via the event backbone (no direct synchronous domain-to-domain calls). **Status: Complete** |
| Multi-tenancy isolation | Aurora PostgreSQL database-per-tenant | Tenant routing uses tenant_id from JWT and platform connection routing posture. **Status: Complete** |
| Tenant metadata + rollout control | Tenant Registry + feature flags | Platform baseline capability. **Status: Inherited** |
| Secrets and key management | AWS Secrets Manager + AWS KMS (per tenant) | Platform baseline posture for credentials and encryption controls. **Status: Inherited** |
| Rules / policy evaluation | InRule | Platform baseline includes InRule for tenant-specific rules; FXFER scope/governance is defined by the decision register (Section 16). rules engine scope/governance. **Status: Partial** |
| Observability | OpenTelemetry | All services/workers emit traces/metrics/logs; tenant_id + correlationId + traceId must be propagated end-to-end. **Status: Complete** |
| Browser security | CSRF/session posture | CSRF/session strategy. **Status: Partial** |
| Scheduling & redrive | Triggering, retries, rerun/redrive tooling | scheduling/trigger model and rerun/redrive controls. **Status: Partial** |
| Legacy SOAP/WCF | Migration/bridge posture | legacy SOAP/WCF migration posture. **Status: Partial** |
| Partner connectivity | Connectivity, onboarding, protocol constraints | partner endpoint connectivity pattern. **Status: Partial** |
| File payload classification | Per file type policies (PII/PHI, scanning, storage) | file payload classification by file type. **Status: Partial** |
| Tracking/artifact retention | Retention policy | retention windows for tracking/artifacts. **Status: Partial** |
| Version support windows | Backward compatibility policy | version support window policy. **Status: Partial** |

## 3.5 End-to-End Request Flow

Status: Partial â€” core configuration and execution flows documented; scheduling and redrive controls are defined by the decision register (Section 16).

This section captures the minimum two operator-centric end-to-end flows required to reason about FXFER behavior: (1) configuration writes, and (2) transfer execution.

**Flow 1 â€” Operator configuration write (create/update)**

Key requirements that apply to this flow:

- Validation must occur at **two boundaries**: request/schema validation at the BFF boundary, and domain/business validation at the domain command boundary.
- Audit and troubleshooting must be correlation-first: `tenantId`, `correlationId`, and `traceId` must tie together the browser request, the command handling, and any resulting events.

```
Participants:
 Operator | UI | BFF | FXFER Command API | Aurora (tenant DB) | Event Backbone

Operator UI BFF FXFER Command API Aurora (tenant DB) Event Backbone
 | | | | | |
 |--(1) Auth/OIDC flow ---------->| | | |
 |<-(2) JWT w/ tenant_id--------- | | | |
 | | | | | |
 |--(3) Create/Update request---->|--(4) HTTPS (JWT) -->|--(5) HTTPS (JWT) ---->| |
 | | | (schema validation)| (authz + domain val)| |
 | | | |--(6) Write config ---->|--(7) Commit -------->|
 | | | | + audit correlation | |
 | | | |--(8) Publish event --------------------------->|
 | |<-(9) Response---|<-(9) Response-------| | |

Notes:
 - (8) Publish can be implemented via an outbox pattern.
 Use a transactional outbox to prevent â€œwrite succeeded, publish failedâ€ drift.
```

**Flow 2 â€” Transfer execution (trigger â†’ work item â†’ worker â†’ outcomes)**

Key requirements that apply to this flow:

- The write model (domain API + per-tenant DB) is authoritative for execution intent and outcomes.
- Workers consume work with at-least-once semantics and must therefore report outcomes idempotently.

```
Participants:
 Trigger | FXFER Command API | Aurora (tenant DB) | Event Backbone | Worker | External Endpoint | FXFER Command API

Trigger FXFER Command API Aurora (tenant DB) Event Backbone Worker External Endpoint
 | | | | | |
 |--(1) Trigger intent-->| | | | |
 | (UI/Scheduler/API) |--(2) Validate+AuthZ-->| | | |
 | |--(3) Persist intent-->|--(4) Commit --------->| | |
 | |--(5) Publish work item ---------------------->|--(6) Consume ---->| |
 | | | | |--(7) Protocol I/O->|
 | | | | |<-(7) Results ------|
 | |<-(8) Record outcome --|<-(9) Persist outcome--| | |
 | | (idempotent cmd) | (authoritative) | | |
 | |--(10) Publish outcome event ----------------->| | |

Notes:
 - (1) Triggering model is not settled.
 scheduling/trigger model and rerun/redrive controls.
 - (7) Partner/on-prem connectivity posture is not settled.
 partner endpoint connectivity pattern.
 - Worker secrets (endpoint credentials, certificates) are retrieved tenant-scoped.
 Inherited from ARB: per-tenant Secrets Manager credentials and per-tenant KMS posture (Provider Plus Modernization ARB packet (v5, June 2026)).
```

## 3.6 Component Interaction Matrix

Status: Partial â€” interaction responsibilities documented; concrete timeout/retry numbers and redrive tooling are defined by the decision register (Section 16).

The matrix below is written at the contract level (auth context, tenant propagation, resiliency expectations, and observability), without inventing specific numeric timeouts.

| From | To | Interface | Auth context propagation | Resiliency posture (policy-level) | Idempotency boundary | Observability expectations |
|---|---|---|---|---|---|---|
| UI micro-frontend | BFF | HTTPS | Operator JWT (tenant_id claim) attached per request. CSRF/session strategy impacts whether tokens are stored in cookies vs in-memory. | Retries only for safe GETs; no automatic retries for non-idempotent writes from the browser. | N/A (browser initiates). | Emit client-side correlation headers; BFF emits server traces/logs/metrics. |
| BFF | FXFER Domain APIs (command/query) | HTTPS (private) | BFF uses either token passthrough or an on-behalf-of exchange consistent with platform identity; exact method is not settled here. Tenant_id must be preserved. | Bounded retries for idempotent operations; circuit-breaker posture on transient network failures. | support `Idempotency-Key` on command endpoints invoked by UI and worker paths. | Propagate `traceparent`/correlation headers; domain APIs emit spans tagged with `tenantId` and command/query name. |
| FXFER Domain APIs | Aurora PostgreSQL (per tenant) | SQL | DB credentials are tenant-scoped and retrieved via Secrets Manager. | Standard DB retry for transient connection failures; no retries that can cause duplicate writes inside a single logical command without idempotency. | Write-side idempotency enforced at the command handler boundary (see worker outcome writes). | DB access spans; structured logs without payload leakage; metrics for DB latency/error rate. |
| FXFER Domain APIs | Tenant Registry / flags | HTTPS/SQL (platform capability) | Service identity (platform) and tenant_id for lookups. | Cacheable reads; bounded retries for transient errors. | N/A | Traces for lookups; metrics for cache hit/miss when applicable. |
| FXFER Domain APIs | Event Backbone (SNS/SQS pattern) | Async publish | Service identity; tenant_id included in envelope. | At-least-once publish semantics; dead-letter posture for invalid payloads. transactional outbox for publish consistency. | Message consumers must be idempotent (inbox/idempotency keys). | Publish spans + event metrics (publish success/failure, queue depth/age). |
| Transfer Worker(s) | Event Backbone | Async consume | Service identity; tenant_id read from envelope and applied to subsequent calls. | At-least-once consumption; retries with backoff; DLQ for poison messages. redrive controls and operator tooling. | Worker must treat work item handling as idempotent by executionId/attempt. | Consumer lag metrics; spans per message with tenantId and executionId tags; structured logs without secrets. |
| Transfer Worker(s) | External Endpoints | SFTP/FTPS/HTTPS (as applicable) over secure transport | Worker uses tenant-scoped credentials/certificates. | Timeouts, retries, and circuit-breaking are protocol-specific with bounded retries and DLQ posture . Connectivity uses private subnets + inspected egress . | Protocol operations must avoid duplicate delivery where possible; where not possible, outcomes must be recorded idempotently and duplicate delivery risk must be surfaced. | Per-endpoint latency/error metrics; redaction/masking in logs; traces must never include payload bytes . |
| Transfer Worker(s) | FXFER Domain APIs (commands) | HTTPS (private) | Service identity; tenant_id from envelope. | Bounded retries for transient network errors, paired with command idempotency. | Record-outcome commands must be idempotent on (tenantId, executionId, stepId, attempt). | Correlated trace across worker span and API span; metrics for outcome write failures. |
| Other domains | Event Backbone | Async consume | Tenant-scoped envelopes. | At-least-once consumption; DLQ on schema violations. | Consumer-side idempotency required. | Cross-domain trace correlation via traceId/correlationId in event envelope. |

## 3.7 Deployment and Runtime View

Status: Partial â€” ARB-aligned runtime view captured; compute placement (especially workers) and partner connectivity are defined by the decision register (Section 16).

```

 (Operator Browser)
 |
 | HTTPS
 v
 +-------------------------------+
 | CloudFront + S3 |
 | - Shell + FXFER micro-frontend|
 +-------------------------------+
 |
 | HTTPS
 v
 +-------------------------------+ +-------------------------------+
 | API Gateway (Edge) |<------->| Platform Identity Broker (OIDC)|
 | - JWT validation | +-------------------------------+
 | - WAF policy (regional) |
 +-------------------------------+
 |
 | HTTPS (private integration)
 v
 +--------------------------------------------------------------+
 | Private Compute Plane (AWS) |
 | |
 | +--------------------+ +-------------------------------+ |
 | | FXFER BFF | | FXFER Domain APIs | |
 | | .NET 10 Native AOT | | .NET 10 (AOT preferred) | |
 | +---------+----------+ +---------------+---------------+ |
 | | | |
 | | HTTPS | SQL (tenant-routed)|
 | v v |
 | +--------------------+ +------------------------------+ |
 | | Event Backbone | | Aurora PostgreSQL | |
 | | SNS/SQS pattern | | - db_shared (tenant registry)| |
 | +---------+----------+ | - per-tenant FXFER DBs | |
 | | +------------------------------+ |
 | | consume ^ |
 | v | |
 | +--------------------+ | |
 | | Transfer Worker(s) |---- controlled egress -----------+ |
 | | (protocol I/O) | | |
 | +---------+----------+ | |
 | | | |
 | v | |
 | External Endpoints (partners / on-prem) | |
 | |
 | +--------------------+ |
 | | Secrets Manager + |<------------------------------------+
 | | KMS (per tenant) |
 | +--------------------+
 +--------------------------------------------------------------+

Notes:
 - Compute packaging is inherited from platform posture in Provider Plus Modernization ARB packet (v5, June 2026).
 final service boundaries and deployables (including Lambda vs container/EKS runtime per workload).
 - Worker runtime placement may require containers for long-running protocol operations.
 scheduling/trigger model and rerun/redrive controls.
 - External connectivity, allowlisting, and onboarding are defined by the decision register (Section 16).
 partner endpoint connectivity pattern.
```

## 3.8 Key Sequence Diagrams

Status: Partial â€” key behavioral sequences captured at a conservative level; scheduling/redrive and suspension control details are defined by the decision register (Section 16).

The sequences below are intentionally behavior-focused. They demonstrate **where validation occurs**, **where outcomes become authoritative**, and **how tenant + correlation context flows**, without inventing new controls beyond the target posture.

**Sequence 1 â€” Create/update configuration (including validation failure path)**

```
Operator UI BFF FXFER Command API Aurora (tenant DB)
 | | | | |
 |-- Edit form->| | | |
 |-- Save ------|-- HTTPS (JWT) ->| | |
 | | |-- schema validate -->| |
 | | | (required fields) | |
 | | |-- call command ------>|-- authz + domain val->|
 | | | | (business rules) |
 | | | |-- write config ------>|
 | | | |<- OK / commit --------|
 | |<-- 200 OK ------|<-- 200 OK -----------| |

Validation failure (example):
Operator UI BFF FXFER Command API
 | | | |
 |-- Save ------|-- HTTPS (JWT) ->| |
 | | |-- schema validate -->|
 | | |<- 400/422 ----------|
 |<-- show error| | |

Notes:
 - Domain validation failures should be returned as structured errors suitable for operator remediation.
 - CSRF/session strategy (impacts how the browser authenticates requests).
```

**Sequence 2 â€” Trigger/run execution intent â†’ worker processing â†’ record outcomes**

```
Trigger FXFER Command API Aurora (tenant DB) Event Backbone Worker External Endpoint
 | | | | | |
 |-- request run -------->|-- validate+authz ---->| | | |
 | |-- persist intent ----->|-- commit ------------>| | |
 | |-- publish work item -------------------------->|-- consume -------->| |
 | | | | |-- protocol I/O --->|
 | | | | |<-- result ----------|
 | |<-- record outcome ----|<-- persist outcome ----| | |
 | | (idempotent cmd) | (authoritative) | | |
 | |-- publish outcome event ---------------------->| | |

Notes:
 - scheduling/trigger model and rerun/redrive controls.
 - partner endpoint connectivity pattern.
 - file payload classification by file type (impacts logging/redaction and storage posture).
```

**Sequence 3 â€” Suspension blocks execution + operator unsuspension flow**

```
Operator UI BFF FXFER Command API Aurora (tenant DB)
 | | | | |
 |-- Suspend -->|-- HTTPS (JWT) ->|-- call command ----->|-- validate+authz ---->|-- write suspended flag -->|
 | | | |<- 200 OK -------------|<-- commit ------------------|
 |<-- OK -------| | | |

Scheduler/Trigger FXFER Command API Aurora (tenant DB)
 | | |
 |-- run intent->|-- check suspended ->| (reads config state)
 | |<- reject (suspended)-|

Operator UI BFF FXFER Command API
 | | | |
 |-- Unsuspend->|-- HTTPS (JWT) ->|-- call command ----->|-- validate+authz ---->
 | | |<-- 200 OK -----------|

Notes:
 - trigger model controls how â€œblockedâ€ executions are retried after unsuspension.
 - rules engine scope/governance may influence suspension policy conditions.
 - retention windows for tracking/artifacts (including suspended/blocked intents).
```

**Sequence 4 â€” Rerun / redrive flow (high level)**

```
Operator UI BFF FXFER Command API Aurora (tenant DB) Event Backbone
 | | | | | |
 |-- Request rerun->-- HTTPS (JWT)->-- call command ---->|-- validate+authz ---->| |
 | | | |-- persist rerun intent>|-- commit ------------>|
 | | | |-- publish new work item ---------------------->|
 | |<-- 202/200 -----|<-- 202/200 ---------| | |

Notes:
 - definition of rerun vs redrive (operator permissions, guardrails, DLQ handling).
 - read-model retention/indexing impacts how operators discover eligible reruns.
```

---

# 4. Frontend

## 4.1 Architecture Overview

Status: Partial â€” baseline patterns are set by ARB; module-specific shell conventions and UX details are defined by the decision register (Section 16).

The MMS-CMS-FXFER user experience is delivered as a micro-frontend (MFE) mounted into the Provider Plus shared UI shell, consistent with **ARB-Provider-Plus-Modernization (v5, June 2026)** (Provider Plus Modernization ARB packet (v5, June 2026)). The FXFER UI is an authenticated, internal operator experience focused on:

- Transfer configuration (source locations, destinations, endpoint profiles)
- Credential reference administration (under strict authorization)
- Operational tracking (search and inspect outcomes)

From the browserâ€™s perspective, the architecture is intentionally layered so the browser only has one FXFER backend integration surface: the FXFER BFF.

```
Operator Browser
 |
 | HTTPS (OIDC/JWT, tenant_id claim; trace context)
 v
Provider Plus UI Shell (host)
 |
 | loads
 v
FXFER Micro-Frontend (React + Next.js)
 |
 | HTTPS (browser -> BFF; no direct calls to domain APIs)
 v
FXFER BFF (.NET 10 Native AOT)
 |
 | Internal HTTPS
 v
FXFER Domain APIs (.NET 10; CQRS)
 |
 +--> Aurora PostgreSQL (database-per-tenant)
 |
 +--> Event Backbone (SNS/SQS pattern; cross-domain integration)

Telemetry (OpenTelemetry): Browser (RUM: ) + BFF + APIs -> tenant-scoped correlation/traces
```

**Browser trust boundaries (high level).**

- The browser is an untrusted execution environment. The UI assumes all client state can be manipulated and therefore treats client-side validation as advisory.
- The FXFER BFF is the first FXFER-controlled security boundary. It performs request shaping, enforces a browser-safe surface (CORS, request size constraints, header normalization), and avoids exposing internal domain-service topology to the browser.
- FXFER domain APIs remain the authoritative business boundary: they enforce tenant-scoped invariants, authorization, and state transitions regardless of what the UI or BFF attempts.

**Why a BFF exists (and why UI does not call domain APIs directly).**

The BFF exists to provide a browser-safe, UX-oriented API faÃ§ade and a consistent trust boundary in front of the domain APIs:

- **Response shaping / aggregation:** present UI-friendly view models without exposing internal API boundaries.
- **Error normalization:** normalize downstream errors into a predictable UI contract.
- **Security boundary:** simplify frontend auth handling and centralize safeguards tied to browser threat models.
- **Tenant context enforcement:** ensure every request is tenant-aware, consistent with the ARB requirement for tenant-scoped JWT validation and tenant_id propagation (Provider Plus Modernization ARB packet (v5, June 2026)).

Tenant identity is derived from IRM broker-issued JWTs that carry a `tenant_id` claim (Provider Plus Modernization ARB packet (v5, June 2026)). The UI includes the JWT on browser â†’ BFF calls; the BFF validates the JWT and uses the `tenant_id` claim as the authoritative tenant selector for all downstream calls.

## 4.2 Technology Stack

Status: Partial â€” core stack is inherited from ARB.

The FXFER frontend stack is primarily inherited from the Provider Plus platform posture described in Provider Plus Modernization ARB packet (v5, June 2026). The table below captures the minimum target-state decisions plus items that are closed and captured in the decision register (Section 16).

| Concern | Technology / Pattern | Notes / Status |
|---|---|---|
| UI composition | Micro-frontend mounted into shared shell | Inherited (ARB): â€œReact 19 Next.js micro frontendsâ€ (Provider Plus Modernization ARB packet (v5, June 2026)). FXFER is one remote in the shell. |
| UI framework | React 19 | Inherited (ARB). |
| App framework | Next.js (platform standard) | Inherited (ARB) at the â€œNext.js micro frontendsâ€ level; the specific major version used for FXFER is not asserted here. confirm the Next.js major version and router mode standard used by the shared shell and remotes. |
| Language | TypeScript | the Provider Plus MFE standard is TypeScript for all remotes. |
| MFE composition wiring | Shared shell loader (Module Federation or platform-standard equivalent) | The ARB establishes micro-frontends but does not mandate a specific browser-side composition technology in Provider Plus Modernization ARB packet (v5, June 2026). FXFER uses the same composition wiring approach as the shared shell and other remotes. |
| Styling / components | Shared platform UI kit + consistent theming | FXFER uses the platform-standard component library and theming approach so shell and remote have consistent UX/a11y behavior. |
| Frontend hosting | S3 + CloudFront | Inherited (ARB): micro frontends served from S3 and CloudFront (Provider Plus Modernization ARB packet (v5, June 2026)). |
| Backend integration model | UI â†’ BFF only (no UI â†’ domain APIs) | Inherited (ARB): â€œcalling backend APIs exclusively through the BFFâ€ (Provider Plus Modernization ARB packet (v5, June 2026)). |
| Tenant resolution | `tenant_id` from IRM broker-issued JWT | Inherited (ARB): tenant_id propagation through IRM broker-issued JWTs (Provider Plus Modernization ARB packet (v5, June 2026)). |
| CQRS alignment | Reads via queries/read models; writes via commands | Inherited (ARB): â€œCQRS is mandatoryâ€ (Provider Plus Modernization ARB packet (v5, June 2026)). read-model granularity/indexing/retention affects feasible operator search UX. |
| Frontend observability | Trace context propagation + error reporting | Inherited (ARB): OpenTelemetry everywhere, tagged with `tenant_id`, `correlation_id`, `trace_id` (Provider Plus Modernization ARB packet (v5, June 2026)). confirm the standard frontend/RUM instrumentation approach and exporter path. |
| Accessibility posture | Shared component library + tested patterns | define an explicit accessibility conformance target (for example WCAG 2.1 AA) for all operator UIs, with automated checks in CI. |

## 4.3 Screen Inventory

Status: Partial â€” initial inventory exists; detailed workflow behavior depends on the trigger model and CQRS read-model decisions.

FXFER is an operational UI. The key organizing principle is â€œwhat the operator is trying to doâ€ rather than the internal service structure.

The UI surfaces cluster into three groups:

1. Transfer configuration (source locations, destinations, and execution parameters)
2. Credential administration (endpoint definitions and credential references)
3. Operational tracking (search and inspect file movement outcomes)

Indicative screens (baseline inventory):

- Search Source Locations
- View Transfer Configuration
- Edit Transfer Configuration
- Search Endpoint Credentials
- View/Edit Endpoint Credentials
- Search File Transfer Tracking
- File Transfer Tracking Detail

The following table expands the baseline inventory into an operator-workflow-oriented view.

| Screen | Primary user goal | Primary queries / commands (conceptual) | Sensitive data considerations | Observability notes |
|---|---|---|---|---|
| Search Source Locations | Find transfer configurations to review or change | `SearchSourceLocations (query)` | Configuration metadata may include endpoint identifiers; ensure secrets are never rendered. | Include `correlation_id` on search requests; measure query latency and pagination behavior. read-model indexing/retention directly affects UX. |
| View Transfer Configuration | Validate what will happen when a source location runs | `GetSourceLocationDetail (query)`; `GetDestinationsForSourceLocation (query)` | Redact any secret references that could be abused (for example: key IDs, usernames) per least privilege. | Record view-load timing and downstream dependency failures as structured frontend errors. |
| Edit Transfer Configuration | Create/update a source location and its execution parameters | `CreateSourceLocation (command)` / `UpdateSourceLocation (command)`; `ValidateSourceLocation (query/command)` | Prevent accidental exposure of sensitive endpoint metadata; enforce role-gated edit actions. rules engine scope/governance may change which validations are explainable to users. | Writes must be traceable end-to-end (browser span -> BFF -> command API). Capture user-visible validation errors as metrics (sanitized). |
| Destination view/edit | Define where and how files are delivered | `CreateDestination (command)` / `UpdateDestination (command)`; `GetDestinationDetail (query)` | Destination parameters can embed sensitive operational details (hostnames, paths). Redact aggressively unless operator is privileged. partner connectivity pattern impacts what endpoint details are meaningful/allowed. | Track command outcomes and resulting read-model refresh time (how quickly details update after save). |
| Search Endpoint Credentials | Locate endpoint profiles / credential references | `SearchEndpointCredentials (query)` | Highly sensitive: this surface must not expose secrets; â€œsearchabilityâ€ itself may be restricted. | Ensure correlation/tracing headers exist, but avoid browser-side logging of sensitive values; enforce server-side audit trails for access. |
| View/Edit Endpoint Credentials | Create/update endpoint definitions and secret references (not secrets themselves) | `GetEndpointCredentialDetail (query)`; `CreateEndpointCredential (command)` / `UpdateEndpointCredential (command)` | Strict RBAC; mask values by default; require explicit â€œrevealâ€ actions (if allowed) with additional audit. payload classification may drive redaction and warning banners across tracking views. | Capture privileged actions as structured events (no payloads) so operator support can correlate the browser action with backend audit/trace. |
| Search File Transfer Tracking | Find outcomes for troubleshooting (by source location, time window, status) | `SearchTransferTracking (query)` | Tracking data may include partner identifiers and file names; treat as sensitive until defines payload classification and redaction rules. retention windows impact how far back searches can go and default time windows. | UX performance depends on (projection design, retention, indexes). Instrument empty-result vs error separately. |
| File Transfer Tracking Detail | Investigate one transfer, its attempts, and its diagnostics | `GetTransferTrackingDetail (query)`; `GetTransferAttemptTimeline (query)` | Redact secrets and potentially sensitive error detail; guard â€œdownload artifactâ€ actions behind authorization. classification governs what fields are visible. | This is the primary â€œoperator debugâ€ surface; expose correlation identifiers (at least as support metadata) so backend traces can be found quickly. |

Notes:

- the scheduling/trigger model and rerun/redrive controls will affect whether â€œRun nowâ€, â€œRerunâ€, â€œRedriveâ€, and DLQ-related actions exist as buttons on configuration/tracking screens.
- retention windows for tracking/artifacts impacts default search time windows and how far back operators can troubleshoot.
- CQRS read-model design governs search UX feasibility (filters, response time, and cost of common queries).

## 4.4 Navigation and Shell Integration

Status: Partial â€” integration posture defined; exact shell route conventions and cross-MFE standards require confirmation.

FXFER is mounted into the shared shell and must behave like a â€œgood citizenâ€ micro-frontend:

- It participates in shell-level navigation (global header, sidebar/module nav, user menu) rather than owning its own top-level chrome.
- It supports deep links (copy/paste URLs) to key operational views (configuration detail, tracking detail), subject to authorization.
- FXFER relies on shell-provided session context (tenant, user identity/roles, feature flags surfaced to the browser) rather than re-deriving tenant identity inside the MFE.

Shell integration conventions:

- The shared shell owns global navigation, breadcrumbs, and tab conventions; FXFER does not implement its own top-level chrome.
- FXFER is mounted under a single shell route prefix (consistent with other remotes) and supports deep links to key operational views.
- Return-to-search behavior is URL-driven using querystring state so browser back/forward restores prior results without server-side UI session state.

- reserve a single route prefix for the module (for example `/fxfer/*`) to avoid collisions with other micro-frontends.
- the shell owns login/logout routes and applies route guards before rendering the FXFER MFE.
- confirm shell conventions for breadcrumbs, tab patterns, and â€œreturn-to-searchâ€ behavior so FXFER screens are consistent with other operator modules.

Shell integration must also remain consistent with CQRS:

- After a command (create/update), the UI should navigate to a query-backed view state and tolerate eventual consistency between write completion and projection availability.
- For long-running actions that transition through intermediate states (queued, running, retrying, redriving), FXFER should use the platform-standard â€œin progressâ€ UX pattern once defined. scheduling/trigger model and rerun/redrive controls determine which long-running states are represented.

## 4.5 Tenant Customization

Status: Partial â€” runtime variability posture defined.

Tenant-specific differences (branding, enabled features, validation rules, and data lists) are resolved at runtime via BFF-mediated configuration retrieval, not via per-tenant builds.

This aligns to the ARB posture of â€œper-tenant configuration via a Tenant Registryâ€ and tenant isolation via Aurora PostgreSQL database-per-tenant (Provider Plus Modernization ARB packet (v5, June 2026)). Even though the UI does not connect to databases directly, the tenant isolation model matters because it drives how tenant identity is selected and propagated end-to-end.

Tenant customization principles:

- **No per-tenant UI builds:** FXFER UI assets are not rebuilt per tenant; tenant differences are data/config driven at runtime.
- **Tenant identity comes from auth:** tenant identity is derived from the IRM broker JWT `tenant_id` claim (Provider Plus Modernization ARB packet (v5, June 2026)).
- **BFF-mediated variability:** the browser does not call the Tenant Registry directly; tenant-specific lists, flags, and rule metadata are retrieved via the BFF.

Customization mechanisms (pattern-level):

- **Feature flags:** ARB introduces a feature flag service for per-tenant runtime toggles (Provider Plus Modernization ARB packet (v5, June 2026)). deliver a tenant-scoped â€œFXFER feature manifestâ€ to the browser at session start so UI can enable/disable screens/actions without rebuild.
- **Tenant registry lookups:** BFF queries the Tenant Registry and/or shared configuration services and returns only the FXFER-relevant subset to the UI.
- **Rules and explanations:** rules engine scope/governance determines whether the UI can show â€œwhyâ€ explanations for validation and policy decisions. UI treats validation/error messages as server-supplied text/codes rather than encoding rules client-side.

## 4.6 API Communication

Status: Partial â€” primary posture is set (UI â†’ BFF only); concrete contract details and long-running UX mechanics are defined by the decision register (Section 16).

**Non-negotiable boundary:** the UI does not talk to databases and does not call FXFER domain APIs directly. All browser traffic targets the FXFER BFF, which then calls FXFER domain APIs over internal HTTPS.

**Authentication and tenant context.**

- Browser â†’ BFF requests are authenticated using an IRM broker-issued JWT.
- The JWT includes a `tenant_id` claim and the platform validates tenant_id claims â€œat every hopâ€ (Provider Plus Modernization ARB packet (v5, June 2026)).
- The BFF derives the tenant context from the validated JWT and applies it consistently to downstream calls.

Downstream propagation details are intentionally not over-specified in this section:

- BFF uses either token passthrough or an on-behalf-of exchange consistent with platform identity; tenant_id must be preserved end-to-end.

**Request headers and correlation propagation (high level).**

The ARB baseline requires OpenTelemetry signals tagged with `tenant_id`, `correlation_id`, and `trace_id` (Provider Plus Modernization ARB packet (v5, June 2026)). For UI â†’ BFF communication:

- `Authorization: Bearer <JWT>` is present on authenticated requests.
- standardize a single correlation header name (for example `x-correlation-id`) and ensure the UI forwards or generates it for every request.
- propagate W3C Trace Context (`traceparent` / `tracestate`) to connect frontend spans to backend spans.

**Write idempotency for operator retries.**

Operators will retry actions (save, rerun, redrive) when failures occur. Command APIs must therefore support idempotency (also reinforced in Section 6). For UI â†’ BFF writes:

- support an `Idempotency-Key` header on command endpoints invoked by UI paths.

**Error normalization contract.**

The UI must not depend on internal error shapes of downstream services.

- BFF normalizes all errors into a Problem Details-style response (RFC 7807 compatible) with safe, non-PII fields and stable error codes.

**Long-running work: observing outcomes without inventing a specific mechanism.**

Many FXFER operations are not instantaneous from the operatorâ€™s perspective (for example: scheduled/triggered runs, transfer execution, retries). The UI needs an explicit observation model that is compatible with CQRS and the event backbone.

- Primary CQRS-friendly mechanism: the UI reads from query/read models that are updated asynchronously from the command side. read-model granularity/indexing/retention has direct impact on feasible UI queries and perceived performance.
- Operator-triggered execution control: scheduling/trigger model and rerun/redrive controls determine what â€œin progressâ€ states exist, how they are represented, and what actions can be taken.
- History and â€œhow far backâ€: retention windows for tracking/artifacts determine default search windows and whether older executions can be investigated.

Optional push-style UX (event-driven updates, notifications) may exist, but the platform-standard mechanism is not established here and must be treated as open until confirmed. operator-facing â€œlive updateâ€ mechanism for long-running state changes (poll-only vs push).

## 4.7 Hosting, Security, Observability and Accessibility

Status: Partial â€” hosting posture is inherited from ARB; security headers, CSRF/session strategy, and frontend RUM specifics are defined by the decision register (Section 16).

**Hosting posture (inherited baseline).**

FXFER UI assets are hosted as static micro-frontend content served from S3 via CloudFront, consistent with the platform posture described in Provider Plus Modernization ARB packet (v5, June 2026).

Implications:

- Static assets must be cacheable and versioned; sensitive data is never embedded at build time.
- Authentication is enforced for API calls (browser â†’ BFF) via JWT validation with tenant_id claims at the edge and in the BFF (Provider Plus Modernization ARB packet (v5, June 2026)). protected routes are enforced via shell-level route guards. Static assets themselves must not be treated as confidential.

**Security (browser-specific).**

FXFER screens expose operational data and may touch highly sensitive configuration surfaces (credential references). Browser security therefore emphasizes preventing data exfiltration, preventing cross-tenant leakage, and ensuring safe handling of authenticated sessions.

- **CSP / XSS:** ship a strict Content Security Policy suitable for React/Next.js (avoid `unsafe-inline`; restrict `script-src`, `style-src`, and `connect-src` to only the BFF and approved telemetry endpoints).
- **CSRF / session strategy:** CSRF/session strategy for a micro-frontend UI calling a dedicated BFF.
- **Session/token storage:** avoid storing long-lived tokens in `localStorage`; prefer browser-safe session handling aligned to the chosen solution.
- **Clickjacking:** deny framing (CSP `frame-ancestors`), except where explicitly required by the platform shell.
- **Sensitive data handling:** file payload classification by file type impacts what tracking fields can be shown, what must be redacted, and whether warning banners are required.

**Observability.**

The ARB baseline requires OpenTelemetry signals tagged with `tenant_id`, `correlation_id`, and `trace_id` (Provider Plus Modernization ARB packet (v5, June 2026)). For the frontend:

- The UI propagates correlation and trace headers to the BFF (see Section 4.6) so backend spans can be joined to user actions.
- Frontend errors should be captured as structured events (sanitized) so operator-visible failures can be traced end-to-end.

The specific browser telemetry/RUM mechanism is not asserted as fact:

- confirm the standard frontend/RUM instrumentation approach and exporter path.

**Accessibility.**

The ARB packet does not establish an explicit accessibility conformance target in Provider Plus Modernization ARB packet (v5, June 2026). Therefore:

- define and enforce an explicit accessibility standard (for example WCAG 2.1 AA) for operator UIs via shared component library standards and automated checks.

## 4.8 Public or Anonymous Flows

Status: N/A â€” MMS-CMS-FXFER UI is an authenticated operator experience.

FXFER is not a partner-facing portal and is not a public enrollment-style workflow. It exists to support state and internal operations staff configuring and operating controlled file transfers.

Therefore:

- No anonymous access flows are in scope for FXFER.
- No â€œpublic JWTâ€ or unauthenticated onboarding flows are in scope.
- The UI is treated as an internal operational surface where every request is authenticated and tenant-scoped.

If future requirements introduce partner-facing screens (for example: partner-managed endpoint onboarding or partner-visible tracking), they must be treated as a separate product/workflow with its own threat model, authorization model, and UX requirements. Such work must be reconciled with:

- partner endpoint connectivity pattern (impacts what partners can configure and how that configuration is validated)
- file payload classification by file type (impacts what tracking details can be safely exposed)

---

# 5. BFF (Backend for Frontend)

## 5.1 Purpose and Responsibilities

Status: Complete â€” responsibilities and trust boundaries defined.

The MMS-CMS-FXFER BFF is the module-owned API facade for the FXFER micro-frontend (React + Next.js in a shared shell) consistent with the platform target posture in Provider Plus Modernization ARB packet (v5, June 2026).

Trust boundaries:

- The browser is an untrusted environment.
- The BFF is the first FXFER-controlled trust boundary for browser-originated traffic.
- Domain APIs remain authoritative for domain validation, authorization, and state transitions.

The BFF is explicitly not allowed to access databases directly. All persistence is performed by FXFER domain services.

Concrete responsibilities of the BFF (pattern-level; specific endpoints are cataloged in OpenAPI):

- Browser-safe API facade: expose only UI-required, least-privilege operations; hide internal topology.
- Tenant enforcement and propagation: require tenant context, ensure it is consistently applied, and propagate it to downstream calls. Tenant isolation relies on Aurora PostgreSQL database-per-tenant routing in the domain boundary (see Provider Plus Modernization ARB packet (v5, June 2026)).
- Response shaping and aggregation: map domain DTOs into UI-friendly view models and perform safe fan-out to query endpoints when a screen needs multiple datasets.
- Error normalization: provide a stable error contract for the UI; normalize errors into an RFC 7807-compatible Problem Details response.
- Observability propagation: preserve end-to-end correlation and tracing context; standardize a single correlation header name, and propagate W3C Trace Context.
- Sensitive-data hygiene: ensure PII/secrets are not present in responses, logs, traces, or metrics attributes; redact before logging.

Conceptual request flow:

```
Browser (untrusted)
 |
 | HTTPS (JWT-presenting requests)
 v
FXFER BFF (.NET 10 Native AOT) [first FXFER trust boundary]
 |
 | HTTPS (service-to-service, tenant-scoped)
 v
FXFER Domain Services (authoritative)
 |
 | DB writes/reads (per-tenant database)
 v
Aurora PostgreSQL (database-per-tenant)

FXFER Domain Services --> Event Backbone (SNS/SQS pattern; async) [cross-domain only]
```

## 5.2 Routes

Status: Complete â€” BFF surface is defined and governed.

The BFF HTTP surface is contract-first and is limited to capability parity with the legacy operator experience: configuration management, endpoint profile management, tracking search/detail, and reference lookups required by the UI.

All FXFER BFF routes are prefixed under:

- /api/file-transfer/v1

Platform-standard health endpoints are exposed separately:

- GET /api/health/live
- GET /api/health/ready

### BFF route inventory (v1)

| Capability area | Route | Methods | Auth | Notes |
|---|---|---|---|---|
| Transfer configurations | /api/file-transfer/v1/transfer-configurations | GET, POST | Operator bearer JWT | List/search via query params; create new configuration. |
| Transfer configuration detail | /api/file-transfer/v1/transfer-configurations/{sourceLocationId} | GET, PUT, PATCH | Operator bearer JWT | PATCH is used for soft-disable/toggle active. |
| FTP server profiles | /api/file-transfer/v1/ftp-servers | GET, POST | Operator bearer JWT | List/create endpoint profiles. |
| FTP server profile detail | /api/file-transfer/v1/ftp-servers/{ftpServerId} | PUT, PATCH | Operator bearer JWT | PATCH is used for soft-disable/invalidate. |
| FTP server options | /api/file-transfer/v1/ftp-servers/options | GET | Operator bearer JWT | Name/id options list for dropdowns. |
| File tracking | /api/file-transfer/v1/file-tracking | GET | Operator bearer JWT | Search/list tracking records; filters via query params. |
| File tracking detail | /api/file-transfer/v1/file-tracking/{fileId} | GET | Operator bearer JWT | Tracking header + attempt history; redacted. |
| Source location options | /api/file-transfer/v1/source-locations | GET | Operator bearer JWT | Dropdown options. |
| Data lists | /api/file-transfer/v1/datalists | GET | Operator bearer JWT | Query params include contentId, startsWith, localeId. |
| Inbound target metadata | /api/file-transfer/v1/inbound-target-metadata | GET | Operator bearer JWT | Query param: contractName. |

Browser-facing security posture:

- Browser calls the BFF with bearer JWT on every request; the BFF is stateless and does not create authentication sessions/cookies.
- CORS is allow-listed to the approved shell origins.
- For state-changing requests, the BFF validates Origin/Referer and rejects mismatches.

## 5.3 Composed Endpoints

Status: Partial â€” composition patterns documented.

Composed endpoints exist to reduce browser round-trips and to keep the UI implementation from needing to understand domain API topology. Composition is limited to:

- Parallel query fan-out and join (response shaping)
- Response caching for safe, high-frequency reads (tenant-scoped keys)
- Normalizing error and empty-state behavior into consistent UI contracts

Composition explicitly does not include domain logic. Any authoritative validations, permissions, and state transitions remain in domain services.

Typical composition shapes (examples are conceptual; not route names):

| Composition (Concept) | What It Returns | Typical Downstream Calls (Conceptual) | Key Rules |
|---|---|---|---|
| â€œConfiguration detail view modelâ€ | A single payload that drives a config detail screen | One or more query reads for config + reference lists | No server-side mutation; safe to retry; tenant-scoped. |
| â€œTracking detail view modelâ€ | Tracking summary + attempt history + derived display fields | Query reads for tracking header + attempt list | Redaction must follow payload classification; never leak secrets. |
| â€œOperator dashboard summaryâ€ | Counts/summary cards for key operational statuses | Multiple query reads, aggregated | Must be bounded (timeouts + concurrency caps) to avoid N+1 cascades. |
| â€œBootstrap payloadâ€ | Everything needed to render initial page without extra calls | Reference lists + feature flags/config | restrict to FXFER-relevant subset only. |

Fan-out guardrails:

- The BFF caps concurrency per request (bulkhead) so one screen cannot exhaust outbound connections.
- Compositions fail â€œloudlyâ€ with structured errors rather than returning partial/ambiguous payloads unless explicitly designed as partial-success.

## 5.4 Session and Token Model

Status: Complete â€” token-only (no cookie session) posture defined.

The platform edge validates JWTs as described in Provider Plus Modernization ARB packet (v5, June 2026). FXFER adopts a stateless, token-only browser posture per 

Tenant derivation:

- The BFF derives `tenant_id` from a validated JWT claim.
- The BFF must not accept `tenant_id` as a query parameter by default because it becomes an untrusted caller-controlled input. If a query parameter or header fallback is introduced for operational tooling, it must be treated as explicitly privileged and audited.

Token handling:

- Browser calls the BFF through API Gateway using `Authorization: Bearer <JWT>` on every request.
- The BFF does not introduce cookie-based UI sessions and does not persist operator tokens server-side.
- For domain API calls that require operator context, the BFF forwards the operator JWT (or an equivalent on-behalf-of token) and preserves tenant and correlation context end-to-end.
- Avoid storing long-lived tokens in `localStorage`; prefer in-memory handling and platform-standard refresh patterns.

Observability and sensitive data requirements for auth/session flows:

- Correlation, trace, and tenant context must be attached to all inbound/outbound spans and logs (OpenTelemetry), tagging signals with `tenant_id`, `correlation_id`, and `trace_id` per Provider Plus Modernization ARB packet (v5, June 2026).
- Secrets (tokens, secret references, credentials) must never be returned to the browser or emitted to logs/traces/metrics attributes.

## 5.5 Contract Governance

Status: Partial â€” governance approach described.

The BFF is treated as an explicit contract boundary. BFF endpoints are not considered stable until they are represented in an OpenAPI contract.

Minimum governance posture:

- OpenAPI as the source of truth (generated from code or code generated from spec; exact direction is ).
- Error contract: RFC 7807-compatible Problem Details with stable error codes and no PII.
- Idempotency: accept an `Idempotency-Key` for BFF-mediated commands where operator retries are expected.
- Tenant scoping: tenant is derived from JWT; the contract must not allow cross-tenant resource access by accepting a caller-supplied tenant identifier.
- Schema evolution: additive-first changes; breaking changes require a version bump.

Contract change controls (target posture):

- Linting: OpenAPI lint rules for naming, pagination, and error shapes.
- Review: security review for any new composed endpoint to confirm no secret/PII leakage.
- Compatibility checks: automated diffs preventing accidental breaking changes to stable routes.

## 5.6 Technology and Resilience

Status: Partial â€” baseline technology fixed.

The BFF is implemented in .NET 10 Native AOT posture.

 The BFF is implemented in .NET 10 Native AOT.

Technology posture (target-state intent; implementation specifics remain within the .NET 10 baseline described in Provider Plus Modernization ARB packet (v5, June 2026)):

- ASP.NET Core API endpoints suitable for Native AOT.
- OpenTelemetry instrumentation for inbound requests and outbound HTTP dependencies.
- Typed HTTP clients with explicit timeouts and bounded retries.

Conceptual middleware pipeline (ordered; descriptive, not framework-specific):

```
Request
 |
 +--> Correlation / Trace Context
 | - Read/generate correlation id (e.g., 'x-correlation-id')
 | - Read trace context ('traceparent'/'tracestate')
 |
 +--> Authentication
 | - Validate/accept edge-validated JWT context
 |
 +--> Tenant Resolution
 | - Extract tenant_id from JWT claim
 | - Attach tenant_id to request context + OTel span attributes
 |
 +--> Authorization
 | - Enforce operator permissions for the requested action
 |
 +--> Input Validation and Request Limits
 | - Validate payload shape
 | - Enforce max request size / timeouts
 |
 +--> Route Handler (proxy or composition)
 | - Forward to domain services (no DB access)
 | - Apply bounded concurrency for fan-out
 |
 +--> Error Normalization
 | - Problem Details mapping
 | - No stack traces or secrets in responses
 v
Response
```

Resilience patterns (policy-level; exact parameters are environment-specific):

- Timeouts are mandatory on all outbound calls.
- Retries are limited to safe/idempotent operations (typically reads); retries are bounded with jittered backoff.
- Circuit breaker and bulkheads protect the BFF process from downstream slowness/outages.
- Idempotency for commands:
 - accept `Idempotency-Key` for write operations so operator retries do not duplicate work.
 - The BFF forwards idempotency context to the command API; the domain boundary remains authoritative for deduplication.
- Caching (if used) must be tenant-scoped, short-lived, and limited to safe reads; cached payloads must not include secrets.

---

# 6. Domain Services

## 6.1 Service Pattern and Boundaries

Status: Partial â€” high-level boundary defined; decomposition open.

MMS-CMS-FXFER domain services own authoritative behavior of the file transfer domain: validating commands, authorizing actions, persisting configuration and tracking, and emitting events that represent what happened in a tenant-scoped way.

 final service boundaries and deployables (single domain API vs decomposed services).

Domain boundary responsibilities:

- Authoritative validation: enforce invariants and state transitions regardless of what the UI/BFF validated.
- Authorization: enforce operator permissions at the domain boundary (the BFF is not an authorization authority).
- Tenant isolation enforcement: ensure tenant scoping applies to every read/write and to event envelopes, consistent with the database-per-tenant posture in Provider Plus Modernization ARB packet (v5, June 2026).
- Durable state transitions: only domain services can create or mutate authoritative FXFER state.
- Domain events: publish events that reflect authoritative outcomes and state changes for cross-domain consumers.

Within the domain boundary, MMS-CMS-FXFER owns four concerns as first-class responsibilities:

- Configuration (source locations, destinations, execution parameters).
- Credentials (endpoint definitions and secret references), with strict authorization and redaction.
- Tracking (searchable file movement history, statuses, and exception snapshots).
- Execution coordination (triggering intent, run lifecycle, and safe worker interaction patterns).

CQRS implications for the UI/BFF (platform-mandated in Provider Plus Modernization ARB packet (v5, June 2026)):

- Commands (writes) and queries (reads) are treated as separate concerns.
- Read models may be eventually consistent relative to a command that just completed.
- The UI/BFF must be prepared for transient â€œnot yet visibleâ€ behavior after a successful command until projections catch up.
- CQRS read-model granularity/indexing/retention strategy for tracking/search.

Command model (write semantics):

- Command endpoints change configuration or record authoritative outcomes.
- Commands enforce tenant-scoped invariants and authorization at the domain boundary (not only in the BFF).
- Where operator retries are expected, commands support idempotency so resubmission does not create duplicates.

Query model (read semantics):

- Queries are tenant-scoped by design and must not allow cross-tenant leakage.
- Tracking/search queries are paged and filterable, and must be supported by indexed access paths.
- Failures are returned as explicit errors and are observable; failures must not be silently converted into empty results.

Rules evaluation posture:

- The domain boundary is the natural home for policy decisions that affect validation and state transitions.
- FXFER does not introduce a rules engine; validations and state transitions are configuration-driven plus deterministic code.

## 6.2 Service Catalog

Status: Partial â€” richer catalog defined.

| Service | Type | Responsibilities | Data Owned | Events Published (Conceptual) | Events Consumed (Conceptual) |
|---|---|---|---|---|---|
| FXFER Domain API (combined) | API | Commands and queries for configuration and tracking; emits domain events; enforces tenant isolation and authorization | Write model + (if co-located) read access paths, tenant-scoped | Fxfer.ConfigurationChanged; Fxfer.DeliveryOutcomeRecorded; Fxfer.SuspensionStateChanged | None (scheduled execution is internal) |
| FXFER Command API | Command API | Accepts commands; enforces invariants; persists authoritative changes; records outcomes from worker; returns acknowledgements | Write model (tenant-scoped) | Fxfer.ConfigurationChanged; Fxfer.DeliveryOutcomeRecorded; Fxfer.SuspensionStateChanged | None (scheduled execution is internal) |
| FXFER Query API / Read Facade | Query API | Serves tenant-scoped query surfaces for operator search/detail views; reads from projections/read models | Read model(s) optimized for tracking/search | N/A (query-only) | N/A |
| FXFER Projection / Read Model Worker | Worker | Builds/updates projections from domain events; applies retention/index tuning; supports query/read needs | Projection stores and indexes | N/A (typically) | FXFER domain events; potentially platform events needed for enrichment |
| FXFER Transfer Worker | Worker | Executes protocol I/O with external endpoints; records outcomes via domain command API; applies bounded retry and DLQ/redrive behaviors | Ephemeral working state only (no authoritative DB writes) | Outcome/attempt events are emitted via the domain boundary | Work items / scheduled triggers  |

Notes:

- If FXFER remains a single deployable API, the Command API and Query API rows are treated as logical roles inside that deployable rather than separate services.

## 6.3 Legacy API Surface Mapping

Status: Partial â€” legacy surfaces identified.

Legacy MMS-CMS-FXFER exposes multiple host styles including SOAP/WCF-style contracts and HTTP endpoints. The target state standardizes on versioned HTTP APIs (with explicit command/query intent) for internal use and for UI interactions (via the BFF).

 migration strategy for legacy SOAP/WCF callers.

Target-state mapping posture (conservative):

- Do not introduce new SOAP/WCF dependencies.
- Inventory and classify current callers and contract shapes before selecting a deprecate-vs-bridge strategy.
- If bridging is required, the bridge is treated as an anti-corruption layer that translates legacy contracts into the target command/query APIs.

## 6.4 API Design Standards

Status: Partial â€” baseline described.

APIs must be versioned and tenant-aware, following the platform posture in Provider Plus Modernization ARB packet (v5, June 2026).

Command vs query semantics:

- Commands are explicit mutations that create or transition authoritative state.
 - Commands are idempotent where operator retries are expected ( `Idempotency-Key`).
 - Commands return an acknowledgement (for example: accepted/created + identifiers) rather than â€œcomputed state.â€
- Queries are read-only and must not have side effects.
 - Queries support pagination and bounded filtering.

Versioning posture:

Pagination and filtering posture:

- Pagination is mandatory for list/search results.
- prefer cursor-based pagination for large tracking datasets to avoid deep offset scans. If offset pagination is used, it must be bounded with a maximum page size and indexed filters.

Tenant scoping posture:

- Tenant identity is derived from JWT claim (`tenant_id`) and applied implicitly server-side.
- APIs must not allow callers to override tenant identity via query parameters.

Idempotency and concurrency posture:

- Commands that create work or record outcomes are safe to retry only when idempotency is applied.
- support optimistic concurrency on updates (for example ETag/version fields) to prevent silent lost updates.

## 6.5 Synchronous Communication

Status: Complete â€” allowed sync paths and prohibitions defined.

Synchronous calls are allowed within the FXFER module boundary only:

- UI -> BFF -> FXFER domain APIs
- FXFER workers -> FXFER domain APIs

Cross-domain integration is asynchronous only, via the single event backbone (SNS/SQS pattern) per Provider Plus Modernization ARB packet (v5, June 2026).

Typical command flow (eventual consistency implied):

```
UI (Browser) BFF Command API DB (tenant) Event Backbone
 | | | | |
 | POST (command) | | | |
 +----------------->| | | |
 | | POST (forward) | | |
 | +--------------------->| validate + authorize | |
 | | +--------------------->| commit state |
 | | | | |
 | 202 Accepted/OK | | | |
 |<-----------------+<---------------------+ | |
 | | | publish event (async)--------------------->|
 | | | | |
 | later: GET query | | | |
 +----------------->| GET (forward) | | |
 +--------------------->| Query API / Read Facade (projection-driven) |
```

Typical query flow:

```
UI (Browser) BFF Query API / Read Facade Read Model Store
 | | | |
 | GET (query) | | |
 +----------------->| | |
 | | GET (forward) | |
 | +--------------------->| read tenant projection |
 | | +---------------------------->|
 | | | |
 | 200 OK | | |
 |<-----------------+<---------------------+ |
```

## 6.6 Persistence Pattern and Transactions

Status: Partial â€” pattern documented.

Persistence is owned by the domain. UI and BFF have no database access.

Tenant isolation and routing:

- Each request is tenant-scoped based on `tenant_id` derived from a validated JWT claim.
- The domain boundary uses tenant context to route to the correct Aurora PostgreSQL database-per-tenant (platform posture in Provider Plus Modernization ARB packet (v5, June 2026)).

Transactions and event durability posture:

- Domain services persist authoritative state before emitting domain events.
- Event publication is asynchronous relative to the synchronous command response.
- use a transactional outbox (or equivalent durable handoff) to avoid â€œDB write succeeded but publish failedâ€ drift.

Read model durability posture:

- Read models are derived from events (CQRS) and tuned for operator query needs.
- CQRS read-model granularity/indexing/retention strategy.
- retention windows for tracking/artifacts.

## 6.7 Resiliency

Status: Partial â€” baseline posture documented.

Resiliency requirements are dominated by external endpoint instability, but must be implemented without compromising tenant isolation or operability.

Target-state resiliency posture:

- Bounded retries: retries exist, but are explicitly bounded and observable; no infinite loops.
- Dead-letter and redrive: work items that exceed retry policy are parked for operator-visible recovery.
- scheduling/trigger model and rerun/redrive controls (including who may rerun, how reruns are represented, and what safety checks exist).

Observability requirements (operator support):

- All services and workers emit OpenTelemetry traces/metrics/logs tagged with `tenant_id`, `correlation_id`, and `trace_id` per Provider Plus Modernization ARB packet (v5, June 2026).
- Error responses to the UI include non-sensitive correlation identifiers so operators can locate traces/logs without exposing secrets.

Integration-specific decision points that affect resiliency policy:

- partner endpoint connectivity pattern (network posture influences timeout/retry feasibility).
- file payload classification by file type (impacts what diagnostics can be recorded/exposed).

---

# 7. Eventing

## 7.1 Event Backbone Strategy

Status: Partial â€” platform posture is inherited; FXFER-specific contract catalog is defined by the decision register (Section 16).

**Authoritative posture (inherited).**

Provider Plus cross-domain integration is **asynchronous only** via a single Event Backbone, reached through an Event Adapter. No domain makes synchronous API calls to another domain's services. This posture is authoritative in Provider Plus Modernization ARB packet (v5, June 2026).

**FXFER eventing intent.**

- FXFER publishes tenant-aware, schema-versioned events to the Event Backbone for cross-domain awareness and downstream processing.
- CQRS is mandatory: FXFER uses events to build query-optimized read models.
- FXFER must not introduce cross-domain synchronous dependencies; any cross-domain coupling is mediated through the Event Backbone.

**Trust boundaries and publication rules.**

- **Authoritative producers:** FXFER domain services are the authoritative producers of FXFER business events.
- **Workers:** transfer workers produce observations and outcomes and must record them via the FXFER domain boundary (command endpoints / domain APIs). Workers must not publish business events directly.
 - allow workers to emit only non-business operational telemetry directly to observability backends (not to the Event Backbone).
- **Projectors/read models:** CQRS projectors consume events to build read models for operator UX.

**Publish/consume flow (conceptual) with correlation context.**

```
(inside FXFER)

Worker -> FXFER Domain API -> (persist) -> Outbox -> Event Adapter -> Event Backbone -> Consumer Queue -> Consumer

Correlation context carried end-to-end:
- tenant_id, correlation_id, trace_id (canonical envelope)
- these values are mapped into OpenTelemetry attributes/log fields by every publisher/consumer
```

## 7.2 Event Types

Status: Partial â€” canonical envelope is inherited; concrete event catalog is defined by the decision register (Section 16).

**Canonical event envelope (inherited).**

The platform defines a canonical event envelope containing:

- `tenant_id`
- `event_name`
- `event_version`
- `event_id`
- `occurred_at`
- `correlation_id`
- `trace_id`
- `producer`

This envelope is authoritative in Provider Plus Modernization ARB packet (v5, June 2026).

publish the canonical envelope in message headers/attributes (not only in the JSON body) so routing/observability does not require payload inspection. The precise mapping of envelope fields to SNS/SQS message attributes is not settled in this document.

**FXFER event taxonomy.**

FXFER contracts should be organized into a small number of stable event families aligned to authoritative state transitions in the FXFER domain.

| Event family | Type | What it represents | Notes |
|---|---|---|---|
| `Fxfer.ConfigurationChanged` | Domain event | A configuration aggregate changed (create/update/enable/disable/suspend) | split into per-aggregate families once consumers are known (e.g., endpoint profile vs transfer definition). |
| `Fxfer.RunStateChanged` | Domain event | A run moved between lifecycle states (requested -> started -> completed/failed/canceled) | trigger/scheduling model affects run state model. |
| `Fxfer.DeliveryOutcomeRecorded` | Domain event | A per-destination delivery attempt completed with an outcome | Outcome taxonomy must support operator troubleshooting and avoid leaking sensitive payload details. |
| `Fxfer.SuspensionStateChanged` | Domain event | Source location or destination suspended/unsuspended | Supports operator workflows and downstream notifications. |
| `Fxfer.NotificationTriggered` | Domain event | A notification-worthy condition occurred | notification strategy/routing. |
| `Fxfer.RuleSetChanged` | Domain event | A ruleset reference changed | rules engine scope/governance. |

**Data minimization rule (non-negotiable).**

- Event payloads must not include secrets.
- Event payloads must not include raw provider file contents.
- file payload classification and redaction rules govern whether file names, paths, and diagnostics may appear in event payloads; default posture is opaque identifiers plus safe, operator-necessary metadata.

## 7.3 Published Events

Status: Complete â€” publish intent, boundaries, and governance posture defined.

FXFER publishes events for two primary purposes:

1. Cross-domain awareness (other domains can react asynchronously without coupling to FXFER APIs).
2. Read-model derivation and operator workflows (CQRS projections, dashboards, notifications).

| Event family | Purpose | Producer | Tenant scope | Data classification notes | Likely consumers |
|---|---|---|---|---|---|
| `Fxfer.ConfigurationChanged` | Notify that transfer configuration changed (auditability, cache invalidation) | FXFER domain service | Tenant-scoped | No secrets; credential values must be references/handles only. | Reporting/analytics; notification/correspondence subscriber(s) as needed. |
| `Fxfer.RunStateChanged` | Provide operator-visible run lifecycle signals; enable downstream automation | FXFER domain service | Tenant-scoped | Do not include file contents; keep error details bounded and non-sensitive. | depends on trigger model and rerun/redrive controls. |
| `Fxfer.DeliveryOutcomeRecorded` | Emit per-destination delivery outcomes for downstream acknowledgement and reporting | FXFER domain service | Tenant-scoped | Avoid embedding partner credentials, remote paths, or payload-derived PII; use opaque ids where feasible. | Reporting/analytics; downstream workflows that depend on transfer outcomes. |
| `Fxfer.SuspensionStateChanged` | Notify that operational suspension/unsuspension occurred | FXFER domain service | Tenant-scoped | Include only safe reason codes/categories; avoid secrets and excessive endpoint detail. | notification strategy/routing. |

**PII/secrets posture (explicit).**

- PII and secrets must not appear in event payloads.
- Sensitive values must be referenced via opaque IDs/handles (e.g., endpoint profile id, credential reference id), resolvable only through authenticated/authorized FXFER APIs.

## 7.4 Consumed Events

Status: Complete â€” FXFER does not require cross-domain event consumption for the target-state MVP.

FXFER does not require cross-domain consumed events to schedule or execute transfers. Scheduling is driven by EventBridge schedules and internal work queues . Tenant policy variability is handled via the rules engine integration model .

any consumed event must be treated as untrusted input across a trust boundary and validated at the FXFER domain boundary before it affects authoritative state.

## 7.5 Producerâ€“Consumer Matrix

Status: Complete â€” minimum producer/consumer responsibilities defined.

| Event family | Producer | FXFER internal projector/read model | Other domains | Notifications/alerting |
|---|---|---|---|---|
| `Fxfer.ConfigurationChanged` | FXFER domain service | Yes (CQRS projection) | Reporting/analytics; downstream consumers that require config awareness. | Notification/correspondence subscriber(s) as needed. |
| `Fxfer.RunStateChanged` | FXFER domain service | Yes (CQRS projection) | Reporting/analytics; downstream workflows dependent on run completion. | Notification/correspondence subscriber(s) for operator-facing status. |
| `Fxfer.DeliveryOutcomeRecorded` | FXFER domain service | Yes (CQRS projection) | Reporting/analytics; downstream workflows dependent on delivery outcomes. | Notification/correspondence subscriber(s) for failures/exceptions. |
| `Fxfer.SuspensionStateChanged` | FXFER domain service | Yes (CQRS projection) | Reporting/analytics as needed. | Ops alerting + notification/correspondence subscriber(s) for suspension/unsuspension. |

## 7.6 Topics and Channels

Status: Complete â€” naming posture defined; concrete topic/queue names are implementation details.

**Authoritative naming posture (inherited).**

Topic naming follows the `pp.<domain>.<stream>` convention, with tenant identity propagated in the canonical envelope (and preferably headers/attributes), not through per-tenant topics. Topics are environment-scoped, not tenant-scoped. This posture is defined in Provider Plus Modernization ARB packet (v5, June 2026).

adopt a small set of stable FXFER streams, for example:

- `pp.fxfer.events` : primary domain event stream for FXFER
- `pp.fxfer.dlq` : dead-letter stream/queue(s) for operational recovery

the platform's version support window policy impacts whether streams need explicit major-version splitting.

**SNS/SQS pattern (conceptual).**

- Publish to a domain topic/stream.
- Each consumer owns its own queue subscription.
- Each queue has a DLQ with operator-visible redrive.

## 7.7 Delivery Guarantees

Status: Complete â€” delivery guarantees and operator safety controls defined.

**Baseline delivery posture.**

- At-least-once delivery: events may be delivered more than once.
- Consumer idempotency: every consumer must tolerate duplicates without creating duplicate business effects.
- Bounded retries: retries are explicitly bounded; failures beyond policy are dead-lettered.
- DLQ + redrive: dead-letter queues exist for operational recovery with safe, audited redrive controls .

**Idempotency and deduplication.**

consumers use `(tenant_id, event_id)` as the primary deduplication key and maintain a bounded processed-event ledger per consumer.

**Ordering posture.**

No strict ordering guarantees are required. Consumers must treat events as unordered and rely on idempotency plus explicit timestamps/state in the payload where sequencing matters.

**Operator safety and rerun/redrive.**

Privileged reruns are operator-controlled, fully audited, and performed via CLI rerun mode targeting a specific tenant and tracked file identifier.

## 7.8 Schema Governance

Status: Complete â€” governance posture stated and version support policy defined.

**Contract-first governance.**

each published event family is defined in an AsyncAPI (or platform-equivalent) contract artifact that includes:

- canonical envelope fields (as required by Provider Plus Modernization ARB packet (v5, June 2026))
- payload schema (explicitly excluding secrets)
- examples (sanitized)
- compatibility rules and deprecation notes

**Compatibility rules (policy-level).**

- Prefer additive change (new optional fields) for evolving payloads.
- Treat removals/renames/type changes as breaking and require a new `event_version`.
- Consumers must be capable of running side-by-side during version transitions.

**Version support window.**


**Schema validation posture.**

enforce schema compatibility checks in CI so breaking changes cannot be published accidentally.

---

# 8. External Interactions and Shared Services

## 8.1 Inventory

Status: Complete â€” major boundaries captured and constrained by the decision register.

This section enumerates the external trust boundaries and shared services FXFER relies on.

| Boundary / shared service | Interaction type | Used by | Purpose | Notes |
|---|---|---|---|---|
| Enterprise identity broker (OIDC) | HTTP/OIDC | UI/BFF | Operator authentication and token issuance | Inherited platform posture per Provider Plus Modernization ARB packet (v5, June 2026). |
| Platform ingress/gateway | HTTP | UI/BFF/APIs | Request authentication/authorization enforcement at the edge | Inherited platform posture per Provider Plus Modernization ARB packet (v5, June 2026). |
| Tenant Registry (platform) | HTTP and/or platform SDK | BFF / domain | Resolve tenant configuration overlays and tenant-scoped feature toggles | Tenant Registry is a named shared platform contract in Provider Plus Modernization ARB packet (v5, June 2026). |
| Feature flag service | HTTP and/or platform SDK | UI/BFF/domain | Tenant-scoped feature toggles (runtime) | deliver a tenant-scoped FXFER feature manifest to the UI at session start. |
| Secrets management service | Managed secret store | Domain/worker | Store and rotate endpoint credentials; retrieve at runtime via least privilege | Secrets must not be stored in application DB (see Section 9.6). Product selection is inherited from platform AWS standards in Provider Plus Modernization ARB packet (v5, June 2026). |
| Event Backbone (via Event Adapter) | Async (SNS/SQS pattern) | Domain | Cross-domain events (publish/consume) | Sole cross-domain integration mechanism per Provider Plus Modernization ARB packet (v5, June 2026). |
| Observability backend(s) | OTel exporter | UI/BFF/domain/worker | Central traces/metrics/logs; operator dashboards/alerts | Signals must be taggable with `tenant_id`, `correlation_id`, `trace_id` per Provider Plus Modernization ARB packet (v5, June 2026). |
| External partner / on-prem endpoints | Protocol-specific | Worker | Execute file movement | partner endpoint connectivity pattern (network/egress posture). |
| Legacy inbound integration surface | HTTP/SOAP/WCF (legacy) | Legacy inbound callers (time-boxed) | Backward-compatibility bridge (if required) | legacy SOAP/WCF migration posture. |
| Rules engine (if shared) | API / SDK | Domain | Evaluate transfer routing/filters/policies (if required) | rules engine scope/governance. |

## 8.2 Interface Specifications

Status: Complete â€” key interfaces and constraints captured.

The table below describes the allowed interaction patterns and the minimum interface expectations.

| Interaction | Type | AuthN/AuthZ posture | Tenant scoping | Data classification | Resilience notes | Observability requirements |
|---|---|---|---|---|---|---|
| Browser (MFE) -> FXFER BFF | HTTP (JSON) | Auth at edge + BFF validates JWT/claims; BFF enforces RBAC | Tenant context derived from verified claims; forwarded to domain APIs | Operator-visible data may include sensitive operational details; avoid secrets | BFF uses bounded retries only for safe idempotent calls; circuit-breaker posture on downstream errors | Propagate `correlation_id`/trace context from shell; emit spans per route tagged with `tenant_id` |
| BFF -> FXFER domain APIs | HTTP (private) | Service-to-service identity; BFF passes tenant context | Tenant context explicit on every request | No secrets returned; secret values never returned to browser | Retry only for idempotent calls; fail fast for auth/validation | Link BFF spans to domain spans (shared `trace_id`) |
| Worker -> FXFER domain APIs | HTTP (private) | Service identity; worker authorized for execution scope | Tenant context carried explicitly per work item | Payload diagnostics must respect payload-handling constraints (no payload bytes, no secrets) | Workers implement bounded retries with jitter; failures recorded as outcomes | Every transfer attempt emits spans/logs with `tenant_id`, `correlation_id`, `trace_id` |
| FXFER domain -> Event Backbone (publish) | Async (via Event Adapter) | Service identity; publish rights scoped to FXFER streams | `tenant_id` in canonical envelope | No secrets/PII in payload | At-least-once; publish failures handled via durable handoff | Publish spans + metrics (publish failures, queue age) |
| Worker -> partner endpoints | Protocol-specific | Credentials retrieved via secret store; least privilege | Tenant-scoped configs resolve to tenant-specific endpoints | Logging/storage must respect payload-handling constraints (no payload bytes, no secrets) | Timeout/retry policy explicit; repeated failures lead to suspension/parking | Log only safe endpoint metadata; include correlation ids for operator troubleshooting |

Partner endpoint connectivity follows 

## 8.3 Inbound Access

Status: Complete â€” modern inbound path is explicit and legacy inbound compatibility posture is defined.

**Allowed inbound path (target state).**

- Operator access is via the platform shell hosting the FXFER micro-frontend.
- The micro-frontend calls only the FXFER BFF.
- The FXFER BFF calls FXFER domain APIs.
- UI and BFF do not access databases directly.

**Explicitly disallowed in target state.**

- Direct browser calls to FXFER domain APIs (bypassing BFF).
- Any direct inbound calls from external partners into FXFER services.
- Any cross-domain synchronous calls (domain -> domain), per Provider Plus Modernization ARB packet (v5, June 2026).

**Legacy inbound compatibility.**


**Inbound call sequence (operator UI).**

```
User Browser
 |
 | 1) Navigate to FXFER screen (shell route)
 v
Shared Shell (auth/session)
 |
 | 2) Loads FXFER MFE remote
 v
FXFER Micro-Frontend (React + Next.js)
 |
 | 3) HTTPS request (JWT + tenant context)
 v
FXFER BFF (.NET 10 Native AOT)
 |
 | 4) HTTPS (private) to FXFER domain APIs
 v
FXFER Domain APIs (.NET 10)
 |
 | 5) Command/query; DB-per-tenant routing
 v
Aurora PostgreSQL (database-per-tenant)
```

## 8.4 Shared Platform Service Usage

Status: Partial â€” usage mapped; product-level selections and connectivity constraints are not fully specified.

FXFER uses shared platform services rather than embedding custom equivalents, consistent with Provider Plus Modernization ARB packet (v5, June 2026).

| Need | Platform service (generic) | How FXFER uses it |
|---|---|---|
| Authentication (operators) | Enterprise identity broker (OIDC) | Shell/BFF validate tokens; tenant context comes from verified claims. |
| Authorization | Platform RBAC + module roles | BFF enforces UX permissions; domain APIs enforce authoritative access for all commands/queries. |
| Tenant registry | Tenant Registry API | Resolve per-tenant configuration overlays affecting transfers and feature enablement. |
| Feature flags | Feature flag service | Enable/disable screens/actions and potentially worker behaviors per tenant. |
| Secrets | Managed secret store | Store endpoint credentials/keys/certs; retrieve at runtime via least privilege; never store secrets in app DB. |
| Eventing | Event Adapter + Event Backbone | Publish/consume events using the canonical envelope and contract-first governance. |
| Observability | OpenTelemetry pipeline + dashboards/alerts | Emit traces/metrics/logs tagged with `tenant_id`, `correlation_id`, `trace_id`. |
| Scheduling/orchestration (if shared) | Platform scheduler/workflow service | decide whether scheduling is internal to FXFER or provided externally. |
| Partner connectivity | Approved egress/connection patterns | finalize partner connectivity posture (private connectivity, allow lists, protocol gateways). |

## 8.5 Reports

Status: Partial â€” reporting patterns described; concrete report catalog and destinations are defined by the decision register (Section 16).

FXFER reporting is primarily operator-facing and should be derived from CQRS read models and event-derived projections (not ad-hoc production database access).

Report outputs fall into two broad categories:

- Operational dashboards (near real-time): run status, failure rates, queue depth/age, suspensions.
- Exports/queries (point-in-time): configuration inventory, tracking history, exception summaries.

CQRS read-model granularity/indexing/retention constrains which reports can be served efficiently.

retention windows for tracking/artifacts determine how far back reports can go without archive retrieval.

define a minimal report set for Phase 1 that is sufficient for operational parity:

| Report (pattern-level) | Primary audience | Likely backing store | Notes |
|---|---|---|---|
| Configuration inventory | Operators | Read model | Must exclude secrets; show only credential references/handles. |
| tracking history and outcomes | Operators | Read model | Must support tenant-scoped, paged queries. |
| Failure/exception summary | Operators | Read model | Exception content must be redacted and must not include secrets or payload bytes. |
| Suspension audit | Operators/compliance | Read model + events | Tracks who/what suspended/unsuspended and why. |

Destinations/formats for exports (CSV, PDF, API-only) are not decided in this document.

---

# 9. Data Architecture and Persistence

## 9.1 Overview

Status: Partial â€” scope is clear; physical model and retention details are defined by the decision register (Section 16).

FXFER owns high-value operational state: configuration and tracking. FXFER is not a long-lived repository for provider file payloads; it is a controlled transfer mechanism that ensures predictable, auditable movement of files across heterogeneous endpoints.

The target state treats three data categories explicitly:

- Configuration: what should happen (source locations, destinations, endpoint profiles, rules/filters).
- Tracking: what did happen (runs, attempts, outcomes, timelines, exceptions).
- Secrets: how endpoints are accessed (credentials/keys/certificates) â€” never stored in the application database, only referenced.

**Write boundaries (non-negotiable).**

- UI and BFF have no direct database access.
- FXFER domain APIs own all authoritative writes.
- Workers record outcomes through FXFER domain APIs (domain validates and applies state transitions).

## 9.2 Data Model

Status: Partial â€” conceptual aggregates and relationships outlined; schema and read-model design are defined by the decision register (Section 16).

**Conceptual aggregate / entity catalog (target state).**

The catalog below names the likely aggregates/entities FXFER needs in order to support the capabilities in Section 1.2. Names are conceptual and may be refined.

| Aggregate / entity (conceptual) | What it represents | Key relationships | Notes |
|---|---|---|---|
| Transfer Definition (aka Source Location) | Top-level definition of a transfer job (what to pull/push, when eligible) | 1->N Destinations; 1->N Runs | trigger/scheduling model influences attributes/state machine. |
| Destination | Delivery target under a transfer definition | N->1 Transfer Definition; N->1 Endpoint Profile | Holds non-secret routing parameters + references. |
| Endpoint Profile | Reusable endpoint connectivity definition (non-secret) | Referenced by Destinations | Must reference secret handles, not store secret values. |
| Credential Reference | Opaque handle/reference to secret material | Referenced by Endpoint Profiles | Secrets are stored in managed secret store; app DB contains only references. |
| Run | A single execution instance (requested/started/completed) | N->1 Transfer Definition; 1->N Delivery Attempts; 1->N File Records | Lifecycle and rerun semantics are |
| File Record (tracking) | Per-file tracking record representing observed file and processing state | N->1 Run; may relate to multiple Destinations | governs what file metadata may be retained. |
| Delivery Attempt | Per-destination attempt to deliver a file (or a batch) | N->1 Run; N->1 Destination; N->1 File Record(s) | Must represent bounded retry outcomes. |
| Timeline Event | Normalized timeline entry for operator visibility | N->1 Run or N->1 Transfer Definition | Stored as safe, structured data; avoid secrets. |
| Exception Snapshot | Structured error record capturing failure classification and safe diagnostics | N->1 Delivery Attempt | Must obey data minimization and classification. |
| Suspension | Operational suspension state (with history) | Applies to Transfer Definition and/or Destination | Supports manual and optional automated transitions. |
| Notification Subscription (optional) | What notifications to emit for which conditions | Linked to tenant settings and/or Transfer Definition | Depends on notification approach . |

**CQRS read model posture.**

CQRS is mandatory per Provider Plus Modernization ARB packet (v5, June 2026). Query-heavy operator screens should be served from read models derived from events and/or from authoritative write stores.

decide read-model granularity, indexing strategy, and retention strategy (especially for high-volume tracking and long-lived tracking history).

**Conceptual ERD-style sketch (not final).**

```
 [TransferDefinition]
 |
 | 1..N
 v
 [Destination] ---- N..1 ---- [EndpointProfile] ---- 1..1 ---- [CredentialReference]
 |
 | 1..N
 v
 [Run] ---- 1..N ---- [DeliveryAttempt] ---- 1..N ---- [ExceptionSnapshot]
 |
 | 1..N
 v
 [FileRecord]
 |
 v
 [TimelineEvent] (events can attach to Run or TransferDefinition)
```

## 9.3 Data Ownership

Status: Partial â€” ownership boundaries are explicit; projector/read-model ownership remains to be finalized.

**Authoritative ownership.**

- FXFER domain services own the authoritative configuration and tracking write model.
- State transitions are applied only through FXFER domain APIs (commands).
- Events represent facts emitted by the domain after state is persisted.

**Consumers (non-authoritative).**

- BFF and UI are consumers of read models and domain query endpoints.
- Workers are consumers of configuration (what to do) and producers of observations (what happened), but do not own state transitions.

FXFER owns its own projections/read models required for operator UX; cross-domain consumers own their projections and must not depend on FXFER internal read schemas.

## 9.4 Database and Schema Design

Status: Partial â€” tenancy posture is fixed; physical schema patterns and indexing/partitioning details are defined by the decision register (Section 16).

**Authoritative store (inherited).**

Tenant isolation uses Aurora PostgreSQL database-per-tenant, as defined in Provider Plus Modernization ARB packet (v5, June 2026).

**Tenant routing concept (high level).**

- Incoming requests (UI/BFF/worker) carry verified tenant context (`tenant_id`).
- Domain APIs route each request to the correct tenant database based on that tenant context.
- Events also carry `tenant_id` in the canonical envelope so consumers can apply the same routing discipline.

**Schema design intent (policy-level).**

- Prefer normalized command-side schemas for correctness of configuration and run state.
- Use read-optimized projections for operator queries (CQRS).
- Indexing supports fast, paged searches by common operational pivots (time window, transfer definition, run status, destination, outcome category).

prefer cursor-based pagination for large tracking datasets to avoid deep offset scans.

read-model strategy and retention must be finalized to prevent unbounded tracking growth.

## 9.5 Legacy-to-Target Data Mapping

Status: Partial â€” mapping approach defined; table/column mapping remains to be produced from legacy inventory.

This document does not enumerate legacy tables/columns. Instead, it defines the conservative approach for mapping legacy data into the target model.

**Approach (high level).**

- Inventory legacy configuration and tracking data as authoritative input for migration planning.
- Map legacy concepts to target aggregates (Transfer Definition, Destination, Endpoint Profile, credential references, runs/outcomes).
- Define explicit transform rules where legacy data is denormalized or embeds environment-specific endpoint details.

**Risk drivers.**

- legacy SOAP/WCF migration posture may require temporary dual representations of certain concepts.
- payload classification affects whether any legacy tracking fields can be carried forward safely.

## 9.6 Data Classification and Privacy

Status: Partial â€” classification categories defined; file-type-specific payload classification is defined by the decision register (Section 16).

The platform posture is that Provider Plus processes provider PII and does not process/store/transmit PHI, per Provider Plus Modernization ARB packet (v5, June 2026).

file payload classification by file type is required to finalize what file-derived metadata may be stored, logged, and published as events.

| Data category | Examples | Sensitivity posture | Must not do |
|---|---|---|---|
| Configuration (non-secret) | endpoint hostnames, ports, eligibility rules | Sensitive operational data | Do not expose cross-tenant; do not treat as public. |
| Credential references | secret ids/handles, key/cert references | Highly sensitive (indirect) | Never store secrets in app DB; never emit secrets in events. |
| Tracking and outcomes | run state, per-file outcomes, timestamps, outcome categories | Potentially sensitive | Avoid storing raw payload or excessive endpoint details. |
| Exceptions/diagnostics | error codes, bounded stack traces, remote error messages | Sensitive | Do not persist secrets; sanitize messages to remove credentials/payload fragments. |
| Artifacts (if any) | reconciliation files, transfer manifests | Sensitive | retention windows; avoid long-lived storage by default. |

Explicit rule: secrets never stored in the application DB; only references/handles.

## 9.7 Lifecycle and Retention

Status: Partial â€” retention posture described; concrete windows are defined by the decision register (Section 16).


Retention must balance:

- operator troubleshooting needs (sufficient history)
- cost and performance (avoid unbounded growth)
- privacy and minimization (retain only what is needed)

implement time-based retention with explicit purge jobs per tenant, with optional archival to lower-cost storage for long-lived compliance needs. This proposal does not choose a specific storage technology.

## 9.8 Data Migration and Reconciliation

Status: Partial â€” migration options articulated; approach selection is deferred.

evaluate the following migration options; do not select until legacy interface posture and retention needs are clarified:

| Option | What migrates | Benefits | Risks / constraints |
|---|---|---|---|
| Bulk configuration load | Configuration only | Fast path to operational continuity | Requires robust validation; cannot migrate in-flight runs cleanly. |
| Bulk config + limited history | Config + last N days of tracking history | Better operator continuity | Depends on payload-handling and retention constraints for what is safe/required. |
| Dual-write bridge (temporary) | Legacy continues writing while target also writes | Safer cutover for critical flows | Complexity; depends on per-tenant cutover sequencing and routing controls. |
| Event replay seeding | Seed read models from historical events (if available) | Aligns with CQRS | Only feasible if legacy emits trustworthy events; not assumed. |

parity is evaluated primarily through tracking outcomes and endpoint-side observable behavior (file delivered/not delivered), not by comparing internal logs.

## 9.9 Schema Migrations

Status: Partial â€” governance posture stated; tool selection is intentionally not fixed.

Schema migrations must be safe across many isolated tenant databases.

use an expand/contract migration discipline:

- Expand: add new nullable columns/tables first; keep old readers working.
- Migrate: backfill data where required.
- Contract: remove old columns only after all readers are upgraded.

Governance expectations:

- Migrations are reviewed as part of normal code review.
- Each migration is repeatable and idempotent per tenant.
- Migration execution is observable (per-tenant success/failure metrics and logs).

## 9.10 Legacy History and Archive

Status: Partial â€” archive patterns described; commitments depend on retention and legacy posture.

retention windows drive whether historical run/exception data must be migrated or can remain accessible via legacy archives.

legacy SOAP/WCF migration posture affects how long legacy components remain in operation (and therefore how long legacy history remains live).

consider one of the following patterns:

- Export legacy history to an archive store (read-only) with tenant-scoped access controls.
- Keep legacy system in read-only mode for a bounded decommission window, with explicit retirement criteria.
- Migrate only a bounded window of history into the target read model (requires retention decision).

## 9.11 Data Volumes

Status: Partial â€” drivers identified; quantitative estimates require stakeholder input.

Data volumes are dominated by tracking growth (runs, file records, delivery attempts, exceptions) and are directly constrained by retention policy.

retention windows are the single biggest determinant of long-term storage volume.

| Driver | Why it matters | Notes |
|---|---|---|
| Runs per day per tenant | Drives Run rows and timeline volume | run frequency varies widely by tenant; scale without per-tenant customization. |
| Files per run | Drives FileRecord and DeliveryAttempt fan-out | file counts can spike; avoid O(n) UI queries without paging/indexing. |
| Destinations per transfer definition | Multiplies delivery attempts | Destination fan-out should be explicit in tracking. |
| Retry policy | Multiplies attempt records and exception snapshots | Bounded retries are mandatory; exact bounds are a tuning decision. |
| Retention window | Controls steady-state storage | |
| Read model strategy | Controls index size and query cost | |

define per-tenant operational limits (maximum page sizes, bounded query windows by default) so operator UX remains responsive even for high-volume tenants.

---

# 10. Security, Identity and Tenancy

## 10.1 Entry Point and Trust Boundaries

Status: Complete â€” trust boundaries and allowed call paths defined.

MMS-CMS-FXFER operates in a **zero-trust** posture inherited from Provider Plus Modernization ARB packet (v5, June 2026): requests are not trusted by default, and tenant context is validated and propagated on every hop.

**Allowed call paths (who can talk to whom).**

- **Browser** (Shell + FXFER micro-frontend) calls the **BFF** via the platform edge.
- **BFF** calls **FXFER domain APIs** only.
- **Workers** call **FXFER domain APIs** only (no direct database access).
- **FXFER domain** publishes/consumes cross-domain messages only via the **Event Adapter** to/from the **Event Backbone** (SNS/SQS pattern).
- **Workers** are the only component that initiate outbound connectivity to **external partner endpoints** (protocol I/O).

**Trust boundaries (conceptual).**

```
 +-----------------------------+ +----------------------------------------------+ +------------------------------+
 | Browser Boundary | | Internal Platform Boundary | | External Partner Boundary |
 | (untrusted client runtime) | | (managed compute + VPC network) | | (untrusted external systems) |
 +-----------------------------+ +----------------------------------------------+ +------------------------------+
 | Browser | HTTPS | API Gateway (JWT validate + tenant_id extract) | egress | Partner endpoint(s) |
 | - Shell + FXFER MFE +-------->+ BFF (.NET 10 Native AOT) +-------->+ (SFTP/FTPS/HTTPS/...) |
 | - trace context (W3C) | JWT | FXFER Domain APIs (.NET 10) | TLS | |
 | - correlation_id | | FXFER Worker(s) | | |
 +-----------------------------+ | | | +------------------------------+
 | | domain events |
 | v |
 | Event Adapter <-> Event Backbone (SNS/SQS) |
 +----------------------------------------------+

 Mandatory propagation/tagging at every boundary crossing:
 - `tenant_id` (claims + canonical event envelope)
 - `correlation_id` (request/work item + event envelope)
 - `trace_id` (distributed tracing)
```

**Propagation rule (non-negotiable).** `tenant_id` is derived from verified identity context (JWT claims or worker work item context). It must never be accepted from request bodies or client-provided headers as the source of truth.

## 10.2 Identity and Authentication

Status: Partial â€” OIDC/JWT posture described; session/CSRF strategy is defined by the decision register (Section 16).

**User authentication (browser-based operator access).**

- User-facing authentication uses **OIDC** via the enterprise identity broker (IRM broker), federating to state IdPs and the Gainwell IdP, as defined in Provider Plus Modernization ARB packet (v5, June 2026).
- Authorization flows use the **FAPI 2.0 profile** protections (PAR + PKCE) (inherited).

**Token model (pattern level).**

- All authenticated browser requests carry a **signed JWT**.
- JWTs are validated at the **platform edge** (API Gateway) before reaching BFF or domain APIs (inherited).
- The `tenant_id` claim is mandatory for any request that operates on tenant-scoped resources (inherited).

**Machine-to-machine identity (workers and internal calls).**

- Workloads use platform-managed identities and least-privilege execution roles (inherited).
- where an HTTP hop requires an application-layer bearer token between internal components, use a short-lived JWT minted via the platform identity broker and validate it at API Gateway consistently with user flows.

CSRF/session strategy.

- FXFER uses a Next.js micro-frontend mounted into a shared shell and calls the BFF through API Gateway.
- The UI uses token-only calls: the browser sends `Authorization: Bearer <JWT>` on every request, and the BFF does not establish a cookie-based UI session. With no ambient cookies, CSRF is effectively out-of-scope; XSS protections (CSP, safe storage) remain mandatory.

## 10.3 Authorization

Status: Complete â€” RBAC intent, separation-of-duties, and enforcement points defined.

**RBAC posture (inherited).** Authorization is **role-based** using claims in JWTs (validated at the edge). FXFER-specific permissions must be enforced in two places:

- **BFF enforcement:** blocks disallowed operations early and avoids exposing privileged flows to unauthorized users.
- **Domain enforcement:** is authoritative; it must reject any request that lacks required permissions even if invoked without the BFF (defense-in-depth).

**Separation of duties (SoD) â€” credential references are privileged.**

- Credential material is never stored in application databases.
- Any action that creates/updates/deletes **credential references/handles** is considered a privileged administrative operation.
- separate the permission to **manage credential references** from the permission to **execute transfers** and the permission to **view transfer payload metadata**, so operational staff can run workflows without being able to exfiltrate or modify credentials.

**Authorization â€œcapabilitiesâ€ catalogue (pattern level).**

| Capability | Examples of covered actions | Enforcement (minimum) |
|---|---|---|
| View tracking and operational status | Search runs, view attempt timelines, view error categories | BFF + Domain |
| Manage transfer definitions | Create/update/disable definitions; manage routing rules | BFF + Domain |
| Execute / request runs | Trigger a run, request rerun/redrive (subject to | BFF + Domain |
| Manage credential references (privileged) | Create/update/delete credential references; rotate handles; test connectivity | BFF + Domain |
| Tenant admin (privileged) | Enable/disable features; configure tenant-scoped limits and controls | BFF + Domain |
| Break-glass operations (privileged) | Any access that bypasses normal least-privilege controls | Domain + platform audit |

the domain exposes an explicit â€œauthorization deniedâ€ audit event whenever a privileged operation is attempted without permissions, so security monitoring can detect probing.

## 10.4 Tenant Context and Isolation

Status: Complete â€” tenant context propagation rules and isolation controls defined.

**Isolation boundary (inherited).** Tenant isolation uses **Aurora PostgreSQL database-per-tenant** as defined in Provider Plus Modernization ARB packet (v5, June 2026).

**Tenant context propagation rules (canonical).**

| Hop type | `tenant_id` source of truth | Required propagation | Notes |
|---|---|---|---|
| Browser â†’ BFF (via API Gateway) | Verified JWT claim (edge validated) | `tenant_id`, `correlation_id`, `trace_id` | `correlation_id` is created at the first entry point if missing and returned to callers for support use. |
| BFF â†’ Domain APIs | Derived from verified user context | `tenant_id`, `correlation_id`, `trace_id` | BFF must not allow the UI to choose tenant_id outside the authenticated tenant context. |
| Worker â†’ Domain APIs | Work item context (tenant-scoped) | `tenant_id`, `correlation_id`, `trace_id` | Workers must be incapable of â€œguessingâ€ other tenant IDs; tenant_id is required on every work item. |
| Domain â†’ Event Backbone (via adapter) | Domain request context | Canonical event envelope includes `tenant_id`, `correlation_id`, `trace_id` | Events are the cross-domain contract; tenant routing depends on tenant_id in the envelope. |

**DB-per-tenant routing (conceptual).**

- Domain APIs route each request to the tenantâ€™s database based on the verified `tenant_id` context (inherited).
- domain services fail fast if `tenant_id` is absent/invalid, rather than attempting any â€œdefault tenantâ€ behavior.

**Observability tagging (inherited).** All OpenTelemetry signals must be taggable with `tenant_id`, `correlation_id`, and `trace_id` (per Provider Plus Modernization ARB packet (v5, June 2026)).

## 10.5 Data Protection

Status: Partial â€” protection posture defined; payload classification and retention are defined by the decision register (Section 16).

**Encryption in transit (inherited).**

- TLS is enforced from browser to edge and for all service-to-service calls (platform baseline is defined in Provider Plus Modernization ARB packet (v5, June 2026)).
- Worker outbound connections to partner endpoints must use encrypted protocols where supported (e.g., FTPS/SFTP/HTTPS). Where partner constraints require exceptions, they must be explicitly risk-assessed.

**Encryption at rest (inherited).**

- Tenant databases are encrypted at rest.
- Worker staging storage (ephemeral volumes, object storage, scratch space) must be encrypted at rest.

**Secrets handling (non-negotiable).**

- Secrets must not be stored in application databases. The domain stores **references/handles only**.
- Workers retrieve secret material directly from the managed secrets system using least-privilege execution identity; secret values are held **in-memory only** for the duration of the protocol operation.
- prohibit writing secrets to disk (including temporary files) and prohibit logging of secret-bearing request/response frames.

**Worker staging data protection (file payloads and working copies).**

- Workers must implement a strict staging lifecycle: create â†’ use â†’ scrub/cleanup, regardless of success/failure.
- Staged payloads must be tenant-scoped (no shared directories across tenants) and must be eligible for secure deletion/cleanup.

**Redaction and minimization (ties to payload classification).**

- Logs, metrics labels, events, and audit records must avoid storing file payload content.
- file payload classification by file type.
 - Until classification is resolved, FXFER should treat file names, file paths, and endpoint identifiers as potentially sensitive and apply minimization and redaction-by-default.
 - define an explicit allowlist of file metadata fields that may appear in logs/events (e.g., hashes, sizes, counts) and a denylist for fields that must never be logged.

## 10.6 Audit and Security Monitoring

Status: Partial â€” audit event catalogue and monitoring posture defined; break-glass is defined by the decision register (Section 16).

**Audit posture (inherited + FXFER-specific).**

- Platform audit includes CloudTrail, GuardDuty, WAF logs, and database auditing as defined in Provider Plus Modernization ARB packet (v5, June 2026).
- The platform posture also requires immutable audit storage (e.g., S3 Object Lock / WORM) for audit events as defined in Provider Plus Modernization ARB packet (v5, June 2026).
- FXFER must additionally emit **application audit events** for configuration changes, privileged operations, and run/outcome lifecycle transitions.

**Audit event catalogue (pattern level).**

All audit events include (at minimum): `tenant_id`, `correlation_id`, `trace_id`, timestamp, actor identity (or â€œsystemâ€), action, target object identifiers, and outcome.

| Audit event | When emitted | Actor | Notes |
|---|---|---|---|
| `TransferDefinition.Created/Updated/Disabled/Deleted` | Configuration mutation | User | Include before/after summary (redacted), change reason (if required), and version identifiers. |
| `CredentialReference.Created/Updated/Rotated/Deleted` | Credential handle lifecycle | Privileged user | Do not include secret material; include only handle identifiers and target endpoint identifiers (redacted as needed). |
| `ConnectivityTest.Requested/Completed` | When a user validates an endpoint config | User | Tie outcomes to endpoint type and failure category; avoid payload/log leakage. |
| `DeliveryAttempt.Started/Completed/Failed` | Per endpoint attempt lifecycle | System | Must be idempotent and deduplicated by attempt id. |
| `DLQ.ItemQuarantined/RedriveRequested/RedriveApproved/RedriveExecuted` | DLQ and redrive control points | User + system | Redrive safety and controls are governed by |
| `Authorization.Denied` | Privileged action attempted without permission | User | emit for monitoring/probing detection. |
| `BreakGlass.Requested/Granted/Revoked` | Exceptional access | Privileged user | break-glass access model must define approval and auditing rules. |
| `TransferOutcome.Recorded` | When the domain persists an attempt outcome | System | Captures run/attempt id, outcome category, and timing. Avoid payload content. |

break-glass access model for sensitive metadata.

**Security monitoring and alerting (pattern level).**

- Alerting is based on platform security signals (auth failures, WAF blocks, suspicious access patterns) plus FXFER application audits.
- Monitoring must explicitly include cross-tenant anomaly detection (attempted access to non-owned tenant_id resources) and audit pipeline health (audit emission failures must page).
- define a small set of â€œhigh-signalâ€ security alarms for FXFER operations (credential reference changes, repeated partner authentication failures, repeated authorization denials).

---

# 11. Batch and File Processing

Status: Partial â€” worker model and safety controls defined; triggering/rerun controls are defined by the decision register (Section 16).

MMS-CMS-FXFER is a batch-and-file domain. Protocol I/O is executed by workers that interact with unreliable external networks; the worker is therefore treated as an **edge-touching** component with stricter network, secrets, and auditing requirements.

**Worker model (target posture).**

- Workers execute **tenant-scoped work items** that reference an authoritative execution intent (e.g., run id / attempt id) maintained by the FXFER domain.
- Workers perform protocol operations (connect/list/download/upload/rename/delete as applicable) and then record outcomes via **domain APIs**.
- Workers do not write directly to tenant databases.

scheduling/trigger model and rerun/redrive controls.

- The mechanism that creates execution intent (time-based schedule, event-triggered, user-triggered, or hybrid) must be explicitly defined.
- Regardless of the trigger mechanism, rerun and redrive controls must be designed to be safe under at-least-once delivery semantics.

partner endpoint connectivity pattern.

- The outbound connectivity posture (allowlist, egress proxy, certificate pinning, private connectivity options) must be defined as part of the approved partner connectivity model.

**Operational behaviors (non-negotiable).**

- **Safe staging:** strict lifecycle (create â†’ use â†’ cleanup), tenant-scoped directories, encrypted-at-rest staging.
- **Bounded retries:** explicit timeouts and backoff per protocol operation, with â€œgive upâ€ states that surface in tracking.
- **Idempotency:** avoid duplicate deliveries without explicit operator intent.
- **DLQ quarantine:** repeatedly failing items are quarantined with traceability, then only redriven via controlled operator workflows.
- **Durable tracking:** every outcome (including early failures like auth failure/unreachable host) is recorded as durable domain state.

**Worker execution sequence (pattern level).**

The following sequence shows the minimum expected interaction pattern for scheduled execution and per-file tracking.

```
Participants:
  Trigger/Scheduler (scheduled)
  Worker
  FXFER Domain API
  Managed Secrets Store
  Staging (encrypted)
  Partner Endpoint
  Event Adapter / Event Backbone

Trigger/Scheduler -> Worker: ScheduledTick(tenant_id)
Worker -> FXFER Domain API: ListActiveTransferConfigurations(tenant_id)
FXFER Domain API -> Worker: Transfer configs + secret references (handles only)

Worker -> Managed Secrets Store: Resolve secret(handle) [least-privilege execution identity]
Managed Secrets Store -> Worker: Secret material (in-memory)

Worker -> Staging: Stage working copy (encrypted-at-rest)
Worker -> Partner Endpoint: Execute protocol I/O (connect/list/download/upload/...)

Worker -> FXFER Domain API: UpsertTrackingRecord(file_identity_key, metadata)
Worker -> FXFER Domain API: RecordDeliveryOutcome(fileId, destinationId, outcome, idempotency_key)
FXFER Domain API -> Event Adapter / Event Backbone: Publish DomainEvent(outcome recorded)
```

**Idempotency and duplicate prevention.**

- Workers and domain APIs must assume at-least-once execution and message delivery.
- Idempotency keys are derived from tenant context plus file identity plus destination (and attempt number where applicable) so repeated submissions do not create duplicated state transitions.
- Domain APIs reject backwards state transitions unless initiated through an explicit privileged rerun flow.

**DLQ / redrive operator safety requirements.**

- Make it visible which items are safe to redrive (idempotent) vs require operator decision.
- Require a human-visible reason and capture an audit trail for each redrive.
- Prevent a redrive storm (rate limit per tenant; bounded concurrency).

**Retention and payload logging constraints.**

- File payload bytes are treated as opaque and are never persisted or emitted to APIs/events/logs/traces/metrics.
- Tracking retention follows the tenant database retention posture; FXFER does not introduce an FXFER-specific purge policy.

---

# 12. Infrastructure, Delivery and Operations

## 12.1 Compute and Scaling

Status: Complete â€” compute posture inherited; FXFER scaling and noisy-neighbor controls defined.

**Platform compute posture (inherited).**

- Domain APIs and BFF run on AWS managed compute: **AWS Lambda and/or Amazon EKS Auto Mode**, per Provider Plus Modernization ARB packet (v5, June 2026).
- The FXFER BFF is a **.NET 10 Native AOT** layer and has no direct database access (inherited).

**Worker workload placement (needs explicit choice).**

- Workers must support long-running and I/O-heavy file operations.
- confirm whether FXFER workers run as Lambda (with bounded execution time and ephemeral storage constraints) or as EKS workloads (longer-lived processes) given file size and protocol variability.

**Scaling posture (policy level).**

- **Horizontal scale-out** is the default for workers (add consumers to increase throughput).
- **Per-tenant concurrency caps** are mandatory to prevent noisy-neighbor impact.
- **Backpressure** must be applied when partner endpoints are slow/unreachable to avoid retry storms.

implement per-tenant concurrency by requiring workers to acquire a â€œleaseâ€ from the domain (or a tenant-scoped limiter) before starting an attempt; leases expire automatically to avoid deadlocks.

implement a partner-endpoint circuit breaker per tenant+endpoint to bound retries and protect the overall worker fleet.

## 12.2 Network and Ingress

Status: Partial â€” ingress posture inherited; egress posture constrained by open partner connectivity pattern.

**Ingress (inherited).**

- API ingress is through **AWS API Gateway** as the single entry point, with JWT authorizer validation, tenant_id extraction, throttling, and routing (inherited from Provider Plus Modernization ARB packet (v5, June 2026)).
- Layer-7 inspection is applied by **AWS WAF** (regional web ACL on API Gateway; WAF on ALB for container workloads) per the ARB posture.

**Internal service-to-service network posture (pattern level).**

- BFF and domain APIs communicate over internal networks (VPC / cluster network) under least privilege.
- treat internal calls as untrusted-by-default and require explicit authentication/authorization at the service boundary (consistent with â€œvalidate every requestâ€ posture).

**Outbound/egress for workers (edge-touching).**

- Worker egress must be tightly controlled (destination allowlists, routing through shared security inspection, and explicit logging/auditing of connection attempts).
- partner endpoint connectivity pattern.
 - Outbound traffic is inspected and routed using the shared security network controls (private subnets + inspected egress) per the Provider Plus Modernization ARB packet (v5, June 2026).

## 12.3 Delivery

Status: Complete â€” CI/CD flow and rollout controls described at a pattern level.

CI/CD aligns to the platformâ€™s managed delivery posture described in Provider Plus Modernization ARB packet (v5, June 2026).

**Delivery pipeline (pattern).**

- Build once per commit: compile, unit test, package immutable artifacts.
- Run security and compliance scans before deploy.
- Deploy through staged environments with canary/linear rollout and automatic rollback on alarms.

```
 Commit/PR
 |
 v
 Build + Unit Tests
 |
 v
 Static checks (lint/format, SAST, dependency scan, SBOM)
 |
 v
 Package (AOT binaries / images) + IaC synth/plan
 |
 v
 Deploy (dev -> test -> stage -> prod)
 |
 +--> Canary/linear traffic shifting + automatic rollback on alarms (inherited)
```

treat event schema changes and API contract changes as first-class deploy artifacts; a change that breaks forward/backward compatibility must be rejected by CI gates (see Â§12.4).

## 12.4 CI Quality Gates

Status: Partial â€” gate categories enumerated; concrete tooling is inherited from platform CI/CD standards.

Quality gates must prevent regressions in security posture, tenant isolation, and contract compatibility. The platform baseline in Provider Plus Modernization ARB packet (v5, June 2026) includes mandatory scanning and security controls.

**Minimum gate categories (pattern level).**

- Formatting / linting (language and IaC)
- Unit tests (domain logic, worker orchestration, BFF mapping)
- Contract checks
 - OpenAPI schema validation and backward/forward compatibility checks
 - Event schema versioning checks (producer/consumer compatibility)
- Database and schema compatibility checks (migration validation; rollback safety)
- IaC checks (policy-as-code, misconfiguration scanning)
- Secrets and credential scanning (prevent committing secrets)
- SBOM generation and vulnerability scanning (dependencies and container images)
- Tenant isolation verification (inherited: tenant isolation test suite runs as a mandatory CI gate per ARB)

version support window policy.

- Version support policy impacts contract compatibility gates (how long older event/API versions must remain supported).

## 12.5 Observability

Status: Complete â€” signal model, tagging, and operational dashboard/alert categories defined.

**Signals (inherited).** FXFER components are instrumented with OpenTelemetry (SDK + exporters) per Provider Plus Modernization ARB packet (v5, June 2026).

**Mandatory tags (inherited).** All traces, logs, and metrics must be taggable with `tenant_id`, `correlation_id`, and `trace_id`.

**Operational dashboards (minimum set).**

- BFF: request rate, latency (P50/P95), error rate, auth failures
- Domain APIs: command/query latency, error rate, throttling/limits reached, tenant-specific hot paths
- Workers: backlog depth, in-flight attempts, success/failure rates by endpoint type, retry counts, timeouts
- DLQ: depth, age of oldest message, redrive activity
- Partner endpoints: connectivity failures, auth failures, handshake failures, transfer timeouts (by tenant)
- Event Backbone: publish failures, consumer lag for FXFER projections and downstream consumers

define a standard â€œtenant healthâ€ view aggregating (API error rate + backlog + endpoint failure rate) so operations can detect per-tenant incidents quickly.

## 12.6 Runbooks

Status: Partial â€” runbook inventory provided; procedures depend on open trigger/redrive decisions.

Runbooks are required to make operator actions safe under at-least-once processing and under partial failure (endpoint down, secrets unavailable, transient network failures).

**Runbook inventory (minimum set).**

| Scenario | Primary signals | First actions | Safety notes |
|---|---|---|---|
| Partner endpoint outage / connectivity failures | Worker failure spike; endpoint-specific timeouts; retries increasing | Quarantine new attempts for that endpoint; notify tenant; validate endpoint reachability | Avoid retry storms; respect per-tenant concurrency caps (noisy-neighbor risk). |
| DLQ growth / stuck work items | DLQ depth rising; oldest message age rising | Pause new work ingestion; classify failure category; prepare controlled redrive | Redrive must be idempotent and auditable. |
| Rerun requested by operator | User request + audit events | Validate requested scope (single attempt vs full run); confirm idempotency preconditions | Rerun controls must prevent double delivery by default. |
| Credential rotation incident | Increased auth failures; recent credential changes audit events | Validate credential reference; rotate/revoke as needed; run connectivity test | Credential references are privileged; ensure SoD + auditing are preserved. |
| Tenant isolation incident suspicion | Cross-tenant anomaly alerts; unexpected tenant_id access patterns | Escalate immediately; freeze privileged operations; capture correlation_id/trace_id evidence | Treat as security incident; preserve evidence; verify routing/claims enforcement. |

## 12.7 Disaster Recovery

Status: Complete â€” DR posture inherited; FXFER-specific restore considerations captured.

**Platform DR baseline (inherited).** The ARB packet defines a primary region with cross-region disaster recovery posture, including backups and restore procedures, with configurable service-level objectives per state/tenant (Provider Plus Modernization ARB packet (v5, June 2026)).

**FXFER-specific DR considerations (pattern level).**

- FXFER requires **per-tenant database restore** procedures (database-per-tenant) that can restore a single tenant without impacting others.
- Worker processing must be able to resume safely after DR failover without duplicating deliveries.
- on DR restore, rehydrate worker queues from durable domain state (â€œwhat remains to doâ€) rather than relying on any in-flight queue state.

retention windows for tracking/artifacts.

- Retention decisions affect backup volumes and restore times. DR plans must be revisited once retention windows are finalized.

---

# 13. Non-Functional Requirements

Status: Partial â€” indicative targets captured; validation and acceptance criteria are defined by the decision register (Section 16).

MMS-CMS-FXFER quality attributes are dominated by reliability under failure, tenant isolation, credential security, and operational supportability. The NFRs below are intended to be measurable and testable; where numeric targets are included, they are either inherited from the ARB baseline or explicitly stated.

**Indicative NFR targets.**

| Metric | Target | Measurement |
|---|---|---|
| Portal initial page load (FXFER micro-frontend) | P95 <= 3 seconds | RUM + synthetic browser tests |
| Tracking search (first page) | P95 <= 5 seconds | API metrics + DB query timing |
| Transfer trigger-to-start latency | <= 60 seconds | Worker metrics + queue timestamps |
| Worker throughput | Sustain >= 100 req/sec per tenant (inherited platform target) | Load tests + worker metrics + backlog depth |
| UI save interactions (create/update config) | P95 <= 2 seconds | Frontend timing + BFF/API traces |
| Tracking detail view (single movement) | P95 <= 2 seconds | API metrics + DB query timing |
| Event publication after outcome recorded | P95 <= 60 seconds | Event backbone metrics + consumer lag |

UI and tracking latency targets above are indicative for FXFER and must be validated with representative tenant data volumes and retention settings.

binding acceptance criteria + validation datasets.

- The NFR verification plan must define acceptance datasets (endpoint types, file sizes, error modes) and explicit binding criteria for â€œsupported endpoint typesâ€, including negative testing.

**Reliability and correctness under failure.**

| Reliability scenario | Required behavior | Evidence (how we prove it) |
|---|---|---|
| Partner endpoint unavailable | Work is retried within bounded limits; operator sees failure category quickly | Worker metrics (timeouts/retries) + tracking state transitions + alert thresholds |
| Duplicate work item delivery | No duplicate delivery without explicit rerun intent | Idempotency key enforcement + domain state machine invariants + redrive audit trail |
| Partial success (some destinations succeed, others fail) | Outcomes are recorded per-destination with clear final run summary | Tracking model completeness checks + UI views + audit events |
| Event backbone transient failures | Domain persists outcomes before publish; publish is retried safely | Event adapter metrics + publish retry evidence (pattern-level) |

**Tenant isolation as an NFR.**

- No request may read or mutate data outside its `tenant_id` boundary.
- Tenant context must be enforced consistently at: API Gateway â†’ BFF â†’ Domain â†’ DB routing â†’ Event envelope.
- Evidence includes tenant isolation tests in CI and cross-tenant anomaly monitoring.

**Security controls as measurable NFRs.**

| Control | Measurable requirement | Notes |
|---|---|---|
| Audit completeness | 100% of privileged actions emit an application audit event; audit sink failures alert | â€œPrivileged actionsâ€ includes credential reference changes and rerun/redrive operations. |
| Secret-handling invariants | Secret values never persist in application DB; never appear in logs/events | Validated via code scanning + runtime sampling and redaction tests. |
| Payload minimization | Logs/events avoid file payload content; metadata allowlist enforced | Constrained by |

**Operability / supportability.**

- MTTR posture depends on having correlation_id/trace_id available from UI through worker outcomes to domain state.
- Runbook coverage must include: DLQ handling, rerun controls , credential incidents, and tenant isolation incidents.

**Performance and scalability drivers (ties to open retention/read-model decisions).**

- retention windows directly impact storage growth, query performance, and backup/restore times.
- CQRS read-model granularity/indexing/retention directly impact tracking search performance and long-term operability.
- version support window policy impacts how long legacy event/API versions must be supported, affecting operational complexity.

---

# 14. Testing and Quality Strategy

## 14.1 Test Levels

Status: Complete â€” test levels and contract posture defined.

FXFER verification is organized around two principles:

- Contract-first seams: the UI (Shell/MFE) talks only to the BFF; the BFF and workers talk to FXFER domain APIs; domain APIs publish/consume cross-domain events. Each seam has an explicit contract artifact and merge-blocking verification (see Provider Plus Modernization ARB packet (v5, June 2026)).

**Contract testing posture (HTTP and events).**

- Event seams (domain published events and consumer adapters) are specified and validated as schema/version-compatible artifacts.
 - version support window policy (how long older event/API versions remain supported) bounds the forward/backward compatibility gates.

**Test levels and where they run.**

| Test level | Scope | What it proves | Where it runs |
|---|---|---|---|
| Unit | Shell/MFE, BFF, domain APIs, workers | Pure logic correctness: validation rules, state transitions, retry classification, mapping functions | Local + CI |
| Component | Single process (e.g., BFF route handler; worker handler; domain API endpoint + validators) with dependencies stubbed | Boundary behavior, error normalization, and invariants at the component edge | Local + CI |
| Contract (HTTP) | OpenAPI for BFF and domain APIs | Producer conforms to OpenAPI; breaking changes are detected before merge | CI (merge gate) + pre-prod |
| Contract (Event) | Schemas for FXFER published/consumed events | Producer/consumer remain schema-compatible; event evolution follows compatibility rules | CI (merge gate) + pre-prod |
| Integration (data) | Domain APIs + Aurora PostgreSQL (database-per-tenant) | Persistence semantics, migrations, CQRS projection correctness, and isolation boundaries at the DB layer | CI + pre-prod |
| Integration (eventing) | Workers/projections + queues/topics | Duplicate delivery and retries are safe (idempotency/dedup), DLQ behavior is bounded, and projection lag is observable | CI + pre-prod |
| Integration (external adapters) | Endpoint adapters/workers + endpoint simulators | Protocol correctness and deterministic failure classification without calling real partner endpoints | CI + pre-prod |
| End-to-end (user journeys) | Browser -> Shell/MFE -> BFF -> domain APIs -> workers/projections | UX workflows work end-to-end: create config, initiate/observe transfer, view tracking, handle failures | Pre-prod (and optionally CI for smoke) |
| Performance / load | Domain APIs, workers, projections, tracking queries | Meets throughput/latency and backlog recovery expectations for representative tenant volumes | CI (nightly) + pre-prod |

**Multi-tenant / tenant-isolation coverage (required across levels).**

- Negative tests: cross-tenant access attempts must fail (tenant context derives from identity claims and is propagated consistently).
- Routing tests: the correct tenant database is selected for the same request path under different tenants (database-per-tenant).
- Event envelope tests: produced/consumed events include tenant context and correlation context, and consumers reject missing/invalid tenant context.

## 14.2 Golden Data Sets

Status: Partial â€” dataset classes defined; payload classification/retention are defined by the decision register (Section 16).

FXFER requires a small number of versioned, synthetic datasets that are safe to run repeatedly (including in CI) and that exercise both happy path and failure modes.

Golden datasets are defined as **manifests** (tenant configuration + endpoint simulator configuration + expected outcomes) plus **seed data** (domain state, projection state) that can be loaded deterministically.

file payload classification by file type.

retention windows for tracking/artifacts.

- Until retention is finalized, datasets include parameterized "aged" tracking records so read-model behavior can be validated under multiple assumed windows.

**Recommended dataset classes (synthetic).**

| Dataset class | Primary purpose | Coverage emphasis | Notes |
|---|---|---|---|
| Smoke (single-tenant) | Fast confidence per change set | Minimal config, one endpoint type, basic tracking | Runs in CI; proves contracts + basic invariants |
| Parity (multi-tenant) | Tenant isolation and behavioral parity | 2-3 tenants with different config overlays; negative tests | Exercises isolation boundaries and tag propagation |
| Failure-mode (multi-tenant) | Reliability under partner failures | Auth failures, timeouts, transient errors, partial success | Requires endpoint simulators that can deterministically fail |
| Scale (representative volumes) | Performance, backlog recovery, and read-model scaling | Many tracking records; concurrency and retry behavior | Runs nightly/pre-prod; bounded by per-tenant concurrency caps and bounded retries |

**Endpoint simulators (non-production).**

maintain deterministic endpoint simulators for each supported endpoint type (as determined by and each failure class (auth, timeout, throttling, partial transfer). The simulator contract should support:

- Deterministic responses keyed by correlation_id and/or a test-case ID (so retries are repeatable).
- Explicit fault injection: drop connection, delay response, return protocol-level errors.
- Telemetry hooks to emit "received" events that can be correlated to FXFER delivery attempts.

scheduling/trigger model and rerun/redrive controls.

- Dataset execution must be able to drive the system under the selected trigger model (scheduled vs event-driven vs operator-driven) without inventing production scheduling constraints.

## 14.3 Generated Test Artifacts

Status: Complete â€” CI artifact categories and publishing posture defined.

CI generates and publishes a small set of artifacts per build that serve as audit evidence and as inputs to downstream verification.

**Artifact categories (pattern level).**

- Test run reports: unit/component/integration/e2e summaries with per-suite pass/fail.
- Contract artifacts:
 - OpenAPI documents for BFF and domain APIs (versioned with the code).
 - Event contract documents and schemas for FXFER published/consumed events (see and ).
 - Compatibility outputs: contract diffs and a machine-readable "breaking vs non-breaking" classification.
- Golden dataset manifests: dataset definitions, seed parameters, and expected-outcome snapshots.
- Performance evidence (when executed): run configuration, load profile, and latency/throughput summaries.

## 14.4 Traceability Matrix

Status: Partial â€” lightweight matrix defined; acceptance datasets and some requirement IDs are defined by the decision register (Section 16).

FXFER traceability is intentionally lightweight: the goal is to link (a) requirements/invariants/contracts to (b) verification harnesses and (c) operational signals, so defects can be diagnosed with evidence rather than inference.

binding acceptance criteria + validation datasets.

- The matrix below references dataset classes (Smoke/Parity/Failure-mode/Scale). Binding acceptance criteria cannot be finalized until validation datasets are approved.

maintain a traceability table keyed by stable IDs (Decisions, Proposals, and contract identifiers) that maps to tests and to observable runtime signals.

| Trace ID | Concern (rule/contract) | Verification (tests) | Operational signals (detection) |
|---|---|---|---|
| | Tenant isolation (database-per-tenant) | Parity dataset; negative tests for cross-tenant access; routing tests | OTel tags: `tenant_id`; anomaly alerts for cross-tenant access patterns |
| | Contract compatibility gating | Contract (HTTP) tests against OpenAPI; contract (Event) tests against schemas | CI gate status; consumer error counters by event_name |
| Requirement | Rerun/redrive controls | Failure-mode dataset; DLQ/redrive tests; idempotency tests under duplicates | DLQ depth/age; redrive audit events; worker retry counters |
| Requirement | Retention windows | Scale dataset with aged records; read-model query performance tests | DB size/IOPS; query P95; storage growth trend by tenant |
| Requirement | Read-model granularity/indexing | Integration (data) tests for tracking queries; performance tests on representative search patterns | Read-model lag; query plan regressions; projection backlog |

---
# 15. Legacy Transition and Retirement

## 15.1 Transition Pattern

Status: Partial â€” cutover shape defined; legacy posture details are defined by the decision register (Section 16).

migrate FXFER using a per-tenant phased cutover with an explicit validation window (strangler/parallel-run discipline) rather than a single "big bang". This keeps tenant risk bounded and makes rollback a routable decision rather than an outage response.

- Phase cutover unit: tenant (state/program) to align with database-per-tenant isolation and tenant-specific endpoints (see Provider Plus Modernization ARB packet (v5, June 2026)).

legacy SOAP/WCF migration posture.

- Transition runbooks must remain compatible with multiple switch-over patterns (bridge vs client-migrate vs retire), until the posture is closed.

scheduling/trigger model and rerun/redrive controls.

- The transition plan assumes there is an operator-controlled "stop the line" mechanism and an auditable redrive/rerun model; privileged reruns are performed via CLI rerun mode and are fully auditable.

## 15.2 Coexistence

Status: Partial â€” coexistence model defined at pattern level; legacy source-of-truth boundaries remain to be confirmed.

Coexistence is defined as a controlled period where both legacy and target are running, but only one is authoritative for each tenant and each business object at any point in time.

legacy source-of-truth boundaries for FXFER business objects during transition.

- Until source-of-truth boundaries are confirmed, coexistence must be treated as a routing decision with explicit "reads from" and "writes to" rules per object.

define an explicit source-of-truth matrix per tenant for the transition window.

| Business object | Pre-cutover (proposal) | Validation window (proposal) | Post-cutover (proposal) |
|---|---|---|---|
| Transfer configuration | Legacy authoritative | Target authoritative; legacy read-only (if needed for parity) | Target authoritative |
| Endpoint credential references | Legacy authoritative | Target authoritative (with full audit); legacy changes frozen except break-fix | Target authoritative |
| Transfer run state/outcomes | Legacy authoritative | Target authoritative; parity checks compare summaries | Target authoritative |
| Tracking/search read models | Legacy authoritative | Target read models built and validated against golden datasets + sampled production-shaped data | Target authoritative |

retention windows for tracking/artifacts.

- Coexistence planning must include retention/archival handling for legacy tracking artifacts so retirement does not violate contractual retention.

## 15.3 Phases

Status: Partial â€” phased plan proposed; entry/exit gates must be confirmed with stakeholders.

use the following phased delivery model. Each phase has explicit entry/exit gates with observable verification (tests + operational signals).

| Phase | Intent | Entry criteria (minimum) | Exit criteria (minimum) |
|---|---|---|---|
| Phase 0 â€” Foundations | Contracts, observability, and test harnesses exist before migration pressure | OpenAPI for BFF/domain APIs drafted; event contracts drafted (see ); OTel tagging plan defined | Contract gates active in CI; golden Smoke dataset passes; telemetry tags present end-to-end |
| Phase 1 â€” Target capability in pre-prod | Build target FXFER surfaces and validate on simulators | Endpoint simulators available ; Failure-mode dataset defined | Parity + Failure-mode datasets pass; DLQ behavior observed and bounded; runbooks drafted |
| Phase 2 â€” Pilot tenant cutover | Migrate one tenant with tight operational oversight | Tenant-specific runbook + rollback plan approved; partner endpoint connectivity pattern agreed | Pilot tenant achieves acceptance criteria ; operational metrics stable |
| Phase 3 â€” Scale-out + retirement | Repeatable tenant onboarding; retire legacy per deprecation windows | Repeatable onboarding checklist; automation for tenant routing and validation | Legacy traffic for migrated tenants is zero; retirement criteria met (see 15.6) |

## 15.4 Legacy Consumer Switch-Over

Status: Partial â€” switch-over patterns defined; SOAP/WCF posture is defined by the decision register (Section 16).

legacy SOAP/WCF migration posture.

Until the SOAP/WCF posture is closed, switch-over must support multiple patterns without requiring FXFER to commit prematurely:

- Bridge (anti-corruption layer): a facade translates legacy SOAP/WCF contracts into target command/query APIs.
- Client migration: legacy consumers are migrated to target HTTP APIs (OpenAPI-first) and legacy endpoints are deprecated.
- Dual-run with cutover routing: traffic is routed to either legacy or target per tenant/consumer, with a defined deprecation window.

select the switch-over pattern(s) per consumer class (internal vs external) and define the deprecation window policy aligned to 

## 15.5 Rollback

Status: Partial â€” rollback and "stop the line" posture defined; automation details depend on trigger model.

Rollback is treated as a **routing change** first, not a data deletion.

adopt an explicit "stop the line" model per tenant:

- Triggers: tenant-isolation anomaly, contract break detected in pre-prod, sustained partner endpoint failure, DLQ age beyond threshold, or correctness defects in tracking/outcomes.
- Immediate actions: stop new triggers for the tenant, quarantine in-flight work items, and preserve evidence (correlation_id/trace_id and audit events).
- Data safety: do not delete target data during rollback; preserve it for forensic analysis and replay/reconciliation.

eventing/queue delivery is at-least-once, requiring idempotent processing and deduplication at consumers.

## 15.6 Retirement Criteria

Status: Partial â€” criteria defined; deprecation windows and retention are defined by the decision register (Section 16).

Legacy retirement is permitted only when all of the following are true:

- Traffic: no production callers for the legacy FXFER surfaces for migrated tenants (measured via gateway/service metrics).
- Contracts: deprecation window satisfied for the legacy contract versions.
 - version support window policy.
- Data: retention obligations for legacy tracking artifacts are satisfied.
 - retention windows for tracking/artifacts.
- Operations: runbooks exist for DLQ/redrive, credential incidents, and partner endpoint outages (see Section 12.6).
- Audit: privileged operations and rerun/redrive actions are fully audited and reviewable.

## 15.7 Legacy Changes During Transition

Status: Partial â€” governance posture defined; approval workflow must be confirmed.

During the transition window, legacy changes must be tightly governed to avoid permanent parity drift.

enforce the following legacy-change governance rules:

- Allowed by default: break-fix changes required to keep legacy stable until tenant cutover.
- Not allowed by default: new feature work that creates new contract shapes or new endpoint types without an explicit migration plan.
- Exception path: any legacy change that affects externally visible behavior must include (a) a documented parity impact and (b) an update plan for the target contracts/tests.

---
