# Documents

* [provider-file-transfer-v2: legacy architecture](ARCHITECTURE.md) - The legacy module as the code graph sees it: projects, references, packages, entry points, data, outside calls, and where the graph loses the trail.
* [Brief: group the doors of module provider-file-transfer-v2 into business flows](GROUPING-BRIEF.md) - Instructions the tool gave the writer session for grouping the doors into flows.

# Directories

* [components](components/) - 260 documents
* [flows](flows/) - Each business flow's specs (FLOW, RULES, DATA, LINKS, REVIEW) and its legacy map.
* [target](target/) - The target architecture: deployables and where every component goes.

# Machine-readable artifacts

* [classes.yaml](classes.yaml) - Every class of the module: tier, methods, what it uses, tables, packages, the doors reaching it.
* [components.yaml](components.yaml) - The components: each one's tier, classes, dependencies, flows, doors and build wave.
* [doors.yaml](doors.yaml) - Every entry point (door) of the module, with its proposed flow.
* [flows.yaml](flows.yaml) - The business flows and their doors, chosen in spec-generate's grouping stage.
* [rule-owners.yaml](rule-owners.yaml) - Each business rule and the component that owns it (from where its cited code lives).
