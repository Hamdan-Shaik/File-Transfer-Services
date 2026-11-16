---
type: Playbook
title: 'Playbook: split a big flow into parts'
description: 'A flow with many doors is too much for one session to specify: it ends up summarising.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 2dced9b3219060a8fdda59eec28b4d87adbf63e3f7598ec9d1341eecaf9db67f
---

# Playbook: split a big flow into parts

A flow with many doors is too much for one session to specify: it ends up summarising. You split
it into **parts**, each a sub-feature a business person would recognise, so that one session per
part can read every door's code and write its rules in full.

Your input is the brief (path given in the task): the flow's doors with their kind, name, class,
file and endpoints, and the most doors a part may hold. What each door reaches is in
`technical/doors/<door id>.yaml`; the flow's technical map is the flow's `TECH.md`.

## How to work

1. Read the brief. Look at the door names, their classes and endpoints, and where unclear the code
   or the door's yaml file.
2. Name the flow's sub-features first (for a search flow: by provider, by status, saved searches,
   export of results...), then put each door in the one it serves. Doors that work on the same
   screen, the same record or the same step belong together; the same operation in two classes
   (a service and the controller in front of it, an old and a new version) belongs in one part.
3. Respect the limit: no part holds more doors than the brief allows. Every door is in exactly one
   part. Two parts at least.

## The file: PARTS.yaml

Write exactly the file the brief names, in this shape:

```yaml
parts:
  - id: search-by-provider          # kebab-case, at most 40 characters, unique
    name: Search applications by provider identifiers
    doors: [door-id-1, door-id-2]
  - id: export-results
    name: Export search results
    doors: [door-id-3]
```

Use the door ids exactly as the brief lists them. A checker verifies that every door is in exactly
one part, that no part is over the limit and that the ids are valid.
