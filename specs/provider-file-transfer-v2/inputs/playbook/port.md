---
type: Playbook
title: 'Playbook: port one part of a component'
description: 'You are **porting legacy code** into a new system, unattended: nobody answers questions.'
tags:
- flow-pack
- provider-file-transfer-v2
status: draft
generated:
  by: csharp-doc-agent/0.1.0
cef:
  pack: provider-file-transfer-v2
  body_sha256: 7c002421f79f51065e1568d01a44a09bb67a27b3279e9d8e33aa5ff35a19f59e
---

# Playbook: port one part of a component

You are **porting legacy code** into a new system, unattended: nobody answers questions. Your brief lists a
slice of one component of the legacy system, at most a few dozen methods, and shows you **their actual
source code**. Translate each of them into the target, method by method, with its full logic. You are
not re-imagining the feature from a description: the legacy code is the specification, and depth is the
point. The behaviour stays exactly the same; only the technology and the structure change.

Your inputs (paths in the brief):

- the brief: for each legacy class, its role in the target (api, application, domain, infrastructure,
  ui, batch), the target file(s) it goes into, and for each method its legacy source, the target method
  TARGET.md maps it to, and the business rules whose code it holds;
- the component's spec under `technical/components/<id>/`: TARGET.md (where things go, names,
  signatures), SPEC.md (what each method does and why), CONTRACT.md (what it offers and uses);
- the CONTRACT.md of each component it uses (already built in the repository);
- the target-state document (`inputs/target-state.md`), above all its folder structure and the inside of
  a service;
- the new repository (the current folder), with the skeletons and the parts and components built so far.

## How to port

1. Read the brief completely, then TARGET.md and CONTRACT.md for names, signatures and where the
   component lives.
2. **Put each class where its role belongs** in the target-state structure: a controller or service
   operation in the API layer, a handler, service or validator in the application layer, an entity,
   model, rule or event in the domain layer, a repository, a stored-procedure call or a client for an
   outside system in the infrastructure layer, screens in the front end, jobs in the batch project.
   **One public type per file, the file named after the type.** Keep the legacy classes apart as they
   were: never put several of them in one file, and merge two only where TARGET.md says so.
3. **Translate every listed method, line by line in meaning**:
   - keep every branch, condition, loop, guard, default value, ordering, calculation, message text,
     constant and limit;
   - keep every call: a call into code of this module becomes a call to its ported counterpart; a call
     into code the module does not own (a framework, a platform service, a package such as a rules
     engine) goes through the port, adapter or client the target state gives for it, with the same
     arguments;
   - keep every data access: the same tables, columns, stored procedures, filters and order, through the
     target's data layer;
   - change only what the target technology forces: the web framework, dependency injection instead of
     static singletons, async calls, the logging and configuration APIs, the error format.
   A method TARGET.md marks `drop:` is not ported; say why in a comment where its caller is.
   A screen is ported to the front end the target state gives, not translated control by control: what
   the person sees and can do stays (every message and confirmation with its exact text, what each selection
   refreshes, what each button validates and saves), while how the old toolkit showed it may change. Logic
   TARGET.md maps behind an API goes there.
4. Put the rule id in a comment where each rule is implemented (`// F14-031: ...`), and write a unit test
   for each rule, named with its id (`F14_031_rejects_an_empty_npi`), that checks the legacy behaviour
   with the real values. Test the branches of the methods you ported.
5. **Never leave a stub**: no `NotImplementedException`, no `TODO`, no empty body, no placeholder value.
   If something cannot be ported as is, port the closest faithful version and explain the gap in one
   comment line.
6. If earlier parts of this component already created a class, add your methods to it; do not rewrite
   or remove what they wrote.
7. Run the build command and the test command from the brief, and fix what fails.
8. Never change the spec pack or the legacy source.
9. Never add code only to satisfy a check. If TARGET.md or the target state drops or replaces something, such as a
   desktop screen, a local file, a local log file, raw error text, or settings read from a local file, follow the design,
   even if a check then reports it. A person reads what stays open. Never put a connection string, key or other secret in
   code, in configuration or in anything a browser loads.

## When you finish

A checker reads the result: every listed method has its ported target method with a real body; each
body keeps its legacy method's calls, messages and constants; every target file exists in its role's
place and holds one public type named after it; every rule is cited; nothing is a stub; the deployable
builds and its tests pass. Reply with one line: DONE.
