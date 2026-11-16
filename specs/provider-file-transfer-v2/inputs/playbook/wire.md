---
type: Playbook
title: 'Playbook: wire one business flow end to end'
description: Every component of the new system is built.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 25156e7f9692f53ee6bd3048e485021a0f44ed9908fc82bdb4e74125f7059681
---

# Playbook: wire one business flow end to end

Every component of the new system is built. You make **one business flow** work end to end across
them and prove it with its scenarios: the screen, the BFF route, the API endpoint, the command or
query, the domain logic, the data, the events and any batch follow-up, connected the way the flow's
spec says. You do not rewrite components; you connect them and fill the gaps between them.

Your inputs (paths in the task):

- the flow's spec under `technical/flows/<flow>/`: FLOW.md (the steps), RULES.md and COMPONENTS.md
  (which component owns which rule);
- the flow's scenarios, `business/<flow>.feature`, written in business language for business
  readers: the step definitions you write are where each business step meets the technical entry
  point (the screen action, route or command FLOW.md and the components' contracts name);
- the specs of those components under `technical/components/<id>/` (TARGET.md says where each is);
- the target-state document;
- the new repository (the current folder) with every component built.

## How to work

1. Read FLOW.md's steps and COMPONENTS.md, then each component's TARGET.md and CONTRACT.md.
2. Follow the steps through the code. Where a step crosses from one component to the next (a
   screen calling the BFF, the BFF calling the service, a command publishing an event that a
   projector or batch job consumes), make sure the call, route, registration or subscription exists
   and matches the contracts. Add what is missing: dependency-injection registrations, routes,
   handlers, mappings, subscriptions, configuration.
3. Turn the flow's `.feature` into end-to-end tests: copy it unchanged into the specs test project of the
   deployable that serves the flow's first step and write step definitions that drive the flow
   through its public entry point.
4. Run the build command of every deployable the flow touches and the end-to-end tests; fix what
   fails, in the wiring first, and in a component only when the component disagrees with its own spec.
5. Write what you connected or decided into `docs/decisions/<flow>.md` (one line each).

Never change the spec pack. Write only inside the current folder.
