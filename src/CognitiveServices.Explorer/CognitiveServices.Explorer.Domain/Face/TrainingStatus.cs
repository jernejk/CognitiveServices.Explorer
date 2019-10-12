using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveServices.Explorer.Domain.Face
{
    public class TrainingStatus
    {
        public string Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastActionDateTime { get; set; }
        public string Message { get; set; }
    }
}
