//------------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2022. All rights reserved. 
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-------------------------------------------------------------------------------------------
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events;
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.Interfaces;
using HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers
{
    public class FtsSourceLocationProjectionWriter : ProjectionWriterBase<FtsSourceLocation>,
                                                                     IHandleEvent<FtsSourceLocationAdded>,
                                                                     IHandleEvent<FtsSourceLocationUpdated>,
                                                                     IHandleEvent<FtsSourceLocationInvalidated>
    {
        #region Private Fields
        private readonly IDbContextBase contextBase;
        private Guid ftsCommonSourceLocationId;
        #endregion

        #region Constructors
        public FtsSourceLocationProjectionWriter(IDbContextBase dataBaseContext)
            : base(dataBaseContext)
        {
            this.contextBase = dataBaseContext;
        }
        #endregion

        #region Public Methods
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
        #endregion

        #region Private Methods
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
        #endregion
    }
}
