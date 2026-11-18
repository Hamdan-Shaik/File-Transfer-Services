// Legacy patterns, ported from UX scripts.

// From `hp.ua3.fileTransfer.FileTransferConfigurationEdit.js` / `hp.ua3.FileTransfer.CommonSourceFile.js`.
const ftpRegexp = new RegExp('^\\/([^/\\\\?:<>*|]+\\/)*([^/\\\\?:<>*|]+\\ )$');

// From `hp.ua3.fileTransfer.viewEditFtpServerCredentials.js`.
const ipPattern = /^(?!0)(?!.*\.$)((1?\d?\d|25[0-5]|2[0-4]\d)(\.|$)){4}$/;

export function validateFtpFormat(args: { sourceAddress: string; protocolId: string }) {
  const withTrailingSpace = `${args.sourceAddress} `;
  if (args.protocolId === '1' || args.protocolId === '2' || args.protocolId === '4') {
    return ftpRegexp.test(withTrailingSpace);
  }
  return true;
}

export function validateIpAddressFormat(ip: string) {
  return ipPattern.test(ip);
}

// Port of Target rule: monthly when first/last day or runOnThisDay != 0.
export function deriveRunScheduleId(args: {
  ftsSourceLocationId?: string | null;
  runOnFirstDay: boolean;
  runOnLastDay: boolean;
  runOnThisDay: number;
}): '1' | '2' {
  if (!args.ftsSourceLocationId) {
    return '1';
  }

  const isMonthly = args.runOnFirstDay || args.runOnLastDay || args.runOnThisDay !== 0;
  return isMonthly ? '2' : '1';
}

