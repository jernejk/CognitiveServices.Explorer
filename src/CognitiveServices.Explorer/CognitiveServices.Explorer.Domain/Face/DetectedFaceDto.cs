using System.Collections.Generic;

namespace CognitiveServices.Explorer.Domain.Face
{
    public class DetectedFaceDto
    {
        public string faceId { get; set; }
        public Facerectangle faceRectangle { get; set; }
    }

    public class Facerectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

}
