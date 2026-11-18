import fs from 'node:fs';
import path from 'node:path';

const candidate = path.join(process.cwd(), 'out', '_next', 'static', 'chunks', 'remoteEntry.js');
const destination = path.join(process.cwd(), 'out', 'remoteEntry.js');

if (fs.existsSync(candidate)) {
  fs.copyFileSync(candidate, destination);
}

