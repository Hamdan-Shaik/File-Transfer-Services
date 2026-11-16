- Use `@module-federation/nextjs-mf` to emit `remoteEntry.js` and expose `./App`.
- Use static export (`output: 'export'`) and serve locally from `out/`.
- Use `public/runtime-config.json` for non-secret runtime configuration (BFF base URL).
- Use a minimal path-based router inside `src/remote/App.tsx` as a placeholder; upgrade to platform-standard routing when wiring flows.

