---
id: logging-errors
version: 0.2.0
owners:
- unassigned
applies_to: []
---
# Logging and error handling standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-LOG` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repository’s designated owner before adoption |
| Scope | Changed diagnostics, exception propagation, public error handling, and correlation in application code. |

## Applicability and precedence

Read [core](../core.md) for requirement keywords, current-source handling, conflicts, and exceptions. Apply this file only within its stated scope. Specific contracts supply values and mechanisms; they do not silently override mandatory rules. Resolve conflicts using SDD-CORE-002.

## Required runtime inputs

Existing logging API and event conventions, correlation context, exception/public-response pipeline, data classification, and expected failure/cancellation behavior. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-LOG-001` | MUST emit diagnostics through the established logging abstraction and structured-field conventions. SHOULD use stable message templates rather than interpolating variable values into message text. Event names and severity MUST follow the current project conventions. | Logging-call review and representative captured events. |
| `SDD-LOG-002` | MUST preserve required correlation context through the supported mechanism. MUST NOT invent a competing correlation scheme or place sensitive data in correlation identifiers. | Context-propagation review and captured correlation fields where affected. |
| `SDD-LOG-003` | MUST define the emitting layer for each added failure event and avoid logging the same failure repeatedly at successive layers. Distinct operational events MAY be logged when each provides a required, separate diagnostic purpose. | Trace of the relevant failure path and event counts/content where duplicate emission is a risk. |
| `SDD-LOG-004` | MUST apply SDD-SEC data-classification rules to log fields, exception messages, and stack/context data. MUST NOT log raw payloads or secrets as a convenience. Required redaction MUST use the approved mechanism. | Captured diagnostics with synthetic sensitive markers and review of exception content. |
| `SDD-LOG-005` | MUST preserve the intended distinction between expected outcomes, unexpected failures, and cancellation. MUST NOT silently discard exceptions or return success for failed work. Rethrowing MUST preserve stack information as required by SDD-CS-007. | Failure/cancellation tests and exception-path review. |
| `SDD-LOG-006` | MUST translate public errors through the established boundary and supplied error contract. MUST NOT expose implementation stack traces, secret values, or internal connection details to callers. | Actual public error-response tests and sanitized diagnostic inspection. |
| `SDD-LOG-007` | MUST add catches, retries, or fallbacks only when their behavior is justified by the current task/design. Catch-and-log/rethrow duplication and unspecified retry policies MUST NOT be introduced to make errors appear handled. | Review of recovery policy and relevant failure tests; retry behavior checked only where authorized. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

Illustrative logging syntax, applicable only if the existing logger supports this API:

```csharp
logger.LogInformation("Processed {ItemCount} items", count);
```

**Noncompliant:** Interpolate a raw request payload into a log message, potentially exposing personal data and secrets. A structured API alone does not make its values safe.

## Enforcement

Use the repository’s existing logging capture/test utilities and error-pipeline tests. Check safe content, meaningful fields, correlation, and duplicate events where changed. Do not mandate a vendor, telemetry protocol, severity mapping, retention period, or audit architecture. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.

