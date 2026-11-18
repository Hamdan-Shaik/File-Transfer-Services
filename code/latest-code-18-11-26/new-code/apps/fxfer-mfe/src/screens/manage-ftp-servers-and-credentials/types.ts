export type EditorModel = {
  savedMessage?: string;
  saveActionText?: string;
};

export type ListItemModel = {
  name: string;
  id: string;
};

export type GridSort = {
  field: string;
  dir: 'asc' | 'desc';
};

export type GridFilter = {
  field: string;
  value: string | number | boolean | null | undefined;
};

export type GridState = {
  page?: number;
  pageSize?: number;
  sort?: GridSort[];
  filter?: GridFilter[];
};

export function getGridFilterValue(gridState: GridState | null | undefined, field: string): string {
  if (!gridState?.filter) {
    return '';
  }

  const hit = gridState.filter.find((f) => f.field === field);
  if (!hit || hit.value === null || hit.value === undefined) {
    return '';
  }
  return String(hit.value);
}

