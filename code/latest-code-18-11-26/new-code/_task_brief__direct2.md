# Brief: port part 1 of 1 of component `backend-data-access-event-handlers-2`

- Deployable: `fxfer-projection-worker` at `workers/fxfer-projection-worker`; build: `dotnet build workers/fxfer-projection-worker`; tests: `dotnet test workers/fxfer-projection-worker`
- The component's folder (architecture.yaml): `workers/fxfer-projection-worker/src/Infrastructure/Projections`
- The component's spec: `technical/components/backend-data-access-event-handlers-2/` (TARGET.md for names, signatures and places; SPEC.md for what each method does; CONTRACT.md)
- 18 legacy methods or classes, 317 legacy lines
- Playbook: `inputs/playbook/port.md`

## What to port

### `FtpServerProjectionWriter`: role **application**, target file(s) `FtpServerProjectionWriter.cs` (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs`)

#### `FtpServerProjectionWriter.AddFtpServer(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerAdded)` (lines 74-82) â†’ `(inlined)` Â· rules F02-035

```csharp
        private void AddFtpServer(FtpServerAdded @event)
        {
            FtpServer newFtpServer = new FtpServer();
            MapperWrapper.Mapper.Map(@event, newFtpServer);
            newFtpServer.CreationDate = DateTime.UtcNow;
            newFtpServer.LastModifiedDate = DateTime.UtcNow;

            this.Add(newFtpServer);
        }
```

#### `FtpServerProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerAdded)` (lines 33-36) â†’ `FtpServerProjectionWriter.HandleFtpServerInvalidatedAsync(...)`

```csharp
        public void Handle(FtpServerAdded @event)
        {
            this.AddFtpServer(@event);
        }
```

#### `FtpServerProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerInvalidated)` (lines 60-70) â†’ `FtpServerProjectionWriter.HandleFtpServerInvalidatedAsync(...)` Â· rules F02-037

```csharp
        public void Handle(FtpServerInvalidated @event)
        {
            this.Update(
                        @event.FtpServerId,
                        e =>
                        {
                            e.IsValid = @event.IsValid;
                            e.OperatorId = @event.OperatorId;
                            e.LastModifiedDate = DateTime.UtcNow;
                        });
        }
```

#### `FtpServerProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtpServerUpdated)` (lines 38-58) â†’ `FtpServerProjectionWriter.HandleFtpServerInvalidatedAsync(...)` Â· rules F02-036

```csharp
        [MaintainabilityDetails(ApprovedIndex = 58, ReviewedOnSprint = 96, ReviewerName = "Hector Murga")]
        public void Handle(FtpServerUpdated @event)
        {
            this.Update(
                        @event.FtpServerId,
                        e =>
                        {
                            e.FtpDescription = @event.FtpDescription;
                            e.FtpServerIp = @event.FtpServerIp;
                            e.FtpServerName = @event.FtpServerName;
                            e.FtpPassword = @event.FtpPassword;
                            e.FtpPort = @event.FtpPort;
                            e.FtpProxyAddress = @event.FtpProxyAddress;
                            e.FtpProxyPort = @event.FtpProxyPort;
                            e.FtpTransferMode = @event.FtpTransferMode;
                            e.FtpUser = @event.FtpUser;
                            e.UseSSHAuth = @event.UseSSHAuth;
                            e.LastModifiedDate = DateTime.UtcNow;
                            e.OperatorId = @event.OperatorId;
                        });
        }
```

### `FtsInboundTargetProjectionWriter`: role **application**, target file(s) `FtsInboundTargetProjectionWriter.cs` (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsInboundTargetProjectionWriter.cs`)

#### `FtsInboundTargetProjectionWriter.AddFtsInboundTarget(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetAdded)` (lines 52-55) â†’ `(inlined)`

```csharp
        private void AddFtsInboundTarget(FtsInboundTargetAdded ftsInboundTargetAdded)
        {
            this.Add(this.MapFtsInboundTarget(ftsInboundTargetAdded));
        }
```

