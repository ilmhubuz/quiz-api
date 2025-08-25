using Quiz.CSharp.Api.Contracts.Dto;

namespace Quiz.CSharp.Api.Validators;

public class UpdateQuestionDtoValidator : AbstractValidator<UpdateQuestionDto>
{
    public UpdateQuestionDtoValidator()
    {
        RuleFor(q => q.Id)
            .GreaterThan(0)
            .WithMessage("The ID cannot be equal to 0.");
        RuleFor(q => q.Subcategory).NotEmpty();
        RuleFor(q => q.Difficulty).NotEmpty();
        RuleFor(q => q.Prompt).NotEmpty();
        RuleFor(q => q.EstimatedTimeMinutes)
            .GreaterThan(0)
            .WithMessage("Estimated time must be greater than 0 minutes.");

        RuleFor(q => q.Metadata).NotNull()
            .SetInheritanceValidator(v =>
            {
                v.Add(new McqMetadataValidator());
                v.Add(new TrueFalseMetadataValidator());
                v.Add(new FillMetadataValidator());
                v.Add(new ErrorSpottingMetadataValidator());
                v.Add(new OutputPredictionMetadataValidator());
                v.Add(new CodeWritingMetadataValidator());
            });
    }
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
        RuleFor(m => m.Options).NotEmpty();
        RuleForEach(m => m.Options)
            .SetValidator(new McqOptionDtoValidator());
        RuleFor(m => m.CorrectAnswerIds).NotEmpty();
    }
}

public class McqOptionDtoValidator : AbstractValidator<McqOptionDto>
{
    public McqOptionDtoValidator()
    {
        RuleFor(o => o.Id).NotEmpty();
        RuleFor(o => o.Text).NotEmpty();
    }
}

public class TrueFalseMetadataValidator : UpdateQuestionMetadataBaseValidator<TrueFalseMetadata>
{
    public TrueFalseMetadataValidator()
    { }
}

public class FillMetadataValidator : UpdateQuestionMetadataBaseValidator<FillMetadata>
{
    public FillMetadataValidator()
    {
        RuleFor(m => m.FillHints).NotNull();
        RuleFor(m => m.CodeWithBlank).NotNull();
        RuleFor(m => m.CorrectAnswer).NotEmpty();
    }
}

public class ErrorSpottingMetadataValidator : UpdateQuestionMetadataBaseValidator<ErrorSpottingMetadata>
{
    public ErrorSpottingMetadataValidator()
    {
        RuleFor(m => m.CodeWithError).NotNull();
        RuleFor(m => m.CorrectAnswer).NotEmpty();
    }
}

public class OutputPredictionMetadataValidator : UpdateQuestionMetadataBaseValidator<OutputPredictionMetadata>
{
    public OutputPredictionMetadataValidator()
    {
        RuleFor(m => m.Snippet).NotEmpty();
        RuleFor(m => m.ExpectedOutput).NotEmpty();
    }
}

public class CodeWritingMetadataValidator : UpdateQuestionMetadataBaseValidator<CodeWritingMetadata>
{
    public CodeWritingMetadataValidator()
    {
        RuleFor(m => m.Rubric).NotNull();
        RuleFor(m => m.Examples).NotNull();
        RuleFor(m => m.Solution).NotNull();
        RuleFor(m => m.TestCases).NotNull();
    }
}