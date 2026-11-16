namespace FxferService.Domain.Contracts.Exceptions;

public class ExcludedFileExtensions
{
    // F02-034, F02-051: excluded extension list used by mask validation.
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
        msc,
    }
}