#### `FtsInboundTargetProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetAdded)` (lines 32-36) â†’ `FtsInboundTargetProjectionWriter.HandleInboundTargetInvalidatedAsync(...)`

```csharp
        public void Handle(FtsInboundTargetAdded ftsInboundTargetAdded)
        {
            this.AddFtsInboundTarget(ftsInboundTargetAdded);
            return;
        }
```

#### `FtsInboundTargetProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetInvalidated)` (lines 38-48) â†’ `FtsInboundTargetProjectionWriter.HandleInboundTargetInvalidatedAsync(...)`

```csharp
        public void Handle(FtsInboundTargetInvalidated ftsInboundTargetInvalidated)
        {
            this.Update(
                   ftsInboundTargetInvalidated.FtsInboundTargetId,
                   e =>
                   {
                       e.IsValid = false;
                       e.LastModifiedDate = DateTime.UtcNow;
                       e.OperatorId = ftsInboundTargetInvalidated.UserID;
                   });
        }
```

#### `FtsInboundTargetProjectionWriter.MapFtsInboundTarget(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsInboundTargetAdded)` (lines 57-69) â†’ `(optional helper)`

```csharp
        private FtsInboundTarget MapFtsInboundTarget(FtsInboundTargetAdded ftsInboundTargetAdded)
        {
            FtsInboundTarget ftsInboundTarget = new FtsInboundTarget();
            MapperWrapper.Mapper.Map(ftsInboundTargetAdded, ftsInboundTarget);
            ftsInboundTarget.FtsInboundTargetId = Guid.NewGuid();
            ftsInboundTarget.FTEMetadataId = ftsInboundTargetAdded.FTEMetadataId;
            ftsInboundTarget.ImportJobName = ftsInboundTargetAdded.ImportJobName;
            ftsInboundTarget.ContractName = ftsInboundTargetAdded.ContractName;
            ftsInboundTarget.OperatorId = ftsInboundTargetAdded.OperatorId;
            ftsInboundTarget.CreationDate = DateTime.UtcNow;
            ftsInboundTarget.LastModifiedDate = DateTime.UtcNow;
            return ftsInboundTarget;
        }
```

### `FtsIntegrationFileProjectionWriter`: role **application**, target file(s) `FtsIntegrationFileProjectionWriter.cs` (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsIntegrationFileProjectionWriter.cs`)

#### `FtsIntegrationFileProjectionWriter.AddFtsIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileAdded)` (lines 57-63) â†’ `(inlined)`

```csharp
        private void AddFtsIntegrationFile(IntegrationFileAdded integrationFileAdded)
        {
            foreach (GroupInfo groupInfo in integrationFileAdded.GroupInfo.Where(e => e.NumberOfRecords > 0))
            {
                this.Add(this.MapFtsIntegrationFile(integrationFileAdded, groupInfo));
            }
        }
```

#### `FtsIntegrationFileProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileAdded)` (lines 35-39) â†’ `FtsIntegrationFileProjectionWriter.HandleIntegrationFileUpdatedAsync(...)`

```csharp
        public void Handle(IntegrationFileAdded integrationFileAdded)
        {
            this.AddFtsIntegrationFile(integrationFileAdded);
            return;
        }
```

#### `FtsIntegrationFileProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileUpdated)` (lines 41-53) â†’ `FtsIntegrationFileProjectionWriter.HandleIntegrationFileUpdatedAsync(...)`

```csharp
        public void Handle(IntegrationFileUpdated integrationFileUpdated)
        {
            this.Update(
                   integrationFileUpdated.FtsIntegrationFileId,
                   e =>
                   {
                       e.ConfigId = integrationFileUpdated.ConfigId;
                       e.IsProcessed = true;
                       e.IsValid = integrationFileUpdated.IsValid;
                       e.LastModifiedDate = DateTime.UtcNow;
                       e.OperatorId = integrationFileUpdated.OperatorId;
                   });
        }
```

#### `FtsIntegrationFileProjectionWriter.MapFtsIntegrationFile(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.IntegrationFileAdded, HPP.FileTransfer.BAS.FileTransferSvc.Contracts.Domain.GroupInfo)` (lines 65-80) â†’ `(optional helper)`

