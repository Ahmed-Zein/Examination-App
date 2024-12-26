using Application.DTOs;
using Core.Interfaces;

namespace Infrastructure.Cache;

public class SubjectsListKey : ICacheKey<List<SubjectDto>>
{
    public string CacheKey => "SubjectsList";
}