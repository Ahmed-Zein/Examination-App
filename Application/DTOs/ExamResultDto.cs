using Core.Enums;

namespace Application.DTOs;

public class ExamResultDto
{
    public int TotalScore { get; set; }
    public decimal StudentScore { get; set; }
    public ExamResultStatus Status { get; set; } = ExamResultStatus.UnSubmitted;
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public int ExamId { get; set; }
}