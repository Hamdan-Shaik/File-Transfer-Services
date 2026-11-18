//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2021. All rights reserved. 
//
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------
using HP.HSP.UA3.Core.BAS.CQRS.Interfaces;
using System;
using System.ServiceModel;

namespace HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions
{
    public class FileTransferValidationException : Exception, IFaultGenerator
    {
        public FileTransferValidationException(string fieldName, string validationMessage, string message)
            : base(message)
        {
            this.FieldName = fieldName;
            this.ValidationErrorMessage = validationMessage;
        }

        public FileTransferValidationException(string fieldName, string validationMessage)
            : this(fieldName, validationMessage, string.Empty)
        {
        }

        public FileTransferValidationException(string validationMessage)
            : this(string.Empty, validationMessage)
        {
        }

        public FileTransferValidationException()
            : this(string.Empty)
        {
        }

        public string FieldName { get; set; }

        public string ValidationErrorMessage { get; set; }

        public FaultException GenerateFault()
        {
            return new FaultException<FileTransfertBASValidationException>(new FileTransfertBASValidationException(this.FieldName, this.ValidationErrorMessage), new FaultReason(this.ValidationErrorMessage));
        }
    }
}

