---
type: Playbook
title: 'Playbook: as-is spec for one flow'
description: You are writing the **as-is specification** of one business flow of a legacy system that is being moved to a new technology stack.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 385c8e1692bd22eb822e10047f993f070b821fa2696d762ee4fb986f58b03049
---

# Playbook: as-is spec for one flow

You are writing the **as-is specification** of one business flow of a legacy system that is being
moved to a new technology stack. The behaviour stays exactly the same; only the stack changes.
The spec says what the legacy code does today, in business words, with every rule traced to the
line of code that implements it. People who do not read code (business SMEs) review it: they
read only the flow's scenarios (the `.feature` file), so that file must be plain business language.

Your input is the flow's **BRIEF.md** (path given in the task). It lists the flow's doors (where
the flow starts), the code to read, the stored procedures and tables it reaches, the shared
pieces it uses (treat those as given; do not describe their insides), the calls it makes into
code the system does not own, and the exact output files.

## How to work

1. Read BRIEF.md completely.
2. Read the code it lists, starting at each door and following the calls. Read the stored
   procedure bodies: most data rules live there. When a procedure is defined more than once
   (install script plus upgrade scripts), the latest definition wins; say which one you used.
3. Write down every **rule**: a condition, a calculation, a limit, a default, an ordering, a
   permission, an error answer, a side effect. Check each against the code before writing it.
   Never describe what the code "probably" does: if you cannot confirm it, leave it out.
   Work **door by door**: every door has behaviour of its own (what it accepts, checks, changes
   and answers), so every door gets rules of its own. A door is never covered by a sentence about
   the flow as a whole.
4. Write the five files below, at the paths the brief gives: four in the flow's technical folder,
   and the scenarios in `business/`. Never change anything in the source repository.

## The files

### FLOW.md: what the flow is

```
# <Business name of the flow>

<Two or three sentences a business person understands: who does what, and what they get.>

## Who starts it
## Doors
| Door | Request | Answers |          one row per door id from the brief, with its endpoint if any
## Steps
1. ... (RULE-ID, RULE-ID)                every step cites the rules it applies
## Outcome
<what the caller gets back, with a realistic example using real values from the code>
## Errors
<what goes wrong and what the caller sees>
```

Every door id listed in the brief must appear in FLOW.md.

### RULES.md: the rules, each traced to code

Start with a table of the source files you cite, each with a short label, then the rules,
grouped under plain headings. Every rule has exactly this shape:

```
**<PREFIX>-007** <the rule in one or two plain sentences, business words, real values>
- Doors: <door id>[, <door id>]
- Source: <LABEL or path>:<line>[-<line>][, more citations]
- Legacy: <what the code does today, concretely>
- Decision: keep
```

- IDs use the prefix from the brief and count up from 001 without gaps.
- `Doors:` names the door ids (from the brief) whose requests this rule applies to. **Every door
  needs at least one rule of its own**: a rule whose `Doors:` line names it among at most 3
  doors. A rule shared by many doors (paging, permissions) may name them all, but it does not
  count as any door's own rule.
- `Source:` cites **real lines**: a path relative to the source root (or a label from your
  table) and line numbers that exist in that file. Cite the line where the rule is decided,
  not the method header.
- `Decision:` is `keep` unless the new stack forces a change (`change`, say why) or the rule
  is dead code (`drop`, say why). Behaviour is not being redesigned.
- Add `- ⚑ Flag: <reason>` when a business person should look: surprising behaviour, a likely
  legacy bug, an edge case that matters (an empty page instead of an error, a count that
  includes archived items...). Do not "fix" it: describe it.

### DATA.md: the data it touches

Tables (read for / written for / key columns), stored procedures (called for / where defined:
file and line), and the shape of each answer (field, meaning, rule). Every table and stored
procedure named in the brief must appear here, by name.

### LINKS.md: what it depends on

