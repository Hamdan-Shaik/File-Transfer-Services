import type { ListItemModel } from '../models';

export class TrackingSearchResultModel {
  direction: ListItemModel;
  fileId: string | null = null;
  fileName: string | null = null;
  fileStatus: ListItemModel;
  createDate: Date | null = null;
  size: number | null = null;
  tradingPartnerId: string | null = null;
  transportType: ListItemModel;

  constructor() {
    this.direction = {};
    this.fileStatus = {};
    this.transportType = {};
  }
}

