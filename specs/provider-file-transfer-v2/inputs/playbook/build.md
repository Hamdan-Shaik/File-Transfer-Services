---
type: Playbook
title: 'Playbook: build one target-state requirement'
description: 'You are adding something the **target state asks for and the legacy code never did**: a rollback of a published change, row-level tenant isolation, token validation, retries.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 966218e34966c28720dd36cf415ef1c23d68a128fe4bd2bf5a931259d92edce1
---

# Playbook: build one target-state requirement

You are adding something the **target state asks for and the legacy code never did**: a rollback of a
published change, row-level tenant isolation, token validation, retries. There is no legacy code to
port, so the target-state document and the component's spec are the specification. You work unattended:
nobody answers questions.

Your inputs (paths in the task):

- the requirement: its id (T-001), its sentence, and the target-state section it comes from;
- the target-state document (`inputs/target-state.md`): read that section completely, and the sections
  it refers to;
- the component's spec under `technical/components/<id>/`, when the requirement belongs to one: SPEC.md
  specifies it under its id, TARGET.md and CONTRACT.md give names and places;
- the new repository (the current folder), with the skeletons and the ported components.

## How to build it

1. Read the section and the spec, then the code it extends: the component's ported classes and the
   deployable's cross-cutting setup.
2. Build exactly what the section asks for, inside the deployable's existing structure. Extend the
   classes that are there; add a class only where the structure has no place for it. Do not rewrite
   ported logic.
3. Put the requirement id in a comment where it is implemented (`// T-004: ...`), and write a test for
   each thing the section asks for, named with the id (`T_004_restores_the_state_before_publish`). A
   test must fail if the behaviour is missing.
4. Leave no placeholder: no `NotImplementedException`, no empty type, no interface without an
   implementation. If the section cannot be built as written (it needs a service that does not exist,
   or contradicts another section), build what you can and write the gap, with the section, in
   `docs/decisions/<deployable>.md`.
5. Run the build and the tests from the task and fix what fails.

Never change the spec pack. Write only inside the current folder.
