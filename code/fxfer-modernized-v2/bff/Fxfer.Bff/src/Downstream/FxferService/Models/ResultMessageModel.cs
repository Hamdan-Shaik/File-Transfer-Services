namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class ResultMessageModel
{
    public ResultMessageModel(CoreEnumerations.Messaging.MessageType type, string message)
    {
        Type = type;
        Message = message;
    }

    public CoreEnumerations.Messaging.MessageType Type { get; }
    public string Message { get; }
}

