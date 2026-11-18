---
id: api-contracts
version: 0.2.0
owners:
- unassigned
applies_to: []
---
# API contracts standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-API` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repositoryâ€™s designated owner before adoption |
| Scope | Changed HTTP endpoints and their request/response validation and mapping. Applies to a BFF operation only when one exists in the current task; it does not mandate a BFF architecture. |

## Applicability and precedence

Read [core](../core.md) for requirement keywords, current-source handling, conflicts, and exceptions. Apply this file only within its stated scope. Specific contracts supply values and mechanisms; they do not silently override mandatory rules. Resolve conflicts using SDD-CORE-002.

## Required runtime inputs

Approved operation contract, current authorization requirements, existing endpoint/validation/error pipeline, consumers affected by the change, and serialization configuration. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-API-001` | MUST implement the supplied route, method, binding locations, response shapes, and failure outcomes. MUST NOT invent a versioning, pagination, status-code, or error-envelope convention when the contract is missing. | Contract comparison and HTTP-level tests for changed behavior. |
| `SDD-API-002` | MUST identify externally observable compatibility changes, including field removal/type changes, new required inputs, default behavior, and error semantics. Such changes MUST have explicit authorization before implementation. | Before/after contract review and authorization for intentional breaks. |
| `SDD-API-003` | MUST derive validation rules from explicit requirements and run them through the established validation path. Missing, null, empty, whitespace, bounds, and cross-field semantics MUST be implemented only as specified. MUST NOT silently normalize or default ambiguous inputs. | Rule-to-requirement mapping and relevant valid/invalid/boundary tests. |
| `SDD-API-004` | MUST map only intended client-writable fields into application operations. Server-managed identity, ownership, audit, and concurrency values MUST remain governed by their authoritative mechanism. Outbound models MUST include only contract-approved fields. | Field mapping review and protected-field/output-exposure tests. |
| `SDD-API-005` | MUST preserve the specified difference between omitted values and explicit clearing in partial updates. If the request representation cannot express that distinction, the dependent implementation MUST wait for a contract decision. | Request representation review and omission/clearing tests when applicable. |
| `SDD-API-006` | MUST use the established application interface and error pipeline for the operation. MUST NOT duplicate existing validation or exception translation, or introduce direct data access that violates a supplied boundary. | Call-path review and tests through the relevant pipeline. |
| `SDD-API-007` | MUST apply SDD-SEC for caller authorization, trusted context, and data exposure, and SDD-LOG for public error disclosure. A client-supplied identifier MUST NOT substitute for authorization. | Access/failure tests at the actual boundary and review against the referenced rules. |
| `SDD-API-008` | MUST keep maintained API descriptions and generated contract artifacts consistent with changed behavior through the existing repository process. MUST NOT create a second conflicting source of contract truth. | Contract generation/check results if available, plus affected documentation diff. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

**Compliant:** A supplied partial-update contract says omission preserves a value and explicit null clears it; the request representation and tests distinguish both cases.

**Noncompliant:** Convert both omission and null to a default value, silently changing update behavior.

## Enforcement

Use the projectâ€™s HTTP integration/contract tests for binding, serialization, middleware, and response behavior; use focused unit tests for local validation and mapping. Isolated handler tests cannot establish the HTTP contract. No API framework, validation library, mapper, or error RFC is adopted implicitly. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.


