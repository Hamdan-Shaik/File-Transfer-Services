import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  use: {
    baseURL: 'http://localhost:3000'
  },
  projects: [
    {
      name: 'edge',
      use: {
        browserName: 'chromium',
        channel: 'msedge'
      }
    }
  ],
  webServer: {
    command: 'python scripts/serve_export_spa.py --port 3000',
    url: 'http://localhost:3000/fxfer/source-locations/',
    reuseExistingServer: true
  }
});

