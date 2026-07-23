# Claude Instructions

How Claude Code should work in this project.

## Workflow

- Implement **one file at a time**. Write a single script, let me review it, then
  move on to the next — do not batch multiple new/changed files in one go.

## C# conventions

See [[CSharp Conventions]].

## Git

- The only git command you may run is `git commit`.
- Do not push or pull.
- Do not stage or unstage files (no `git add`, `git restore`, `git reset`, etc.). The user manages staging themselves; commit only what they have already staged.
- Suggesting commit messages is welcome.
