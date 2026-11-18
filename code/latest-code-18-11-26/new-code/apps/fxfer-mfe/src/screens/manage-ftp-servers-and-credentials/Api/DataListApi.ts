import { fxferFetchJson } from '@/api/FxferBffClient';
import type { ListItemModel } from '@/screens/manage-ftp-servers-and-credentials/types';

type DataListItem = {
  id: string;
  name: string;
};

function buildQueryString(params: Record<string, string | undefined | null>): string {
  const parts: string[] = [];
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null) {
      continue;
    }
    if (value.trim() === '') {
      continue;
    }
    parts.push(`${encodeURIComponent(key)}=${encodeURIComponent(value)}`);
  }
  return parts.length === 0 ? '' : `?${parts.join('&')}`;
}

export class DataListApi {
  public static async getDataListData(args: {
    contentId: string;
    dataListType?: string | null;
    localeId?: string | null;
  }): Promise<ListItemModel[]> {
    const dataListType = args.dataListType ?? '';
    if (dataListType.toUpperCase() !== 'MODULE') {
      return [];
    }

    // Legacy: always queries active modules sorted by ModuleName asc.
    const qs = buildQueryString({
      contentId: 'Module',
      localeId: args.localeId ?? undefined
    });

    const items = await fxferFetchJson<DataListItem[]>(`/datalists${qs}`);
    const safe = Array.isArray(items) ? items : [];
    safe.sort((a1, a2) => a1.name.localeCompare(a2.name));
    return safe.map((i) => ({ name: i.name, id: i.id }));
  }
}

