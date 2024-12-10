using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers;

public class AppMappersProfiles : Profile
{
    public AppMappersProfiles()
    {
        _examMapper();
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
        CreateMap<Subject, CreateSubjectDto>().ReverseMap();
    }

    private void _answerMapper()
    {
        CreateMap<Answer, AnswerDto>().ReverseMap();
        CreateMap<Answer, StudentAnswer>().ReverseMap();
        CreateMap<Answer, CreateAnswerDto>().ReverseMap();
    }

    private void _questionMapper()
    {
        CreateMap<Question, QuestionDto>().ReverseMap();
        CreateMap<Question, StudentQuestion>().ReverseMap();
        CreateMap<Question, CreateQuestionDto>().ReverseMap();
    }

    private void _examMapper()
    {
        CreateMap<Exam, ExamDto>().ReverseMap();
        CreateMap<Exam, StudentExam>().ReverseMap();
        CreateMap<Exam, CreateExamDto>().ReverseMap();
    }
}