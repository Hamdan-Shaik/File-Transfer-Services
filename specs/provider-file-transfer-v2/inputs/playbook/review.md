---
type: Playbook
title: 'Playbook: review a flow''s rules against the code'
description: You are the reviewer of one flow's as-is specification.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: ada1d846d5f341de43c2819d1400f8b2b24c097cdec4d6e439a853c9f11ea19f
---

# Playbook: review a flow's rules against the code

You are the reviewer of one flow's as-is specification. Another session wrote its rules; you
read the **code** of every door and list what the rules miss or get wrong. You do not change the
spec: a writer session closes the gaps you find.

Your input is the brief (path given in the task): the flow's doors, the code they reach, the data
and the outside calls, and the flow's RULES.md and FLOW.md to review.

## How to work

1. Read RULES.md and FLOW.md.
2. Take the doors one by one and read each one's code from the entry point down, including the
   handlers, validators, queries and stored procedures it reaches.
3. Look for behaviour the rules do not state, or state wrongly:
   - a condition or branch with no rule (an `if`, a `switch` case, an early return);
   - a validation, a limit, a default, a calculation, a rounding, an ordering;
   - a permission or role check; a status change; a message, event or e-mail sent;
   - an error answer and when it happens;
   - a rule whose text or values do not match the code it cites.
4. Report only what you confirmed in the code, with the line. Do not report style, naming or
   things the new system will do differently.

## The file: REVIEW.md

A title line, then one entry per finding, in this shape:

```
**GAP-001** <the behaviour the rules miss or get wrong, in plain words, with real values>
- Door: <door id>
- Kind: missing | wrong
- Source: <path>:<line>[-<line>]
- Rule: <the rule id it corrects, for `wrong`; omit for `missing`>
```

IDs count up from GAP-001. If, after reading every door's code, the rules miss nothing, write the
title line and the single sentence `No gaps found.` A checker verifies that every finding names a
door of the flow and cites lines that exist.
