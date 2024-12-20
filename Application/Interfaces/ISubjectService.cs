using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface ISubjectService
{
    Task<List<SubjectDto>> GetAllSubjects();
    Task<Result<SubjectDto>> GetById(int id);
    Task<List<SubjectDto>> CreateSubject(List<CreateSubjectDto> subjectDto);
    Task<Result<SubjectDto>> UpdateSubject(UpdateSubjectDto subjectDto, int id);
    Task<Result> DeleteSubject(int subjectI);
}