namespace System.ServiceModel;

public sealed class FaultException<TDetail> : Exception
{
    public FaultException(TDetail detail)
        : base()
    {
        Detail = detail;
    }

    public FaultException(TDetail detail, string message)
        : base(message)
    {
        Detail = detail;
    }

    public TDetail Detail { get; }
}

