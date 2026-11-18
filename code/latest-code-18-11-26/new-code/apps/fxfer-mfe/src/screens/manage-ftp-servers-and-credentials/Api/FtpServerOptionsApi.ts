import { fxferFetchJson } from '@/api/FxferBffClient';
import type { ListItemModel } from '@/screens/manage-ftp-servers-and-credentials/types';

type FtpServerOption = {
  ftpServerId: string;
  ftpServerName: string;
};

export class FtpServerOptionsApi {
  public static async getFtpServerOptions(): Promise<ListItemModel[]> {
    const options = await fxferFetchJson<FtpServerOption[]>('/ftp-servers/options');
    const safe = Array.isArray(options) ? options : [];

    return safe.map((o) => ({ name: o.ftpServerName, id: String(o.ftpServerId) }));
  }
}

