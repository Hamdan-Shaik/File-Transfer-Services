using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Fxfer.ExecutionWorker.Application.Helpers;

internal static class WCFBinding
{
    internal enum BindingTypes
    {
        BasicHttpBinding,
        WSHttpBinding,
    }

    internal static Binding ReturnBinding(BindingTypes bindingType)
    {
        return bindingType switch
        {
            BindingTypes.BasicHttpBinding => new BasicHttpBinding(),
            BindingTypes.WSHttpBinding => new WSHttpBinding(),
            _ => throw new ArgumentOutOfRangeException(nameof(bindingType), bindingType, null),
        };
    }
}

