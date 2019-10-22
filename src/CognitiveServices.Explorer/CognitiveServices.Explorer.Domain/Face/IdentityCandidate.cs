namespace CognitiveServices.Explorer.Domain.Face
{
    public class IdentityCandidate
    {
        public string FaceId { get; set; }
        public Candidate[] Candidates { get; set; }
    }

    public class Candidate
    {
        public string PersonId { get; set; }
        public float Confidence { get; set; }
    }

}
