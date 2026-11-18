using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Fxfer.TransferWorker.Application.Helpers;

public static class WCFBinding
{
    public enum BindingTypes
    {
        BasicHttpBinding,
        BasicHttpsBinding,
    }

    public static Binding ReturnBinding(BindingTypes bindingType)
    {
        return bindingType switch
        {
            BindingTypes.BasicHttpBinding => new BasicHttpBinding(),
            BindingTypes.BasicHttpsBinding => new BasicHttpsBinding(),
            _ => throw new ArgumentOutOfRangeException(nameof(bindingType), bindingType, "Unknown binding type."),
        };
    }
}