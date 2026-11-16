# Documents

* [Playbook: target architecture for one module](architecture.md) - The module's behaviour is specified per business flow: the specs in technical/flows/<flow>/ (FLOW.md, RULES.md) and the scenarios in business/<flow>.feature.
* [Playbook: build one target-state requirement](build.md) - You are adding something the **target state asks for and the legacy code never did**: a rollback of a published change, row-level tenant isolation, token validation, retries.
* [Change the code for a request](change-code.md) - The specs have been reviewed and merged.
* [Work out what a request would change](change-impact.md) - Nothing changes in this step.
* [Review a change PR](change-review.md) - You are helping a person review this PR.
* [Update the specs for a request](change-specs.md) - The impact has been written.
* [Split a change document into requests](change-split.md) - A person gave you a document saying what is missing from the specs or what the system should do.
* [Playbook: the five spec files of one component](component.md) - A component is a group of the legacy module's classes (one tier: ui, backend, data or shared) that the new system rebuilds in one place.
* [Playbook: spec of one deployable](deployable.md) - The target architecture (technical/target/architecture.yaml, technical/target/TARGET-ARCHITECTURE.md) lists the deployables of the new system and where every component goes.
* [Playbook: answer a review of a flow's rules](fix.md) - A reviewer read the code of a flow against its rules and listed what they miss or get wrong in REVIEW.md.
* [Playbook: as-is spec for one flow](flow.md) - You are writing the **as-is specification** of one business flow of a legacy system that is being moved to a new technology stack.
* [Playbook: group one module's doors into business flows](group.md) - You are deciding **which business flows a legacy module has**.
* [Playbook: fix why a task keeps failing](heal-fix.md) - A task kept failing, and another session worked out why.
* [Playbook: find the common cause of a group of failing tasks](heal-supervise.md) - Many code-generation tasks failed with the same kind of problem.
* [Playbook: find out why a task keeps failing](heal-triage.md) - A code-generation task failed its checks several times.
* [Playbook: check claims about ported code](heal-verify.md) - Another session says some behaviour of a legacy method is not missing, only moved or replaced.
* [Playbook: the rules of one part of a flow](part.md) - A big flow is written in parts.
* [Playbook: split a big flow into parts](parts.md) - A flow with many doors is too much for one session to specify: it ends up summarising.
* [Playbook: port one part of a component](port.md) - You are **porting legacy code** into a new system, unattended: nobody answers questions.
* [Playbook: review a flow's rules against the code](review.md) - You are the reviewer of one flow's as-is specification.
* [Playbook: skeleton of one deployable](scaffold.md) - You are creating the **skeleton** of one deployable of a new system: everything it needs to build, start and be tested, but no business logic yet.
* [Playbook: wire one business flow end to end](wire.md) - Every component of the new system is built.
