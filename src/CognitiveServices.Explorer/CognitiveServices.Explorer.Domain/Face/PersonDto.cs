using System.Collections.Generic;

namespace CognitiveServices.Explorer.Domain.Face
{
    public class PersonDto
    {
        /*
         * 
        {
    "personId": "25985303-c537-4467-b41d-bdb45cd95ca1",
    "persistedFaceIds": [
        "015839fb-fbd9-4f79-ace9-7675fc2f1dd9",
        "fce92aed-d578-4d2e-8114-068f8af4492e",
        "b64d5e15-8257-4af2-b20a-5a750f8940e7"
    ],
    "name": "Ryan",
    "userData": "User-provided data attached to the person."
}
         */

        public string PersonId { get; set; }
        public string Name { get; set; }
        public List<string> PersistedFaceIds { get; set; }
        public string UserData { get; set; }
    }
}
