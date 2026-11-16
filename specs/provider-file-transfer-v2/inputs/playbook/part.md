---
type: Playbook
title: 'Playbook: the rules of one part of a flow'
description: A big flow is written in parts.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: e4292510eb4a1e575aaf35d992b7ef47b6b3942ce9d4f87e6abffc56b2e9a8d3
---

# Playbook: the rules of one part of a flow

A big flow is written in parts. You write the **rules of one part**: what the legacy code does,
in business words, every rule traced to the line that implements it. The tool merges the parts'
rules into the flow's RULES.md and renumbers them; another session writes the rest of the flow.

Your input is the brief (path given in the task): the part's doors, the code they reach, the
stored procedures, tables, shared pieces and outside calls, and the exact output file.

## How to work

1. Read the brief completely.
2. Take the doors **one by one**. For each, read its code from the entry point down: what it
   accepts, what it checks, what it reads and changes, what it calls, what it answers, and what
   goes wrong. Follow the calls past the points where the brief says the graph stopped. Read the
   stored procedure and query bodies.
3. Write down every **rule**: a condition, a calculation, a limit, a default, an ordering, a
   permission, an error answer, a side effect, a state change, a message sent. Check each against
   the code. Never describe what the code "probably" does: if you cannot confirm it, leave it out.
4. Write only the file the brief names. Leave the other parts' doors to them.

## The file: RULES.md

A title line, then the rules grouped under plain headings. Every rule has exactly this shape:

```
**<PREFIX>-007** <the rule in one or two plain sentences, business words, real values>
- Doors: <door id>[, <door id>]
- Source: <path>:<line>[-<line>][, more citations]
- Legacy: <what the code does today, concretely>
- Decision: keep
```

- IDs use the prefix from the brief (P1, P2, ...) and count up from 001 without gaps.
- `Doors:` names the door ids whose requests the rule applies to. **Every door of the part needs
  at least one rule of its own**: a rule naming it among at most 3 doors. Most doors need several.
- `Source:` cites **real lines** by their full path relative to the source root (no label table
  in a part), and line numbers that exist. Cite the line where the rule is decided.
- `Decision:` is `keep` unless the new stack forces a change (`change`, say why) or the rule is
  dead code (`drop`, say why).
- Add `- ⚑ Flag: <reason>` when a business person should look: surprising behaviour, a likely
  legacy bug, an edge case that matters. Describe it; do not fix it.

A checker verifies the shape, that every cited line exists and that every door of the part has a
rule of its own.
