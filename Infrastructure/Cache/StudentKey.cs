using Application.DTOs;
using Core.Interfaces;

namespace Infrastructure.Cache;

public class StudentKey(string studentId) : ICacheKey<StudentDto>
{
    public string CacheKey => $"StudentId_{studentId}";
}