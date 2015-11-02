using System.Collections.Generic;

namespace RingCentral.SDK.Helper
{
    public class SmsHelper
    {
        public List<To> to { get; set; }
        public From from { get; set; }
        public string text { get; set; }

        public SmsHelper(string toPhoneNumber, string fromPhoneNumber, string smsText)
        {

            to = new List<To>();

            var toSms = new To { phoneNumber = toPhoneNumber };

            to.Add(toSms);

            var fromSms = new From { phoneNumber = fromPhoneNumber };

            from = fromSms;

            text = smsText;
        }

        public class To
        {
            public string phoneNumber { get; set; }
        }

        public class From
        {
            public string phoneNumber { get; set; }
        }

    }
}
