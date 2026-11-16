import { TrackingEventModel } from './TrackingEventModel';
import { TrackingExceptionModel } from './TrackingExceptionModel';
import { TrackingInformationModel } from './TrackingInformationModel';

export class TrackingDetailModel {
  events: TrackingEventModel[];
  exceptions: TrackingExceptionModel[];
  information: TrackingInformationModel;

  constructor() {
    this.events = [];
    this.information = new TrackingInformationModel();
    this.exceptions = [];
  }
}

