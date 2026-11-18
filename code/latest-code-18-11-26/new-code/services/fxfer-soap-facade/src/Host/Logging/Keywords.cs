using System.Diagnostics.Tracing;

namespace Fxfer.SoapFacade.Host.Logging;

public static class Keywords
{
    public const EventKeywords Requests = (EventKeywords)0x1L;
    public const EventKeywords ServiceInitialization = (EventKeywords)0x2L;
}

