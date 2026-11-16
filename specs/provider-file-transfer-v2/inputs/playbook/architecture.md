---
type: Playbook
title: 'Playbook: target architecture for one module'
description: 'The module''s behaviour is specified per business flow: the specs in technical/flows/<flow>/ (FLOW.md, RULES.md) and the scenarios in business/<flow>.feature.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 72eadefe6a1b17ec72a412b69c036c974bff3ed13bbbb5d75f390b8cef3ccc98
---

# Playbook: target architecture for one module

The module's behaviour is specified per business flow: the specs in `technical/flows/<flow>/`
(FLOW.md, RULES.md) and the scenarios in `business/<flow>.feature`. The tool has cut the module's code into **components** in
`technical/components.yaml`: groups of legacy classes by tier (`ui`, `backend`, `data`, `shared`),
each with its classes, methods, dependencies, the flows it serves, its endpoints and tables. You
now decide **what the new system is made of** and **where every component goes** in it. Behaviour
does not change; the structure follows the target-state document exactly.

Your input is **ARCHITECTURE-BRIEF.md** (path in the task): the components to place, the flows,
the target-state document, and the output files.

## How to work

1. Read the target-state document completely, above all its service catalog, BFF, front end,
   batch and shared-library sections, and its **target folder structure** (the module layout and
   the inside of a service). The folder structure decides every path you write; the service
   catalog decides which services exist.
2. Read `technical/components.yaml` and, where a component's purpose is unclear, its
   `INVENTORY.yaml` and the flows it serves.
3. List the deployables the target state defines for this module, with the paths it gives: domain
   services, the BFF, micro-frontends, batch jobs, workers, projectors, libraries. Do not invent
   layers or merge services the target state keeps apart. Services the target state places outside
   this module (shared services, other modules, external systems) go under `uses`: they are called,
   never generated here. Anything this module builds itself is a deployable, never a `uses` entry:
   its own database schema (the migrations of the tables its components use) is a deployable of kind
   `schema` at the folder the target state gives for it, because only a deployable gets a task that
   builds it.
4. **Place every component** listed in the brief, those no door reaches too: a component no door
   reaches is still legacy code the new system needs, and the graph may only have missed the calls
   into it. Leaving code out is a person's decision, made in `inputs/coverage-decisions.yaml`, never
   yours. Give each the deployable it belongs to (by bounded context:
   the domain service whose business the component's flows are) and the folder inside it, following
   the target's inside-of-a-service layout. As a guide: `backend` API components go to the service's
   API folder (or the BFF when they only shape screens), other `backend` components to its
   application folder; `data` components to its infrastructure folder; `shared` components to its
   domain folder or a module library; `ui` components to a micro-frontend; components behind batch
   doors to a batch job. A `ui` component holds what a person works in, so it never goes to an API or a
   service, even when its legacy class also holds business logic.
5. Order the deployables so each depends only on ones listed before it, and order the flows for
   building: flows that create the core records first, then the flows that build on them.

## The files

### technical/target/architecture.yaml (machine-read; exactly this shape)

```yaml
module: <module name>
deployables:
  - name: <kebab-case name>
    kind: <dotnet-service | bff | mfe | glue-job | airflow-dag | worker | projector | library | schema | infra | other>
    path: <folder in the new repository, from the target folder structure>
    stack: <language / framework / runtime>
    role: <one sentence>
    build: <shell command, run at the repository root, that proves it builds>
    depends_on: [<deployable names>]
    legacy: [<legacy folders it is built from that are not C#, e.g. components/Persistence/APP_DB/**; optional>]
placement:
  <component id>: {deployable: <deployable name>, folder: <folder inside the deployable>}
uses:
  - name: <shared service or external system>
    path: <where it lives, if it is in the repository>
    role: <what this module uses it for>
flow_order: [<every flow id, in build order>]
```

- `build` must work on a fresh checkout once the deployable exists: for example
  `dotnet build <path>` for .NET, `npm --prefix <path> ci && npm --prefix <path> run build` for
  Node.js, `python -m compileall -q <path>` for Python; for a `schema`, a command that checks its
  migrations (for example their tool's validate or dry-run command).
- Every component the brief lists has exactly one placement; `folder` is relative to the deployable.
- `legacy` names the legacy files that are not C# a deployable is built from, as folders or globs relative to the
  legacy source: a `schema` deployable lists the DDL folders of this module's own database. Leave out the DDL of a
  database another system owns, sample or demo databases, and old variants for another engine: a person decides
  those.
- Every flow appears exactly once in `flow_order`.

### technical/target/requirements.yaml (machine-read)

The specs so far come from the legacy code, so they hold only what the old system did. List here what
the target state requires that **no legacy code does**: behaviour it adds (rolling back a published
change, say), and the platform behaviour it demands of this module's code (row-level tenant isolation,
validating every token, retries with backoff, audit records). Read the whole target-state document for
these, above all its rules for generated code, security, data and error-handling sections. Leave out
what is only a choice of technology: that is the deployables' stacks.

```yaml
requirements:
  - id: T-001                    # T-001, T-002, ... in the order you list them
    text: <one sentence: what the new system must do>
    section: <the target-state section it comes from, e.g. "7.4 Rollback">
    deployable: <the deployable that builds it>
    component: <the placed component it extends, or leave it out when none fits>
```

- Each item is one thing a test can prove. Split a section that asks for several.
- Give it the component whose responsibility it extends (the component that publishes, for a rollback
  of a publish). Its spec then specifies it, and the item is built right after that component.
- Write `requirements: []` only if the target state truly adds nothing.

### technical/target/TARGET-ARCHITECTURE.md

A readable page: the deployables with their paths and one line each, how they talk to each other
(APIs, events), which shared services and external systems they use, a diagram (Mermaid is fine),
a table of where the components go, and the build order with the reason for it.

Write only inside the pack's `technical/target/` folder. Never change the source repository.
