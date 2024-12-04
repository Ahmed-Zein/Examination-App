using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Mappers;

public class AppMappersProfiles : Profile
{
    public AppMappersProfiles()
    {
        _studentMapper();
    }

    private void _studentMapper()
    {
        CreateMap<AppUser, StudentDto>();
    }
}