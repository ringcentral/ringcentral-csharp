using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceLocator
    {
        static Lazy<ISubscriptionService> Implementation = new Lazy<ISubscriptionService>(() => CreateSubscriptionService(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

    /// <summary>
    /// Current settings to use
    /// </summary>
    public static ISubscriptionService Current
    {
      get
      {
        var ret = Implementation.Value;
        if (ret == null)
        {
          throw NotImplementedInReferenceAssembly();
        }
        return ret;
      }
    }

    static ISubscriptionService CreateSubscriptionService()
    {
#if PORTABLE
        return null;
#else
        return new SubscriptionServiceImplementation("","");
#endif
    }

    internal static Exception NotImplementedInReferenceAssembly()
    {
      return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
  }
    
}
