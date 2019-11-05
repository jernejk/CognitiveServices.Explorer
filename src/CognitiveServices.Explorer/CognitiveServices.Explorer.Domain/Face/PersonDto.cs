using System.Collections.Generic;

namespace CognitiveServices.Explorer.Domain.Face
{
    public class PersonDto
    {
        public string? PersonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string>? PersistedFaceIds { get; set; }
        public string? UserData { get; set; }
    }
}
