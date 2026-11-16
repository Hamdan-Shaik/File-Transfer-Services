---
type: Playbook
title: 'Playbook: group one module''s doors into business flows'
description: You are deciding **which business flows a legacy module has**.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 5ea92d7f0e30a88bdef4e3d1ee59b0d0f13c98124708bf2b458269236a8ed846
---

# Playbook: group one module's doors into business flows

You are deciding **which business flows a legacy module has**. A door is a place where something
starts: an API action, a WCF operation, an MVC action, a screen, a batch job. A **flow** is one
thing a user or the system does, end to end, in business words: "Submit an enrollment
application", "Review and approve an application", "Nightly revalidation notices". Each flow
later gets its own as-is spec, written by a separate session, so the grouping decides how the
whole module is described.

Your input is **GROUPING-BRIEF.md** (path in the task). It lists every door of the module by class,
with what the code graph saw each door reach (tables, procedures, where its walk stopped), and the
tool's own proposal, which groups doors only by shared tables. That proposal is a starting point:
where the graph could not see a door's data, it left the door alone in a flow of its own.

## How to work

1. Read the brief. Group by what the doors **do for the business**, not by class or file.
2. **Work top-down.** First write down the module's business processes, the way a business
   reviewer would list them (for an order system, for example: register a customer, place an
   order, change or cancel it, approve it, ship it, invoice it, handle returns, nightly reports).
   The brief gives the number of flows to expect. Then put
   every door in the process it serves. **Never make one flow per operation:** a list of
   operations renamed as flows is not a grouping, and the tool rejects a grouping where most flows
   have a single door.
3. Decide from names, request and response types, and the tables listed. Where they do not decide,
   read the code: the door method, what it calls, the screen or client that calls it (an MVC
   controller calling a service's client proxy tells you which operations serve one screen).
   Read only what you need; do not survey the whole repository.
4. Put doors together when they serve the same business action: the operations behind one screen
   or one use case (search, view, save, submit of the same thing, and every edit panel of one
   form), a command and the operations that complete it, a batch job and the operations only it
   uses. Keep apart doors that serve different actions even when they touch the same tables.
5. Aim for flows a business reviewer recognises: usually 5 to 30 doors each. A flow of 1 door is
   fine when that door really is a business action on its own, which is rare. Split a flow that
   mixes unrelated processes; never make one flow for a whole module.
6. When the same operation exists in two generations of the code, or in a service and the
   controller in front of it (the brief lists operation names found in several classes), put
   those doors in the **same** flow; its spec describes the behaviour once and names every door.
7. Set aside, with a reason, doors that are not business behaviour: health checks, pings, version
   or metadata endpoints, test hooks, framework plumbing.
8. Every door id in the brief appears **exactly once**: in one flow or under `not_flows`.

## The file

### technical/flows.yaml (machine-read; exactly this shape)

```yaml
flows:
  - id: <kebab-case id, e.g. submit-enrollment-application>
    name: <business name, e.g. Submit an enrollment application>
    doors: [<door ids>]
not_flows:
  - door: <door id>
    reason: <why it is not business behaviour>
```

- `id`: lowercase letters, digits and hyphens, at most 60 characters, unique. It becomes the flow's
  folder name, so name the business action, not the class.
- `name`: the flow's business name, as a reviewer would say it.
- Use the door ids exactly as the brief writes them.

The tool checks the file (every door placed once, ids valid and unique, every flow named and with
doors, every set-aside door with a reason, and not most flows a single door) and builds the flow
folders from it.

Write only `technical/flows.yaml`. Never change the source repository.
