import { Miscellaneous } from '../../../src/shared/file-transfer/constants/Miscellaneous';

test('F02_023_miscellaneous_constants_match_legacy', () => {
  expect(Miscellaneous).toEqual({
    FileToEvent: 'FileToEvent',
    UNC: 3,
  });
});
