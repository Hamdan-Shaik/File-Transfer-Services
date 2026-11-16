---
type: Pack Overview
title: 'provider-file-transfer-v2: flow-based spec pack (stage 1)'
description: Stage 1 lists the module's doors, walks each door's footprint down to stored procedures and tables, separates shared plumbing, and proposes business flows.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T06:21:10Z
cef:
  pack: provider-file-transfer-v2
  body_sha256: fb38b3df27176c2b2217fae3ab89349a751c68a77bf8eb4602f6fd6bb47b3207
---

# provider-file-transfer-v2: flow-based spec pack (stage 1)

Stage 1 lists the module's doors, walks each door's footprint down to stored
procedures and tables, separates shared plumbing, and proposes business flows.
Nothing here is written by AI; everything is read from the code graph.

The pack has two halves, linked rule by rule:

- `business/`: what the system must do, for business reviewers: stage 2 writes one plain-language
  Gherkin file per business flow here (`business/<flow>.feature`), and nothing else.
- `technical/`: what code does it, for developers and code generation. Stage 2 writes each flow's
  specs next to its map (`technical/flows/<flow>/`: FLOW.md, RULES.md, DATA.md, LINKS.md, the review).
  - `technical/doors.yaml`: every door in the module; `technical/doors/<door>.yaml`: what each touches.
  - `technical/flows/<flow>/FOOTPRINT.yaml`: what each proposed flow touches; `TECH.md` next to it: the
    same as a technical map (call chains, classes, data, outside calls, where the graph stops).
  - `technical/classes.yaml`: every class of the module with its tier, methods and what it uses;
    stage 2 cuts the components from it.
  - `technical/ARCHITECTURE.md`: the legacy module's projects, references, packages, entry points, data.
- `review/flow-list.md`: the proposed flows; `review/shared-list.md`: the shared pieces.

Stage 2 (`spec-generate`) has an agent group the doors into business flows, writes each flow's rules
and Gherkin from the code, reviews them, cuts the components, places them in the target
architecture and writes each component's five spec files.
