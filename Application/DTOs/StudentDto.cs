using Core.Entities;

namespace Application.DTOs;

public class StudentDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public List<ExamResult> ExamResults { get; set; } = [];
}