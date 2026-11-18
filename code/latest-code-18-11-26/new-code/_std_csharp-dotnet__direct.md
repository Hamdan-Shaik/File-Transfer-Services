---
id: csharp-dotnet
version: 0.2.0
owners:
- unassigned
applies_to:
- '**/*.cs'
- '**/*.csproj'
- '**/Directory.*.props'
---
# C# / .NET coding standard

| Metadata | Value |
|---|---|
| Standard ID | `SDD-CS` |
| Version | `0.1.0-draft` |
| Status | Proposed v1; not yet approved for enforcement |
| Owner | Unassigned; record the repository’s designated owner before adoption |
| Scope | Changed C# code and .NET project configuration. Applies within the project’s existing runtime, language, and dependency constraints. |

## Applicability and precedence

Read [core](../core.md) for requirement keywords, current-source handling, conflicts, and exceptions. Apply this file only within its stated scope. Specific contracts supply values and mechanisms; they do not silently override mandatory rules. Resolve conflicts using SDD-CORE-002.

## Required runtime inputs

Project files, SDK pinning if present, package management files, editor/analyzer configuration, representative code, and current task contracts. Discover these in the supplied task and repository first. If an unresolved input affects correctness, security, or compatibility, pause the dependent change and report the exact gap; continue independent work.

## Rules and verification

| Rule ID | Requirement | Verification evidence |
|---|---|---|
| `SDD-CS-001` | MUST use the target project’s supported C#/.NET features and installed APIs. Runtime, language-version, package-version, and deployment-mode changes MUST NOT be introduced implicitly by a coding task. | Project/configuration diff and build against the repository-supported environment. |
| `SDD-CS-002` | SHOULD follow configured naming, formatting, visibility, and file organization. MUST keep new public members limited to the required contract; MUST NOT invent a new project-wide style. | Formatter/analyzer output where available and public API review. |
| `SDD-CS-003` | MUST handle nullable values according to the contract and configured nullability context. MUST NOT use null-forgiving operators, blanket pragmas, or fabricated defaults to hide unresolved input semantics. Local suppressions require a documented, demonstrable invariant. | Relevant compiler/analyzer findings and null/absent-value tests for changed behavior. |
| `SDD-CS-004` | MUST await or explicitly manage asynchronous work according to the application lifecycle. In asynchronous request/application paths, MUST NOT introduce blocking waits such as Task.Wait or Task.Result, or untracked fire-and-forget work. Async void is limited to an existing event contract that requires it. | Async call-path review and relevant analyzer findings; completion/failure tests where lifecycle changes. |
| `SDD-CS-005` | MUST propagate cancellation tokens through operations that support cancellation when the task’s execution contract supplies one. Cancellation MUST NOT be translated into successful completion or an unrelated business failure. | Token flow review and cancellation tests when cancellation behavior changes. |
| `SDD-CS-006` | MUST release resources owned by the current scope through their supported disposal mechanism, including asynchronous disposal where required. MUST NOT dispose shared or injected resources whose lifecycle is owned elsewhere. | Ownership/disposal review and resource-lifecycle tests where the change is material. |
| `SDD-CS-007` | MUST preserve exception stack information when rethrowing the same exception. Exception translation and diagnostic ownership MUST follow SDD-LOG; MUST NOT swallow exceptions to conceal failed work. | Exception-path review and affected error tests. |
| `SDD-CS-008` | MUST respect existing dependency lifetimes and mutable-state ownership. MUST NOT capture a shorter-lived dependency in a longer-lived object or share request-specific mutable state across requests. | Registration/lifetime review and concurrency/isolation tests when affected. |
| `SDD-CS-009` | MUST use explicit contract semantics for parsing, serialization, timestamps, numeric conversions, and enum handling. MUST NOT introduce culture-dependent parsing, timezone conversion, or rounding where it changes the contract unintentionally. | Contract comparison and targeted conversion/serialization tests. |

## Approved patterns and examples

Use current, maintainer-approved examples in the target project and record their actual paths. No repository example was supplied with this draft. The following illustrates the rule, not an approved application implementation.

Illustrative C# syntax only; these are not repository APIs.

```csharp
catch
{
    throw; // Preserves the original stack when rethrowing.
}
```

**Noncompliant:** `catch (Exception ex) { throw ex; }` resets stack information at the rethrow. Do not add either catch block if it serves no purpose.

## Enforcement

Run the repository-supported build and configured analyzers/formatter. Use focused tests for changed value semantics, cancellation, lifecycle, or concurrency. A build alone does not establish those behaviors. Nullability policy, analyzer severities, and package versions remain repository inputs. Record actual commands and results. Rule IDs provide references for reviews, skills, and checklists; they are newly proposed IDs and must be reconciled with any existing catalog before adoption.