- **Shared pieces used**: from the brief; one line each on what this flow uses them for.
- **Related flows**: flows of the same module that change data this flow reads (or the
  reverse), and why that matters.
- **Contracts**: every call into code the system does not own (from the brief's external
  calls): the library or service, the method, the arguments as passed (constants as their
  values), and what this flow relies on it returning. These calls must keep working exactly
  the same in the new system. Every package named in the brief's external calls must appear
  here. If there are none, write "None".

### <flow-id>.feature: the behaviour as scenarios, for business readers

This file goes in `business/` (its path is in the brief). It is the **only business document** of
the flow: business people read it on its own, without FLOW.md or RULES.md, so it must read as plain
business language from the first line to the last. Say what a person or another organisation does
and what they see, with real values ("a provider with 3 unread messages"), never how the code does
it ("IsRead is false").

```
Feature: <business name>
  <Two or three sentences: who does what, and what they get.>

  @rule:<PREFIX>-001 @keep
  Scenario: <what happens, in business words>
    Given ...
    When ...
    Then ...
```

- Every rule whose decision is `keep` or `change` has at least one scenario tagged
  `@rule:<ID>` (a scenario may carry several tags). `drop` rules have none. Tag each scenario
  `@keep`, `@change` or `@flag` to match its rule. Only tag rule IDs that exist in RULES.md.
- Use Scenario Outline with Examples for limits, paging and boundaries.
- **No technical words anywhere in the text** (the tags are the only ids allowed): no API,
  endpoint, WCF, HTTP, status codes (200, 400, 404), GET/POST/PUT, URLs or paths, query, command,
  handler, controller, service, routed, model, payload, DTO, database, table, column, stored
  procedure, null, boolean, GUID, exception, token, anti-forgery, cache, log levels, exit codes,
  file names, code names (`NpiNumber`, `ChangeEmailNotification`, `NPI_REQUIRED`), or backticks.
  A checker rejects any line that uses them.
- **Every rule still gets a scenario, even a technical one.** Describe what it means for the
  people involved, not the mechanism:

  | The rule says | Write instead |
  | --- | --- |
  | A WCF caller / an API caller posts a query | "staff search ...", "another system asks for ..." |
  | The caller receives HTTP 200 with the routed result | "they see the matching applications" |
  | HTTP 400 with the validation result | "the provider is told what is missing" |
  | HTTP 404 with body `N` | "no border-city answer is given" |
  | Requires an anti-forgery token / HTTP PUT | "a change only counts when it comes from the provider's own signed-in session" |
  | Returns null / an empty string | "no allowed address types are offered" |
  | Throws "DeleteAuditTempFailed" | "the submission stops and the provider is told it could not be completed" |
  | Logged at debug level | "the problem is recorded for the support team" |
  | The exit code becomes the process exit code | "the operations team is told whether the run succeeded" |
  | Field "StatesID" / "ProviderName" | "the state", "the provider's name" |
  | Status "ReturnToProvider" | "returned to the provider" |
  | A GUID audit-trail ID | "an audit entry", or a business reference such as the tracking number |

  For example:

  ```
  @rule:F02-054 @keep
  Scenario: An address check only counts when it comes from the provider's own session
    Given a provider is signed in and editing their addresses
    When an address check arrives that did not come from the provider's own session
    Then the address is not checked
  ```

## When RULES.md is already written

A big flow is written in parts: one session per part wrote that part's rules, and the tool merged
them into RULES.md before your session. The brief says so and gives the range of ids. Keep every
rule and its id; correct a rule only where the code says otherwise; add rules only for behaviour
no part covers (steps that cross parts), numbered after the last. Then write the other four files
from these rules.

## Before you finish

Re-read RULES.md against the code once more and fix any citation you are not sure of. A
checker will verify that every cited line exists, that every door has a rule of its own, that
every rule has a scenario, that the scenarios use no technical words and the feature has a business
summary, and that every door, table, procedure and external package from the brief is covered.
