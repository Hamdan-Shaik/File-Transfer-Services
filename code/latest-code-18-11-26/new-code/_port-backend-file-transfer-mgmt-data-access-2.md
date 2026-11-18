# Brief: port part 2 of 3 of component `backend-file-transfer-mgmt-data-access`

- Deployable: `fxfer-transfer-worker` at `workers/fxfer-transfer-worker`; build: `dotnet build workers/fxfer-transfer-worker`; tests: `dotnet test workers/fxfer-transfer-worker`
- The component's folder (architecture.yaml): `workers/fxfer-transfer-worker/src/Infrastructure/DataAccess`
- The component's spec: `technical/components/backend-file-transfer-mgmt-data-access/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 30 legacy methods or classes, 383 legacy lines
- Playbook: `inputs/playbook/port.md`
- Parts 1 to 1 of this component are written already: add to the classes they created, do not rewrite them.

## What to port

### `DataAccessFactory`: role **application**, target file(s) `DataAccessFactory.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\DataAccessFactory.cs`)

#### `DataAccessFactory.CreateErrorLogDataAccess(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.ErrorLogDetailModel)` (lines 35-38) → `DataAccessFactory.CreateErrorLogDataAccess`

```csharp
        public IDataAccess<int> CreateErrorLogDataAccess(ErrorLogDetailModel errorLog)
        {
            return new CreateErrorLogDataAccess(errorLog);
        }
```

#### `DataAccessFactory.CreateFileManagementDataAccess(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FileManagementRequestModel)` (lines 65-68) → `DataAccessFactory.CreateFileManagementDataAccess`

```csharp
        public IDataAccess<FileManagementResponseModel> CreateFileManagementDataAccess(FileManagementRequestModel requestModel)
        {
            return new CreateFileManagementDataAccess(requestModel);
        }
```

#### `DataAccessFactory.CreateFtsFileDeliveryDataAccess(System.Guid, System.Guid, string, string, System.Collections.Generic.List<HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.DestinationModel>)` (lines 60-63) → `DataAccessFactory.CreateFtsFileDeliveryDataAccess`

```csharp
        public IDataAccess<Guid> CreateFtsFileDeliveryDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, List<DestinationModel> destinations)
        {
            return new CreateFtsFileDeliveryDataAccess(fileId, destinationId, destinationFileName, operatorId, destinations);
        }
```

#### `DataAccessFactory.CreateFtsFileExceptionEventDataAccess(System.Guid, System.Guid, string, string, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.ErrorLogDetailModel)` (lines 70-73) → `DataAccessFactory.CreateFtsFileExceptionEventDataAccess`

```csharp
        public IDataAccess<Guid> CreateFtsFileExceptionEventDataAccess(Guid fileId, Guid destinationId, string destinationFileName, string operatorId, ErrorLogDetailModel errorLog)
        {
            return new CreateFtsFileExceptionEventDataAccess(fileId, destinationId, destinationFileName, operatorId, errorLog);
        }
```

#### `DataAccessFactory.CreateFtsSuspensionDataAccess(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.FtsSuspensionSearchCriteria)` (lines 15-18) → `DataAccessFactory.CreateFtsSuspensionDataAccess`

```csharp
        public IDataAccess<FtsSuspensionModel> CreateFtsSuspensionDataAccess(FtsSuspensionSearchCriteria searchQuery)
        {
            return new FtsSuspensionDataAccess(searchQuery);
        }
```

#### `DataAccessFactory.CreateGetConfigMetaDataDataAccess(System.Guid)` (lines 40-43) → `DataAccessFactory.CreateGetConfigMetaDataDataAccess`

```csharp
        public IDataAccess<SourceLocationModel> CreateGetConfigMetaDataDataAccess(Guid fileId)
        {
            return new GetConfigMetaDataDataAccess(fileId);
        }
```

#### `DataAccessFactory.CreateGetPendingFileListDataAccess()` (lines 20-23) → `DataAccessFactory.CreateGetPendingFileListDataAccess`

```csharp
        public IDataAccess<List<PendingFileModel>> CreateGetPendingFileListDataAccess()
        {
            return new GetPendingFileListDataAccess();
        }
```

