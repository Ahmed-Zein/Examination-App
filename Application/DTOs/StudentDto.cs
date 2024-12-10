using Core.Entities;
using Core.Enums;

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

public class ExamResultDto
{
    public int TotalScore { get; set; }
    public decimal StudentScore { get; set; }
    public ExamResultStatus Status { get; set; } = ExamResultStatus.UnSubmitted;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string AppUserId { get; set; } = string.Empty;
    public int ExamId { get; set; }
}