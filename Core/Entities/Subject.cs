using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Subject : BaseModel
{
    [StringLength(256, MinimumLength = 3)] public string Name { get; set; } = string.Empty;

    public List<Exam> Exams { get; set; } = [];
}