import { buildFtpServerCredentialsSearchModel } from '../../../src/shared/file-transfer/api/ftpServerCredentialsSearch';
import type { SearchCriteriaModel } from '../../../src/shared/file-transfer/models/SearchCriteriaModel';

test('F01_042_reads_first_matching_search_field_value_by_name', () => {
  const searchCriteria: SearchCriteriaModel = {
    searchFields: [
      { fieldName: 'FtpServerName', fieldValue: 'first' },
      { fieldName: 'FtpServerName', fieldValue: 'second' },
      { fieldName: 'FtpDescription', fieldValue: 'desc' },
      { fieldName: 'FtpUser', fieldValue: 'user' },
    ],
  };

  expect(buildFtpServerCredentialsSearchModel(searchCriteria)).toEqual({
    ftpServerName: 'first',
    ftpDescription: 'desc',
    ftpUser: 'user',
    isValid: null,
  });
});

test('F01_043_sets_isValid_true_when_search_field_non_empty', () => {
  const searchCriteriaWithIsValid: SearchCriteriaModel = {
    searchFields: [{ fieldName: 'IsValid', fieldValue: 'any-non-empty' }],
  };

  expect(buildFtpServerCredentialsSearchModel(searchCriteriaWithIsValid)).toEqual({
    ftpServerName: '',
    ftpDescription: '',
    ftpUser: '',
    isValid: true,
  });

  const searchCriteriaWithEmptyIsValid: SearchCriteriaModel = {
    searchFields: [{ fieldName: 'IsValid', fieldValue: '' }],
  };

  expect(buildFtpServerCredentialsSearchModel(searchCriteriaWithEmptyIsValid)).toEqual({
    ftpServerName: '',
    ftpDescription: '',
    ftpUser: '',
    isValid: null,
  });
});

