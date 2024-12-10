using Application.DTOs;
using FluentResults;

namespace Application.Interfaces;

public interface IStudentServices
{
    Task<List<StudentDto>> GetAllAsync();
    Task<Result<StudentDto>> GetByIdAsync(string id);
    Task<Result<StudentDto>> ToggleStudentLock(string studentId);
}