using Quiz.CSharp.Api.Dtos.Question;

namespace Quiz.CSharp.Api.Validators.Question;

public class UpdateQuestionValidator : AbstractValidator<UpdateQuestion>
{
    public UpdateQuestionValidator()
    {
        RuleFor(q => q.Subcategory)
            .NotEmpty().WithMessage("Subcategory cannot be null.");

        RuleFor(q => q.Difficulty)
            .NotEmpty().WithMessage("Difficulty is required.");

        RuleFor(q => q.Prompt)
            .NotEmpty().WithMessage("Prompt is required.");

        /*RuleFor(q => q.QuestionType)
            .NotEmpty().WithMessage("Question type is required.")
            .Must(IsSupportedQuestionType)
            .WithMessage("Invalid question type.");*/

        RuleFor(q => q.Metadata)
            .NotNull().WithMessage("Metadata is required.")
            .SetInheritanceValidator(v =>
            {
                v.Add(new McqMetadataValidator());
                v.Add(new TrueFalseMetadataValidator());
                v.Add(new FillInTheBlankMetadataValidator());
                v.Add(new ErrorSpottingMetadataValidator());
                v.Add(new OutputPredictionMetadataValidator());
                v.Add(new CodeWritingMetadataValidator());
            });
    }

    private bool IsSupportedQuestionType(string type) =>
        new[] { "mcq", "true_false", "fill", "error_spotting", "output_prediction", "code_writing" }.Contains(type);
}

public class UpdateQuestionMetadataBaseValidator<T> : AbstractValidator<T>
    where T : UpdateQuestionMetadataBase
{
    protected UpdateQuestionMetadataBaseValidator()
    {
        RuleFor(m => m.CodeAfter).NotNull();
        RuleFor(m => m.CodeBefore).NotNull();
        RuleFor(m => m.Explanation).NotNull();
    }
}

public class McqMetadataValidator : UpdateQuestionMetadataBaseValidator<McqMetadata>
{
    public McqMetadataValidator()
    {
        RuleFor(m => m.Options)
            .NotEmpty().WithMessage("At least one option is required.");
        
        RuleForEach(m => m.Options)
            .SetValidator(new McqOptionDtoValidator());
        
        RuleFor(m => m.CorrectAnswerIds)
            .NotEmpty().WithMessage("At least one correct answer is required.");
    }
}

public class McqOptionDtoValidator : AbstractValidator<McqOptionDto>
{
    public McqOptionDtoValidator()
    {
        RuleFor(o => o.Id)
            .NotEmpty().WithMessage("Option Id is required.");

        RuleFor(o => o.Text)
            .NotEmpty().WithMessage("Option text is required.");
    }
}

public class TrueFalseMetadataValidator : UpdateQuestionMetadataBaseValidator<TrueFalseMetadata>
{
    public TrueFalseMetadataValidator()
    { }
}

public class FillInTheBlankMetadataValidator : UpdateQuestionMetadataBaseValidator<FillInTheBlankMetadata>
{
    public FillInTheBlankMetadataValidator()
    {
        RuleFor(m => m.FillHints)
            .NotNull().WithMessage("Fill hints cannot be null. Provide at least an empty list.");

        RuleFor(m => m.CodeWithBlank)
            .NotNull().WithMessage("CodeWithBlank cannot be null.");

        RuleFor(m => m.CorrectAnswer)
            .NotEmpty().WithMessage("Correct answer is required.");
    }
}

public class ErrorSpottingMetadataValidator : UpdateQuestionMetadataBaseValidator<ErrorSpottingMetadata>
{
    public ErrorSpottingMetadataValidator()
    {
        RuleFor(m => m.CodeWithError)
            .NotNull().WithMessage("CodeWithError cannot be null.");

        RuleFor(m => m.CorrectAnswer)
            .NotEmpty().WithMessage("Correct answer is required.");
    }
}

public class OutputPredictionMetadataValidator : UpdateQuestionMetadataBaseValidator<OutputPredictionMetadata>
{
    public OutputPredictionMetadataValidator()
    {
        RuleFor(m => m.Snippet)
            .NotEmpty().WithMessage("Snippet is required.");

        RuleFor(m => m.ExpectedOutput)
            .NotEmpty().WithMessage("Expected output is required.");
    }
}

public class CodeWritingMetadataValidator : UpdateQuestionMetadataBaseValidator<CodeWritingMetadata>
{
    public CodeWritingMetadataValidator()
    {
        RuleFor(m => m.Rubric)
            .NotNull().WithMessage("Rubric cannot be null. Provide at least an empty list.");

        RuleFor(m => m.Examples)
            .NotNull().WithMessage("Examples cannot be null. Provide at least an empty list.");

        RuleFor(m => m.Solution)
            .NotNull().WithMessage("Solution cannot be null.");

        RuleFor(m => m.TestCases)
            .NotNull().WithMessage("Test cases cannot be null. Provide at least an empty list.");
    }
}