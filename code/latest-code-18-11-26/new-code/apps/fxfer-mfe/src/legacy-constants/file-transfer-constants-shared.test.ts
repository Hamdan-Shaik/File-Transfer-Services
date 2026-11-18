import { ApplicationSettings } from './ApplicationSettings';
import { Constants } from './Constants';
import { FileTransferConstants } from './FileTransferConstants';
import { FILE_TRANSFER_CONSTANTS_SHARED } from './file-transfer-constants-shared';

test('shared file transfer constants match legacy values', () => {
  expect(FileTransferConstants.ModuleName).toBe(FILE_TRANSFER_CONSTANTS_SHARED.ModuleName);
  expect(FileTransferConstants.ApplicationSettings).toEqual(ApplicationSettings);
  expect(FileTransferConstants.Constants).toEqual(Constants);

  expect(ApplicationSettings.ExternalFTECommonFolder).toBe('ExternalFTECommonFolder');
  expect(Constants.FTPCode).toBe('1');
  expect(Constants.SFTPCode).toBe('2');
  expect(Constants.FTPSCode).toBe('4');
  expect(Constants.GuidFormat).toBe('00000000-0000-0000-0000-000000000000');

  expect(FILE_TRANSFER_CONSTANTS_SHARED.ModuleName).toBe('FileTransfer');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.ApplicationSettings.ExternalFTECommonFolder).toBe(
    'ExternalFTECommonFolder'
  );
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Constants.FTPCode).toBe('1');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Constants.SFTPCode).toBe('2');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Constants.FTPSCode).toBe('4');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Constants.GuidFormat).toBe(
    '00000000-0000-0000-0000-000000000000'
  );
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunOnThisDay).toBe('0');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunScheduleMonhly).toBe('2');
  expect(FILE_TRANSFER_CONSTANTS_SHARED.Localization.DataListsItems.RunScheduleWeekly).toBe('1');
});

