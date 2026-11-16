---
type: Playbook
title: 'Playbook: the five spec files of one component'
description: 'A component is a group of the legacy module''s classes (one tier: ui, backend, data or shared) that the new system rebuilds in one place.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 826ac9e3c673c20801f0f3bde58c479bdf40f176b987332fb39eabe65fa531bb
---

# Playbook: the five spec files of one component

A component is a group of the legacy module's classes (one tier: `ui`, `backend`, `data` or
`shared`) that the new system rebuilds in one place. You write its **technical specification**:
what every class and method does today, which business rules it implements, and exactly what to
build in the target. Code is generated from these files, method by method, so they must be
complete and concrete. Behaviour stays exactly the same; the structure and technology are the
target state's.

Your input is **COMPONENT-BRIEF.md** (path in the task): the component, where it goes in the
target, the components it uses, the flows it serves, its legacy endpoints, and the full text of
the business rules it owns. Every class and method you must cover is in its **INVENTORY.yaml**
(signature, file and lines, tables, calls). The legacy source is read-only at the path in the task.

## How to work

1. Read the brief and INVENTORY.yaml. Then read the code of **every class and method** in the
   inventory, from its file and lines: inputs, conditions, calculations, data read and written,
   calls, errors.
2. Read the business rules it owns (in the brief) and the flows' FLOW.md and scenarios, so you know
   why each piece of code exists.
3. Read the target-state document's structure and standards, and the spec of the deployable it goes
   into (`technical/target/deployables/<deployable>/`).
4. Write the five files below. Write only inside the component's folder. Never change the source
   repository.

## The files

### SPEC.md: what the component does today

```
# <component id>: <what it is, in a few words>

<Two or three sentences: its job, who calls it, what it relies on.>

## <ClassName>
<What the class is for.>

### <MethodName>(<parameters>)
- Purpose: ...
- Inputs: ...
- Logic: the steps, with real values (limits, defaults, statuses, messages)
- Data: tables or stores read and written
- Errors: what goes wrong and what the caller gets
- Rules: <rule ids it implements, if any>

## Rules implemented
| Rule | In short | Where | Flow |
| F14-031 | ... | EnrollmentSearchHandler.Search | [search-enrollment-applications](../../../technical/flows/search-enrollment-applications/RULES.md) |

## Not carried over
- <ClassName.MethodName>: <why: dead code, plumbing the target framework provides, ...>
```

- Every class in the inventory has its section; every method has its own sub-section **or** a line
  under *Not carried over* with the reason.
- *Rules implemented* lists **every** rule the brief says it owns, each linking to its flow's
  `technical/flows/<flow>/RULES.md`.
- If the brief lists **target-state requirements** it owns (T-001, ...), add a section *Target-state
  requirements*. Specify each one under its id like a method: what it does, the data it reads and
  writes, its errors, and the target-state section it comes from. TESTS.md gets a test for each id.

### TARGET.md: what to build in the target

- **Where**: the deployable and the folder, written as the full path the brief gives.
- **What to create**: every class (or module, component, job) with its methods and signatures, in
  the target's language and conventions, and what each does.
- **Mapping**: a table with one row per legacy method: `| Legacy | Target | Note |`, where Target is
  `Class.Method` in the new code, `drop: <reason>` or `merged into Class.Method`. Every method of the
  inventory has a row.
- Dependencies on other components (by id) and on shared libraries of the target state.
- In a `ui` component, a screen's event handlers are behaviour, not plumbing: what a selection refreshes,
  what a button validates, every warning and confirmation. Map each to the front-end function that does it
  there. `drop:` is for what the target framework really does by itself, and never for a method that shows
  a person a message. Where the legacy screen also decides or saves, map that part to the API the target
  state gives for it, and name it in CONTRACT.md.

### CONTRACT.md: what it offers and uses

What other components rely on: interfaces, commands, queries and events with their signatures and
payloads; the data it owns. What it uses from each component it depends on (by id).

### openapi.yaml: its HTTP surface

An OpenAPI 3.0 document. A component that serves legacy endpoints describes their new versions in
the target's API style (`/api/v1/...`, the target's error format) and lists every legacy endpoint
under `x-legacy-endpoints` (each exactly as the brief writes it), so the new path of each is clear.
A component without an HTTP surface writes a minimal document (`openapi`, `info`, `paths: {}`) with
`x-no-http-surface: true`.

### TESTS.md: the tests to write

For every owned rule, a unit test named with its rule id (`F14_031_rejects_an_empty_npi`), stated at
this component's level (given this data, when this command or call, then this result); for the
methods, the edge cases SPEC.md describes; contract tests for openapi.yaml and for CONTRACT.md. A
component has no `.feature` of its own: the business scenarios live in `business/<flow>.feature` and
are run end to end when the flow is wired.

A checker verifies that every inventory class and method appears in SPEC.md and in TARGET.md, that
every owned rule is in SPEC.md (with its flow's link) and in TESTS.md, that
TARGET.md states the path, that CONTRACT.md names every component it depends on, and that
openapi.yaml lists every legacy endpoint.
