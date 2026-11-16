---
type: Playbook
title: Update the specs for a request
description: The impact has been written.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 7e6976ade5cd73a2491c5cc152814405319a6e9d8dcc8c9735af079b7d08dcbf
---

# Update the specs for a request

The impact has been written. Now make the specs say what it says, and nothing more.

- Edit only the files listed in this brief, and in `RULES.md` only the rules listed. The check compares every other rule
  with the last commit.
- Edit the body only. Leave the metadata block at the top of each file exactly as it is, and do not stamp, regenerate or
  reformat anything. The tool brings metadata up to date separately.
- Do not run git commands. `change-pr` commits later.
- Do not write code.
- Save files as UTF-8 without a byte-order mark, and keep their line endings.

## RULES.md

Write each rule exactly as this brief shows it. Rules start on their own line as `**F14-031** sentence`, followed by their
`- ` lines. Add new rules after the flow's last rule. Never renumber or delete a rule: to retire one, set
`- Decision: drop`.

## Scenarios (`business/<flow>.feature`)

- Add or change the scenarios in the brief. Tag each with the rules it shows: `@rule:F14-031`.
- Every rule that is not dropped needs a scenario.
- Business language only. A check rejects technical words and code names.
- A check also rejects scenarios tagged with a rule that does not exist.

## Component specs

For each rule's owner, in `technical/components/<owner>/`:

- `SPEC.md`: name the rule by its full id under what the component implements, and describe the behaviour where the method
  it belongs to is described. Write out every id: a range such as `F14-001..004` hides what is missing.
- `TESTS.md`: add the test that proves the rule, with the rule id.
- `CONTRACT.md`, `openapi.yaml`, `TARGET.md`, `DATA.md`: only if the brief lists them.

## When you are done

Run the check the command printed. If it reports problems, fix them and run it again. If it says the specs moved or the
impact does not name a file, stop and tell the developer rather than working around it.
