import {
  AddFileTransferConfigurationModel,
  DestinationConfigurationModel,
  FileTransferConfigurationModel,
  FileTransferConfigurationSearchCriteriaModel,
  FtpServerModel,
  FtpServerSearchCriteriaModel,
} from '../shared/file-transfer/models';
import { FileTransferConstants } from '../shared/file-transfer/constants/FileTransferConstants';

type Requestor = unknown;

type RequestModel<T> = {
  requestor: Requestor;
  data: T;
};

type StatusCodeType = 'OK' | string;

type ResultModel = {
  statusCode: StatusCodeType;
};

type ResponseModel<T> = {
  result: ResultModel;
  data: T | null;
};

type LocalizationLabel = { text: string };

type LocalizationProvider = {
  getLabel: (localCode: string, labelKey: string) => LocalizationLabel;
};

type UserProfile = { localCode: string };

type UserConfigurationManager = {
  userProfile: UserProfile;
  generateRequestor: () => Requestor;
};

type TenantConfigurationManager = {
  localizationProvider: LocalizationProvider;
  resolve: <T>(componentName: string) => T;
};

type ContextHelper = {
  userConfigurationManager: UserConfigurationManager;
  tenantConfigurationManager: TenantConfigurationManager;
  buildMvcUrl: (controller: string, action: string) => string;
};

type IFileTransferQueryServiceDataProvider = {
  GetFileTransferTrackingDetail: (
    request: RequestModel<string>,
  ) => ResponseModel<TrackingDetailModel>;
  GetFileTransferConfigurationInformation: (
    request: RequestModel<string>,
  ) => ResponseModel<FileTransferConfigurationModel> | null;
};

type EditorModel = {
  entityLabelName: string;
  step: number;
  stepCount: number;
  editMode: string;
  editDirection: string;
  cancelActionUrl: string;
  isDirtyDataEnabled: boolean;
  isResetEnabled: boolean;
  showSummaryError: boolean;
  isCancelEnabled: boolean;
  saveActionText?: string;
};

type EditorHost = { editor: EditorModel };

type OverlayEditor = Record<string, unknown>;

type ButtonInfoModel = {
  isEnabled: boolean;
  labelText: string;
  order: number;
  postprocessFunction: string;
};

class TrackingSearchCriteriaModel {
  editor: EditorModel | null = null;
}

class TrackingDetailModel {
  editor: OverlayEditor | null = null;
}

const OverlayEditorModel = {
  ParamTitle: 'ParamTitle',
  ParamCloseOverlayFunction: 'ParamCloseOverlayFunction',
  ParamShowHelpIcon: 'ParamShowHelpIcon',
  ParamIsOverlayDirtyDataEnabled: 'ParamIsOverlayDirtyDataEnabled',
  ParamShowSummaryError: 'ParamShowSummaryError',
  ParamLeftButtons: 'ParamLeftButtons',
  ParamRightButtons: 'ParamRightButtons',
} as const;

const CoreLocalization = {
  Buttons: {
    Search: 'CoreConstants.Localization.Labels.Buttons.Search',
    Cancel: 'CoreConstants.Localization.Labels.Buttons.Cancel',
  },
} as const;

const CoreEnumerations = {
  Messaging: {
    StatusCodeType: {
      OK: 'OK' as const,
    },
  },
} as const;

const CoreMvc = {
  Controllers: {
    Home: 'Home',
  },
  Actions: {
    MyHome: 'MyHome',
  },
} as const;

export class FileTransferController {
  private static configModel: AddFileTransferConfigurationModel | null = null;

  public constructor(private readonly contextHelper: ContextHelper) {}

  // legacy: [HttpGet]
  // legacy: [MaintainabilityDetails(ApprovedIndex = 56, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
  public SearchFileTransferTracking() {
    const criteria = new TrackingSearchCriteriaModel();

    criteria.editor = this.initializeEditor(
      FileTransferConstants.Localization.Entities.FileTransfer,
      1,
      1,
      'Create',
      'Next',
      this.contextHelper.buildMvcUrl(CoreMvc.Controllers.Home, CoreMvc.Actions.MyHome),
      false,
      true,
      true,
      true,
    );

    criteria.editor.saveActionText =
      this.contextHelper.tenantConfigurationManager.localizationProvider.getLabel(
        this.contextHelper.userConfigurationManager.userProfile.localCode,
        CoreLocalization.Buttons.Search,
      ).text;

    const actionResult = this.view(criteria);

    void this.GenerateInvalidModelStateRepsonse();
    void this.GenerateAddFileTransferBaseConfigurationResult();
    this.UpdateFileTransferBaseConfiguration();

    return actionResult;
  }

  public SearchFileTransfer() {
    const actionResult = this.SearchSourceLocations();

    void this.GenerateAddFileTransferBaseConfigurationResult();
    this.UpdateFileTransferBaseConfiguration();

    return actionResult;
  }

