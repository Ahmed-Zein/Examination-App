using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers;

public class AppMappersProfiles : Profile
{
    public AppMappersProfiles()
    {
        _studentMapper();
        _subjectMapper();
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
}