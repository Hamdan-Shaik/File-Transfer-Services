# File-Transfer-Services

This repo contains generated **code**, **specs**, and **UI** outputs for the File Transfer domain.

## Repo layout

- `code/`
  - Generated service/backend code.
  - This folder can contain **multiple iterations** of generated code (ex: `*-v1`, `*-v2`, etc.).
- `specs/`
  - Generated specifications (and/or inputs used to generate them).
  - This folder can contain **multiple iterations** of generated specs (ex: `*-v1`, `*-v2`, etc.).
- `UI/`
  - Generated UI packs.
  - This folder can contain **multiple UI items**. To run any UI item, go into that item's `app/` folder.

## Running a UI item (local dev)

Prereqs: Node.js + npm.

1. `cd` into the UI item you want.
2. Go into its `app/` folder.
3. Install dependencies: `npm i` (same as `npm install`).
4. Start the dev server: `npm run dev`.

### Example: `fxfer-v8`

PowerShell:

```powershell
cd File-Transfer-Services\UI\fxfer-v8\app
npm i
npm run dev
```

Then open the URL printed by the dev server (typically `http://localhost:5173`).