  public GetSourceLocations() {
    const actionResult = this.SearchSourceLocations();

    void this.GenerateInvalidModelStateRepsonse();
    void this.GenerateAddFileTransferBaseConfigurationResult();
    this.UpdateFileTransferBaseConfiguration();

    return actionResult;
  }

  public FileTransferTrackingView(fileId: string) {
    let trackingData = new TrackingDetailModel();

    if (fileId !== null && fileId !== undefined && fileId !== '') {
      const provider =
        this.contextHelper.tenantConfigurationManager.resolve<IFileTransferQueryServiceDataProvider>(
          FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider,
        );

      const detailRequest: RequestModel<string> = {
        requestor: this.contextHelper.userConfigurationManager.generateRequestor(),
        data: fileId,
      };

      const providerResult = provider.GetFileTransferTrackingDetail(detailRequest);

      const resultModel = providerResult.result;
      if (
        resultModel.statusCode === CoreEnumerations.Messaging.StatusCodeType.OK &&
        providerResult.data != null
      ) {
        trackingData = providerResult.data;
      }
    }

    const rightButtons: ButtonInfoModel[] = [];
    const leftButtons: ButtonInfoModel[] = [];

    const rightbutton1: ButtonInfoModel = {
      isEnabled: true,
      labelText:
        this.contextHelper.tenantConfigurationManager.localizationProvider.getLabel(
          this.contextHelper.userConfigurationManager.userProfile.localCode,
          CoreLocalization.Buttons.Cancel,
        ).text,
      order: 1,
      postprocessFunction: 'cancelFileTranferTrackingView',
    };
    rightButtons.push(rightbutton1);

    const parameters: Record<string, unknown> = {};
    parameters[OverlayEditorModel.ParamTitle] =
      this.contextHelper.tenantConfigurationManager.localizationProvider.getLabel(
        this.contextHelper.userConfigurationManager.userProfile.localCode,
        FileTransferConstants.Localization.Labels.Titles.FileTransferTracking,
      ).text;
    parameters[OverlayEditorModel.ParamCloseOverlayFunction] =
      'processFileTransfertrackingClose';
    parameters[OverlayEditorModel.ParamShowHelpIcon] = true;
    parameters[OverlayEditorModel.ParamIsOverlayDirtyDataEnabled] = true;
    parameters[OverlayEditorModel.ParamShowSummaryError] = true;
    parameters[OverlayEditorModel.ParamLeftButtons] = leftButtons;
    parameters[OverlayEditorModel.ParamRightButtons] = rightButtons;

    trackingData.editor = this.initializeOverlayEditor(parameters);

    const actionResult = this.view(trackingData);
    return actionResult;
  }

  public SearchSourceLocations() {
    let actionResult = this.view();

    const criteria = new FileTransferConfigurationSearchCriteriaModel();

    const criteriaWithEditor =
      criteria as unknown as FileTransferConfigurationSearchCriteriaModel & EditorHost;

    criteriaWithEditor.editor = this.initializeEditor(
      FileTransferConstants.Localization.Entities.FileTransfer,
      1,
      1,
      'Create',
      'Next',
      this.contextHelper.buildMvcUrl(CoreMvc.Controllers.Home, CoreMvc.Actions.MyHome),
      false,
      true,
      true,
      true,
    );

    criteriaWithEditor.editor.saveActionText =
      this.contextHelper.tenantConfigurationManager.localizationProvider.getLabel(
        this.contextHelper.userConfigurationManager.userProfile.localCode,
        CoreLocalization.Buttons.Search,
      ).text;

    actionResult = this.view(criteria);

    return actionResult;
  }

  public AddFileBaseConfiguration(
    addFileTransferConfiguration: AddFileTransferConfigurationModel,
  ) {
    let actionResult = this.view();
    if (FileTransferController.configModel == null) {
      FileTransferController.configModel = new AddFileTransferConfigurationModel();
    }

    FileTransferController.configModel.ownerSubsystemId =
      FileTransferController.configModel.ownerSubsystem.id ?? null;

    const viewBag: Record<string, unknown> = {};
    viewBag.FileTransferViewMode = 'add';

    if (addFileTransferConfiguration.ftsSourceLocationId == null) {
      addFileTransferConfiguration.isActive = true;
      actionResult = this.view(addFileTransferConfiguration, { viewBag });
    } else {
      actionResult = this.view(FileTransferController.configModel, { viewBag });
    }

    return actionResult;
  }

  public FileTransferConfigurationView(ftsSourceLocationId: string) {
    let fileTransferConfigModel = new FileTransferConfigurationModel();
    fileTransferConfigModel.ftsSourceLocationId = ftsSourceLocationId;

    let actionResult = this.view();

    fileTransferConfigModel = this.GetFileTransferConfiguration(fileTransferConfigModel);

    actionResult = this.view(fileTransferConfigModel, {
      viewPath: '~/Views/FileTransfer/FileTransferConfigurationView.cshtml',
    });

    return actionResult;
  }