#### `DataAccessFactory.CreateGetSourceLocationsDataAccess()` (lines 30-33) → `DataAccessFactory.CreateGetSourceLocationsDataAccess`

```csharp
        public IDataAccess<List<SourceLocationModel>> CreateGetSourceLocationsDataAccess()
        {
            return new GetSourceLocationsDataAccess();
        }
```

#### `DataAccessFactory.CreateQueryFtsNotificationDataAccess(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.GetNotificationSearchCriteria)` (lines 25-28) → `DataAccessFactory.CreateQueryFtsNotificationDataAccess`

```csharp
        public IDataAccess<List<FtsNotificationModel>> CreateQueryFtsNotificationDataAccess(GetNotificationSearchCriteria searchQuery)
        {
            return new QueryFtsNotificationDataAccess(searchQuery);
        }
```

#### `DataAccessFactory.CreateUpdateFileManagementStatusDataAccess(System.Guid, string)` (lines 45-48) → `DataAccessFactory.CreateUpdateFileManagementStatusDataAccess`

```csharp
        public IDataAccess<Guid> CreateUpdateFileManagementStatusDataAccess(Guid fileId, string fileStatus)
        {
            return new UpdateFileManagementStatusDataAccess(fileId, fileStatus);
        }
```

#### `DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 50-53) → `DataAccessFactory.CreateUpdateSourceSuspendStatusDataAccess(Guid)`

```csharp
        public IDataAccess<bool> CreateUpdateSourceSuspendStatusDataAccess(FtsSourceLocation ftsFile)
        {
            return new UpdateSourceSuspendStatusDataAccess(ftsFile);
        }
```

#### `DataAccessFactory.GetSuspendedLocation(System.Guid)` (lines 55-58) → `DataAccessFactory.GetSuspendedLocation`

```csharp
        public IDataAccess<FtsSourceLocation> GetSuspendedLocation(Guid sourceLocationId)
        {
            return new GetSuspendedLocation(sourceLocationId);
        }
```

### `FileStatusConstant`: role **application**, target file(s) `FileStatusConstant.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccessConstants.cs`)

#### `FileStatusConstant` (lines 10-16)

```csharp
    public static class FileStatusConstant
    {
        public const string Completed = "1";
        public const string Error = "2";
        public const string Transfering = "3";
        public const string UNCProtocolCD = "3";
    }
```

### `FtsSuspensionDataAccess`: role **infrastructure**, target file(s) `FtsSuspensionDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\FtsSuspensionDataAccess.cs`)

#### `FtsSuspensionDataAccess.CreateSourceLocationEntityData(IDbContextBase)` (lines 40-53) → `FtsSuspensionDataAccess.ExecuteQueryAsync`

```csharp
        private FtsSuspension CreateSourceLocationEntityData(IDbContextBase dbcontext)
        {
            Entities.FtsSuspension entitySourceLocation = new Entities.FtsSuspension()
            {
                SuspensionId = Guid.NewGuid(),
                FtsSourceLocationId = this.query.SourceLocationId,
                ErrorText = this.query.ErrorText,
                StackTrace = this.LimitLength(this.query.StackTrace, 1000),
                OperatorId = this.query.OperatorId,
                LastModifiedTs = DateTime.UtcNow,
                CreatedTs = DateTime.UtcNow
            };
            return entitySourceLocation;
        }
```

#### `FtsSuspensionDataAccess.ExecuteQuery(IDbContextBase)` (lines 20-38) → `FtsSuspensionDataAccess.ExecuteQueryAsync`

```csharp
        protected override FtsSuspensionModel ExecuteQuery(IDbContextBase dbcontext)
        {
            FtsSuspensionModel suspensionModel = new FtsSuspensionModel();
            var sourceLocations = dbcontext.Set<FtsSourceLocation>().Where(x => x.FtsSoureceLocationId == this.query.SourceLocationId).FirstOrDefault();

            if (sourceLocations != null)
            {
                Entities.FtsSuspension entitySourceLocation = this.CreateSourceLocationEntityData(dbcontext);
                dbcontext.Set<FtsSuspension>().Add(entitySourceLocation);

                sourceLocations.IsSuspended = true;
                sourceLocations.IsSuspendedChangedDate = DateTime.UtcNow;
                dbcontext.SaveChanges();

                suspensionModel.SuspensionId = entitySourceLocation.SuspensionId;
            }

            return suspensionModel;
        }
```

