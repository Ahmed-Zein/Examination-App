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
        CreateMap<AppUser, StudentDto>().ReverseMap();
    }

    private void _subjectMapper()
    {
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<CreateSubjectDto, Subject>().ReverseMap();
    }

    private void _answerMapper()
    {
        CreateMap<Answer, AnswerDto>().ReverseMap();
        CreateMap<CreateAnswerDto, Answer>().ReverseMap();
    }

    private void _questionMapper()
    {
        CreateMap<Question, QuestionDto>().ReverseMap();
        CreateMap<CreateQuestionDto, Question>().ReverseMap();
    }
}