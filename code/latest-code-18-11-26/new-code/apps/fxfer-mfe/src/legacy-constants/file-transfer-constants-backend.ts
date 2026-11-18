import { BusinessValidationExceptions } from './BusinessValidationExceptions';
import { Help } from './Help';
import { Ioc } from './Ioc';
import { Localization } from './Localization';
import { MVC } from './MVC';
import { SearchFields } from './SearchFields';
import { WebApi } from './WebApi';

export const FILE_TRANSFER_CONSTANTS_BACKEND = {
  BusinessValidationExceptions,
  Ioc,
  Localization,
  Miscellaneous: {
    FileToEvent: 'FileToEvent',
    UNC: 3
  },
  MVC,
  SearchFields,
  Help,
  WebApi,
  Xml: {
    URI: {
      NamespaceFileTransfer: 'urn:hp.com:hsp:ua3:FileTransfer:schemas'
    }
  }
} as const;

