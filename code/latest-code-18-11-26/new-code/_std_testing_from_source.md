---
id: testing
version: 0.2.0
owners:
- unassigned
applies_to: []
---
# Behavior verification and testing standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-TEST` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repository’s designated owner before adoption |
| Scope | Tests and verification evidence for changed behavior, generated code, regressions, and explicitly requested modernization comparisons. |

## Applicability and precedence

Read [core](../core.md) for requirement keywords, current-source handling, conflicts, and exceptions. Apply this file only within its stated scope. Specific contracts supply values and mechanisms; they do not silently override mandatory rules. Resolve conflicts using SDD-CORE-002.

## Required runtime inputs

Approved behavior contract/acceptance examples, existing test suites and fixtures, documented commands and prerequisites, and legacy comparison context only when parity is requested. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-TEST-001` | MUST derive expected outcomes from an independent requirement, approved example, or explicitly labeled characterization source. MUST NOT compute expected results using the implementation under test or change expectations merely to obtain a passing result. | Assertion-to-source review and independent expected values. |
| `SDD-TEST-002` | MUST select a test boundary that can establish the claimed property. Mocked unit tests MUST NOT be claimed as proof of real middleware, serialization, authorization, database translation, or transaction behavior. | Test-boundary review and relevant integration evidence. |
| `SDD-TEST-003` | MUST cover changed observable behavior with meaningful success, specified failure, and boundary cases where applicable. SHOULD add a regression test for a reproducible defect. MUST NOT introduce arbitrary coverage percentages or redundant tests that merely mirror implementation. | Requirement-to-case mapping; regression failure before the fix when feasible. |
| `SDD-TEST-004` | MUST use controlled, synthetic test data and isolate mutable state. Tests MUST NOT depend on incidental ordering or shared production data. SHOULD control clocks/randomness/external dependencies using supported fixtures; fixed sleeps SHOULD NOT substitute for observing completion. | Fixture/setup/cleanup review and repeatability checks when nondeterminism is a concern. |
| `SDD-TEST-005` | For changed security or tenant behavior, MUST verify allowed and denied cases at the affected boundary. A valid single-tenant success case MUST NOT be reported as proof of isolation. | Negative access/cross-tenant tests as required by SDD-SEC-001 and SDD-SEC-002. |
| `SDD-TEST-006` | When legacy parity is requested, MUST record the legacy version, relevant context, equivalent inputs, expected comparison, and approved differences. Configuration coverage and functional parity MUST be reported as separate claims. | Comparison fixtures/results with explicit context and intentional differences. |
| `SDD-TEST-007` | MUST use the established framework and supported test commands unless a change is authorized. MUST NOT skip failing tests, weaken assertions, or disable checks solely to make generated code pass. | Test/configuration diff and executed command results. |
| `SDD-TEST-008` | MUST report verification as pass, fail, blocked, or not applicable with evidence. Blocked checks MUST identify the missing prerequisite; not-applicable checks MUST explain scope. A review without executed tests MUST NOT be described as a test pass. | Task/PR evidence containing commands, outcomes, and limitations. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

**Compliant:** An approved requirement specifies how a tenant override affects a field; a test supplies explicit configuration and asserts the documented outcome.

**Noncompliant:** Run the new code, copy its output into expected results, and call this proof of legacy parity without a legacy reference.

## Enforcement

Run the scoped tests and existing required related gates. Use broader testing when impact warrants it or failures reveal additional risk. No test framework, coverage threshold, legacy-oracle source, or environment topology is selected by this standard. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.

