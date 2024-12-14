using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateQuestionDtoValidator : AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Question text is required.")
            .MaximumLength(200).WithMessage("Question text must not exceed 200 characters.");

        RuleFor(x => x.Answers)
            .NotEmpty().WithMessage("At least one answer is required.")
            .Must(answers => answers.Count > 1 && answers.Any(a => a.IsCorrect))
            .WithMessage("At least one correct answer is required.");

        RuleForEach(x => x.Answers)
            .SetValidator(new CreateAnswerDtoValidator());
    }
}

public class CreateQuestionListDtoValidator : AbstractValidator<List<CreateQuestionDto>>
{
    public CreateQuestionListDtoValidator()
    {
        RuleForEach(x => x).SetValidator(new CreateQuestionDtoValidator());
    }
}

public class CreateAnswerDtoValidator : AbstractValidator<CreateAnswerDto>
{
    public CreateAnswerDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Answer text is required.")
            .MaximumLength(100).WithMessage("Answer text must not exceed 100 characters.");
    }
}