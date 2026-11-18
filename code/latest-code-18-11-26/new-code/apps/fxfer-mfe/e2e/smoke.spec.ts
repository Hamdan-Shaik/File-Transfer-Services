import { test, expect } from '@playwright/test';

test('fxfer-mfe loads a route', async ({ page }) => {
  await page.addInitScript(() => {
    window.__providerPlusShell = {
      auth: { getAccessToken: async () => 'jwt' },
      correlationId: '11111111-1111-4111-8111-111111111111',
      trace: {}
    };
  });

  await page.goto('/fxfer/source-locations');
  await expect(page.getByRole('heading', { name: 'Source Locations' })).toBeVisible();
});