#### `FtsSuspensionDataAccess.LimitLength(string, int)` (lines 55-63) → `FtsSuspensionDataAccess.LimitLength`

```csharp
        private string LimitLength(string source, int maxLength)
        {
            if (source.Length <= maxLength)
            {
                return source;
            }

            return source.Substring(0, maxLength);
        }
```

### `GetConfigMetaDataDataAccess`: role **infrastructure**, target file(s) `GetConfigMetaDataDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\GetConfigMetaDataDataAccess.cs`)

#### `GetConfigMetaDataDataAccess.ExecuteQuery(IDbContextBase)` (lines 27-67) → `GetConfigMetaDataDataAccess.ExecuteQueryAsync`

```csharp
        protected override SourceLocationModel ExecuteQuery(IDbContextBase dbcontext)
        {
            this.SetSoureceLocationId(dbcontext);

            var sourceLocation = dbcontext.Set<FtsSourceLocation>().AsNoTracking().FirstOrDefault(x => x.FtsSoureceLocationId == this.soureceLocationId);
            SourceLocationModel source = null;
            if (sourceLocation != null)
            {
                var ftpServer = new FtpServer();
                if (sourceLocation.FtpServerId != Guid.Empty && sourceLocation.FtpServerId != null)
                {
                    ftpServer = sourceLocation.FtpServer;
                }

                source = new SourceLocationModel
                {
                    SourceLocationId = sourceLocation.FtsSoureceLocationId,
                    ConfigurationId = sourceLocation.Configid,
                    SourceProtocolCode = sourceLocation.SourceProtocalCode,
                    SourceAddress = sourceLocation.SourceAddress,
                    OwnerEmail = sourceLocation.OwnerEmail,
                    OwnerName = sourceLocation.OwnerName,
                    SendNotificationsIndicator = sourceLocation.SendNotificationsIndicator,
                    FtpServerDetail = new FtpServerModel
                    {
                        FtpServerId = ftpServer.FtpServerId,
                        FtpServerName = ftpServer.FtpServerName,
                        FtpServerIp = ftpServer.FtpServerIp,
                        FtpTransferMode = ftpServer.FtpTransferMode,
                        FtpUser = ftpServer.FtpUser,
                        FtpPort = ftpServer.FtpPort,
                        FtpPassword = ftpServer.FtpPassword,
                        SSHAuth = ftpServer.SSHAuth
                    }
                };
                var destinations = this.GetDestinations(dbcontext);
                source.Destinations.AddRange(destinations);
            }

            return source;
        }
```

#### `GetConfigMetaDataDataAccess.GetDestinations(IDbContextBase)` (lines 78-125) → `GetConfigMetaDataDataAccess.ExecuteQueryAsync`

```csharp
        private List<DestinationModel> GetDestinations(IDbContextBase dbcontext)
        {
            var deliveredDestinationIds = dbcontext.Set<FtsFileDelivery>().AsNoTracking().Where(x => x.FileId == this.fileId).Select(s => s.DestinationId).ToList();

            var result = dbcontext.Set<FtsDestinationLocations>().AsNoTracking().Where(x => x.FtsSourceLocationId == this.soureceLocationId && x.IsValid && (!deliveredDestinationIds.Contains(x.DestinationId) || x.DestIsArchive)).ToList();
            var destinations = new List<DestinationModel>();
            foreach (var item in result)
            {
                var destination = new DestinationModel
                {
                    DestinationId = item.DestinationId,
                    DestinationAddress = item.DestinationAddress,
                    DestinationProtocolCode = item.DestProtocolCD,
                    IsOverwriteAtDestination = item.OverwriteAtDest,
                    IsCompressAtDestination = item.CompressAtDest,
                    IsRenameAtDestination = item.ReNameAtDest,
                    ContactName = item.ContactName,
                    EmailId = item.ContactEmail,
                    IsArchive = item.DestIsArchive,
                    RenamePattern = item.RenamePattern,
                    EncryptionCode = item.CdeEncryption,
                    EncryptionPublicKey = item.EncryptionPublicKey,
                    EncryptionPrivateKey = item.EncryptionPrivateKey,
                    PrivateKeyPassword = item.PrivateKeyPassword,
                    FTEMetadataId = item.FTEMetadataId,
                    ContractName = item.ContractName
                };

                if (item.FtpServerId != Guid.Empty)
                {
                    destination.FtpServerDetail = dbcontext.Set<FtpServer>().AsNoTracking().Where(x => x.FtpServerId == item.FtpServerId).Select(f => new FtpServerModel
                    {
                        FtpServerId = f.FtpServerId,
                        FtpServerName = f.FtpServerName,
                        FtpServerIp = f.FtpServerIp,
                        FtpTransferMode = f.FtpTransferMode,
                        FtpUser = f.FtpUser,
                        FtpPort = f.FtpPort,
                        FtpPassword = f.FtpPassword,
                        SSHAuth = f.SSHAuth
                    }).FirstOrDefault();
                }

                destinations.Add(destination);
            }

            return destinations;
        }
```

