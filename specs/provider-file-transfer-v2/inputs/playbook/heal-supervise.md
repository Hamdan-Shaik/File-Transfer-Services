---
type: Playbook
title: 'Playbook: find the common cause of a group of failing tasks'
description: Many code-generation tasks failed with the same kind of problem.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 389b2102d09dec73f578a8f32e33b0d70caa4bff909d78c66161821ec1488352
---

# Playbook: find the common cause of a group of failing tasks

Many code-generation tasks failed with the same kind of problem. One cause usually sits behind them, and fixing
it once is better than fixing each task alone. Nobody will answer questions. You change nothing except your
verdict file: your advice is tried on a few of these tasks first, and on the rest only if it works there. Never advise
adding code only to satisfy a check: if the design replaces what a check names, say the check is wrong.

Your brief names the kind of problem, every task in the group, and a few samples in full. For each sample, read
the real things: its problems, its brief with the legacy source (`technical/target/tasks/`), the component's
TARGET.md, and the new code in the current folder. Look for what the samples share.

## Decide

- **The code is wrong the same way:** say exactly what a session must do to fix it. A session reads your advice
  beside one task's own problems, so write it to apply to any task in the group, with what to look for and what
  to change. Name real files and methods as examples.
- **The plan is wrong the same way** (a target file that should not exist or has another name, a layer the target
  state's structure does not have): list the corrections under `layout`, one per class per task.
- **Only a person can decide** (the specs contradict the target state, the behaviour is unclear, the check itself
  looks wrong): set `needs_person: true`, and write what you found and what you would do in `suggested_fix`. It
  goes in the report for the person who reads it after the run.

Never advise weakening a test, leaving a stub, or removing behaviour to get past a check. If logic is missing, the
advice is to port it from the legacy source. If an earlier verdict for this group is in your brief, it did not
work: say something different, or hand it to a person.

## The verdict

Write this YAML to the file your prompt names; leave out what does not apply:

```yaml
diagnosis: <two or three sentences: the shared cause, with the evidence>
advice: <exactly what a session must do; applies to any task in the group>
layout:
  - task: <task id>
    class: <class as that task's brief names it>
    target_files: [<file name>, ...]    # [] when it should have no file of its own
    role: <layer>
needs_person: false
suggested_fix: <for a person: what you would do, and why you could not>
```
