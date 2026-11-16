import { ApplicationSettings } from '../../../src/shared/file-transfer/constants/ApplicationSettings';
import { Constants } from '../../../src/shared/file-transfer/constants/Constants';

test('F02_023_application_settings_constants_match_legacy', () => {
  expect(ApplicationSettings).toEqual({
    ExternalFTECommonFolder: 'ExternalFTECommonFolder',
  });
});

test('file_transfer_constants_match_legacy', () => {
  expect(Constants).toEqual({
    FTPCode: '1',
    SFTPCode: '2',
    FTPSCode: '4',
    GuidFormat: '00000000-0000-0000-0000-000000000000',
  });
});

