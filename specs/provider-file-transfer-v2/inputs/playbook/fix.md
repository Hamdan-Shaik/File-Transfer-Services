---
type: Playbook
title: 'Playbook: answer a review of a flow''s rules'
description: A reviewer read the code of a flow against its rules and listed what they miss or get wrong in REVIEW.md.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 3741d589da3cc0a5e9b4572c59a19bbe3c50dc3327377e225f2e286416a232c7
---

# Playbook: answer a review of a flow's rules

A reviewer read the code of a flow against its rules and listed what they miss or get wrong in
REVIEW.md. You close every finding in the flow's spec.

Your input is the brief (path given in the task): the review, the flow's own brief (doors, code,
data) and the rule-id prefix with the next free number.

## How to work

1. Read REVIEW.md, then the flow's RULES.md, FLOW.md and feature file.
2. For each finding, read the cited code yourself and confirm it.
   - **missing**: add a rule in the shape of `inputs/playbook/flow.md` (with its `- Doors:` line),
     numbered from the next free id, under the right heading.
   - **wrong**: correct the rule it names, keeping its id.
   - Give the rule you added or corrected a line `- Review: GAP-NNN` naming the finding.
   - If the code shows the finding is not right, still answer it: add `- Review: GAP-NNN` to the
     rule it is about, with one sentence saying why the rule stands.
3. Add or update the scenarios in the feature file (`business/<flow>.feature`, plain business
   language with no technical words, as `flow.md` describes) for every rule you added or changed (tagged
   `@rule:<id>`), and the steps of FLOW.md that apply them.
4. Keep every other rule and its id. Change only the files the brief lists.

A checker verifies every finding is answered by a `- Review:` line and runs the flow's checks again.
