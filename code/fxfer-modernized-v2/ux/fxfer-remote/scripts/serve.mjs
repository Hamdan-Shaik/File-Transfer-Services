import http from 'node:http';
import fs from 'node:fs/promises';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const root = process.cwd();
const outDir = path.join(root, 'out');
const port = Number.parseInt(process.env.PORT ?? '3000', 10);

const mimeByExt = {
  '.css': 'text/css',
  '.html': 'text/html',
  '.js': 'text/javascript',
  '.json': 'application/json',
  '.png': 'image/png',
  '.svg': 'image/svg+xml',
  '.txt': 'text/plain',
  '.woff2': 'font/woff2',
};

function toSafePath(urlPathname) {
  const decoded = decodeURIComponent(urlPathname);
  const abs = path.resolve(outDir, '.' + decoded);
  if (!abs.startsWith(outDir)) {
    return null;
  }
  return abs;
}

const server = http.createServer(async (req, res) => {
  const url = new URL(req.url ?? '/', 'http://localhost');
  let filePath = toSafePath(url.pathname);
  if (!filePath) {
    res.writeHead(400);
    res.end('Bad request');
    return;
  }

  try {
    const stat = await fs.stat(filePath);
    if (stat.isDirectory()) {
      filePath = path.join(filePath, 'index.html');
    }
  } catch {
    // fall through
  }

  try {
    const data = await fs.readFile(filePath);
    const ext = path.extname(filePath);
    res.setHeader('Content-Type', mimeByExt[ext] ?? 'application/octet-stream');
    res.writeHead(200);
    res.end(data);
  } catch {
    res.writeHead(404);
    res.end('Not found');
  }
});

server.listen(port, () => {
  console.log(`Serving ${outDir} on http://localhost:${port}`);
});

