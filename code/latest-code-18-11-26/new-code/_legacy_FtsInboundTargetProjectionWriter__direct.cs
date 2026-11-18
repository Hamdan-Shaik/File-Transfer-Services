//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2022. All rights reserved.
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.EventSourcing;
using HPP.Core.BAS.CQRS.Extensions;
using HPP.Core.BAS.CQRS.Interfaces;
using HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities;
using System;

namespace HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers
{
    public class FtsInboundTargetProjectionWriter : ProjectionWriterBase<FtsInboundTarget>,
                                                      IHandleEvent<FtsInboundTargetAdded>,
                                                       IHandleEvent<FtsInboundTargetInvalidated>
    {
        #region Private Fields
        private IEventStore eventStore;
        #endregion

        #region Constructor
        public FtsInboundTargetProjectionWriter(IDbContextBase context) : base(context)
        {
            this.eventStore = new EventStore();
        }
        #endregion

        #region EventHandlers
        public void Handle(FtsInboundTargetAdded ftsInboundTargetAdded)
        {
            this.AddFtsInboundTarget(ftsInboundTargetAdded);
            return;
        }

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
        #endregion

        #region Private Methods
        private void AddFtsInboundTarget(FtsInboundTargetAdded ftsInboundTargetAdded)
        {
            this.Add(this.MapFtsInboundTarget(ftsInboundTargetAdded));
        }

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
        #endregion
    }
}

