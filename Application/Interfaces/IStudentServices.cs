using Application.DTOs;
using Application.Models;
using Core.Models;
using FluentResults;

namespace Application.Interfaces;

public interface IStudentServices
{
    Task<PagedData<StudentDto>> GetAllAsync(PaginationQuery query);
    Task<Result<StudentDto>> GetByIdAsync(string id);
    Task<Result<StudentDto>> ToggleStudentLock(string studentId);
}