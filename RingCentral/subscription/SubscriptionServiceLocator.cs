using System;
using System.Threading;

namespace RingCentral.Subscription
{
    public class SubscriptionServiceLocator
    {
        private static readonly Lazy<ISubscriptionService> Implementation =
            new Lazy<ISubscriptionService>(() => CreateSubscriptionService(), LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        ///     Current settings to use
        /// </summary>
        public static ISubscriptionService Current
        {
            get
            {
                ISubscriptionService ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        private static ISubscriptionService CreateSubscriptionService()
        {
#if PORTABLE
            return null;
#else
            return new SubscriptionServiceImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return
                new NotImplementedException(
                    "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}