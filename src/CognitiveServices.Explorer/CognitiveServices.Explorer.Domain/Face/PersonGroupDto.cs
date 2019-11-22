using System.Runtime.Serialization;

namespace CognitiveServices.Explorer.Domain.Face
{
    [DataContract]
    public class PersonGroupDto
    {
        [DataMember(Name = "personGroupId")]
        public string PersonGroupId { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "userData")]
        public string UserData { get; set; }

        [DataMember(Name = "recognitionModel")]
        public string RecognitionModel { get; set; }
    }
}
