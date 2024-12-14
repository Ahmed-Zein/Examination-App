namespace Application.DTOs;

public class StudentBaseDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
}

public class StudentDto : StudentBaseDto
{
    public bool IsLocked { get; set; } = false;
    public List<ExamResultDto> ExamResults { get; set; } = [];
}