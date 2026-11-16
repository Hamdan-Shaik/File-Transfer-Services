---
type: Playbook
title: Change the code for a request
description: The specs have been reviewed and merged.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 2f71c8f26ed3583291b3e52228cd29617194bf79d35c5abdfb41c398a7aed7ea
---

# Change the code for a request

The specs have been reviewed and merged. Now change the existing code to match them.

- Edit the code that is there. Do not regenerate or rewrite files, because earlier fixes live in them.
- Implement exactly the rules in this brief, with their full behaviour. Read the component's `SPEC.md`, `TARGET.md` and
  `CONTRACT.md` for names and places.
- The specs are read-only in this step. If a spec looks wrong, stop and tell the developer: the fix is a new request.
- Put each rule's id in a comment where it is implemented, and in the name of the test that proves it.
- Add a test for every rule, following the deployable's existing tests. Use the scenarios in this brief.
- Leave no stubs (`NotImplementedException`, `TODO`, `not implemented`).
- If you need a package, use a version that exists. Do not guess one.
- Do not run git commands. `change-pr` commits later.

Touching a file the brief doesn't list is allowed when the change needs it. The check lists those files in the PR
description, so a reviewer can look at them.

## When you are done

Run the check the command printed. It builds the deployable and runs its tests. If it reports problems, fix them and run it
again. If a build or test fails only because it can't reach the network, ask the developer to allow network access rather
than changing the code.
