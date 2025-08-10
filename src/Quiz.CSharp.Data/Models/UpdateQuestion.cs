using System.Text.Json.Serialization;

namespace Quiz.CSharp.Data.Models;

public class UpdateQuestion
{
    public int CollectionId { get; set; }
    public required string Subcategory { get; set; }
    public required string Difficulty { get; set; }
    public required string Prompt { get; set; }
    public int EstimatedTimeMinutes { get; set; }

    public required UpdateQuestionMetaData Metadata { get; set; }
    public bool IsActive { get; set; }
}

[JsonDerivedType(typeof(UpdateMcqMetaData), typeDiscriminator: "mcq")]
[JsonDerivedType(typeof(UpdateTrueFalseMetaData), typeDiscriminator: "true_false")]
[JsonDerivedType(typeof(UpdateFillMetaData), typeDiscriminator: "fill")]
[JsonDerivedType(typeof(UpdateErrorSpottingMetaData), typeDiscriminator: "error_spotting")]
[JsonDerivedType(typeof(UpdateOutputPredictionMetaData), typeDiscriminator: "output_prediction")]
[JsonDerivedType(typeof(UpdateCodeWritingMetaData), typeDiscriminator: "code_writing")]
public class UpdateQuestionMetaData
{
    public string Type { get; set; } = string.Empty;
    public UpdateQuestionMetadata Metadata { get; set; } = new();
    public string CodeBefore { get; set; } = string.Empty;
    public string CodeAfter { get; set; } = string.Empty;
    public required string Prompt { get; set; } = string.Empty;
    public List<UpdateQuestionOption> Options { get; set; } = new();
    public List<string> Answer { get; set; } = new();
    public string Explanation { get; set; } = string.Empty;
    public string CodeWithBlank { get; set; } = string.Empty;
    public string CodeWithError { get; set; } = string.Empty;
    public string Snippet { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public List<UpdateTestCase> TestCases { get; set; } = new();
    public List<string> Examples { get; set; } = new();
    public List<string> Rubric { get; set; } = new();
}

public class UpdateMcqMetaData : UpdateQuestionMetaData
{
    public new List<UpdateQuestionOption> Options { get; set; } = new();
    public new List<string> Answer { get; set; } = new();
}

public class UpdateTrueFalseMetaData : UpdateQuestionMetaData
{
    public bool CorrectAnswer { get; set; }
}

public class UpdateFillMetaData : UpdateQuestionMetaData
{
    public new List<string> Answer { get; set; } = new();
}

public class UpdateErrorSpottingMetaData : UpdateQuestionMetaData
{
    public new string CodeWithError { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

public class UpdateOutputPredictionMetaData : UpdateQuestionMetaData
{
    public string ExpectedOutput { get; set; } = string.Empty;
}

public class UpdateCodeWritingMetaData : UpdateQuestionMetaData
{
    public new string Solution { get; set; } = string.Empty;
    public new List<UpdateTestCase> TestCases { get; set; } = new();
}

public class UpdateQuestionMetadata
{
    public string Category { get; set; } = string.Empty;
    public string Subcategory { get; set; } = string.Empty;
}

public class UpdateQuestionOption
{
    public string Id { get; set; } = string.Empty;
    public string Option { get; set; } = string.Empty;
}

public class UpdateTestCase
{
    public string Input { get; set; } = string.Empty;
    public string ExpectedOutput { get; set; } = string.Empty;
}