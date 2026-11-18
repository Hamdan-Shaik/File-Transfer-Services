import { test, expect } from '@playwright/test';

test.describe('search-and-track-file-transfers', () => {
  test.beforeEach(async ({ page }) => {
    await page.addInitScript(() => {
      // Provide shell auth+correlation hooks used by FxferBffClient.
      window.__providerPlusShell = {
        auth: { getAccessToken: async () => 'jwt' },
        correlationId: '11111111-1111-4111-8111-111111111111',
        trace: {}
      };
    });
  });

  test('Staff open the file transfer tracking search screen', async ({ page }) => {
    await page.goto('/fxfer/file-tracking');

    await expect(page.getByRole('heading', { name: 'File Tracking' })).toBeVisible();

    await expect(page.getByLabel('File Id')).toHaveValue('');
    await expect(page.getByLabel('File Name')).toHaveValue('');
    await expect(page.getByLabel('File Status')).toHaveValue('');
    await expect(page.getByRole('button', { name: 'Search' })).toBeVisible();
    await expect(page.getByRole('link', { name: 'Cancel' })).toHaveAttribute('href', '/Home/MyHome');
  });

  test('Staff search the tracking list of file transfers', async ({ page }) => {
    await page.route('**/api/file-transfer/v1/file-tracking**', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          items: [
            {
              fileId: '11111111-1111-4111-8111-111111111111',
              fileSourceLocationId: '22222222-2222-4222-8222-222222222222',
              fileName: 'a.txt',
              fileStatus: 'OK',
              fileDirection: 'IN',
              fileTransportType: 'UNC',
              createdDate: new Date().toISOString(),
              tradingPartner: 'TP1',
              checkSumCode: 'X',
              fileSize: 1,
              sourceLocAddress: 'unc://share',
              sourceLocConfigName: 'SRC',
              originatingServerName: '',
              events: [],
              exceptions: []
            }
          ],
          total: 1,
          page: 1,
          pageSize: 50
        })
      });
    });

    await page.goto('/fxfer/file-tracking');
    await page.getByRole('button', { name: 'Search' }).click();

    await expect(page.getByLabel('Total matches')).toHaveText('Total: 1');
    await expect(page.getByRole('cell', { name: 'a.txt' })).toBeVisible();
  });

  test('Staff see empty list and total 0 when no matches', async ({ page }) => {
    await page.route('**/api/file-transfer/v1/file-tracking**', async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({ items: [], total: 0, page: 1, pageSize: 50 })
      });
    });

    await page.goto('/fxfer/file-tracking');
    await page.getByRole('button', { name: 'Search' }).click();

    await expect(page.getByLabel('Total matches')).toHaveText('Total: 0');
    await expect(page.getByText('(no results)')).toBeVisible();
  });
});

