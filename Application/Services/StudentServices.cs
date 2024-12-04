using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Core.Interfaces.Repositories;
using FluentResults;

namespace Application.Services;

public class StudentServices(IUnitOfWork unitOfWork, IMapper mapper) : IStudentServices
{
    private readonly IStudentRepository _studentRepository = unitOfWork.StudentRepository;

    public async Task<List<StudentDto>> GetAllAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return mapper.Map<List<StudentDto>>(students);
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
}