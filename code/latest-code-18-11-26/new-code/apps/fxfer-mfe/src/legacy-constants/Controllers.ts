import { FILE_TRANSFER_CONSTANTS_BACKEND } from './file-transfer-constants-backend';

export const Controllers = {
  MVC: FILE_TRANSFER_CONSTANTS_BACKEND.MVC.Controllers,
  WebApi: FILE_TRANSFER_CONSTANTS_BACKEND.WebApi.Controllers
} as const;

