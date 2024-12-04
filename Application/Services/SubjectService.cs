using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repositories;
using FluentResults;

namespace Application.Services;

public class SubjectService(IUnitOfWork unitOfWork, IMapper mapper) : ISubjectService
{
    private readonly ISubjectRepository _subjectRepository = unitOfWork.SubjectRepository;

    public async Task<List<SubjectDto>> GetAllSubjects()
    {
        var subjects = await _subjectRepository.GetAllAsync();
        return mapper.Map<List<SubjectDto>>(subjects);
    }

    public async Task<Result<SubjectDto>> GetById(int id)
    {
        var subjectResult = await _subjectRepository.GetByIdAsync(id);

        return subjectResult switch
        {
            { IsSuccess: true } => Result.Ok(mapper.Map<SubjectDto>(subjectResult.Value)),
            { IsSuccess: false } => Result.Fail<SubjectDto>(subjectResult.Errors),
        };
    }

    public async Task<SubjectDto> CreateSubject(CreateSubjectDto subjectDto)
    {
        var subject = mapper.Map<Subject>(subjectDto);

        await _subjectRepository.AddAsync(subject);
        await unitOfWork.CommitAsync();

        return mapper.Map<SubjectDto>(subject);
    }

    public async Task<Result<SubjectDto>> UpdateSubject(UpdateSubjectDto subjectDto, int id)
    {
        var subjectResult = await _subjectRepository.GetByIdAsync(id);
        if (subjectResult.IsFailed)
            return subjectResult.ToResult<SubjectDto>();

        var subject = subjectResult.Value;
        subject.Name = subjectDto.Name;

        await unitOfWork.CommitAsync();

        return mapper.Map<SubjectDto>(subject);
    }
}