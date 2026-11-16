using System.Diagnostics.Tracing;

namespace FxferService.Application.Hosting;

public static class Keywords
{
    public const EventKeywords Requests = (EventKeywords)0x1L;
    public const EventKeywords ServiceInitialization = (EventKeywords)0x2L;
}
