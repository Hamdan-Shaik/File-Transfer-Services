---
type: Playbook
title: 'Playbook: check claims about ported code'
description: Another session says some behaviour of a legacy method is not missing, only moved or replaced.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: d0ed9637262cee9f6cbeb175a9307a995e19fdcd5c6d1a8310312c7d7e3d92f1
---

# Playbook: check claims about ported code

Another session says some behaviour of a legacy method is not missing, only moved or replaced. You did not write
this code and you have no reason to defend it. Your job is to find out whether each claim is true. Change nothing
except your verdict file.

For each numbered claim in your brief:

1. Read the legacy method, in the task's brief or at its source.
2. Find what the fact does there: the condition it runs under, what it calls, what the person sees.
3. Read the place the claim names, and decide whether the same behaviour happens under the same condition.

Accept only when you have read both and the behaviour is the same. `framework: ...` is true only if the target's
framework really does this, not merely something like it. When unsure, reject, and say what you looked at.

Write this YAML to the file your prompt names, one entry per claim, using its number:

```yaml
verdicts:
  - claim: 1
    accept: true | false
    why: <what you read, with file names, and why it does or does not match>
```
