using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveServices.Explorer.Application
{
    public class ServiceCost
    {
        public ServiceCost()
        {
            ServiceName = string.Empty;
            Unit = string.Empty;
        }

        public ServiceCost(string serviceName, float cost, string unit)
        {
            ServiceName = serviceName;
            Cost = cost;
            Unit = unit;
        }

        public static ServiceCost FaceApiTransaction(float cost = 1)
        {
            return new ServiceCost
            {
                ServiceName = "FaceApi",
                Cost = cost,
                Unit = "tr"
            };
        }

        public string ServiceName { get; set; }
        public float Cost { get; set; }
        public string Unit { get; set; }
    }
}
