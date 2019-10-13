using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveServices.Explorer.Domain.Face
{
    public class CurlRequestData
    {
        public string RequestName { get; set; }

        public string Curl { get; set; }

        public string SubscriptionKey { get; set; }

        public string DisplayCurl(bool showSubscriptionKey)
        {
            string subscriptionKey = showSubscriptionKey ? SubscriptionKey : "***";
            return Curl?.Replace("[token]", subscriptionKey ?? string.Empty);
        }
    }
}
