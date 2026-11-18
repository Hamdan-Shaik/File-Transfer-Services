---
id: security-privacy
version: 0.2.0
owners:
- unassigned
applies_to: []
---
# Security and privacy standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-SEC` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repository’s designated owner before adoption |
| Scope | Changed trust boundaries, authorization checks, data handling, configuration, and diagnostic exposure. Tenant-specific rules apply only to tenant-scoped operations. |

## Applicability and precedence

Read [core](../core.md) for requirement keywords, current-source handling, conflicts, and exceptions. Apply this file only within its stated scope. Specific contracts supply values and mechanisms; they do not silently override mandatory rules. Resolve conflicts using SDD-CORE-002.

## Required runtime inputs

Current access policy, trusted identity/context mechanism, field classification, secret-management mechanism, and any task-specific audit or retention requirements. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-SEC-001` | MUST use the established trusted identity mechanism and enforce the supplied authorization policy before protected work. MUST NOT trust a caller-provided identity, role, or tenant identifier merely because it is well-formed. | Authorized and unauthorized behavior tests at the relevant entry and resource boundary. |
| `SDD-SEC-002` | For tenant-scoped work, MUST enforce the authorized tenant/resource scope across each boundary changed by the task. Missing or mismatched scope MUST NOT fall back to a global/default tenant. The isolation mechanism MUST come from current design inputs, not this standard. | Positive and cross-tenant/missing-context negative tests at the affected boundary. |
| `SDD-SEC-003` | MUST restrict client-writable and response fields to the approved contract and access policy. Guess-resistant identifiers alone MUST NOT be treated as authorization. | Field-level and resource-level access review with relevant negative tests. |
| `SDD-SEC-004` | MUST obtain secrets through the approved configuration/secret mechanism. MUST NOT commit credentials or place secrets in source, fixtures, logs, public error responses, or generated documentation. | Diff/secret scan using available repository tooling and configuration review. |
| `SDD-SEC-005` | MUST treat external values as data when constructing queries or commands. Use the established parameterization/safe API; where identifiers cannot be parameterized, use an explicit allowed set. MUST NOT concatenate untrusted input into executable syntax. | Query/command construction review and relevant injection-focused tests. |
| `SDD-SEC-006` | MUST follow supplied data classification and minimization rules in outputs, logs, tests, and storage changes. MUST NOT copy production personal data into fixtures or expose unclassified values in diagnostics. MUST NOT assume PHI or a regulatory regime solely from a generic template. | Field classification review, synthetic fixtures, and checks for sensitive exposure. |
| `SDD-SEC-007` | MUST preserve established authentication and transport-validation controls. MUST NOT bypass authorization, accept unvalidated tokens, or disable certificate validation to make integration or tests succeed. | Security-related configuration/code diff and applicable negative tests. |
| `SDD-SEC-008` | When a task requires audit records, retention, deletion, or cryptographic changes, MUST use the current approved policy and mechanism. Diagnostic logs MUST NOT be represented as sufficient audit evidence without that policy. | Applicable policy reference and behavior evidence; otherwise mark the requirement unresolved. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

**Compliant:** Resolve caller context through the existing trusted mechanism, authorize access to the requested tenant/resource, and reject a mismatched scope.

**Noncompliant:** Read a tenant ID from a request header and select its data without proving that the caller may access it. This example does not prescribe where authentication or tenant routing runs.

## Enforcement

Use existing secret/security checks plus focused authorization, tenant-boundary, and exposure tests. Review all changed trust boundaries manually. Passing static checks alone does not prove tenant isolation or regulatory compliance. Do not select an authentication provider, database isolation topology, or cryptographic design in this file. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.

