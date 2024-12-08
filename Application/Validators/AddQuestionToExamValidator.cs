using Application.DTOs;
using Application.Interfaces.Persistence;
using FluentValidation;

namespace Application.Validators;

public class AddQuestionToExamValidator : AbstractValidator<AddQuestionToExamDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddQuestionToExamValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(e => e.ExamId)
            .GreaterThan(0).WithMessage("Invalid exam Id");

        RuleFor(e => e.SubjectId)
            .GreaterThan(0).WithMessage("Invalid subject Id");

        RuleFor(e => e.QuestionIds)
            .NotEmpty().WithMessage("QuestionIds cannot be empty.");


        RuleFor(dto => dto)
            .MustAsync(_validateAgainstSubject)
            .WithMessage("Some QuestionIds do not exist for the given subject.");
        // .MustAsync(_validateAgainstExam)
        // .WithMessage("Some QuestionIds already exist in this exam.");
    }

    private async Task<bool> _validateAgainstSubject(AddQuestionToExamDto dto, CancellationToken _)
    {
        var existingSubjectQuestions = await _unitOfWork.QuestionRepository.GetQuestionsIdBySubject(dto.SubjectId);
        return dto.QuestionIds.All(id => existingSubjectQuestions.Contains(id));
    }

    // private async Task<bool> _validateAgainstExam(AddQuestionToExamDto dto, CancellationToken _)
    // {
    //     var existingExamQuestions = await _unitOfWork.QuestionRepository.GetIdByExam(dto.ExamId);
    //     return !dto.QuestionIds.Any(id => existingExamQuestions.Contains(id));
    // }
}