```csharp
        private FtsIntegrationFile MapFtsIntegrationFile(IntegrationFileAdded integrationFileAdded, GroupInfo groupInfo)
        {
            FtsIntegrationFile ftsIntegrationFile = new FtsIntegrationFile();
            MapperWrapper.Mapper.Map(integrationFileAdded, ftsIntegrationFile);
            ftsIntegrationFile.FtsIntegrationFileId = Guid.NewGuid();
            ftsIntegrationFile.GroupId = groupInfo.GroupId;
            ftsIntegrationFile.ConfigId = null;
            ftsIntegrationFile.FileName = integrationFileAdded.FileName;
            ftsIntegrationFile.ETFOutboundMetadataDefnId = integrationFileAdded.ETFOutboundMetadataId;
            ftsIntegrationFile.IsProcessed = false;
            ftsIntegrationFile.CreationDate = DateTime.UtcNow;
            ftsIntegrationFile.LastModifiedDate = DateTime.UtcNow;
            ftsIntegrationFile.IsValid = true;
            ftsIntegrationFile.OperatorId = integrationFileAdded.OperatorId;
            return ftsIntegrationFile;
        }
```

### `FtsSourceLocationProjectionWriter`: role **application**, target file(s) `FtsSourceLocationProjectionWriter.cs` (legacy `bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs`)

#### `FtsSourceLocationProjectionWriter.AddDestination(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsDestinationLocationAdded, string)` (lines 191-216) â†’ `FtsSourceLocationProjectionWriter.AddDestination(...)`

```csharp
        private void AddDestination(FtsDestinationLocationAdded dest, string userId)
        {
            this.contextBase.Set<FtsDestinationLocations>().Add(new FtsDestinationLocations()
            {
                DestinationId = Guid.NewGuid(),
                FtsSourceLocationId = this.ftsCommonSourceLocationId,
                DestinationAddress = dest.DestinationAddress,
                DestProtocolCD = dest.DestinationProtocol,
                OverwriteAtDest = dest.OverWriteAtDestination,
                CompressAtDest = dest.CompressAtDestination,
                ReNameAtDest = dest.RenameAtDestination,
                DestIsArchive = dest.DestinationArchive,
                CreationDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                IsValid = dest.IsValid,
                FtpServerId = dest.FtpServerId,
                OperatorId = dest.OperatorId,
                RenamePattern = dest.RenamePattern,
                ContactName = dest.ContactName,
                ContactPhone = dest.ContactPhone,
                ContactEmail = dest.ContactEmail,
                FTEMetadataId = dest.FTEMetadataId,
                DestinationType = dest.DestinationType,
                ContractName = dest.ContractName
            });
        }
```

#### `FtsSourceLocationProjectionWriter.AddDestinationAddress(System.Collections.Generic.List<HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsDestinationLocationAdded>, string)` (lines 165-187) â†’ `FtsSourceLocationProjectionWriter.AddDestinationAddressAsync(...)`

```csharp
        private void AddDestinationAddress(List<FtsDestinationLocationAdded> destinationAddres, string userId)
        {
            var existingDestination = from result in this.contextBase.Set<FtsDestinationLocations>()
                                      select new
                                      {
                                          DestinationId = result.DestinationId
                                      };

            foreach (FtsDestinationLocationAdded destAddress in destinationAddres)
            {
                var destinationAddress = existingDestination.Where(x => x.DestinationId == destAddress.DestinationId).FirstOrDefault();
                if (destinationAddress != null)
                {
                    this.UpdateDestination(destAddress, userId);
                }
                else
                {
                    this.AddDestination(destAddress, userId);
                }
            }

            this.contextBase.SaveChanges();
        }
```

#### `FtsSourceLocationProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationAdded)` (lines 36-87) â†’ `FtsSourceLocationProjectionWriter.HandleSourceLocationInvalidatedAsync(...)`

