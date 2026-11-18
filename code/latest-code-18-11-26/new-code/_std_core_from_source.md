---
id: core
version: 0.2.0
owners:
- unassigned
applies_to: []
---
# Core coding standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-CORE` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repository’s designated owner before adoption |
| Scope | All code changes governed by this pack, including generated code and reviews. |

## Interpretation, applicability, and precedence

The uppercase keywords **MUST**, **MUST NOT**, **SHOULD**, and **MAY** express requirements, prohibitions, defaults with justified departures, and permitted options using the RFC 2119 / RFC 8174 convention. They are proposed obligations in this draft, not evidence of existing organizational approval.

Load core plus only the language/cross-cutting standards applicable to the task. More specific guidance refines this standard; it does not silently waive another rule. Repository instructions govern routing. An explicit approved precedence or scoped exception resolves conflicts; otherwise apply SDD-CORE-002.

Keep changing architectural decisions in current HLD/LLD/ADR or task inputs. This pack does not select a runtime version, AOT mode, deployment platform, ORM, tenant topology, eventing pattern, or error schema.

## Required runtime inputs

Current task requirements; root and scoped agent instructions; approved, applicable HLD/LLD/ADRs; repository configuration and reference implementations. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-CORE-001` | MUST ground implementation choices in current task requirements and applicable approved design inputs. An old ARB or nearby code alone MUST NOT be treated as approval for an architecture decision. | Review records identify current inputs and the decision supporting any material design choice. |
| `SDD-CORE-002` | MUST identify conflicts between requirements, approved design inputs, standards, and implementation. Follow an explicitly documented precedence where it resolves the conflict; otherwise pause only the dependent change and request the missing decision. MUST NOT silently use document recency or existing code to invent precedence. | Conflicting sources and the resolution or blocked work are recorded. |
| `SDD-CORE-003` | MUST preserve specified behavior and public contracts unless a change is explicitly authorized. Legacy behavior MUST NOT automatically override an approved target requirement. | Diff and relevant tests trace to the requested behavior; intentional differences are identified. |
| `SDD-CORE-004` | MUST respect the approved module boundaries and reuse existing supported libraries/utilities where they fit. A new dependency or boundary change MUST have task or design justification; convenience alone is insufficient. | Review of references, project dependency changes, and affected call paths. |
| `SDD-CORE-005` | SHOULD follow scoped naming, layout, and formatting conventions established by repository configuration and approved examples. MUST NOT perform unrelated rewrites or reformatting as part of a focused task. | Diff inspection and the repository formatter/analyzer checks, where configured. |
| `SDD-CORE-006` | MUST distinguish verified facts from assumptions, unresolved questions, and unexecuted checks. MUST NOT claim a test passed, compliance was established, or an example was approved without evidence. | Completion record includes actual commands/results and remaining limitations. |
| `SDD-CORE-007` | MUST preserve existing verification gates. MUST NOT weaken tests, disable analyzers, or broaden suppressions merely to make a change pass. A justified exception MUST record the affected rule, scope, reason, approval evidence, and review/expiry condition through the existing process. | Diff of tests/configuration and any exception record. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

**Compliant:** Use the project’s supported runtime and current approved design for an endpoint; report a conflicting historical deployment constraint.

**Noncompliant:** Add an AOT-specific implementation constraint because an older ARB once selected AOT, without checking current applicability.

## Enforcement

Use the repository’s existing build, format, analyzer, dependency, and test gates. Manual review remains necessary for source applicability, architecture conflicts, behavior changes, and exceptions. No particular enforcement tool or CI command is selected by this document. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.

## Keyword references

- [RFC 2119](https://www.rfc-editor.org/rfc/rfc2119): requirement terminology.
- [RFC 8174](https://www.rfc-editor.org/rfc/rfc8174): uppercase keyword convention.
