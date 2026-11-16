namespace Fxfer.Bff.Downstream.FxferService.Models;

public sealed class ResponseModel<T>
{
    public T? Data { get; set; }
    public PagingResultModel? PagingResults { get; set; }
    public ResultModel Result { get; set; } = new();
}