```csharp
        public void Handle(FtsSourceLocationAdded @event)
        {
            this.ftsCommonSourceLocationId = Guid.NewGuid();
            this.Add(new FtsSourceLocation()
            {
                FtsSourceLocationId = this.ftsCommonSourceLocationId,
                Configid = @event.ConfigId,
                ConfigName = @event.ConfigName,
                Description = @event.Description,
                FtpServerId = string.IsNullOrEmpty(@event.FtpServerId) ? (Guid?)null : Guid.Parse(@event.FtpServerId),
                SourceProtocalCode = @event.Protocol,
                SourceAddress = @event.Address,
                SourceFileMask = @event.Mask,
                FileDirection = @event.FileDirection,
                SendNotificationsIndicator = @event.SendNotifications,
                IsValid = @event.IsValid,
                IsSuspended = @event.IsSuspended,
                IsSuspendedChangedDate = @event.IsSuspendedChangedDate,
                OwnerName = @event.OwnerName,
                OwnerModule = Guid.Parse(@event.OwnerModule),
                OwnerPhone = @event.OwnerPhone,
                OwnerEmail = @event.OwnerEmail,
                RunOnMondayIndicator = @event.RunOnMonday,
                RunOnTuesdayIndicator = @event.RunOnTuesday,
                RunOnWedIndicator = @event.RunOnWed,
                RunOnThursIndicator = @event.RunOnThrus,
                RunOnFriIndicator = @event.RunOnFri,
                RunOnSatIndicator = @event.RunOnSat,
                RunOnSunIndicator = @event.RunOnSun,
                RunOnFrstDayOfMonthIndicator = @event.RunOnFrstDayOfMonthInd,
                RunOnLastDayOfMonthIndicator = @event.RunOnLastDayOfMonthInd,
                RunOnThisDayOfMonth = @event.RunOnThisDayOfMonth,
                StartTime24HHMM = @event.StartTime,
                EndTime24HHMM = @event.EndTime,
                TradingPartner = @event.TradingPartner,
                CreationDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                OperatorId = @event.OperatorId
            });

            foreach (FtsDestinationLocationAdded dest in @event.DestinationAddress)
            {
                if (!@event.IsValid)
                {
                    dest.IsValid = false;
                }

                this.AddDestination(dest, null);
            }

            this.contextBase.SaveChanges();
        }
```

#### `FtsSourceLocationProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationInvalidated)` (lines 142-163) â†’ `FtsSourceLocationProjectionWriter.HandleSourceLocationInvalidatedAsync(...)` Â· rules F01-076, F01-077

```csharp
        [MaintainabilityDetails(ApprovedIndex = 58, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
        public void Handle(FtsSourceLocationInvalidated @event)
        {
            this.ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId);
            this.Update(
                 Guid.Parse(@event.FtsSourceLocationsId),
                 e =>
                 {
                     e.OperatorId = @event.OperatorId;
                     e.IsValid = false;
                 });

            var sourceLocationId = Guid.Parse(@event.FtsSourceLocationsId);

            if (@event.DestinationAddress != null)
            {
                this.AddDestinationAddress(@event.DestinationAddress, @event.UserID);
            }

            this.DbContext.Set<FtsDestinationLocations>().Where(f => f.FtsSourceLocationId == sourceLocationId).ToList().ForEach(a => { a.IsValid = false; a.OperatorId = @event.OperatorId; });
            this.contextBase.SaveChanges();
        }
```

#### `FtsSourceLocationProjectionWriter.Handle(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsSourceLocationUpdated)` (lines 89-140) â†’ `FtsSourceLocationProjectionWriter.HandleSourceLocationInvalidatedAsync(...)` Â· rules F01-073, F01-074, F01-075

