using CoreWCF;
using CoreWCF.Channels;

namespace Fxfer.SoapFacade.Host.Soap.Bindings;

public static class SoapBindingFactory
{
    // Contract parity: basicHttpsBinding 4,096,000 bytes; ws2007FederationHttpBinding 2,048,000 bytes.
    public static Binding CreateBasicHttpBinding()
    {
        var binding = new BasicHttpBinding
        {
            Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.None },
            MaxReceivedMessageSize = 4_096_000,
            TransferMode = TransferMode.Streamed
        };

        return binding;
    }

    // Contract parity: basicHttpsBinding 4,096,000 bytes; ws2007FederationHttpBinding 2,048,000 bytes.
    public static Binding CreateBasicHttpsBinding()
    {
        var binding = new BasicHttpBinding
        {
            Security = new BasicHttpSecurity { Mode = BasicHttpSecurityMode.Transport },
            MaxReceivedMessageSize = 4_096_000,
            TransferMode = TransferMode.Streamed
        };

        return binding;
    }

    public static Binding CreateWs2007FederationHttpBinding()
    {
        var binding = new WS2007FederationHttpBinding
        {
            Security = new WSFederationHttpSecurity { Mode = WSFederationHttpSecurityMode.TransportWithMessageCredential },
            MaxReceivedMessageSize = 2_048_000
        };

        return binding;
    }
}
