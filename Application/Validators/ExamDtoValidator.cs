using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class ExamDtoValidator : AbstractValidator<ExamDto>
{
}

public class CreateExamDtoValidator : AbstractValidator<CreateExamDto>
{
    public CreateExamDtoValidator()
    {
        RuleFor(x => x.ModelName)
            .NotEmpty().WithMessage("ModelName is required")
            .MaximumLength(50).WithMessage("ModelName must not exceed 50 characters")
            .MinimumLength(3).WithMessage("ModelName must not be less than 3 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than zero.")
            .LessThanOrEqualTo(TimeSpan.FromHours(2)).WithMessage("Duration must be less than or equal to 5 hours.");
    }
}