//-----------------------------------------------------------------------------------------
// This code is the property of Gainwell Technologies, Copyright (c) 2024. All rights reserved. 
//
// Any unauthorized use in whole or in part without written consent is strictly prohibited.
// Violators may be punished to the full extent of the law.
//-----------------------------------------------------------------------------------------
using System.Collections.Generic;

namespace HP.HSP.UA3.FileTransfer.BAS.FileTransferSvc.Contracts.Exceptions
{
    public class ExcludedFileExtensions
    {
        public enum ExcludedFileExtensionList
        {
            exe,
            dll,
            bat,
            cmd,
            vbs,
            ps1,
            jar,
            app,
            com,
            scr,
            msi,
            js,
            wsf,
            reg,
            hta,
            pif,
            vb,
            py,
            rb,
            sh,
            cpl,
            msc
        }
    }
}

