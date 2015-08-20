using System.Collections.Generic;
using System.Linq;

namespace RingCentral.SDK.Helper
{
    public class PagerHelper
    {
        public List<To> to { get; set; }
        public From from { get; set; }
        public string text { get; set; }

        public PagerHelper(IEnumerable<string> toExtensionNumbers, string fromExtensionNumber, string messageText)
        {

            to = new List<To>();

            foreach (var newNumber in toExtensionNumbers.Select(phoneNumber => new To { extensionNumber = phoneNumber }))
            {
                to.Add(newNumber);
            }

            var fromExtension = new From { extensionNumber = fromExtensionNumber };

            from = fromExtension;

            text = messageText;
        }

        public class To
        {
            public string extensionNumber { get; set; }
        }

        public class From
        {
            public string extensionNumber { get; set; }
        }
    }
}
