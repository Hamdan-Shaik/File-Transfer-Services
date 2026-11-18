# Playbook: find out why a task keeps failing

A code-generation task failed its checks several times. Retrying with the same instructions did not help, so
find the cause before anyone tries again. You change nothing except your verdict file. Your brief lists the
problems and where everything is. Read the real things: the problems, the task's brief with the legacy source,
the component's TARGET.md and the target-state document, then the new code in the current folder.

## The causes

- **code**: the generated code is wrong or incomplete, and a session that knows why can correct it.
- **plan**: the code is right, but the plan asks for something else: a target file that should not exist or has
  another name, or a layer the target state's structure does not have.
- **environment**: the machine or a dependency is the problem: a tool that is missing, a package version that
  does not exist, a broken lockfile.
- **person**: only a person can decide: the specs contradict the target state, or the behaviour is unclear.

Choose what the evidence shows. Do not choose `plan` to get past a check: if the logic is not there, the cause
is `code`.

## "Lost its legacy logic"

For each missing call, message or constant, look for that behaviour in the new code, anywhere in the module.

- It is not there: the cause is `code`.
- It is there under another name or in another file, or the target's framework does it now: propose an exception
  saying where it went. A separate session checks each exception against the legacy code, and rejects it unless
  the behaviour really is there.
- A message a person reads is never replaced by a framework. It must be in the code, in a file you name.

## The verdict

Write this YAML to the file your prompt names; leave out what does not apply:

```yaml
cause: code | plan | environment | person
why: <one or two sentences: the evidence, with file names>
fix: <for code or environment: exactly what to change>
layout:                # for cause plan: only a class's files or role
  - class: <class as the brief names it>
    target_files: [<file name>, ...]    # [] when it should have no file of its own
    role: <layer>
exceptions:            # behaviour that is present elsewhere
  - method: <Class.Method as the brief names it>
    fact: <the missing item exactly as the check prints it, with its quotes if it has them>
    where: <path/in/the/repository:line>, or `framework: <what does it now>` for a call
    reason: <why this keeps the legacy behaviour>
```

You cannot change the checks, the thresholds, the specs or the plan yourself, and nothing else you write is read.
