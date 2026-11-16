---
type: Playbook
title: 'Playbook: spec of one deployable'
description: The target architecture (technical/target/architecture.yaml, technical/target/TARGET-ARCHITECTURE.md) lists the deployables of the new system and where every component goes.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 4c15b2294d3a38c2eb169a97e57f0bf1b49aa12a354ebaf3e29517cd46bf79d8
---

# Playbook: spec of one deployable

The target architecture (`technical/target/architecture.yaml`, `technical/target/TARGET-ARCHITECTURE.md`)
lists the deployables of the new system and where every component goes. You write the spec of **one** of them: its structure and its
contracts. This spec is the blueprint its skeleton is generated from, before any flow's behaviour
is added, so it must say everything a developer needs to set it up the way the target state wants.

Your input is **DEPLOYABLE-BRIEF.md** (path in the task): the deployable's entry in the
architecture, the components placed in it, the deployables it depends on, and the target-state
document.

## How to work

1. Read the target-state document (stack, folder structure, the inside of a service, API and event
   standards, data model, security, observability, resiliency) and the architecture.
2. Read the components placed in it (`technical/components/<id>/INVENTORY.yaml`) and the business
   flows they serve, enough to know its operations, its data and its events.
3. Write the three files below. Follow the target state's conventions exactly: project names,
   folders, frameworks, libraries, API versioning, error format, tenancy, health checks.

## The files

- **README.md**: what the deployable is (two sentences), its path, its stack, what it depends on
  and who depends on it, and a table of the components placed in it (component, folder, what it does).
- **STRUCTURE.md**: the skeleton to create at its path: every project or package and folder, the
  main files (solution or project files, entry point, configuration, Dockerfile, Helm chart, test
  projects), and the cross-cutting setup the target state requires (authentication, tenant
  resolution, logging and tracing, health checks, resilience, persistence base, event publishing),
  each with the library or pattern to use.
- **CONTRACT.md**: everything other deployables rely on. For an API: every endpoint (method, path
  in the target's versioned style, request and response fields with types, status codes, error
  format). For events: every event it publishes or consumes, with its payload. For a library: its
  public interface. For data: the tables or stores it owns (write and read side), with columns.
  Endpoints and events follow the target state's naming; the legacy behaviour behind them comes
  from the business flows' specs.

Every file must state the deployable's path exactly as the brief gives it (README.md and
STRUCTURE.md at least). Write only inside the deployable's folder under
`technical/target/deployables/`.
Never change the source repository.
