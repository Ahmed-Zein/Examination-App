using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers;

public class AppMappersProfiles : Profile
{
    public AppMappersProfiles()
    {
        _answerMapper();
        _subjectMapper();
        _studentMapper();
        _questionMapper();
    }

    private void _studentMapper()
    {
        CreateMap<AppUser, StudentDto>();
    }

    private void _subjectMapper()
    {
        CreateMap<Subject, SubjectDto>();
        CreateMap<CreateSubjectDto, Subject>();
    }

    private void _answerMapper()
    {
        CreateMap<Answer, AnswerDto>();
        CreateMap<AnswerDto, Answer>();
        CreateMap<CreateAnswerDto, Answer>();
    }

    private void _questionMapper()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<QuestionDto, Question>();
        CreateMap<CreateQuestionDto, Question>();
    }
}