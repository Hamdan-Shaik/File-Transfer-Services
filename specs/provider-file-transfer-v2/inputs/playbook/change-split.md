---
type: Playbook
title: Split a change document into requests
description: A person gave you a document saying what is missing from the specs or what the system should do.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 5db9320b88c42e85065c9073ab5864fe2e05fea9ef900cd3f9c26db19e8d4eb1
---

# Split a change document into requests

A person gave you a document saying what is missing from the specs or what the system should do. Turn it into one
request per change, so each change gets its own review and can be undone on its own.

- Two things that could be approved, rejected or undone separately are two requests, even if they sit in one sentence.
- Keep the document's meaning. Do not add requirements, and do not fix or improve what it asks for. If something is
  unclear, ask in `questions`: a person answers it before the request goes on.
- Write for a business reader: no class names, tables or endpoints.
- Every part of the document must end up in a request or in `not_changes`, with the reason (a heading, background,
  something already decided against).
- Use a flow id from the list below. Use `unknown` if you can't tell, and `new` only if no existing flow could hold it.
- Do not edit any spec, and do not write REQUEST.md yourself. The tool creates the requests from your answer.

## Your answer

Write this YAML to the path above. Use block scalars (`|`) for text.

```yaml
requests:
  - name: cancel-notice              # short, lower case with hyphens; it becomes part of the request's id
    title: Notify provider on cancel # one line, in business words
    flow: cancel-enrollment          # a flow id, unknown or new
    parts: [P1, P3]                  # the document parts this came from
    what: |
      What is wrong or missing today.
    desired: |
      What the system should do instead.
    scenarios: |
      Scenario: Provider is told about the cancellation
        Given an active enrollment
        When the enrollment is cancelled
        Then the provider receives a notice within one day
    out_of_scope: |
      What this request does not cover, or None.
    questions: []                    # anything a person must answer first
    severity: medium                 # optional: only if the document states one
not_changes:
  - part: P2
    why: Background only; it asks for nothing.
```