#### `GetConfigMetaDataDataAccess.SetSoureceLocationId(IDbContextBase)` (lines 69-76) → `GetConfigMetaDataDataAccess.ExecuteQueryAsync`

```csharp
        private void SetSoureceLocationId(IDbContextBase dbcontext)
        {
            var fileManagment = dbcontext.Set<FtsFileManagement>().SingleOrDefault(f => f.FileId == this.fileId);
            if (fileManagment != null)
            {
                this.soureceLocationId = fileManagment.FileSourceLocationId;
            }
        }
```

### `GetPendingFileListDataAccess`: role **infrastructure**, target file(s) `GetPendingFileListDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\GetPendingFileListDataAccess.cs`)

#### `GetPendingFileListDataAccess.ExecuteQuery(IDbContextBase)` (lines 19-37) → `GetPendingFileListDataAccess.ExecuteQueryAsync`

```csharp
        protected override List<PendingFileModel> ExecuteQuery(IDbContextBase dbcontext)
        {
            DateTime currentDate = DateTime.UtcNow.AddMinutes(-1);

            return (from fileMgmt in dbcontext.Set<FtsFileManagement>().AsNoTracking()
                    where (string.Compare(fileMgmt.FileStatus, FileStatusConstant.Completed, StringComparison.InvariantCultureIgnoreCase) != 0)
                    || (string.Compare(fileMgmt.FileStatus, FileStatusConstant.Completed, StringComparison.InvariantCultureIgnoreCase) == 0
                    && fileMgmt.FileStatusTs > currentDate)
                    select new PendingFileModel
                    {
                        FileId = fileMgmt.FileId,
                        FileName = fileMgmt.FileName,
                        FileSourceLocationId = fileMgmt.FileSourceLocationId,
                        FileStatus = fileMgmt.FileStatus,
                        FileTrackId = fileMgmt.SakFileTrack,
                        StatusDate = fileMgmt.FileStatusTs,
                        FileSize = fileMgmt.FileSize
                    }).ToList();
        }
```

### `GetSourceLocationsDataAccess`: role **infrastructure**, target file(s) `GetSourceLocationsDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\GetSourceLocationsDataAccess.cs`)

#### `GetSourceLocationsDataAccess.ExecuteQuery(IDbContextBase)` (lines 20-37) → `GetSourceLocationsDataAccess.ExecuteQueryAsync`

```csharp
        protected override List<SourceLocationModel> ExecuteQuery(IDbContextBase dbcontext)
        {
            var results = dbcontext.Set<FtsSourceLocation>().AsNoTracking().ToList();
            List<SourceLocationModel> sourceLocationList = new List<SourceLocationModel>();
            foreach (var data in results)
            {
                var ftpServer = new FtpServer();
                if (data.FtpServerId != null)
                {
                    ftpServer = data.FtpServer;
                }

                SourceLocationModel sourceData = this.GetSourceLocationData(data, ftpServer);
                sourceLocationList.Add(sourceData);
            }

            return sourceLocationList;
        }
```