```csharp
        public void Handle(FtsSourceLocationUpdated @event)
        {
            this.ftsCommonSourceLocationId = new Guid(@event.FtsSourceLocationsId);
            this.Update(
               Guid.Parse(@event.FtsSourceLocationsId),
               e =>
               {
                   e.Configid = @event.ConfigId;
                   e.FtsSourceLocationId = this.ftsCommonSourceLocationId;
                   e.ConfigName = @event.ConfigName;
                   e.Description = @event.Description;
                   e.FtpServerId = string.IsNullOrEmpty(@event.FtpServerId) ? (Guid?)null : Guid.Parse(@event.FtpServerId);
                   e.SourceProtocalCode = @event.Protocol;
                   e.SourceAddress = @event.Address;
                   e.SourceFileMask = @event.Mask;
                   e.FileDirection = @event.FileDirection;
                   e.SendNotificationsIndicator = @event.SendNotifications;
                   e.IsSuspended = @event.IsSuspended;
                   e.IsSuspendedChangedDate = @event.IsSuspendedChangedDate;
                   e.IsValid = @event.IsValid;
                   e.OwnerName = @event.OwnerName;
                   e.OwnerModule = Guid.Parse(@event.OwnerModule);
                   e.OwnerPhone = @event.OwnerPhone;
                   e.OwnerEmail = @event.OwnerEmail;
                   e.RunOnMondayIndicator = @event.RunOnMonday;
                   e.RunOnTuesdayIndicator = @event.RunOnTuesday;
                   e.RunOnWedIndicator = @event.RunOnWed;
                   e.RunOnThursIndicator = @event.RunOnThrus;
                   e.RunOnFriIndicator = @event.RunOnFri;
                   e.RunOnSatIndicator = @event.RunOnSat;
                   e.RunOnSunIndicator = @event.RunOnSun;
                   e.RunOnFrstDayOfMonthIndicator = @event.RunOnFrstDayOfMonthInd;
                   e.RunOnLastDayOfMonthIndicator = @event.RunOnLastDayOfMonthInd;
                   e.RunOnThisDayOfMonth = @event.RunOnThisDayOfMonth;
                   e.StartTime24HHMM = @event.StartTime;
                   e.EndTime24HHMM = @event.EndTime;
                   e.TradingPartner = @event.TradingPartner;
                   e.LastModifiedDate = DateTime.UtcNow;
                   e.OperatorId = @event.OperatorId;
                   e.IsValid = @event.IsValid;
               });

            if (@event.FtsSourceLocationsId.ToString() != null)
            {
                this.AddDestinationAddress(@event.DestinationAddress, @event.UserID);
                if (!@event.IsValid)
                {
                    this.DbContext.Set<FtsDestinationLocations>().Where(f => f.FtsSourceLocationId == this.ftsCommonSourceLocationId).ToList().ForEach(a => { a.IsValid = false; a.OperatorId = @event.OperatorId; });
                    this.contextBase.SaveChanges();
                }
            }
        }
```

#### `FtsSourceLocationProjectionWriter.UpdateDestination(HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events.FtsDestinationLocationAdded, string)` (lines 218-240) â†’ `FtsSourceLocationProjectionWriter.UpdateDestinationAsync(...)`

```csharp
        [MaintainabilityDetails(ApprovedIndex = 54, ReviewedOnSprint = 94, ReviewerName = "Josue Chavez")]
        private void UpdateDestination(FtsDestinationLocationAdded dest, string userId)
        {
            var destAddress = this.contextBase.Set<FtsDestinationLocations>().Where(x => x.DestinationId == dest.DestinationId).FirstOrDefault();

            destAddress.DestinationAddress = dest.DestinationAddress;
            destAddress.DestProtocolCD = dest.DestinationProtocol;
            destAddress.OverwriteAtDest = dest.OverWriteAtDestination;
            destAddress.CompressAtDest = dest.CompressAtDestination;
            destAddress.ReNameAtDest = dest.RenameAtDestination;
            destAddress.DestIsArchive = dest.DestinationArchive;
            destAddress.LastModifiedDate = DateTime.UtcNow;
            destAddress.IsValid = dest.IsValid;
            destAddress.FtpServerId = dest.FtpServerId;
            destAddress.OperatorId = dest.OperatorId;
            destAddress.RenamePattern = dest.RenamePattern;
            destAddress.ContactName = dest.ContactName;
            destAddress.ContactPhone = dest.ContactPhone;
            destAddress.ContactEmail = dest.ContactEmail;
            destAddress.FTEMetadataId = dest.FTEMetadataId;
            destAddress.DestinationType = dest.DestinationType;
            destAddress.ContractName = dest.ContractName;
        }
```

