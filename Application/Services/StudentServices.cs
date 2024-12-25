using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Models;
using Core.Persistence;
using FluentResults;

namespace Application.Services;

public class StudentServices(IUnitOfWork unitOfWork, IMapper mapper) : IStudentServices
{
    private readonly IStudentRepository _studentRepository = unitOfWork.StudentRepository;

    public async Task<PagedData<StudentDto>> GetAllAsync(PaginationQuery query)
    {
        var students = await _studentRepository.GetAllAsync(query);
        return mapper.Map<PagedData<StudentDto>>(students);
    }

    public async Task<Result<StudentDto>> GetByIdAsync(string id)
    {
        var studentResult = await _studentRepository.GetByIdAsync(id);
        return studentResult.IsSuccess switch
        {
            true => Result.Ok(mapper.Map<StudentDto>(studentResult.Value)),
            false => Result.Fail<StudentDto>("Student not found")
        };
    }

    public async Task<Result<StudentDto>> ToggleStudentLock(string studentId)
    {
        var studentResult = await _studentRepository.UpdateUserLock(studentId);
        if (!studentResult.IsSuccess)
            return studentResult.ToResult();

        await unitOfWork.CommitAsync();
        return Result.Ok(mapper.Map<StudentDto>(studentResult.Value!));
    }
}