import type { DestinationConfigurationModel } from './DestinationConfigurationModel';
import type { ListItemModel } from './ListItemModel';

export class AddFileTransferConfigurationModel {
  address: string | null = null;
  configId = 0;
  configName: string | null = null;
  contactName: string | null = null;
  description: string | null = null;
  destinationId: string | null = null;
  destinationAddress: string | null = null;
  destinationProtocol: ListItemModel;
  ftpServer: ListItemModel;
  ftpServerId: string | null = null;
  ftpServerName: string | null = null;
  fileDirection: ListItemModel;
  ftsSourceLocationId: string | null = null;
  isActive = false;
  ownerEmail: string | null = null;
  ownerPhoneNumber: string | null = null;
  ownerName: string | null = null;
  selectedId: string | null = null;
  ownerModule: string | null = null;
  ownerSubsystem: ListItemModel;
  ownerSubsystemId: string | null = null;
  protocolType: ListItemModel;
  runOnMonday = false;
  runOnTuesday = false;
  runOnWednesday = false;
  runOnThursday = false;
  runOnFriday = false;
  runOnSaturday = false;
  runOnSunday = false;
  runOnLastDay = false;
  runOnFirstDay = false;
  runOnThisDay: ListItemModel;
  runSchedule: ListItemModel;
  status: string | null = null;
  suspended = false;
  sourceAddress: string | null = null;
  sourceFileMask: string | null = null;
  sendNotification = false;
  suspend = false;
  timeWindowStart: ListItemModel;
  timeWindowEnd: ListItemModel;
  tradingPartner: string | null = null;
  destinationDetails: DestinationConfigurationModel[];

  constructor() {
    this.destinationDetails = [];
    this.destinationProtocol = {};
    this.ftpServer = {};
    this.fileDirection = {};
    this.ownerSubsystem = {};
    this.protocolType = {};
    this.runOnThisDay = {};
    this.runSchedule = {};
    this.timeWindowEnd = {};
    this.timeWindowStart = {};
  }
}

