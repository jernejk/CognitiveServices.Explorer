using System;
using System.Collections.Generic;
using System.Text;

namespace CognitiveServices.Explorer.Domain.Speech
{
    public class SpeechRecognizedResult
    {
        public string RecognitionStatus { get; set; }
        public int Offset { get; set; }
        public int Duration { get; set; }
        public Nbest[] NBest { get; set; }
    }

    public class Nbest
    {
        public float Confidence { get; set; }
        public string Lexical { get; set; }
        public string ITN { get; set; }
        public string MaskedITN { get; set; }
        public string Display { get; set; }
    }

}
