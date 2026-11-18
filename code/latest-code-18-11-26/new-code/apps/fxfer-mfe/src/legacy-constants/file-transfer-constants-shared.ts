import { DataListsItems } from './localization/DataListsItems';

export const FILE_TRANSFER_CONSTANTS_SHARED = {
  ModuleName: 'FileTransfer',
  ApplicationSettings: {
    ExternalFTECommonFolder: 'ExternalFTECommonFolder'
  },
  Constants: {
    FTPCode: '1',
    SFTPCode: '2',
    FTPSCode: '4',
    GuidFormat: '00000000-0000-0000-0000-000000000000'
  },
  Localization: {
    DataListsItems
  }
} as const;

