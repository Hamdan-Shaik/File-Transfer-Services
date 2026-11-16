import type { SearchCriteriaModel, SearchFieldModel } from '../models';

export function getSearchFieldValue(
  searchCriteria: SearchCriteriaModel,
  fieldName: string,
): string {
  let searchField: SearchFieldModel | undefined;
  const hasCriteria =
    searchCriteria.searchFields != null && searchCriteria.searchFields.length > 0;

  return hasCriteria &&
    (searchField = searchCriteria.searchFields!.find(
      (filter) => filter.fieldName === fieldName,
    )) != null
    ? (searchField.fieldValue as unknown as { toString(): string }).toString()
    : '';
}

