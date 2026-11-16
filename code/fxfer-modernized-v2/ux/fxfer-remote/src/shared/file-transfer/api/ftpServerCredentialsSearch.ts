import type { FtpServerModel, SearchCriteriaModel } from '../models';
import { getSearchFieldValue } from './searchHelper';

// F01-042: Reads the first matching search-field value (by exact field name)
//          and uses it to populate ftpServerName, ftpDescription, and ftpUser.
// F01-043: Any non-empty IsValid search-field value forces isValid=true;
//          empty/missing keeps isValid=null.
export function buildFtpServerCredentialsSearchModel(
  searchCriteria: SearchCriteriaModel,
): Pick<
  FtpServerModel,
  'ftpServerName' | 'ftpDescription' | 'ftpUser' | 'isValid'
> {
  const isValidSearchField = getSearchFieldValue(searchCriteria, 'IsValid');

  return {
    ftpServerName: getSearchFieldValue(searchCriteria, 'FtpServerName'),
    ftpDescription: getSearchFieldValue(searchCriteria, 'FtpDescription'),
    ftpUser: getSearchFieldValue(searchCriteria, 'FtpUser'),
    isValid: isValidSearchField !== '' ? true : null,
  };
}

