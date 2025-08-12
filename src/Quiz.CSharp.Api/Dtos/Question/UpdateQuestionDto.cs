using System.Text.Json.Serialization;

namespace Quiz.CSharp.Api.Dtos.Question;

public class UpdateQuestionDto
{
    public required string Subcategory { get; set; }
    public required string Difficulty { get; set; }
    public required string Prompt { get; set; }
    public int EstimatedTimeMinutes { get; set; }

    public required UpdateQuestionMetaDataDto Metadata { get; set; }
    public bool IsActive { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(UpdateMcqMetaDataDto), "mcq")]
[JsonDerivedType(typeof(UpdateTrueFalseMetaDataDto), "true_false")]
[JsonDerivedType(typeof(UpdateFillMetaDataDto), "fill")]
[JsonDerivedType(typeof(UpdateErrorSpottingMetaDataDto), "error_spotting")]
[JsonDerivedType(typeof(UpdateOutputPredictionMetaDataDto), "output_prediction")]
[JsonDerivedType(typeof(UpdateCodeWritingMetaDataDto), "code_writing")]
public class UpdateQuestionMetaDataDto
{
    public string Type { get; set; } = string.Empty;
    public UpdateQuestionMetadataDto Metadata { get; set; } = new();
    public string CodeBefore { get; set; } = string.Empty;
    public string CodeAfter { get; set; } = string.Empty;
    public required string Prompt { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string CodeWithBlank { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;
    public List<string> Examples { get; set; } = new();
    public List<string> Rubric { get; set; } = new();
}

public class UpdateMcqMetaDataDto : UpdateQuestionMetaDataDto
{
    public List<UpdateQuestionOptionDto> Options { get; set; } = new();
    public List<string> Answer { get; set; } = new();
}

public class UpdateTrueFalseMetaDataDto : UpdateQuestionMetaDataDto
{
    public bool CorrectAnswer { get; set; }
}

public class UpdateFillMetaDataDto : UpdateQuestionMetaDataDto
{
    public List<string> Answer { get; set; } = new();
}

public class UpdateErrorSpottingMetaDataDto : UpdateQuestionMetaDataDto
{
    public string CodeWithError { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

public class UpdateOutputPredictionMetaDataDto : UpdateQuestionMetaDataDto
{
    public string ExpectedOutput { get; set; } = string.Empty;
}

public class UpdateCodeWritingMetaDataDto : UpdateQuestionMetaDataDto
{
    public string Solution { get; set; } = string.Empty;
    public List<UpdateTestCaseDto> TestCases { get; set; } = new();
}

public class UpdateQuestionMetadataDto
{
    public string Category { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
}

public class UpdateQuestionOptionDto
{
    public string Id { get; set; } = string.Empty;
    public string Option { get; set; } = string.Empty;
}

public class UpdateTestCaseDto
{
    public string Input { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
}