import { test, expect } from '@playwright/test';

test('ftp servers page loads', async ({ page }) => {
  await page.goto('/fxfer/ftp-servers');
  await expect(page.getByRole('heading', { name: 'FTP Servers' })).toBeVisible();
});

