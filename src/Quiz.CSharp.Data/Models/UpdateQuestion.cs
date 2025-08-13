namespace Quiz.CSharp.Data.Models;

public class UpdateQuestion
{
    public required string Subcategory { get; set; }
    public required string Difficulty { get; set; }
    public required string Prompt { get; set; }
    public int EstimatedTimeMinutes { get; set; }
    public required string Type { get; set; }  
    public required UpdateQuestionMetaData Metadata { get; set; }
}

public class UpdateQuestionMetaData
{
    // types => true_false, mcq
    public string? CodeBefore { get; set; }
    
    // types => code_writing, true_false, mcq
    public string? CodeAfter { get; set; }
    
    // types => All
    public required string Prompt { get; set; } 
    
    // types => mcq
    public List<UpdateQuestionOption>? Options { get; set; }
    
    // types => output_prediction_1, error_spotting_1, fill_1, true_false_1, mcq
    public List<string>? Answer { get; set; }
    
    // types => All
    public List<QuestionHint> Hints { get; set; } = [];
    
    // types => output_prediction, error_spotting, fill, true_false, mcq
    public string? Explanation { get; set; } 
    
    // types => fill
    public string? CodeWithBlank { get; set; }
    
    // types => error_spotting, 
    public string? CodeWithError { get; set; } 
    
    // types => output_prediction, 
    public string? Snippet { get; set; }  
    
    // types => code_writing, 
    public string? Solution { get; set; }
    
    // types => code_writing, 
    public List<UpdateTestCase>? TestCases { get; set; }
    
    // types => code_writing, 
    public List<string>? Examples { get; set; }
    
    // types => code_writing, 
    public List<string>? Rubric { get; set; }
}

public class QuestionHint
{
    public string Hint { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class UpdateQuestionOption
{
    public string? Id { get; set; }
    public string? Option { get; set; }
}

public class UpdateTestCase
{
    public string? Input { get; set; }
    public string? ExpectedOutput { get; set; }
}