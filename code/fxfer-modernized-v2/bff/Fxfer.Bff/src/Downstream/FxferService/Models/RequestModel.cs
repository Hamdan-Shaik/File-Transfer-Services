namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class RequestModel<T>
{
    public RequestorModel? Requestor { get; set; }
    public T? Data { get; set; }
}

