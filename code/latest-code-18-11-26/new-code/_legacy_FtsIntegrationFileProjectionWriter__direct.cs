//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2023. All rights reserved.
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------
using AutoMapper;
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.EventSourcing;
using HPP.Core.BAS.CQRS.Extensions;
using HPP.Core.BAS.CQRS.Interfaces;
using HPP.FileTransfer.BAS.FileTransferSvc.Contracts.Domain;
using HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities;
using System;
using System.Linq;

namespace HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers
{
    public class FtsIntegrationFileProjectionWriter : ProjectionWriterBase<FtsIntegrationFile>,
                                                      IHandleEvent<IntegrationFileAdded>,
                                                        IHandleEvent<IntegrationFileUpdated>
    {
        #region Private Fields
        private IEventStore eventStore;
        #endregion

        #region Constructor
        public FtsIntegrationFileProjectionWriter(IDbContextBase context) : base(context)
        {
            this.eventStore = new EventStore();
        }
        #endregion

        #region EventHandlers
        public void Handle(IntegrationFileAdded integrationFileAdded)
        {
            this.AddFtsIntegrationFile(integrationFileAdded);
            return;
        }

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
        #endregion

        #region Private Methods
        private void AddFtsIntegrationFile(IntegrationFileAdded integrationFileAdded)
        {
            foreach (GroupInfo groupInfo in integrationFileAdded.GroupInfo.Where(e => e.NumberOfRecords > 0))
            {
                this.Add(this.MapFtsIntegrationFile(integrationFileAdded, groupInfo));
            }
        }

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
        #endregion
    }
}

