namespace Application.DTOs;

public class StudentDto
{
    public bool IsLocked { get; set; } = false;
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public List<ExamResultDto> ExamResults { get; set; } = [];
}