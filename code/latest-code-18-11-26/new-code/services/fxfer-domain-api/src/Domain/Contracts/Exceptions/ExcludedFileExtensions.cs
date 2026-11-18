namespace Fxfer.DomainApi.Contracts.Exceptions;

// Rules: F01-045, F01-065
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