#### `GetSourceLocationsDataAccess.GetSourceLocationData(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtpServer)` (lines 41-56) → `GetSourceLocationsDataAccess.GetSourceLocationData`

```csharp
        private SourceLocationModel GetSourceLocationData(FtsSourceLocation sourceLocData, FtpServer ftpServer)
        {
            SourceLocationModel model = this.GetSourceLocationModel(sourceLocData);

            this.UpdateWeeklyScheduleData(ref model, sourceLocData);

            this.UpdateMonthlyScheduleData(ref model, sourceLocData);

            this.UpdateScheduleStartTime(ref model, sourceLocData);

            this.UpdateScheduleEndTime(ref model, sourceLocData);

            this.UpdateFtpServerData(ref model, ftpServer, sourceLocData.SourceProtocalCode);

            return model;
        }
```

#### `GetSourceLocationsDataAccess.GetSourceLocationModel(HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 58-85) → `GetSourceLocationsDataAccess.GetSourceLocationModel`

```csharp
        [MaintainabilityDetails(ApprovedIndex = 55, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
        private SourceLocationModel GetSourceLocationModel(FtsSourceLocation entitySourceData)
        {
            SourceLocationModel model = new SourceLocationModel();
            model.FtpServerDetail = new FtpServerModel();
            model.Schedule = new SourceScheduleModel()
            {
                DaysToRun = new List<DayOfWeek>(),
                MonthlyProcessDayToRun = new List<int>()
            };
            model.SourceLocationId = entitySourceData.FtsSoureceLocationId;
            model.ConfigurationId = entitySourceData.Configid;
            model.SourceProtocolCode = entitySourceData.SourceProtocalCode;
            model.SourceAddress = entitySourceData.SourceAddress;
            model.FileMask = entitySourceData.SourceFileMask;
            model.OwnerEmail = entitySourceData.OwnerEmail;
            model.OwnerName = entitySourceData.OwnerName;
            model.SendNotificationsIndicator = entitySourceData.SendNotificationsIndicator;
            model.IsActive = entitySourceData.IsValid;
            model.IsSuspended = entitySourceData.IsSuspended;
            model.SuspendedIndChanged = entitySourceData.IsSuspendedChangedDate.HasValue ? entitySourceData.IsSuspendedChangedDate.Value : DateTime.MinValue;
            model.ConfigurationId = entitySourceData.Configid;
            model.ConfigurationName = entitySourceData.ConfigName;
            model.ConfigurationDescription = entitySourceData.Description;
            model.FileDirection = entitySourceData.FileDirection;

            return model;
        }
```

#### `GetSourceLocationsDataAccess.UpdateFtpServerData(ref HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtpServer, string)` (lines 178-188) → `GetSourceLocationsDataAccess.UpdateFtpServerData`

```csharp
        private void UpdateFtpServerData(ref SourceLocationModel sourceModel, FtpServer entityFtpServerData, string protocol)
        {
            if (string.Compare(protocol, ProtocolCodeConstant.UNC, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                sourceModel.FtpServerDetail.FtpServerIp = entityFtpServerData.FtpServerIp;
                sourceModel.FtpServerDetail.FtpUser = entityFtpServerData.FtpUser;
                sourceModel.FtpServerDetail.FtpPassword = entityFtpServerData.FtpPassword;
                sourceModel.FtpServerDetail.FtpPort = entityFtpServerData.FtpPort;
                sourceModel.FtpServerDetail.SSHAuth = entityFtpServerData.SSHAuth;
            }
        }
```

#### `GetSourceLocationsDataAccess.UpdateMonthlyScheduleData(ref HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 87-103) → `GetSourceLocationsDataAccess.UpdateMonthlyScheduleData`

```csharp
        private void UpdateMonthlyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
        {
            if (entitySourceData.RunOnFrstDayOfMonthIndicator)
            {
                sourceModel.Schedule.MonthlyProcessDayToRun.Add(1);
            }

            if (entitySourceData.RunOnLastDayOfMonthIndicator)
            {
                sourceModel.Schedule.MonthlyProcessDayToRun.Add(DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month));
            }

            if (!Convert.ToBoolean(entitySourceData.RunOnThisDayOfMonth))
            {
                sourceModel.Schedule.MonthlyProcessDayToRun.Add(Convert.ToInt32(entitySourceData.RunOnThisDayOfMonth));
            }
        }
```