## The rules these methods implement

**F01-073** When applying a `FtsSourceLocationUpdated` event, the flow updates the persisted source location identified by `FtsSourceLocationsId` and sets its fields directly from the event, including `IsSuspended`, `IsSuspendedChangedDate`, `IsValid`, and `OperatorId`, and updates `LastModifiedDate` to the current UTC timestamp.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:89-132, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:100, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:110, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:126
- Legacy: `FtpServerId` is stored as null when the event's `FtpServerId` is empty; otherwise the code parses it as a GUID.
- âš‘ Flag: `OwnerModule` and non-empty `FtpServerId` are parsed using `Guid.Parse(...)` and will throw if the event contains a non-GUID string.
- Decision: keep

**F01-074** When applying a `FtsSourceLocationUpdated` event, the flow attempts to upsert destination locations by calling `AddDestinationAddress(@event.DestinationAddress, @event.UserID)` whenever `FtsSourceLocationsId` is non-null.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:129-138, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:133, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:165-188
- Legacy: The update path does not check whether `DestinationAddress` is null before calling `AddDestinationAddress(...)`.
- âš‘ Flag: A null `DestinationAddress` list will raise an exception during `foreach` in `AddDestinationAddress(...)`.
- Decision: keep

**F01-075** When applying a `FtsSourceLocationUpdated` event with `IsValid == false`, the flow marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-update-file-transfer-source-location, file-transfer-svc-service-update-file-transfer-source-location-2, file-transfer-svc-service-update-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:134-137, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:136
- Legacy: The flow saves changes after updating destination validity.
- Decision: keep

**F01-076** When applying a `FtsSourceLocationInvalidated` event, the flow marks the source location identified by `FtsSourceLocationsId` as invalid (`IsValid = false`) and stores `OperatorId` from the event.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:143-156
- Legacy: The invalidation projection does not update `LastModifiedDate`.
- Decision: keep

**F01-077** When applying a `FtsSourceLocationInvalidated` event, if `DestinationAddress` is provided the flow upserts destination locations from that list and then marks every destination location row linked to the source location as invalid (`IsValid = false`) and sets each row's `OperatorId` to the event's `OperatorId`.
- Doors: file-transfer-svc-service-invalidate-file-transfer-source-location, file-transfer-svc-service-invalidate-file-transfer-source-location-2, file-transfer-svc-service-invalidate-file-transfer-source-location-3
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:154-163, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:158-162, bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtsSourceLocationProjectionWriter.cs:161
- Legacy: Any destination rows upserted in this step are immediately marked invalid in the same handler.
- Decision: keep

**F02-035** When a `FtpServerAdded` event is handled, a new FTP server row is created with `CreationDate` and `LastModifiedDate` set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-add-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:74-82
- Legacy: The projection writer maps the event to a new `FtpServer` entity, sets both timestamps to `DateTime.UtcNow`, and calls `Add(newFtpServer)`.
- Decision: keep

**F02-036** When a `FtpServerUpdated` event is handled, the existing FTP server row is updated by id and `LastModifiedDate` is set to `DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-update-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:39-57
- Legacy: The projection writer updates all FTP server fields from the event (including credentials and proxy fields), sets `LastModifiedDate = DateTime.UtcNow`, and sets `OperatorId = @event.OperatorId`.
- Decision: keep

**F02-037** When a `FtpServerInvalidated` event is handled, the existing FTP server row is updated by id by setting `IsValid` (to false), `OperatorId`, and `LastModifiedDate = DateTime.UtcNow`.
- Doors: file-transfer-svc-service-controller-invalidate-ftp-server
- Source: bas\FileTransferSvc\HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69, bas\FileTransferSvc\HPP.FileTransfer.BAS.FileTransferSvc.DataAccess\EventHandlers\FtpServerProjectionWriter.cs:60-69
- Legacy: The projection writer sets `IsValid = @event.IsValid`, sets `OperatorId = @event.OperatorId`, and sets `LastModifiedDate = DateTime.UtcNow`.
- Decision: keep

