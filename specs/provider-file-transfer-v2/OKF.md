---
type: Pack Format
title: Open Knowledge Format metadata
description: What the OKF metadata, index and log files in this pack mean.
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 29eb2b903bc676333d5f8655981f353f4b4c7c767566d2992bb429ecdd6ccc62
---

# Open Knowledge Format metadata

This flow pack for `provider-file-transfer-v2` is an [Open Knowledge Format](https://github.com/GoogleCloudPlatform/open-knowledge-format) v0.2 bundle. Every Markdown document starts with a YAML block that the tool writes after each stage; a `.feature` file carries the same block as `#` comments (`# ---` ... `# ---`), which Gherkin ignores. The agent sessions never write it, and the body under it is exactly what they wrote.

## Frontmatter

| Key | Meaning in this pack |
|-----|----------------------|
| `type` | Kind of document: `Flow Specification`, `Business Rules`, `Behavior Specification`, `Interface Contract`, ... |
| `title` | The document's own first heading. |
| `description` | One sentence, also used in the `index.md` entries. |
| `resource` | What the document describes: a flow's door class as the code graph's id (`urn:csharp-doc-agent:<qualified name>`), `urn:csharp-doc-agent:component:<module>/<id>` for a component, or `urn:csharp-doc-agent:deployable:<module>/<name>` for a deployable of the new system. |
| `tags` | `flow`/`component`/`deployable`/`architecture`, the door kind or component tier or deployable kind, and the module. |
| `status` | `draft` until a person signs the document off, then `stable`. |
| `verified` | Who signed it off and when (`okf-verify`). |
| `generated` | `by` is the tool version; `at` is when the item was generated, when known. |
| `sources` | Legacy files by their path **relative to the legacy repository root** (`#L` gives the line), and pack files by their path from the pack root (`/business/...`, `/technical/...`). |
| `cef` | Producer extension: the generation item, its status in `review/GENERATION.yaml`; rule, door, table counts and components for flows; tier, classes, owned rules and placement for components; path, kind and components for deployables; and `body_sha256`. |

## Two halves

`business/` holds what the system must do, for business readers: one Gherkin file per business flow (`business/<flow>.feature`), in plain business language, each scenario tagged with the rule it shows (`@rule:F14-031`). `technical/` holds what code does it: the grouping (`flows.yaml`), each flow's specs with its legacy maps (`technical/flows/<flow>/`: FLOW.md, RULES.md, DATA.md, LINKS.md, REVIEW.md, COMPONENTS.md, FOOTPRINT.yaml, TECH.md), the components (five spec files each, linked to the rules they own through `technical/rule-owners.yaml`) and the target architecture (`technical/target/`).

## Status and sign-off

Every document starts as `draft`, whether or not its checks passed (see `review/CHECKS.md` and `cef.generation`). After review, run `csharp-doc-agent okf-verify <pack> <file or folder> --by <name>`. That adds `verified` and makes the document `stable`. The sign-off is tied to the body through `cef.body_sha256`: if the document is regenerated or edited, the next stamp drops `verified` and the document is `draft` again.

## Changes

`review/changes/documents/` holds change documents as received. `review/changes/requests/<id>/` holds one change: `REQUEST.md`, `IMPACT.md` and its own `log.md`. There, `cef.stage` says how far the change has got (`draft`, `impact`, `specs-updated`, `coded`, `verified`).

## Index and log files

Every folder has an `index.md` listing its documents, subfolders and machine-readable files. The root one declares `okf_version: '0.2'`. `log.md` at the root has one entry per run, newest date first.
