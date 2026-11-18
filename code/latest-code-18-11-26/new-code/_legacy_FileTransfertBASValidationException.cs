//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2021. All rights reserved. 
//
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------
using HP.HSP.UA3.Core.BAS.CQRS.Interfaces;
using System;
using System.Runtime.Serialization;

namespace HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions
{
    [DataContract]
    public class FileTransfertBASValidationException
    {
        public FileTransfertBASValidationException(string fieldName, string message)
        {
            this.FieldName = fieldName;
            this.ValidationErrorMessage = message;
        }

        public FileTransfertBASValidationException(string message)
            : this(string.Empty, message)
        {
        }

        public FileTransfertBASValidationException()
            : this(string.Empty)
        {
        }

        [DataMember]
        public string FieldName { get; set; }

        [DataMember]
        public string ValidationErrorMessage { get; set; }
    }
}

