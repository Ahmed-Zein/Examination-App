using Application.DTOs;
using Core.Interfaces;
using Core.Models;

namespace Infrastructure.Cache;

public class StudentListKey : ICacheKey<PagedData<StudentDto>>
{
    public string CacheKey => "StudentList";
}