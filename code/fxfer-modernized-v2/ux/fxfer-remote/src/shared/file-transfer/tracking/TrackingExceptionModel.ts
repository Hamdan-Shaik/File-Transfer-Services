export class TrackingExceptionModel {
  date: Date | null = null;
  exceptionType: string | null = null;
  message: string | null = null;
  methodName: string | null = null;
  stackTrace: string | null = null;

  constructor() {}
}

