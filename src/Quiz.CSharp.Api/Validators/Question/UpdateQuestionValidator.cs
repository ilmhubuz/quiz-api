using Quiz.CSharp.Api.Dtos.Question;

namespace Quiz.CSharp.Api.Validators.Question;

public class UpdateQuestionValidator : AbstractValidator<UpdateQuestion>
{
    public UpdateQuestionValidator()
    {
        RuleFor(x => x.Subcategory)
            .NotEmpty().WithMessage("Subcategory is required.");

        RuleFor(x => x.Difficulty)
            .NotNull().WithMessage("Difficulty is required.");

        RuleFor(x => x.Prompt)
            .NotEmpty().WithMessage("Prompt is required.");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.");

        RuleFor(x => x.Metadata)
            .NotNull()
            .NotEmpty()
            .WithMessage("MetaData is required for all types.");;
        
        When(q => IsType(q.Type, "true_false", "mcq"), () =>
        {
            RuleFor(q => q.Metadata.CodeBefore)
                .NotNull()
                .WithMessage("CodeBefore is required for types true_false and mcq.");
        });

        When(q => IsType(q.Type, "code_writing", "true_false", "mcq"), () =>
        {
            RuleFor(q => q.Metadata.CodeAfter)
                .NotNull()
                .WithMessage("CodeAfter is required for types code_writing, true_false, and mcq.");
        });
        
        RuleFor(x => x.Metadata.Prompt)
            .NotEmpty()
            .WithMessage("MetaData.Prompt is required for all types.");
        
        When(q => IsType(q.Type, "mcq"), () =>
        {
            RuleFor(q => q.Metadata.Options)
                .NotNull()
                .NotEmpty()
                .WithMessage("Options are required for type mcq.")
                .ForEach(opt => opt.SetValidator(new UpdateQuestionOptionValidator()));
        });

        When(q => IsType(q.Type, "output_prediction", "error_spotting", "fill", "true_false", "mcq"), () =>
        {
            RuleFor(q => q.Metadata.Answer)
                .NotNull()
                .NotEmpty()
                .WithMessage("Answer is required for this question type.");

            RuleFor(q => q.Metadata.Explanation)
                .NotNull()
                .WithMessage("Explanation is required for type question type.");
        });

        When(q => IsType(q.Type, "fill"), () =>
        {
            RuleFor(q => q.Metadata.CodeWithBlank)
                .NotNull()
                .WithMessage("CodeWithBlank is required for type fill.");
        });

        When(q => IsType(q.Type, "error_spotting"), () =>
        {
            RuleFor(q => q.Metadata.CodeWithError)
                .NotNull().WithMessage("CodeWithError is required for type error_spotting.");
        });
        
        When(q => IsType(q.Type, "output_prediction"), () =>
        {
            RuleFor(q => q.Metadata.Snippet)
                .NotNull()
                .WithMessage("Snippet is required for type output_prediction.");
        });
        
        When(q => IsType(q.Type, "code_writing"), () =>
        {
            RuleFor(q => q.Metadata.Solution)
                .NotNull()
                .WithMessage("Solution is required for type code_writing.");

            RuleFor(q => q.Metadata.TestCases)
                .NotNull()
                .WithMessage("TestCases are required for type code_writing.")
                .ForEach(tc => tc.SetValidator(new UpdateTestCaseValidator()));

            RuleFor(q => q.Metadata.Examples)
                .NotNull()
                .WithMessage("Examples are required for type code_writing.");

            RuleFor(q => q.Metadata.Rubric)
                .NotNull()
                .WithMessage("Rubric is required for type code_writing.");
        });
    }

    private static bool IsType(string? currentType, params string[] allowed) =>
        !string.IsNullOrWhiteSpace(currentType) && 
        Array.Exists(allowed, t => string.Equals(t, currentType, StringComparison.OrdinalIgnoreCase));
}

public class UpdateQuestionOptionValidator : AbstractValidator<UpdateQuestionOption>
{
    public UpdateQuestionOptionValidator()
    {
        RuleFor(x => x.Option)
            .NotNull()
            .WithMessage("Option is required.");;
    }
}

public class UpdateTestCaseValidator : AbstractValidator<UpdateTestCase>
{
    public UpdateTestCaseValidator()
    {
        RuleFor(x => x.Input)
            .NotNull() 
            .WithMessage("Input is required.");

        RuleFor(x => x.ExpectedOutput)
            .NotNull() 
            .WithMessage("ExpectedOutput is required.");
    }
}