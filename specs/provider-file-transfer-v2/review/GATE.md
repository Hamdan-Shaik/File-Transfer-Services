# Gate

**Ready for code generation.** Every legacy class is planned, excluded by a fixed rule or decided out of scope.

| Legacy classes | Classes | Lines |
| --- | ---: | ---: |
| planned | 393 | 29597 |

Legacy files: 0 C# files with types the graph never saw, 0 business files with no owner, 0 files of only enums or delegates (carried by the classes that use them).

## Before code generation

Every check passes.

## Deciding what is out of scope

Write `inputs/coverage-decisions.yaml` in the pack. Each entry names one thing and why it is not ported:

```yaml
out_of_scope:
  - path: components/legacy-tools/**
    reason: replaced by the cloud console
  - project: HPP.Example.Reports
    reason: reports move to the reporting platform
  - class: HPP.Example.Security.UserStore
    reason: retired, identity is IRM OIDC
  - component: backend-example-audit
    reason: audit goes to pgAudit and CloudTrail
accepted_links:
  - method: Encryption.Decrypt
    reason: the callers use HPP.Core.Security.Encryption, a package, not this repository's class
```
