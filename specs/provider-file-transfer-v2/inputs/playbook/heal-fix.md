---
type: Playbook
title: 'Playbook: fix why a task keeps failing'
description: A task kept failing, and another session worked out why.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 87db1c1e4f1f510e8df8169329c7e135aad56e023ca38c0f234be70bce97166a
---

# Playbook: fix why a task keeps failing

A task kept failing, and another session worked out why. Your brief gives the problems, the diagnosis and what
to do. Correct the cause, not the symptom.

1. Read the diagnosis, then the code it names. For a port task, its brief shows the legacy source: the legacy code
   is the specification.
2. Make the change the diagnosis calls for, in the current folder only.
   - **Code**: add the missing behaviour in full: every branch, call, message and constant. Everything listed under
     "Exceptions refused" must be in the code; another session has already looked and did not accept it as moved.
   - **Environment**: fix the dependency: use a version that exists (never guess one; if the brief does not name
     it, use what the problem reports as real), and let the package manager write its own lockfile.
3. Never leave a stub, never delete a test or weaken it to pass, and do not change the checks. Do not remove working
   code from earlier tasks.
4. Run the deployable's build and tests if you can, and fix what fails.
5. Never change the spec pack or the legacy source.
6. Never add code only to satisfy a check. If TARGET.md or the target state drops or replaces what a problem names,
   leave that problem open and say so. Never add a connection string, key or other secret to code, configuration or
   anything a browser loads, and never add helpers that nothing calls.

When done, reply with one line: DONE.
