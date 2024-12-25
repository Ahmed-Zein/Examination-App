using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Persistence;
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
            { IsSuccess: false } => Result.Fail<SubjectDto>(subjectResult.Errors)
        };
    }

    public async Task<List<SubjectDto>> CreateSubject(List<CreateSubjectDto> subjectDto)
    {
        var subjects = mapper.Map<List<Subject>>(subjectDto);

        foreach (var subject in subjects) await _subjectRepository.AddAsync(subject);

        await unitOfWork.CommitAsync();

        return mapper.Map<List<SubjectDto>>(subjects);
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

    public async Task<Result> DeleteSubject(int subjectId)
    {
        var repositoryResult = await _subjectRepository.GetByIdAsync(subjectId);
        if (!repositoryResult.IsSuccess)
            return repositoryResult.ToResult();

        var subject = repositoryResult.Value;
        if (subject.Exams.Count > 0)
            return Result.Fail([
                "Cannot delete a subject that has Exams, delete the exams first", ErrorType.BadRequest
            ]);

        _subjectRepository.Delete(subject);
        await unitOfWork.CommitAsync();

        return Result.Ok();
    }
}