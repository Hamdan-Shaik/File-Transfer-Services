---
type: Playbook
title: Work out what a request would change
description: Nothing changes in this step.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: afc608d3da8a5099c28338edcf073508c6fe51bd2d580b438e1e64040fd07d73
---

# Work out what a request would change

Nothing changes in this step. Your answer tells a reviewer exactly which rules would change, and the tool works out the
components, files and severity from it.

1. Read the request, then the rules and scenarios of its flow. If the flow is `unknown`, find it from the flow list.
2. Read the legacy code behind the behaviour. The citations in `RULES.md` are relative to the legacy source.
3. Compare the three: what the spec says, what the legacy code does and what the request wants.

Then decide the outcome:

| Outcome | When |
|---|---|
| `proceed` | The change is clear and fits. |
| `already-covered` | The specs already say this. Name the rules in `why`. |
| `conflict` | It contradicts the target state or another rule. Say which in `why`. |
| `needs-answers` | A person must decide something first. Put it in `questions`. |

For each rule that would change, say what the legacy code does:

| `legacy` | Meaning | `source` |
|---|---|---|
| `same` | The legacy code already does this; the spec missed it. | Required: `file:line` |
| `absent` | The legacy code does not do this; it is new. | Leave empty |
| `different` | The legacy code does something else. A person will decide. | Required: `file:line` |

Rules for your answer:

- `change` and `drop` must name a rule that exists. Do not make up ids. For `add`, leave `id` out: the tool picks it.
- Name the smallest set of rules that delivers the request. List close calls under `checked`.
- `add` needs an `owner`: the component that will implement it, from the list in the brief. Give `doors` when an existing
  entry point reaches the behaviour, or leave it empty for something no legacy door reaches.
- Set `wording_only: true` only if the behaviour stays exactly the same.
- List in `extra_spec_files` any other spec file that must change: `DATA.md` for a table or column, `openapi.yaml` or
  `CONTRACT.md` for an API, `FLOW.md`, `LINKS.md`, `TARGET.md`. Be honest: a file you leave out cannot be edited later,
  and these files raise the severity because other people depend on them.
- Set `new_entry: true` if this needs a new screen, API or job. Use the flow `new` only if no existing flow could hold it.
- Set each `judgement` flag truthfully. They raise the severity, and they are the one part of it only you can see.

## Your answer

Write this YAML to the path above. Use block scalars (`|`) for text.

```yaml
outcome: proceed
why: ""                      # required for already-covered and conflict
summary: |
  One or two sentences a reviewer reads first.
rules:
  - id: F14-007
    action: change           # add, change or drop
    flow: cancel-enrollment
    now: Sets the status only.
    becomes: Sets the status and queues a notice to the provider.
    legacy: absent           # same, absent or different
    source: ""               # file:line when legacy is same or different
    wording_only: false
  - action: add
    flow: cancel-enrollment
    owner: notification-core
    doors: []
    becomes: The provider receives a notice within one day of the cancellation.
    legacy: absent
scenarios: |
  Scenario: Provider is told about the cancellation
    Given an active enrollment
    When the enrollment is cancelled
    Then the provider receives a notice within one day
extra_spec_files: []         # such as technical/flows/cancel-enrollment/DATA.md
new_entry: false
judgement:
  money_or_eligibility: false
  security: false
  other_module: false
checked:
  - rule: F14-008
    why: Same screen, but the status it shows does not change.
questions: []
```
