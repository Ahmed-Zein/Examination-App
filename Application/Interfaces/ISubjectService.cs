using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface ISubjectService
{
    Task<List<SubjectDto>> GetAllSubjects();
    Task<Result<SubjectDto>> GetById(int id);
    Task<SubjectDto> CreateSubject(CreateSubjectDto subjectDto);
    Task<Result<SubjectDto>> UpdateSubject(UpdateSubjectDto subjectDto, int id);
}