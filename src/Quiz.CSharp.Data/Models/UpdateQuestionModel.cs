using System.Text.Json.Serialization;

namespace Quiz.CSharp.Data.Models;

public class UpdateQuestionModel
{
    public int Id { get; set; }
    public required string Subcategory { get; set; }
    public required string Difficulty { get; set; }
    public required string Prompt { get; set; }
    
    public int EstimatedTimeMinutes { get; set; }
    
    public required UpdateQuestionMetadataBase Metadata { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(CodeWritingMetadata), typeDiscriminator: "code_writing")]
[JsonDerivedType(typeof(ErrorSpottingMetadata), typeDiscriminator: "error_spotting")]
[JsonDerivedType(typeof(FillMetadata), typeDiscriminator: "fill")]
[JsonDerivedType(typeof(McqMetadata), typeDiscriminator: "mcq")]
[JsonDerivedType(typeof(OutputPredictionMetadata), typeDiscriminator: "output_prediction")]
[JsonDerivedType(typeof(TrueFalseMetadata), typeDiscriminator: "true_false")]
public abstract class UpdateQuestionMetadataBase
{
    public List<QuestionHint> Hints { get; set; } = [];
    public string CodeAfter { get; set; }  = string.Empty;
    public string CodeBefore { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
}

public class CodeWritingMetadata : UpdateQuestionMetadataBase
{
    public List<string> Rubric { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public string Solution { get; set; } = string.Empty;
    public List<string> TestCases { get; set; } = [];
}

public class ErrorSpottingMetadata : UpdateQuestionMetadataBase
{
    public string CodeWithError { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

public class FillMetadata : UpdateQuestionMetadataBase
{
    public List<string> FillHints { get; set; } = [];
    public string CodeWithBlank { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; }  = string.Empty;
}

public class McqMetadata : UpdateQuestionMetadataBase
{
    public List<McqOptionDto> Options { get; set; } = [];
    public List<string> CorrectAnswerIds { get; set; } = [];
}

public class McqOptionDto
{
    public required string Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
}

public class OutputPredictionMetadata : UpdateQuestionMetadataBase
{
    public string Snippet { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
}

public class TrueFalseMetadata : UpdateQuestionMetadataBase
{
    public bool CorrectAnswer { get; set; }
}

public class QuestionHint
{
    public required string Hint { get; set; }
    public int OrderIndex { get; set; }
} 
