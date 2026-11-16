import type { ListItemModel } from '../models';

export class TrackingSearchCriteriaModel {
  direction: ListItemModel;
  fileId: string | null = null;
  fileName: string | null = null;
  fromDate: Date | null = null;
  fileStatus: ListItemModel;
  fromSize: number | null = null;
  sourceLocationId: ListItemModel;
  toDate: Date | null = null;
  toSize: number | null = null;
  tradingPartnerId: string | null = null;
  transportType: ListItemModel;

  constructor() {
    this.direction = {};
    this.fileStatus = {};
    this.sourceLocationId = {};
    this.transportType = {};
  }
}

