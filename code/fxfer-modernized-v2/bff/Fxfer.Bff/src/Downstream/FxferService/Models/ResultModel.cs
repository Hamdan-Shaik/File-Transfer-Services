namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class ResultModel
{
    public CoreEnumerations.Messaging.StatusCodeType StatusCode { get; set; }
    public List<ResultMessageModel> Messages { get; set; } = new();

    public void Initialize()
    {
        StatusCode = CoreEnumerations.Messaging.StatusCodeType.NotFound;
        Messages = new List<ResultMessageModel>();
    }
}

