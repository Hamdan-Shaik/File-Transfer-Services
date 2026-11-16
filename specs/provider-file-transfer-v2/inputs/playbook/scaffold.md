---
type: Playbook
title: 'Playbook: skeleton of one deployable'
description: 'You are creating the **skeleton** of one deployable of a new system: everything it needs to build, start and be tested, but no business logic yet.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: dbc375480c61aef9e61503ced74cfdda2151cf7b8c53d70c125098f7e66699e4
---

# Playbook: skeleton of one deployable

You are creating the **skeleton** of one deployable of a new system: everything it needs to build,
start and be tested, but no business logic yet. The components placed in it are added afterwards,
one per session, into this skeleton, so its structure must be exactly the target state's.

Your inputs (paths in the task):

- the deployable's spec: README.md, STRUCTURE.md, CONTRACT.md under `technical/target/deployables/<name>/`;
- the target architecture (`technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`),
  including the folder of every component placed in it (create those folders);
- the target-state document;
- the deployables built before this one, already in the current folder (the new repository).

## How to work

1. Read the spec, the architecture and the target-state document.
2. Create everything STRUCTURE.md lists at the deployable's path: projects or packages, solution
   or package files, entry point, configuration, Dockerfile, Helm chart, test projects.
3. Wire the cross-cutting setup the spec names: authentication, tenant resolution, logging and
   tracing, health checks, resilience, persistence base (connections, repositories' base, migrations
   folder), event publishing and consuming.
4. Create the contract surface from CONTRACT.md: the API routes or handlers with request and
   response types, the event types, the data model and its migrations, the public interface of a
   library. A body a component fills in later may be a placeholder now, but every placeholder must be
   gone once the flows are wired: the wiring checks fail on any `NotImplementedException`, empty type
   or interface with no implementation left in the deployable. Use the deployables it depends on
   through their public interfaces; do not rewrite them.
   A `schema` deployable is the module's database: its migrations create every table, view and
   procedure the module's data components use, from their INVENTORY.yaml (legacy DDL and ORM models),
   the legacy DDL folders its `legacy:` lists (under the legacy source) and the target state's data section, which decides names, types and anything it adds (a tenant
   column, row-level security, audit or rollback fields).
5. Add a smoke test per project that proves it starts or loads.
6. Run the build command from the brief and fix what fails.
7. Where the spec leaves something open, choose what the target state implies and write the
   decision into `docs/decisions/<deployable>.md` (one line each).

Never change the spec pack. Write only inside the current folder.