#### `GetSourceLocationsDataAccess.UpdateScheduleEndTime(ref HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 161-176) → `GetSourceLocationsDataAccess.UpdateScheduleEndTime`

```csharp
        private void UpdateScheduleEndTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
        {
            string endTimeData = entitySourceData.EndTime24HHMM;
            int endTimeHours = 0;
            int endTimeMinutes = 0;
            TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
            if (string.IsNullOrEmpty(endTimeData) == false && endTimeData.Length > 3 &&
                int.TryParse(endTimeData.Substring(0, 2), out endTimeHours) && int.TryParse(endTimeData.Substring(2, 2), out endTimeMinutes))
            {
                sourceModel.Schedule.EndTime = new TimeSpan(endTimeHours, endTimeMinutes, 0);
            }
            else
            {
                sourceModel.Schedule.EndTime = scheduledTime;
            }
        }
```

#### `GetSourceLocationsDataAccess.UpdateScheduleStartTime(ref HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 144-159) → `GetSourceLocationsDataAccess.UpdateScheduleStartTime`

```csharp
        private void UpdateScheduleStartTime(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
        {
            string startTimeData = entitySourceData.StartTime24HHMM;
            int startTimeHours = 0;
            int startTimeMinutes = 0;
            TimeSpan scheduledTime = new TimeSpan(0, 0, 0);
            if (string.IsNullOrEmpty(startTimeData) == false && startTimeData.Length > 3 &&
                int.TryParse(startTimeData.Substring(0, 2), out startTimeHours) && int.TryParse(startTimeData.Substring(2, 2), out startTimeMinutes))
            {
                sourceModel.Schedule.StartTime = new TimeSpan(startTimeHours, startTimeMinutes, 0);
            }
            else
            {
                sourceModel.Schedule.StartTime = scheduledTime;
            }
        }
```

#### `GetSourceLocationsDataAccess.UpdateWeeklyScheduleData(ref HPE.HSP.UA3.FT.Batch.FileTransferMgmt.Contracts.Domain.SourceLocationModel, HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess.Entities.FtsSourceLocation)` (lines 105-142) → `GetSourceLocationsDataAccess.UpdateWeeklyScheduleData`

```csharp
        [MaintainabilityDetails(ApprovedIndex = 57, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
        private void UpdateWeeklyScheduleData(ref SourceLocationModel sourceModel, FtsSourceLocation entitySourceData)
        {
            if (entitySourceData.RunOnMondayIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Monday);
            }

            if (entitySourceData.RunOnTuesdayIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Tuesday);
            }

            if (entitySourceData.RunOnWedIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Wednesday);
            }

            if (entitySourceData.RunOnThursIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Thursday);
            }

            if (entitySourceData.RunOnFriIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Friday);
            }

            if (entitySourceData.RunOnSatIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Saturday);
            }

            if (entitySourceData.RunOnSunIndicator)
            {
                sourceModel.Schedule.DaysToRun.Add(DayOfWeek.Sunday);
            }
        }
```

### `GetSuspendedLocation`: role **application**, target file(s) `GetSuspendedLocationDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\GetSuspendedLocation.cs`)

#### `GetSuspendedLocation.ExecuteQuery(IDbContextBase)` (lines 27-30) → `GetSuspendedLocationDataAccess.ExecuteQueryAsync`

```csharp
        protected override FtsSourceLocation ExecuteQuery(IDbContextBase dbcontext)
        {
            return dbcontext.Set<FtsSourceLocation>().SingleOrDefault(s => s.FtsSoureceLocationId == this.sourceLocationId);
        }
```

### `IDataAccess`: role **infrastructure**, target file(s) `IDataAccess.cs` (legacy `batch\FileTransferManagement\HPE.HSP.UA3.FT.Batch.FileTransferMgmt.DataAccess\DataAccess\IDataAccess.cs`)

#### `IDataAccess` (lines 13-18)

```csharp
    public interface IDataAccess<TdacOutput>
    {
        TdacOutput Execute();

        TdacOutput ExecuteWithNoTransaction();
    }
```
