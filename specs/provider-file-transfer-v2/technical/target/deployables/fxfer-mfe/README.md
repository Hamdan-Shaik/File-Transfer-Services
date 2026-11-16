---
type: Deployable Overview
title: 'Deployable: fxfer-mfe'
description: 'Overview of the fxfer-mfe deployable: where it lives, its stack and the flows it serves.'
resource: urn:csharp-doc-agent:deployable:provider-file-transfer-v2/fxfer-mfe
tags:
- deployable
- mfe
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
  at: 2026-11-15T16:53:38Z
sources:
- id: component-backend-data-file-transfer-constants
  resource: /technical/components/backend-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-backend-file-transfer-constants-localization
  resource: /technical/components/backend-file-transfer-constants-localization/SPEC.md
  title: Component placed in it
- id: component-backend-localization-labels
  resource: /technical/components/backend-localization-labels/SPEC.md
  title: Component placed in it
- id: component-shared-data-file-transfer-constants
  resource: /technical/components/shared-data-file-transfer-constants/SPEC.md
  title: Component placed in it
- id: component-ui-configure-source-locations-and-destinations-screens
  resource: /technical/components/ui-configure-source-locations-and-destinations-screens/SPEC.md
  title: Component placed in it
- id: architecture
  resource: /technical/target/architecture.yaml
  title: Target architecture (6 components placed here)
cef:
  pack: provider-file-transfer-v2
  item: deployable:fxfer-mfe
  generation:
    status: done
    attempts: 1
  path: apps/fxfer-mfe
  kind: mfe
  stack: Node.js + Next.js 15 + React 19
  components: 6
  depends_on:
  - fxfer-bff
  body_sha256: d31d69f453aad6ecc705b92519783dd1b405d6f5a2befd2a0c40281b810a1816
---

# Deployable: `fxfer-mfe`

`fxfer-mfe` is the FXFER operator micro-frontend mounted into the Provider Plus shared shell. It provides authenticated screens for FXFER configuration and tracking and calls the FXFER BFF only.

- Deployable kind: `mfe`
- Deployable path (new repository): `apps/fxfer-mfe`
- Stack: Node.js + Next.js 15 (Pages Router) + React 19 + TypeScript
- Build: `npm --prefix apps/fxfer-mfe ci && npm --prefix apps/fxfer-mfe run build`

## Dependencies

- Depends on: `fxfer-bff` (browser-facing API facade)
- Hosted by: Provider Plus shared shell (micro-frontend host)
- Uses (platform services): Platform identity broker (OIDC/JWT), API Gateway (edge), S3 + CloudFront (static hosting)

## Component placement

| Component | Folder (within `apps/fxfer-mfe`) | What it does |
| --- | --- | --- |
| `backend-data-file-transfer-constants` | `src/legacy-constants` | Legacy constant keys and UI/domain string constants used by the operator UI. |
| `shared-data-file-transfer-constants` | `src/legacy-constants` | Shared subset of the legacy constants (module name, protocol codes, etc.). |
| `backend-file-transfer-constants-localization` | `src/legacy-constants/localization` | Legacy localization keys for data lists/entities/messages used by the UI. |
| `backend-localization-labels` | `src/legacy-constants/localization` | Legacy localization label keys (titles/fields/buttons) used by the UI. |
| `ui-configure-source-locations-and-destinations-screens` | `src/screens/configure-source-locations-and-destinations` | Screens for creating/updating transfer configurations (source locations and destinations) and related reference lookups. |
| `ui-manage-ftp-servers-and-credentials-screens` | `src/screens/manage-ftp-servers-and-credentials` | Screens for managing endpoint profiles and credential references (strictly no secret material in the browser). |

## Enterprise-Architecture standards applied

- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `4.1.3 Routing and Navigation` (shell owns top-level routing; deep links supported)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.4.3 Module Federation Configuration` (Module Federation remote exposed for shell integration)
- `strategy/white-papers/engineering/enterprise-micro-frontend-architecture.md` section `5.6.5 Supply Chain Security` (SRI, dependency scanning, CSP posture for CDN-served remotes)

