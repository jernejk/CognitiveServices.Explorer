using System;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Domain.Forms
{

    public class FormDto
    {
        public string status { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastUpdatedDateTime { get; set; }
        public Analyzeresult analyzeResult { get; set; }
        public ErrorDto error { get; set; }
    }

    public class ErrorDto
    {
        public string code;
        public string message;
    }

    public class Analyzeresult
    {
        public string version { get; set; }
        public Readresult[] readResults { get; set; }
        public Pageresult[] pageResults { get; set; }
        public Documentresult[] documentResults { get; set; }
        public object[] errors { get; set; }
    }

    public class Readresult
    {
        public int page { get; set; }
        public string language { get; set; }
        public int angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
        public Line[] lines { get; set; }
    }

    public class Line
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
        public Word[] words { get; set; }
    }

    public class Word
    {
        public int[] boundingBox { get; set; }
        public string text { get; set; }
        public float confidence { get; set; }
    }

    public class Pageresult
    {
        public int page { get; set; }
        public Table[] tables { get; set; }
    }

    public class Table
    {
        public int rows { get; set; }
        public int columns { get; set; }
        public Cell[] cells { get; set; }
    }

    public class Cell
    {
        public int rowIndex { get; set; }
        public int columnIndex { get; set; }
        public int columnSpan { get; set; }
        public string text { get; set; }
        public int[] boundingBox { get; set; }
        public string[] elements { get; set; }
    }

    public class Documentresult
    {
        public string docType { get; set; }
        public int[] pageRange { get; set; }
        public Dictionary<string, FieldData> fields { get; set; }
    }

    public class FieldData
    {
        public string type { get; set; }
        public string valueString { get; set; }
        public string text { get; set; }
        public int page { get; set; }
        public float[] boundingBox { get; set; }
        public float confidence { get; set; }
        public string[] elements { get; set; }
    }

    public class Position
    {
        public string type { get; set; }
        public string valueString { get; set; }
        public string text { get; set; }
        public int page { get; set; }
        public float[] boundingBox { get; set; }
        public float confidence { get; set; }
        public string[] elements { get; set; }
    }
}
