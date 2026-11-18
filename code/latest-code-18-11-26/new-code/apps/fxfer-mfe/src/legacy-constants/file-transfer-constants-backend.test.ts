import { FILE_TRANSFER_CONSTANTS_BACKEND } from './file-transfer-constants-backend';

test('F01_017 schedule ids match legacy constants', () => {
  expect(FILE_TRANSFER_CONSTANTS_BACKEND.Localization.DataListsItems.RunOnThisDay).toBe('0');
  expect(FILE_TRANSFER_CONSTANTS_BACKEND.Localization.DataListsItems.RunScheduleMonhly).toBe('2');
  expect(FILE_TRANSFER_CONSTANTS_BACKEND.Localization.DataListsItems.RunScheduleWeekly).toBe('1');
});

