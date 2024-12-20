using Application.DTOs;
using Application.Models;
using AutoMapper;
using AutoMapper.Internal;
using Core.Entities;
using Core.Enums;
using Core.Models;

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
        _pagedDataMapper();
        _examResultMapper();
    }

    private void _studentMapper()
    {
        CreateMap<AppUser, StudentDto>().ReverseMap();
        CreateMap<AppUser, StudentBaseDto>().ReverseMap();
    }

    private void _pagedDataMapper()
    {
        CreateMap<PagedData<AppUser>, PagedData<StudentDto>>().ReverseMap();
        CreateMap<PagedData<ExamResult>, PagedData<ExamResultDto>>().ReverseMap();
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
        CreateMap<Exam, UpdateExamDto>().ReverseMap();
        CreateMap<Exam, CreateExamDto>().ReverseMap();
    }

    private void _examResultMapper()
    {
        CreateMap<ExamResultDto, ExamResult>().ReverseMap()
            .ForMember(e => e.Student,
                opt => opt.MapFrom<AppUser>(e => e.AppUser));
    }
}