  public DestinationConfigurationEditPanel(
    addDestinationConfiguration: DestinationConfigurationModel,
  ) {
    let actionResult = this.view();

    actionResult = this.view(addDestinationConfiguration);

    return actionResult;
  }

  public FileTransferConfigurationEdit(fileTransferConfigModel: FileTransferConfigurationModel) {
    let actionResult = this.view();

    if (fileTransferConfigModel.ftsSourceLocationId == null) {
      fileTransferConfigModel.isActive = true;
    } else {
      fileTransferConfigModel = this.GetFileTransferConfiguration(fileTransferConfigModel);
    }

    actionResult = this.view(fileTransferConfigModel);

    return actionResult;
  }

  public DestinationConfigurationEdit(
    destinationConfiguration: DestinationConfigurationModel,
  ) {
    let actionResult = this.view();

    actionResult = this.view(destinationConfiguration);

    return actionResult;
  }

  public GetFileTransferConfiguration(fileTransferConfigModel: FileTransferConfigurationModel) {
    const request: RequestModel<string> = {
      requestor: this.contextHelper.userConfigurationManager.generateRequestor(),
      data: fileTransferConfigModel.ftsSourceLocationId as unknown as string,
    };

    const provider =
      this.contextHelper.tenantConfigurationManager.resolve<IFileTransferQueryServiceDataProvider>(
        FileTransferConstants.Ioc.ComponentNames.FileTransferQueryServiceDataProvider,
      );

    const response = provider.GetFileTransferConfigurationInformation(request);

    if (response != null) {
      if (
        response.result.statusCode === CoreEnumerations.Messaging.StatusCodeType.OK &&
        response.data != null
      ) {
        fileTransferConfigModel = response.data;
      }
    }

    return fileTransferConfigModel;
  }

  public SearchFTPServerCredentials() {
    return this.view(this.BuildFTPServerModel());
  }

  public CreateFTPServerCredentials(ftpServerModel: FtpServerModel) {
    this.UpdateFileTransferBaseConfiguration();
    void this.GenerateInvalidModelStateRepsonse();
    return this.AddFTPServerCredentials(ftpServerModel);
  }

  public AddFTPServerCredentials(ftpServerModel: FtpServerModel) {
    return this.partialView(
      ftpServerModel,
      '~/Views/FileTransfer/ViewEditFTPServerCredentials.cshtml',
    );
  }

  private BuildFTPServerModel() {
    const model = new FtpServerSearchCriteriaModel();

    const modelWithEditor = model as unknown as FtpServerSearchCriteriaModel & EditorHost;
    modelWithEditor.editor = this.initializeEditor(
      FileTransferConstants.Localization.Entities.FileTransfer,
      1,
      1,
      'Create',
      'Next',
      this.contextHelper.buildMvcUrl(CoreMvc.Controllers.Home, CoreMvc.Actions.MyHome),
      false,
      true,
      false,
      true,
    );

    modelWithEditor.editor.saveActionText =
      this.contextHelper.tenantConfigurationManager.localizationProvider.getLabel(
        this.contextHelper.userConfigurationManager.userProfile.localCode,
        CoreLocalization.Buttons.Search,
      ).text;

    return model;
  }

  private UpdateFileTransferBaseConfiguration() {
    if (FileTransferController.configModel == null) {
      FileTransferController.configModel = new AddFileTransferConfigurationModel();
    }

    return FileTransferController.configModel;
  }

  private GenerateAddFileTransferBaseConfigurationResult() {
    return { statusCode: CoreEnumerations.Messaging.StatusCodeType.OK };
  }

  private GenerateInvalidModelStateRepsonse() {
    return this.view({});
  }

  private initializeEditor(
    entityLabelName: string,
    step: number,
    stepCount: number,
    editMode: string,
    editDirection: string,
    cancelActionUrl: string,
    isDirtyDataEnabled: boolean,
    isResetEnabled: boolean,
    showSummaryError: boolean,
    isCancelEnabled: boolean,
  ): EditorModel {
    return {
      entityLabelName,
      step,
      stepCount,
      editMode,
      editDirection,
      cancelActionUrl,
      isDirtyDataEnabled,
      isResetEnabled,
      showSummaryError,
      isCancelEnabled,
    };
  }

  private initializeOverlayEditor(parameters: Record<string, unknown>): OverlayEditor {
    return { ...parameters };
  }

  private view(
    model?: unknown,
    options?: { viewPath?: string; viewBag?: Record<string, unknown> },
  ) {
    return {
      viewPath: options?.viewPath,
      viewBag: options?.viewBag,
      model,
    };
  }

  private partialView(model: unknown, viewPath: string) {
    return {
      viewPath,
      model,
      isPartial: true,
    };
  }
}


