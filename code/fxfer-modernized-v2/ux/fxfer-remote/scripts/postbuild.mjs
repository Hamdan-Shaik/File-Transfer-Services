import path from 'node:path';
import { build } from 'esbuild';

const root = process.cwd();
const outDir = path.join(root, 'out');
const dest = path.join(outDir, 'remoteEntry.js');

await build({
  entryPoints: [path.join(root, 'src', 'remote', 'remoteEntry.ts')],
  bundle: true,
  platform: 'browser',
  format: 'esm',
  target: 'es2022',
  outfile: dest,
  sourcemap: true,
  external: ['react', 'react-dom', '@tanstack/react-query'],
});

