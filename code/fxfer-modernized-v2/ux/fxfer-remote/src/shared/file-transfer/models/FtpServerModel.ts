export class FtpServerModel {
  ftpServerId: string | null = null;
  ftpServerName: string | null = null;
  ftpServerIp: string | null = null;
  ftpTransferMode: string | null = null;
  ftpUser: string | null = null;
  ftpPassword: string | null = null;
  ftpPasswordConfirmation: string | null = null;
  ftpDescription: string | null = null;
  ftpPort = 0;
  ftpProxyAddress: string | null = null;
  ftpProxyPort: number | null = null;
  lastModifiedDate: Date | null = null;
  creationDate: Date | null = null;
  isValid: boolean | null = null;
  removePassword: boolean | null = null;
  operatorId: string | null = null;
}

