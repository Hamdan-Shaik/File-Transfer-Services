//--------------------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2023. All rights reserved. 
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//--------------------------------------------------------------------------------------------------
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Events;
using HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Utilities;
using HPP.Core.BAS.CQRS.Base;
using HPP.Core.BAS.CQRS.Extensions;
using HPP.Core.BAS.CQRS.Interfaces;
using HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.Entities;
using System;

namespace HPP.FileTransfer.BAS.FileTransferSvc.DataAccess.EventHandlers
{
    public class FtpServerProjectionWriter : ProjectionWriterBase<FtpServer>,
                                            IHandleEvent<FtpServerAdded>,
                                            IHandleEvent<FtpServerUpdated>,
                                            IHandleEvent<FtpServerInvalidated>
    {
        #region Private Fields
        private IDbContextBase contextBase;
        #endregion

        #region Constructors
        public FtpServerProjectionWriter(IDbContextBase context) : base(context)
        {
            this.contextBase = context;
        }
        #endregion

        #region Event Handlers
        public void Handle(FtpServerAdded @event)
        {
            this.AddFtpServer(@event);
        }

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
        #endregion

        #region Private Methods
        private void AddFtpServer(FtpServerAdded @event)
        {
            FtpServer newFtpServer = new FtpServer();
            MapperWrapper.Mapper.Map(@event, newFtpServer);
            newFtpServer.CreationDate = DateTime.UtcNow;
            newFtpServer.LastModifiedDate = DateTime.UtcNow;

            this.Add(newFtpServer);
        }
        #endregion
    }
}

