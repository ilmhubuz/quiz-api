namespace Quiz.CSharp.Api.Dtos.Question;

public class UpdateQuestion
{
    public required string Subcategory { get; set; }
    public required string Difficulty { get; set; }
    public required string Prompt { get; set; }
    public int EstimatedTimeMinutes { get; set; }

    public required UpdateQuestionMetaData Metadata { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateQuestionMetaData
{
    // type => All
    public required string Type { get; set; }  
    
    public string CodeBefore { get; set; } = string.Empty;
    
    // types => output_prediction, code_writing, error_spotting, fill
    public required UpdateQuestionCategory MetaData { get; set; }
    
    public string CodeAfter { get; set; } = string.Empty;
    
    //  type => output_prediction, code_writing, error_spotting, fill
    public required string Prompt { get; set; } 
    
    public List<UpdateQuestionOption> Options { get; set; } = new();
    
    //  type => output_prediction, error_spotting, fill
    public List<string> Answer { get; set; } = new(); 
    
    //  type => output_prediction, error_spotting, fill
    public string? Explanation { get; set; } 
    
    // type => 
    public bool CorrectAnswer { get; set; }
    
    // type => fill
    public string CodeWithBlank { get; set; } = string.Empty;
    
    // type => error_spotting, 
    public string? CodeWithError { get; set; } 
    
    //  type => , 
    public string ExpectedOutput { get; set; } = string.Empty;
    
    //  type => output_prediction, 
    public string? Snippet { get; set; }  
    
    public string Solution { get; set; } = string.Empty;   
    
    public List<UpdateTestCase> TestCases { get; set; } = new();
    
    //  type => code_writing, 
    public List<string> Examples { get; set; } = new();
    public List<string> Rubric { get; set; } = new();
}

public class UpdateQuestionCategory
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